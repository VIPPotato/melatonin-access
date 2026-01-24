using System.Collections;
using UnityEngine;

public class Dream_past : Dream
{
	private bool isFux;

	private int fuxState;

	private int mod0;

	private int mod1;

	private int mod2;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Darkroom.env.Show();
		Darkroom.env.MakePlayable();
		Interface.env.Cam.Drift(1);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitForSeconds(1f);
		Darkroom.env.McLighter.Light();
		yield return new WaitForSeconds(1f);
		Darkroom.env.McLighter.Release();
		yield return new WaitForSeconds(1f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() == 0)
			{
				string text = DreamWorld.env.DialogBox.GetText();
				if (ControlHandler.mgr.GetCtrlType() == 1)
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", "A"));
				}
				else if (ControlHandler.mgr.GetCtrlType() == 2)
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", "X"));
				}
				else
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", SaveManager.mgr.GetActionKey()));
				}
			}
			while (isFux)
			{
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
	}

	protected override void OnUpdate()
	{
		if (isFux && ControlHandler.mgr.CheckIsActionPressed() && DreamWorld.env.DialogBox.CheckIsActivated() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState == 1)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1);
			}
			else if (fuxState >= 2)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
		if (isActionPressing)
		{
			foreach (MemoryPhoto burnableMemoryPhoto in Darkroom.env.PhotoPulley.GetBurnableMemoryPhotos())
			{
				burnableMemoryPhoto.StartBurn();
			}
			return;
		}
		foreach (MemoryPhoto burnableMemoryPhoto2 in Darkroom.env.PhotoPulley.GetBurnableMemoryPhotos())
		{
			burnableMemoryPhoto2.StopBurn();
		}
	}

	protected override void OnBobble()
	{
		Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
	}

	protected override void OnBeat()
	{
		if (gameMode == 1)
		{
			if (phrase == 1 && bar == 7 && beat == 4)
			{
				Darkroom.env.SwapClotheslineDelayed(timeBeatStarted);
			}
			else if (phrase == 2 && bar == 7 && beat == 4)
			{
				Darkroom.env.SwapClotheslineDelayed(timeBeatStarted);
			}
			else if (phrase == 3)
			{
				if (bar == 1 && beat == 1)
				{
					Darkroom.env.PhotoPulley.SetDragType(1);
				}
				else if (bar == 7 && beat == 4)
				{
					Darkroom.env.SwapClotheslineDelayed(timeBeatStarted);
				}
			}
			else if (phrase == 4)
			{
				if (bar == 1 && beat == 1)
				{
					Darkroom.env.PhotoPulley.SetDragType(2);
				}
				else if (bar == 7 && beat == 4)
				{
					Darkroom.env.SwapClotheslineDelayed(timeBeatStarted);
				}
			}
			else if (phrase == 5 && bar == 1 && beat == 1)
			{
				Darkroom.env.PhotoPulley.SetDragType(0);
			}
		}
		else if (gameMode == 2 && phrase == 1 && bar == 1 && beat == 1)
		{
			Darkroom.env.PhotoPulley.SetDragType(2);
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueHoldReleaseWindow(4, 5);
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
			sequences[0] = 0f;
		}
		else if (sequences[1] > 0f)
		{
			QueueHoldReleaseWindow(4, 4, isHalfBeatAddedToHold: false, isHalfBeatAddedToRelease: true);
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 1);
			sequences[1] = 0f;
		}
		else if (sequences[2] > 0f)
		{
			QueueHoldReleaseWindow(4, 6);
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 3);
			sequences[2] = 0f;
		}
		else if (sequences[3] > 0f)
		{
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
			sequences[3] = 0f;
		}
		else if (sequences[4] > 0f)
		{
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 1);
			sequences[4] = 0f;
		}
		else if (sequences[5] > 0f)
		{
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 3);
			sequences[5] = 0f;
		}
	}

	protected override void OnEvent()
	{
		Darkroom.env.PhotoPulley.SetDragType(eventNum);
	}

	protected override void OnAction()
	{
		Darkroom.env.McLighter.Light();
	}

	protected override void OnActionReleased()
	{
		Darkroom.env.McLighter.Release();
	}

	protected override void OnHit()
	{
		if (accuracy >= 1f)
		{
			Darkroom.env.PlayBurnFeedback();
			Darkroom.env.PhotoPulley.GetDissolvableMemoryPhoto().Dissolve();
		}
	}

	protected override void OnStrike()
	{
		Darkroom.env.McLighter.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			Darkroom.env.McLighter.Sweat.CrossIn();
		}
	}

	protected override void OnResults()
	{
		Darkroom.env.McLighter.Deactivate();
	}
}
