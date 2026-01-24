using System.Collections;
using UnityEngine;

public class Platform : Wrapper
{
	[Header("Children")]
	public Number Number;

	[Header("Fragments")]
	public Fragment shadow;

	public Fragment feedbacker;

	public spriteFragment followFeedback;

	[Header("Props")]
	public int type;

	private float buzzDuration;

	private Coroutine buzzing;

	protected override void Awake()
	{
		SetupFragments();
		shadow.Awake();
		feedbacker.Awake();
		followFeedback.Initiate();
		buzzDuration = gears[0].GetAnimDuration("buzz");
	}

	public void Show()
	{
		gears[0].TriggerAnim("shown");
		sprites[0].TriggerAnim(type.ToString() ?? "");
		sprites[1].TriggerAnim(type + "_" + Random.Range(0, 2));
		sprites[2].SetSpriteColor(new Color(1f, 0.952f, 0.988f));
		shadow.ToggleSpriteRenderer(toggle: false);
		shadow.ToggleAnimator(toggle: false);
		feedbacker.TriggerAnim("hidden");
		Number.Hide();
	}

	public void Hide()
	{
		gears[0].TriggerAnim("hidden");
		Number.Hide();
	}

	public void Bobble()
	{
		gears[1].TriggerAnim("bobble", InfluencerLand.env.GetSpeed());
	}

	public void Buzz(float timeStarted)
	{
		CancelCoroutine(buzzing);
		buzzing = StartCoroutine(Buzzing(timeStarted));
	}

	private IEnumerator Buzzing(float timeStarted)
	{
		gears[0].TriggerAnim("buzz", InfluencerLand.env.GetSpeed());
		sprites[0].TriggerAnim(type + "on");
		sprites[2].SetSpriteColor(new Color(0.866f, 1f, 0.97f));
		yield return new WaitForSeconds(buzzDuration / InfluencerLand.env.GetSpeed());
		sprites[0].TriggerAnim(type.ToString() ?? "");
		sprites[2].SetSpriteColor(new Color(1f, 0.952f, 0.988f));
	}

	public void AwaitSpring()
	{
		gears[1].TriggerAnim("dipped");
		sprites[0].TriggerAnim(type + "on");
		sprites[2].SetSpriteColor(new Color(0.866f, 1f, 0.97f));
		shadow.ToggleSpriteRenderer(toggle: true);
		shadow.ToggleAnimator(toggle: true);
		shadow.TriggerAnim("idled");
	}

	public void Dip(float timeStarted, bool isShort)
	{
		StartCoroutine(Dipping(timeStarted, isShort));
	}

	private IEnumerator Dipping(float timeStarted, bool isShort)
	{
		if (isShort)
		{
			float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() / 4f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			shadow.ToggleSpriteRenderer(toggle: true);
			shadow.ToggleAnimator(toggle: true);
			shadow.TriggerAnim("in", 2f * InfluencerLand.env.GetSpeed());
			float num = gears[1].GetAnimDuration("dip") / 2f / InfluencerLand.env.GetSpeed();
			checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() / 2f - num;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			gears[1].TriggerAnim("dip", 2f * InfluencerLand.env.GetSpeed());
		}
		else
		{
			shadow.ToggleSpriteRenderer(toggle: true);
			shadow.ToggleAnimator(toggle: true);
			shadow.TriggerAnim("in", InfluencerLand.env.GetSpeed());
			float num2 = gears[1].GetAnimDuration("dip") / InfluencerLand.env.GetSpeed();
			float checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() / 2f - num2;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint2);
			gears[1].TriggerAnim("dip", InfluencerLand.env.GetSpeed());
		}
	}

	public void Throw(int throwNum, bool isShort = false)
	{
		int num = ((!isShort) ? 1 : 2);
		shadow.TriggerAnim("out", (float)num * InfluencerLand.env.GetSpeed());
		gears[1].TriggerAnim("throw" + throwNum, InfluencerLand.env.GetSpeed() * (float)num);
	}

	public void LightUp(bool isFeedbackShown, float accuracy = 1f)
	{
		if (isFeedbackShown)
		{
			feedbacker.TriggerAnim("in");
		}
		if (accuracy == 1f)
		{
			sprites[0].TriggerAnim(type + "on");
			sprites[2].SetSpriteColor(new Color(0.866f, 1f, 0.97f));
			followFeedback.SetState(SaveManager.GetLang());
		}
		else if (accuracy == 0.332f)
		{
			sprites[2].SetSpriteColor(new Color(1f, 0.987f, 0.937f));
			followFeedback.SetState(SaveManager.GetLang() + 10);
		}
		else
		{
			sprites[2].SetSpriteColor(new Color(1f, 0.9f, 0.927f));
			followFeedback.SetState(SaveManager.GetLang() + 20);
		}
	}
}
