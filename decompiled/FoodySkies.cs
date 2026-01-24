using System.Collections;
using UnityEngine;

public class FoodySkies : Wrapper
{
	public static FoodySkies env;

	[Header("Children")]
	public Crumbs Crumbs;

	public PizzaBox PizzaBox;

	public McChomper McChomper;

	public Cloud[] Clouds;

	public Food[] Foods;

	public Waters Waters;

	[Header("Fragments")]
	public Fragment zoomer;

	private bool isActivated;

	private int activePizzaBoxNum;

	private int activeFoodNum;

	private Coroutine hitting;

	private Coroutine swapingPizzaBox;

	private Coroutine blinding;

	private const float animTempo = 100f;

	protected override void Awake()
	{
		env = this;
		zoomer.Awake();
		SetupFragments();
		RenderChildren(toggle: false, 6);
	}

	public void Show()
	{
		isActivated = true;
		activeFoodNum = 0;
		RenderChildren(toggle: true, 6);
		zoomer.TriggerAnim("awaiting");
		McChomper.Show();
		McChomper.BobbleDelayed(0f);
		PizzaBox.Show(Random.Range(0, 3));
		PizzaBox.BobbleDelayed(0f);
		Clouds[0].Hide();
		Clouds[1].Hide();
		Clouds[2].Hide();
		Clouds[3].Show();
		Clouds[4].Show();
		Clouds[5].Show();
		Waters.Show();
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.SetFeedbacks(McChomper.Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		Waters.Hide();
		McChomper.Hide();
		PizzaBox.Hide();
		CancelCoroutine(hitting);
		CancelCoroutine(swapingPizzaBox);
		CancelCoroutine(blinding);
		Clouds[0].SetDriftMultiplier(0f);
		Clouds[1].SetDriftMultiplier(0f);
		Clouds[2].SetDriftMultiplier(0f);
		RenderChildren(toggle: false, 6);
	}

	public void Blind()
	{
		CancelCoroutine(blinding);
		blinding = StartCoroutine(Blinding());
	}

	private IEnumerator Blinding()
	{
		zoomer.TriggerAnim("zoomOut");
		DreamWorld.env.GetActiveFeedback().SetLocalZ(-25f);
		Clouds[0].Show();
		Clouds[1].Show();
		Clouds[2].Show();
		Clouds[0].SetLocalX(23.5f);
		Clouds[1].SetLocalX(26f);
		Clouds[2].SetLocalX(29.25f);
		Clouds[0].MoveDistance(new Vector3(-23f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[1].MoveDistance(new Vector3(-23f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[2].MoveDistance(new Vector3(-23f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(1f * GetSpeed());
		Clouds[0].CancelMoving();
		Clouds[0].SetDriftMultiplier(-0.25f);
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[1].CancelMoving();
		Clouds[1].SetDriftMultiplier(-0.2f);
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[2].CancelMoving();
		Clouds[2].SetDriftMultiplier(-0.1f);
	}

	public void Unblind()
	{
		CancelCoroutine(blinding);
		blinding = StartCoroutine(Unblinding());
	}

	private IEnumerator Unblinding()
	{
		zoomer.TriggerAnim("reset");
		DreamWorld.env.GetActiveFeedback().SetLocalZ(0f);
		Clouds[0].SetDriftMultiplier(0f);
		Clouds[0].MoveDistance(new Vector3(-23f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[1].SetDriftMultiplier(0f);
		Clouds[1].MoveDistance(new Vector3(-29f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(0.15f * GetSpeed());
		Clouds[2].SetDriftMultiplier(0f);
		Clouds[2].MoveDistance(new Vector3(-23f, 0f, 0f), 1.5f * GetSpeed());
		yield return new WaitForSeconds(2f * GetSpeed());
		Clouds[0].Hide();
		Clouds[1].Hide();
		Clouds[2].Hide();
	}

	public void ZoomEnd()
	{
		zoomer.TriggerAnim("end");
	}

	public void PlayPeekSfxDelayed(float timeStarted, int num)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, num);
	}

	public void PlayLaunchSfxDelayed(float timeStarted, int num)
	{
		speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, num);
	}

	public void PlayLaunchReverseSfxDelayed(float timeStarted)
	{
		speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, 3);
		speakers[1].SetSoundPitch(3, GetSpeed());
	}

	public void PlayCloseSfxDelayed(float timeStarted)
	{
		speakers[2].TriggerSoundDelayedTimeStarted(timeStarted, 0);
	}

	public void PlayApproachSfxDelayed(float timeStarted, int num)
	{
		speakers[3].TriggerSoundDelayedTimeStarted(timeStarted, num);
	}

	public void PlayDropSfxDelayed(float timeStarted, int num)
	{
		speakers[4].TriggerSoundDelayedTimeStarted(timeStarted, num);
	}

	public void Hit(float timeStarted)
	{
		CancelCoroutine(hitting);
		hitting = StartCoroutine(Hitting(timeStarted));
	}

	private IEnumerator Hitting(float timeStarted)
	{
		speakers[5].TriggerSound(0);
		McChomper.Chomp();
		Crumbs.CrossIn();
		GetActiveFood().Hide();
		PizzaBox.ToggleIsFastThrowing(toggle: false);
		Interface.env.Cam.Sway();
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[5].TriggerSound(2);
		McChomper.Swallow();
	}

	public void PlayMissFeedbackSfx()
	{
		speakers[5].TriggerSound(1);
	}

	public void CancelAllSounds()
	{
		speakers[0].CancelAllSounds();
		speakers[1].CancelAllSounds();
		speakers[2].CancelAllSounds();
		speakers[3].CancelAllSounds();
		speakers[4].CancelAllSounds();
	}

	public void IncreaseActiveFood()
	{
		activeFoodNum++;
		if (activeFoodNum >= 3)
		{
			activeFoodNum = 0;
		}
	}

	public PizzaBox GetActivePizzaBox()
	{
		return PizzaBox;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetAnimTempo()
	{
		return 100f;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 100f;
	}

	public Food GetActiveFood()
	{
		return Foods[activeFoodNum];
	}
}
