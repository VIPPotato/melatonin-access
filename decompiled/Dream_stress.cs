using System.Collections;
using UnityEngine;

public class Dream_stress : Dream
{
	private bool isFux;

	private int fuxState;

	private int rng;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Underworld.env.Show(isCuedUp: false);
		Underworld.env.LavaPool.SetToLowered(0.8518f);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		if (gameMode == 0)
		{
			timeStarted = Technician.mgr.GetDspTime();
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			while (isFux)
			{
				Underworld.env.LavaPool.PopBubble();
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		timeStarted = Technician.mgr.GetDspTime();
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		TriggerSong();
		Interface.env.Circle.SetSpawnEaseType(1);
	}

	protected override void OnUpdate()
	{
		if (isFux && ControlHandler.mgr.CheckIsActionPressed() && DreamWorld.env.DialogBox.CheckIsActivated() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState >= 1)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		if (!isHitWindow)
		{
			Underworld.env.Climb(0.8518f);
			Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		}
		if (beat == 1 || beat == 3)
		{
			Underworld.env.LavaPool.PopBubble();
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueLeftHitWindow(1);
			Underworld.env.CueLeftDelayed(timeBeatStarted);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueRightHitWindow(1);
			Underworld.env.CueRightDelayed(timeBeatStarted);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			QueueHitWindow(1);
			Underworld.env.CueCenterDelayed(timeBeatStarted);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			rng = Random.Range(0, 2);
			if (rng == 0)
			{
				QueueLeftHitWindow(1);
				Underworld.env.CueLeftDelayed(timeBeatStarted);
			}
			else
			{
				QueueRightHitWindow(1);
				Underworld.env.CueRightDelayed(timeBeatStarted);
			}
			sequences[3] = 0f;
		}
	}

	protected override void OnHitWindow()
	{
		if (hitType == 1)
		{
			Underworld.env.LavaPool.LinearRise(beatDelta, 3.4072f);
		}
		else if (hitType == 2)
		{
			Underworld.env.LavaPool.LinearRise(beatDelta, 3.4072f);
		}
		else
		{
			Underworld.env.LavaPool.LinearRise(beatDelta, 5.1108003f);
		}
		Underworld.env.LavaPool.LungeDelayed(timeBeatStarted);
	}

	protected override void OnEvent()
	{
		switch (eventNum)
		{
		case 0:
			Underworld.env.McClimber.ToggleIsDangeorus(toggle: false);
			break;
		case 1:
			Underworld.env.McClimber.ToggleIsDangeorus(toggle: true);
			break;
		}
	}

	protected override void OnHit()
	{
		Underworld.env.PlayJump(hitType);
		if (accuracy == 0.333f)
		{
			if (hitType == 1)
			{
				Underworld.env.McClimber.Move("burnLeft", -5.5f, 3.4072f);
			}
			else if (hitType == 2)
			{
				Underworld.env.McClimber.Move("burnRight", 5.5f, 3.4072f);
			}
			else
			{
				Underworld.env.McClimber.Move("burnUp", 0f, 5.1108003f);
			}
			Underworld.env.BurnMc();
		}
		else if (hitType == 1)
		{
			Underworld.env.McClimber.Move("jumpLeft", -5.5f, 3.4072f);
		}
		else if (hitType == 2)
		{
			Underworld.env.McClimber.Move("jumpRight", 5.5f, 3.4072f);
		}
		else
		{
			Underworld.env.McClimber.Move("jumpUp", 0f, 5.1108003f);
		}
		if (hitType == 1)
		{
			Underworld.env.Feedbacks[0].SetLocalPosition(2.7f, -2.67f);
		}
		else if (hitType == 2)
		{
			Underworld.env.Feedbacks[0].SetLocalPosition(-2.7f, -2.67f);
		}
		else
		{
			Underworld.env.Feedbacks[0].SetLocalPosition(0f, -0.42f);
		}
	}

	protected override void OnStrike()
	{
		Underworld.env.Sweat.SetPosition(Underworld.env.McClimber.GetHeadPosition().x, Underworld.env.McClimber.GetHeadPosition().y);
		Underworld.env.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (hitType == 1)
		{
			Underworld.env.McClimber.Move("burnLeft", -5.5f, 3.4072f);
		}
		else if (hitType == 2)
		{
			Underworld.env.McClimber.Move("burnRight", 5.5f, 3.4072f);
		}
		else
		{
			Underworld.env.McClimber.Move("burnUp", 0f, 5.1108003f);
		}
		Underworld.env.BurnMc();
	}

	protected override void OnExit()
	{
		DreamWorld.env.RecenterZoomer(Underworld.env);
		Underworld.env.Exit();
	}
}
