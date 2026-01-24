using System.Collections;
using UnityEngine;

public class Underworld : Wrapper
{
	public static Underworld env;

	[Header("Children")]
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	public McClimber McClimber;

	public LadderGroup LadderGroup;

	public LavaPool LavaPool;

	[Header("Fragments")]
	public layer[] layers;

	private bool isActivated;

	private int climbDirection = 1;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		layer[] array = layers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		SetupFragments();
		RenderChildren(toggle: false, 2);
	}

	public void Show(bool isCuedUp)
	{
		isActivated = true;
		RenderChildren(toggle: true, 2);
		if (isCuedUp)
		{
			Interface.env.Cam.SetPosition(0f, 0.5f);
		}
		else
		{
			Interface.env.Cam.SetPosition(0f, 0f);
		}
		McClimber.Show(isCuedUp);
		LadderGroup.Show();
		LavaPool.Show();
		DreamWorld.env.SetFeedbacks(Feedbacks);
		layer[] array = layers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Update();
		}
	}

	public void Hide()
	{
		isActivated = false;
		McClimber.Hide();
		LadderGroup.Hide();
		LavaPool.Hide();
		Interface.env.Cam.SetPosition(0f, 0f);
		RenderChildren(toggle: false, 2);
	}

	public void CueLeftDelayed(float timeStarted)
	{
		Interface.env.Cam.DelayBreeze(timeStarted);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		LadderGroup.HintLeftDelayed(timeStarted);
		McClimber.Prep(1);
		LavaPool.DipDelayed(timeStarted);
	}

	public void CueRightDelayed(float timeStarted)
	{
		Interface.env.Cam.DelayBreeze(timeStarted);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		LadderGroup.HintRightDelayed(timeStarted);
		McClimber.Prep(2);
		LavaPool.DipDelayed(timeStarted);
	}

	public void CueCenterDelayed(float timeStarted)
	{
		Interface.env.Cam.DelayShake(timeStarted);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 2);
		McClimber.Prep(3);
		LavaPool.DipLowDelayed(timeStarted);
	}

	public void Climb(float distanceY)
	{
		McClimber.Climb(distanceY);
		climbDirection++;
		if (climbDirection > 2)
		{
			climbDirection = 1;
		}
	}

	public void BurnMc()
	{
		StartCoroutine(BurningMc());
	}

	private IEnumerator BurningMc()
	{
		speakers[1].TriggerSound(3);
		sprites[0].SetPosition(McClimber.GetButtPosition().x, McClimber.GetButtPosition().y);
		sprites[0].TriggerAnim("puff");
		yield return new WaitForSeconds(0.015f);
		sprites[1].SetPosition(McClimber.GetButtPosition().x, McClimber.GetButtPosition().y);
		sprites[1].TriggerAnim("puff");
		yield return new WaitForSeconds(0.0225f);
		sprites[2].SetPosition(McClimber.GetButtPosition().x, McClimber.GetButtPosition().y);
		sprites[2].TriggerAnim("puff");
		yield return new WaitForSeconds(0.0275f);
		sprites[3].SetPosition(McClimber.GetButtPosition().x, McClimber.GetButtPosition().y);
		sprites[3].TriggerAnim("puff");
		yield return new WaitForSeconds(0.03f);
		sprites[4].SetPosition(McClimber.GetButtPosition().x, McClimber.GetButtPosition().y);
		sprites[4].TriggerAnim("puff");
	}

	public void PlayJump(int direction)
	{
		speakers[1].TriggerSound(direction);
	}

	public void Exit()
	{
		layer[] array = layers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disable();
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}
}
