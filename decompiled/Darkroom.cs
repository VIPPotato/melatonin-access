using System.Collections;
using UnityEngine;

public class Darkroom : Wrapper
{
	public static Darkroom env;

	[Header("Children")]
	public ClotheslinePulley ClotheslinePulley;

	public McLighter McLighter;

	public PhotoPulley PhotoPulley;

	private bool isActivated;

	private bool isPlayable;

	private int cueSpeakerNum;

	private const float animTempo = 80f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 2);
	}

	public void Show()
	{
		isPlayable = false;
		isActivated = true;
		RenderChildren(toggle: true, 2);
		PhotoPulley.Show();
		McLighter.Activate();
	}

	public void MakePlayable()
	{
		isPlayable = true;
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.SetFeedbacks(McLighter.Feedbacks);
	}

	public void Hide()
	{
		isPlayable = false;
		isActivated = false;
		PhotoPulley.Hide();
		RenderChildren(toggle: false, 2);
	}

	public void SwapClotheslineDelayed(float timeStarted)
	{
		StartCoroutine(SwappingClotheslineDelayed(timeStarted));
	}

	private IEnumerator SwappingClotheslineDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		ClotheslinePulley.Swap();
		PhotoPulley.Out();
	}

	public void PlayPrepCueDelayed(float timeStarted, int cueNum)
	{
		cueSpeakerNum++;
		if (cueSpeakerNum > 1)
		{
			cueSpeakerNum = 0;
		}
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, cueNum - 1);
	}

	public void PlayBurnFeedback()
	{
		speakers[0].TriggerSound(3);
	}

	public bool CheckIsPlayable()
	{
		return isPlayable;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 80f;
	}
}
