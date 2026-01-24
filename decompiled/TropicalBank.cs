using System.Collections;
using UnityEngine;

public class TropicalBank : Wrapper
{
	public static TropicalBank env;

	[Header("Children")]
	public McCatcher McCatcher;

	public MoneyCloud MoneyCloud;

	public Cloud[] Clouds;

	public FlyArea FlyArea;

	public FlyingPig[] FlyingPigs;

	public BankSky BankSky;

	public Vaults Vaults;

	[Header("Fragments")]
	public Fragment[] billDroppers;

	public Fragment[] cranks;

	public Fragment[] rains;

	public Fragment[] rainSplashes;

	public Fragment flash;

	public Fragment lightning;

	private int billNum;

	private int timesBobbled;

	private bool isActivated;

	private Coroutine bobbling;

	private const float animTempo = 100f;

	protected override void Awake()
	{
		env = this;
		billDroppers[0].Awake();
		billDroppers[1].Awake();
		cranks[0].Awake();
		cranks[1].Awake();
		rains[0].Awake();
		rains[1].Awake();
		flash.Awake();
		lightning.Awake();
		Fragment[] array = rainSplashes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.SetFeedbacks(McCatcher.Feedbacks);
		McCatcher.Show();
		MoneyCloud.Show();
		rains[2].TriggerAnim("backgrounding");
		rains[3].TriggerAnim("backgrounding");
		Cloud[] clouds = Clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].Show();
		}
		FlyingPigs[0].Show();
		FlyingPigs[1].Show();
		FlyingPigs[0].SetRight();
		FlyingPigs[1].SetLeft();
	}

	public void Hide()
	{
		timesBobbled = 0;
		isActivated = false;
		MoneyCloud.Hide();
		McCatcher.Hide();
		CancelCoroutine(bobbling);
		RenderChildren(toggle: false);
	}

	public void BobbleDelayed(float delta, int beat, bool isCameraMoving)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta, beat, isCameraMoving));
	}

	private IEnumerator BobblingDelayed(float delta, int beat, bool isCameraMoving)
	{
		speakers[0].TriggerSoundDelayedDelta(delta, 0);
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		switch (beat)
		{
		case 1:
			DropBill1();
			FlyingPigs[1].TravelLeft();
			break;
		case 3:
			FlyingPigs[1].TravelLeft();
			DropBill2();
			break;
		case 4:
			cranks[0].TriggerAnim("crank");
			cranks[1].TriggerAnim("crank");
			break;
		}
		Splash();
		McCatcher.MoveEyes();
		FlyingPigs[0].TravelRight();
		if (isCameraMoving)
		{
			timesBobbled++;
			if (timesBobbled == 1)
			{
				Interface.env.Cam.MoveToTarget(new Vector3(0f, 2.5f, 0f), 0.05f, isEasingIn: false);
			}
			else if (timesBobbled == 17)
			{
				Interface.env.Cam.MoveToTarget(new Vector3(0f, 0f, 0f), 0.05f, isEasingIn: false);
			}
			else if (timesBobbled == 32)
			{
				timesBobbled = 0;
			}
		}
	}

	public void DropBill1()
	{
		billDroppers[0].SetLocalX(Random.Range(-2f, 2f));
		billDroppers[0].SetY(Interface.env.Cam.GetY() + 7.9f);
		billDroppers[0].TriggerAnim("drop" + billNum, GetSpeed());
		billNum++;
		if (billNum > 1)
		{
			billNum = 0;
		}
	}

	public void DropBill2()
	{
		billDroppers[1].SetLocalX(Random.Range(-2f, 2f));
		billDroppers[1].SetY(Interface.env.Cam.GetY() + 7.9f);
		billDroppers[1].TriggerAnim("drop" + billNum, GetSpeed());
	}

	private void Splash()
	{
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
	}

	public void SplashDelayed(float delta)
	{
		StartCoroutine(SplashingDelayed(delta));
	}

	private IEnumerator SplashingDelayed(float delta)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
		rainSplashes[Random.Range(0, rainSplashes.Length)].TriggerAnim("splash");
	}

	public void ThunderDelayed(float timeStarted, string direction)
	{
		StartCoroutine(ThunderingDelayed(timeStarted, direction));
	}

	private IEnumerator ThunderingDelayed(float timeStarted, string direction)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Shake();
		flash.TriggerAnim("flash");
		lightning.TriggerAnim("strike");
		lightning.SetLocalScale(lightning.GetLocalScale().x, Random.Range(1.1f, 1.45f));
		if (direction == "left")
		{
			lightning.SetLocalX(Random.Range(4f, 8f));
		}
		else
		{
			lightning.SetLocalX(Random.Range(-4f, -8f));
		}
		if (Random.Range(0, 2) == 0)
		{
			lightning.ToggleSpriteFlip(toggle: true);
		}
		else
		{
			lightning.ToggleSpriteFlip(toggle: false);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		rains[0].TriggerAnim(direction ?? "");
		rains[1].TriggerAnim(direction ?? "");
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 100f;
	}
}
