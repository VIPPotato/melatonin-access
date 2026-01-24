using System.Collections;
using UnityEngine;

public class Dream_shopping : Dream
{
	private bool isFux;

	private bool isFuxAlt;

	private int fuxState;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Mall.env.Show();
		Mall.env.Counter.CardMachine.Bobble();
		Mall.env.Counter.McSpender.BobbleDelayed(beatDelta);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Mall.env.Counter.CardMachine.Bobble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Mall.env.Counter.CardMachine.Bobble();
		Mall.env.Counter.McSpender.BobbleDelayed(beatDelta);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Mall.env.Counter.CardMachine.Bobble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			if (SaveManager.mgr.GetScore("Dream_food") <= 1 && SaveManager.mgr.GetScore("Dream_shopping") <= 1 && SaveManager.mgr.GetScore("Dream_followers") <= 1 && SaveManager.mgr.GetScore("Dream_tech") <= 1)
			{
				isFux = true;
			}
			else
			{
				isFuxAlt = true;
			}
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux || isFuxAlt)
			{
				timeStarted = Technician.mgr.GetDspTime();
				Mall.env.Counter.CardMachine.Bobble();
				Mall.env.Counter.McSpender.BobbleDelayed(beatDelta);
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
				yield return null;
			}
		}
		TriggerSong();
		Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
	}

	protected override void OnUpdate()
	{
		if (isFux)
		{
			if (!ControlHandler.mgr.CheckIsActionPressed() || !DreamWorld.env.DialogBox.CheckIsActivated() || !(Time.timeScale > 0f))
			{
				return;
			}
			fuxState++;
			if (fuxState == 1)
			{
				if (SaveManager.GetLang() >= 5)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1);
				}
			}
			else if (fuxState >= 2)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
		else if (isFuxAlt && ControlHandler.mgr.CheckIsActionPressed() && DreamWorld.env.DialogBox.CheckIsActivated() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState >= 1)
			{
				isFuxAlt = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		if (!isHitWindow && (beat == 1 || beat == 3))
		{
			Mall.env.Counter.McSpender.BobbleDelayed(beatDelta);
		}
		Mall.env.Counter.CardMachine.Bobble();
	}

	protected override void OnBeat()
	{
		if (phrase == 5 && bar == 1 && beat == 1)
		{
			Mall.env.StoreDisplay.Slide(timeBeatStarted);
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate();
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate();
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate();
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate();
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(1);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(1);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(1);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(1);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(2);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(2);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(2);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(2);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(3);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(3);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(3);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(3);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[3] = 0f;
		}
		if (sequences[4] > 0f)
		{
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(4);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(4);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(4);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(4);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[4] = 0f;
		}
		if (!(sequences[5] > 0f))
		{
			return;
		}
		if (!Mall.env.StoreDisplay.CheckIsDropping())
		{
			if (Interface.env.Circle.CheckIsActivated())
			{
				Interface.env.Circle.ResetRingNum();
			}
			if (gameMode != 2 && gameMode != 4 && gameMode < 6)
			{
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
				}
				else
				{
					Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
				}
			}
			Mall.env.StoreDisplay.Clear();
			Mall.env.StoreDisplay.Drop(timeBeatStarted);
		}
		if (isFullBeat)
		{
			if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(5);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(5);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
		}
		else if (Mall.env.StoreDisplay.CheckIsDropping2())
		{
			Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(5);
			Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
			Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
		}
		else
		{
			Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(5);
			Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
			Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
		}
		if (Mall.env.StoreDisplay.CheckIsSlid())
		{
			QueueHitWindow(8);
		}
		else
		{
			QueueHitWindow(4);
		}
		sequences[5] = 0f;
	}

	protected override void OnAction()
	{
		Mall.env.PlayFeedback();
		Mall.env.Counter.McSpender.Purchase();
	}

	protected override void OnHit()
	{
		Mall.env.PlayGoodFeedback();
		Mall.env.Counter.ReactGood();
		Mall.env.Counter.CardMachine.ResetMeter();
		Mall.env.StoreDisplay.GetActiveItem().BagUp();
		Mall.env.StoreDisplay.GetActiveItem().AttractFeedback();
	}

	protected override void OnStrike()
	{
		Mall.env.PlayBadFeedback();
		Mall.env.Counter.ReactBad();
		Mall.env.Sweat.Hide();
		Mall.env.Sweat.SetPosition(Mall.env.Counter.CardMachine.GetX() - 2f, Mall.env.Counter.CardMachine.GetY() + 0.67f);
		Mall.env.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		Mall.env.PlayBadFeedback();
		Mall.env.Counter.ReactBad();
		Mall.env.Counter.CardMachine.ResetMeter();
		Mall.env.StoreDisplay.GetActiveItem().AttractFeedback();
		Mall.env.Sweat.CrossIn();
	}
}
