using System.Collections;
using UnityEngine;

public class Matrix : Wrapper
{
	public static Matrix env;

	[Header("Children")]
	public MiniMatrix[] MiniMatrixes;

	public LilBlocks LilBlocks;

	private bool isZoomedFarOut;

	private bool isActivated;

	private Coroutine cueing;

	private Coroutine cueingSound;

	private Coroutine parallaxInFar;

	private const float animTempo = 80f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 3);
	}

	public void Show()
	{
		isActivated = true;
		isZoomedFarOut = false;
		RenderChildren(toggle: true, 3);
		Interface.env.Cam.SetPosition(0f, 0f);
		MiniMatrixes[0].Show();
		MiniMatrixes[MiniMatrixes.Length - 1].Show();
		LilBlocks.Show();
		DreamWorld.env.SetFeedbacks(MiniMatrixes[0].McSwinger.Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		HideArray();
		LilBlocks.Hide();
		CancelCoroutine(cueing);
		CancelCoroutine(cueingSound);
		CancelCoroutine(parallaxInFar);
		RenderChildren(toggle: false, 3);
	}

	public void Tick()
	{
		MiniMatrixes[0].Tick();
	}

	public void ParallaxOut(bool newIsZoomedFarOut)
	{
		CancelCoroutine(parallaxInFar);
		isZoomedFarOut = newIsZoomedFarOut;
		if (isZoomedFarOut)
		{
			ShowArray();
			gears[0].TriggerAnim("parallaxOutFar", GetSpeed());
		}
		else
		{
			gears[0].TriggerAnim("parallaxOut", GetSpeed());
		}
	}

	public void ParallaxIn(float delta, bool newIsZoomedFarOut)
	{
		CancelCoroutine(parallaxInFar);
		parallaxInFar = StartCoroutine(ParallaxingIn(delta, newIsZoomedFarOut));
	}

	private IEnumerator ParallaxingIn(float delta, bool newIsZoomedFarOut)
	{
		isZoomedFarOut = newIsZoomedFarOut;
		if (isZoomedFarOut)
		{
			ShowArray();
			gears[0].TriggerAnim("parallaxInFar", GetSpeed());
			float checkpoint = Technician.mgr.GetDspTime() + MusicBox.env.GetSecsPerBeat() * 31.5f - delta;
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
			HideArray();
			isZoomedFarOut = false;
		}
		else
		{
			HideArray();
			gears[0].TriggerAnim("parallaxIn", GetSpeed());
		}
	}

	public void ToggleIsZoomedOutFar(bool toggle, bool isTrailerMode = false)
	{
		CancelCoroutine(parallaxInFar);
		isZoomedFarOut = toggle;
		if (isTrailerMode)
		{
			env.gears[0].TriggerAnim("parallaxInFar", env.GetSpeed() * 2f, 0.53f);
		}
		else if (isZoomedFarOut)
		{
			ShowArray();
			gears[0].TriggerAnim("parallaxOutFar", 1f, 1f);
		}
		else
		{
			HideArray();
			gears[0].TriggerAnim("parallaxIn", GetSpeed());
		}
	}

	public void ShowArray()
	{
		for (int i = 1; i < env.MiniMatrixes.Length - 1; i++)
		{
			if (!MiniMatrixes[i].CheckIsActivated())
			{
				MiniMatrixes[i].Show();
			}
		}
	}

	public void HideArray()
	{
		for (int i = 1; i < env.MiniMatrixes.Length - 1; i++)
		{
			if (MiniMatrixes[i].CheckIsActivated())
			{
				MiniMatrixes[i].Hide();
			}
		}
	}

	public void Bobble(float duration)
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.Bobble(duration);
			}
		}
	}

	public void ThrowDelayed(float timeStarted, bool isExtended)
	{
		CancelCoroutine(cueing);
		cueing = StartCoroutine(ThrowingDelayed(timeStarted, isExtended));
	}

	private IEnumerator ThrowingDelayed(float timeStarted, bool isExtended)
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.Reset();
			}
		}
		float beatDuration = MusicBox.env.GetSecsPerBeat();
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Breeze();
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix2 in miniMatrixes)
		{
			if (miniMatrix2.CheckIsActivated())
			{
				miniMatrix2.TimeWrappers[0].Activate("throw1");
				miniMatrix2.Portals[1].Activate(1, newIsWarm: true);
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix3 in miniMatrixes)
		{
			if (miniMatrix3.CheckIsActivated())
			{
				miniMatrix3.Portals[0].Activate(2, newIsWarm: true);
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix4 in miniMatrixes)
		{
			if (miniMatrix4.CheckIsActivated())
			{
				miniMatrix4.Portals[0].Deactivate();
				miniMatrix4.Portals[1].Deactivate();
			}
		}
		checkpoint += beatDuration * 1f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Breeze();
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix5 in miniMatrixes)
		{
			if (miniMatrix5.CheckIsActivated())
			{
				miniMatrix5.TimeWrappers[2].Activate("throw2");
				miniMatrix5.Portals[1].Activate(3, newIsWarm: false);
			}
		}
		if (isExtended)
		{
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			miniMatrixes = MiniMatrixes;
			foreach (MiniMatrix miniMatrix6 in miniMatrixes)
			{
				if (miniMatrix6.CheckIsActivated())
				{
					miniMatrix6.TimeWrappers[2].Activate("throw2");
				}
			}
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			miniMatrixes = MiniMatrixes;
			foreach (MiniMatrix miniMatrix7 in miniMatrixes)
			{
				if (miniMatrix7.CheckIsActivated())
				{
					miniMatrix7.TimeWrappers[2].Activate("throw2");
				}
			}
		}
		checkpoint += beatDuration;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix8 in miniMatrixes)
		{
			if (miniMatrix8.CheckIsActivated())
			{
				miniMatrix8.Portals[1].Deactivate();
			}
		}
	}

	public void TossDelayed(float timeStarted, bool isExtended)
	{
		CancelCoroutine(cueing);
		cueing = StartCoroutine(TossingDelayed(timeStarted, isExtended));
	}

	private IEnumerator TossingDelayed(float timeStarted, bool isExtended)
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.Reset();
			}
		}
		float beatDuration = MusicBox.env.GetSecsPerBeat();
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Breeze();
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix2 in miniMatrixes)
		{
			if (miniMatrix2.CheckIsActivated())
			{
				miniMatrix2.TimeWrappers[0].Activate("toss1");
				miniMatrix2.Portals[1].Activate(7, newIsWarm: true);
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix3 in miniMatrixes)
		{
			if (miniMatrix3.CheckIsActivated())
			{
				miniMatrix3.Portals[0].Activate(0, newIsWarm: true);
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix4 in miniMatrixes)
		{
			if (miniMatrix4.CheckIsActivated())
			{
				miniMatrix4.Portals[0].Deactivate();
				miniMatrix4.Portals[1].Deactivate();
			}
		}
		checkpoint += beatDuration * 1f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Breeze();
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix5 in miniMatrixes)
		{
			if (miniMatrix5.CheckIsActivated())
			{
				miniMatrix5.TimeWrappers[2].Activate("toss2");
				miniMatrix5.Portals[1].Activate(6, newIsWarm: false);
			}
		}
		if (isExtended)
		{
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			miniMatrixes = MiniMatrixes;
			foreach (MiniMatrix miniMatrix6 in miniMatrixes)
			{
				if (miniMatrix6.CheckIsActivated())
				{
					miniMatrix6.TimeWrappers[2].Activate("toss2");
				}
			}
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			miniMatrixes = MiniMatrixes;
			foreach (MiniMatrix miniMatrix7 in miniMatrixes)
			{
				if (miniMatrix7.CheckIsActivated())
				{
					miniMatrix7.TimeWrappers[2].Activate("toss2");
				}
			}
		}
		checkpoint += beatDuration;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix8 in miniMatrixes)
		{
			if (miniMatrix8.CheckIsActivated())
			{
				miniMatrix8.Portals[1].Deactivate();
			}
		}
	}

	public void ThrowTossSoundDelayed(float timeStarted, bool isExtended)
	{
		CancelCoroutine(cueingSound);
		cueingSound = StartCoroutine(ThrowingTossingSoundDelayed(timeStarted, isExtended));
	}

	private IEnumerator ThrowingTossingSoundDelayed(float timeStarted, bool isExtended)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		float beatDuration = MusicBox.env.GetSecsPerBeat();
		float checkpoint = timeStarted + beatDuration;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		checkpoint += beatDuration;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		if (isExtended)
		{
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 1);
			checkpoint += beatDuration * 2f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		}
	}

	public void ShootDelayed(float timeStarted)
	{
		CancelCoroutine(cueing);
		cueing = StartCoroutine(ShootingDelayed(timeStarted));
	}

	private IEnumerator ShootingDelayed(float timeStarted)
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.Reset();
			}
		}
		float beatDuration = MusicBox.env.GetSecsPerBeat();
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Breeze();
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix2 in miniMatrixes)
		{
			if (miniMatrix2.CheckIsActivated())
			{
				miniMatrix2.TimeWrappers[0].Activate("shoot2");
				miniMatrix2.Portals[0].Activate(4, newIsWarm: true);
				miniMatrix2.Portals[1].Activate(5, newIsWarm: true);
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix3 in miniMatrixes)
		{
			if (miniMatrix3.CheckIsActivated())
			{
				miniMatrix3.TimeWrappers[1].Activate("shoot2");
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix4 in miniMatrixes)
		{
			if (miniMatrix4.CheckIsActivated())
			{
				miniMatrix4.TimeWrappers[0].Activate("shoot2");
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix5 in miniMatrixes)
		{
			if (miniMatrix5.CheckIsActivated())
			{
				miniMatrix5.TimeWrappers[1].Activate("shoot2");
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix6 in miniMatrixes)
		{
			if (miniMatrix6.CheckIsActivated())
			{
				miniMatrix6.TimeWrappers[0].Activate("shoot2");
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix7 in miniMatrixes)
		{
			if (miniMatrix7.CheckIsActivated())
			{
				miniMatrix7.Portals[0].Deactivate();
				miniMatrix7.Portals[1].ToggleIsWarm(newIsWarm: false);
				miniMatrix7.TimeWrappers[2].Activate("shoot3");
			}
		}
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix8 in miniMatrixes)
		{
			if (miniMatrix8.CheckIsActivated())
			{
				miniMatrix8.Portals[1].Deactivate();
			}
		}
	}

	public void ShootSoundDelayed(float timeStarted)
	{
		CancelCoroutine(cueingSound);
		cueingSound = StartCoroutine(ShootingSoundDelayed(timeStarted));
	}

	private IEnumerator ShootingSoundDelayed(float timeStarted)
	{
		speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		float beatDuration = MusicBox.env.GetSecsPerBeat();
		float checkpoint = timeStarted + beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 3);
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 4);
		checkpoint += beatDuration * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 5);
		checkpoint += beatDuration * 0.5f;
	}

	public void Press()
	{
		speakers[2].TriggerSound(0);
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.WindUp();
			}
		}
	}

	public void Release()
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.Swing();
			}
		}
	}

	public void IdleMcSwingers()
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.Idle();
			}
		}
	}

	public void ResetMcSwingers()
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.Idle();
			}
		}
	}

	public void Strike()
	{
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.McSwinger.Sweat.CrossIn();
			}
		}
	}

	public void Hit()
	{
		speakers[2].TriggerSound(1);
		speakers[2].TriggerSound(Random.Range(2, 5));
		MiniMatrix[] miniMatrixes = MiniMatrixes;
		foreach (MiniMatrix miniMatrix in miniMatrixes)
		{
			if (miniMatrix.CheckIsActivated())
			{
				miniMatrix.TimeWrappers[2].Fly();
			}
		}
	}

	public void CancelAllSounds()
	{
		speakers[0].CancelAllSounds();
		speakers[1].CancelAllSounds();
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsZoomedFarOut()
	{
		return isZoomedFarOut;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 80f;
	}
}
