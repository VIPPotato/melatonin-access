using System.Collections;
using UnityEngine;

public class PizzaBox : Wrapper
{
	[Header("Children")]
	public Wings Wings;

	[Header("Fragments")]
	public Fragment flyer;

	public Fragment hoverer;

	public Fragment repositioner;

	public Fragment box;

	private int boxType;

	private bool isActivated;

	private bool isSwapping;

	private bool isFastThrowing;

	private string foodType;

	private Coroutine bobbling;

	private Coroutine throwing;

	private Coroutine throwingSound;

	private Coroutine swapping;

	protected override void Awake()
	{
		flyer.Awake();
		hoverer.Awake();
		repositioner.Awake();
		box.Awake();
		RenderChildren(toggle: false);
	}

	public void Activate(int newBoxType)
	{
		CancelCoroutine(throwing);
		CancelCoroutine(throwingSound);
		CancelCoroutine(swapping);
		RenderChildren(toggle: true);
		isActivated = true;
		boxType = newBoxType;
		foodType = ((boxType == 0) ? "pizza" : ((boxType == 1) ? "burger" : "donut"));
		box.TriggerAnim("idling" + boxType);
		flyer.TriggerAnim("flyIn", FoodySkies.env.GetSpeed());
		hoverer.TriggerAnim("hover", FoodySkies.env.GetSpeed());
		repositioner.TriggerAnim("positioned1");
		Wings.Show();
	}

	public void Show(int newBoxType)
	{
		CancelCoroutine(throwing);
		CancelCoroutine(throwingSound);
		CancelCoroutine(swapping);
		RenderChildren(toggle: true);
		isActivated = true;
		boxType = newBoxType;
		foodType = ((boxType == 0) ? "pizza" : ((boxType == 1) ? "burger" : "donut"));
		box.TriggerAnim("idling" + boxType);
		flyer.TriggerAnim("flyedIn");
		repositioner.TriggerAnim("positioned1");
		Wings.Show();
	}

	public void Hide()
	{
		CancelCoroutine(bobbling);
		CancelCoroutine(throwing);
		CancelCoroutine(throwingSound);
		CancelCoroutine(swapping);
		isActivated = false;
		Wings.Hide();
		RenderChildren(toggle: false);
	}

