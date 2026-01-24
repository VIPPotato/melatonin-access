using System.Collections;
using UnityEngine;

public class PingBar : Wrapper
{
	[Header("Fragments")]
	public textboxFragment[] labels;

	public Fragment speaker;

	public Fragment fader;

	public Fragment marker;

	public Fragment windowGear;

	public Fragment flash;

	private bool isActivated;

	private bool isPinging;

	private bool isTimingWindow;

	private float timer;

	private int beat;

	private Coroutine pinging;

	private Coroutine timingWindow;

	protected override void Awake()
	{
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		speaker.Awake();
		fader.Awake();
		marker.Awake();
		windowGear.Awake();
		flash.Awake();
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		isActivated = true;
		isPinging = false;
		isTimingWindow = false;
		timer = 0f;
		RenderChildren(toggle: true, 1);
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetState(0);
		}
		fader.TriggerAnim("in");
		marker.ToggleSpriteRenderer(toggle: false);
		flash.TriggerAnim("hidden");
		RefreshWindow();
	}

	public void Deactivate()
	{
		CancelCoroutine(pinging);
		CancelCoroutine(timingWindow);
		isActivated = false;
		isPinging = false;
		isTimingWindow = false;
		timer = 0f;
		flash.TriggerAnim("hidden");
		fader.TriggerAnim("out");
	}

	public void Hide()
	{
		RenderChildren(toggle: false, 1);
	}

	private void Update()
	{
		if (isActivated && isTimingWindow && ControlHandler.mgr.CheckIsActionPressed() && Time.timeScale > 0f)
		{
			StopTimer();
		}
	}

	public void Ping()
	{
		pinging = StartCoroutine(Pinging());
	}

	private IEnumerator Pinging()
	{
		isPinging = true;
		beat = 0;
		float checkpoint = Technician.mgr.GetDspTime() + 0.75f;
		while (isPinging)
		{
			beat++;
			if (beat > 4)
			{
				beat = 1;
			}
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
			TimeWindow(checkpoint);
			float delta = Technician.mgr.GetDspTime() - checkpoint;
			if (beat == 1)
			{
				speaker.TriggerSoundDelayedDelta(delta, 0);
			}
			else
			{
				speaker.TriggerSoundDelayedDelta(delta, 1);
			}
			checkpoint += 0.11667f;
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
			flash.TriggerAnim("flash");
			checkpoint = checkpoint + 0.75f - 0.11667f;
			yield return null;
		}
	}

	private void TimeWindow(float timeStarted)
	{
		CancelCoroutine(timingWindow);
		timingWindow = StartCoroutine(TimingWindow(timeStarted));
	}

	private IEnumerator TimingWindow(float timeStarted)
	{
		isTimingWindow = true;
		while (timer < 0.23334f)
		{
			timer = Technician.mgr.GetDspTime() - timeStarted;
			yield return null;
		}
		isTimingWindow = false;
		timer = 0f;
	}

	private void StopTimer()
	{
		CancelCoroutine(timingWindow);
		speaker.TriggerSound(2);
		marker.ToggleSpriteRenderer(toggle: true);
		marker.SetLocalX(timer / 0.23334f * 10.292f - 5.146f);
		MonoBehaviour.print(timer - 0.11667f);
		isTimingWindow = false;
		timer = 0f;
	}

	public void RefreshWindow()
	{
		windowGear.SetLocalX((float)SaveManager.mgr.GetCalibrationOffsetMs() * 0.044f);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