	public void BobbleDelayed(float delta)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta));
	}

	private IEnumerator BobblingDelayed(float delta)
	{
		Wings.FlapSoundDelayed(delta);
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		hoverer.TriggerAnim("hover", FoodySkies.env.GetSpeed());
		Wings.Flap();
	}

	public void ThrowNormalDelayed(float timeStarted)
	{
		CancelCoroutine(throwing);
		throwing = StartCoroutine(ThrowingNormalDelayed(timeStarted));
	}

	private IEnumerator ThrowingNormalDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("prepA" + boxType);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (!isSwapping)
		{
			flyer.TriggerAnim("flyedIn");
		}
		box.TriggerAnim("tossA" + boxType);
		repositioner.TriggerAnim("positioned2");
		FoodySkies.env.IncreaseActiveFood();
		FoodySkies.env.GetActiveFood().SetLocalY(GetFlyerPositionY());
		FoodySkies.env.GetActiveFood().CrossIn(0, foodType);
		Interface.env.Cam.Breeze();
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("close" + boxType);
		repositioner.TriggerAnim("positioned1");
	}

	public void ThrowSoundNormalDelayed(float timeStarted)
	{
		CancelCoroutine(throwingSound);
		throwingSound = StartCoroutine(ThrowingSoundNormalDelayed(timeStarted));
	}

	private IEnumerator ThrowingSoundNormalDelayed(float timeStarted)
	{
		FoodySkies.env.PlayPeekSfxDelayed(timeStarted, 0);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayLaunchSfxDelayed(checkpoint, 0);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayDropSfxDelayed(checkpoint, 0);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayCloseSfxDelayed(checkpoint);
	}

	public void ThrowSlowDelayed(float timeStarted)
	{
		CancelCoroutine(throwing);
		throwing = StartCoroutine(ThrowingSlowDelayed(timeStarted));
	}

	private IEnumerator ThrowingSlowDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("prepB" + boxType);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (!isSwapping)
		{
			flyer.TriggerAnim("flyedIn");
		}
		box.TriggerAnim("tossC" + boxType);
		repositioner.TriggerAnim("positioned2");
		FoodySkies.env.GetActiveFood().SetLocalY(GetFlyerPositionY());
		FoodySkies.env.GetActiveFood().CrossIn(2, foodType);
		Interface.env.Cam.Breeze();
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("close" + boxType);
		repositioner.TriggerAnim("positioned1");
	}

	public void ThrowSoundSlowDelayed(float timeStarted)
	{
		CancelCoroutine(throwingSound);
		throwingSound = StartCoroutine(ThrowingwSoundSlowDelayed(timeStarted));
	}

	private IEnumerator ThrowingwSoundSlowDelayed(float timeStarted)
	{
		FoodySkies.env.PlayPeekSfxDelayed(timeStarted, 1);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayLaunchSfxDelayed(checkpoint, 1);
		FoodySkies.env.PlayLaunchReverseSfxDelayed(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayDropSfxDelayed(checkpoint, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayCloseSfxDelayed(checkpoint);
	}

	public void ThrowFast(float timeStarted, float delayMultiplier)
	{
		CancelCoroutine(throwing);
		throwing = StartCoroutine(ThrowingFast(timeStarted, delayMultiplier));
	}

	private IEnumerator ThrowingFast(float timeStarted, float delayMultiplier)
	{
		isFastThrowing = true;
		box.TriggerAnim("idling" + boxType);
		if (!isSwapping)
		{
			flyer.TriggerAnim("approach1");
		}
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * delayMultiplier;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (!isSwapping)
		{
			flyer.TriggerAnim("approach2");
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * delayMultiplier;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (!isSwapping)
		{
			flyer.TriggerAnim("approach3");
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * delayMultiplier + 0.067f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("tossB" + boxType);
		if (!isSwapping)
		{
			flyer.TriggerAnim("backup");
		}
		repositioner.TriggerAnim("positioned2");
		if (isFastThrowing)
		{
			FoodySkies.env.GetActiveFood().SetLocalY(GetFlyerPositionY());
			FoodySkies.env.GetActiveFood().CrossIn(1, foodType);
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 0.5f + 0.04667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		box.TriggerAnim("close" + boxType);
		repositioner.TriggerAnim("positioned1");
	}

	public void ThrowSoundFast(float timeStarted, float delayMultiplier)
	{
		CancelCoroutine(throwingSound);
		throwingSound = StartCoroutine(ThrowingSoundFast(timeStarted, delayMultiplier));
	}

	private IEnumerator ThrowingSoundFast(float timeStarted, float delayMultiplier)
	{
		FoodySkies.env.PlayApproachSfxDelayed(timeStarted, 0);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * delayMultiplier;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayApproachSfxDelayed(checkpoint, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * delayMultiplier;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayApproachSfxDelayed(checkpoint, 2);
		checkpoint += MusicBox.env.GetSecsPerBeat() * delayMultiplier;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayLaunchSfxDelayed(checkpoint, 2);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FoodySkies.env.PlayCloseSfxDelayed(checkpoint);
	}

	public void Swap(float timeStarted, int newBoxType)
	{
		CancelCoroutine(swapping);
		swapping = StartCoroutine(Swapping(timeStarted, newBoxType));
	}

	private IEnumerator Swapping(float timeStarted, int newBoxType)
	{
		isSwapping = true;
		float speed = FoodySkies.env.GetSpeed();
		flyer.TriggerAnim("flyOut", speed);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		boxType = newBoxType;
		foodType = ((boxType == 0) ? "pizza" : ((boxType == 1) ? "burger" : "donut"));
		box.TriggerAnim("idling" + boxType);
		flyer.TriggerAnim("flyIn", speed);
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isSwapping = false;
	}

	private float GetFlyerPositionY()
	{
		return flyer.transform.position.y;
	}

	public void ToggleIsFastThrowing(bool toggle)
	{
		isFastThrowing = toggle;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
