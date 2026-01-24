using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamWorld : Wrapper
{
	public static DreamWorld env;

	[Header("Children")]
	public Fader Fader;

	public DialogBox DialogBox;

	private List<Feedback> Feedbacks = new List<Feedback>();

	private float initZ;

	private int activeFeedback;

	private Coroutine transitioning;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		initZ = GetZ();
		SetupFragments();
	}

	public void Show(int state)
	{
		if (state == 0)
		{
			TransitionFromChapter();
		}
		else
		{
			TransitionAsTutorial();
		}
	}

	public void TransitionToChapter()
	{
		CancelCoroutine(transitioning);
		transitioning = StartCoroutine(TransitioningToChapter());
	}

	private IEnumerator TransitioningToChapter()
	{
		gears[0].TriggerAnim("dreamOut");
		Fader.SetParentAndReposition(Interface.env.Cam.GetOuterTransform());
		Fader.SetLocalZ(16f);
		yield return new WaitForSeconds(0.617f);
		Fader.SetColor(1);
		Fader.SetSpeed(10f);
		Fader.Activate();
	}

	public void TransitionFromChapter()
	{
		CancelCoroutine(transitioning);
		gears[0].TriggerAnim("dreamIn");
		Fader.Show();
		Fader.SetColor(1);
		Fader.SetSpeed(10f);
		Fader.Deactivate();
	}

	public void TransitionAsTutorial()
	{
		CancelCoroutine(transitioning);
		gears[0].TriggerAnim("awaiting");
		Fader.SetColor(0);
		Fader.SetSpeed(1f);
		Fader.Show();
		Fader.Deactivate();
	}

	public void PlayTransitionSound()
	{
		speakers[0].TriggerSound(0);
		speakers[0].SetParent(null);
		speakers[0].DestroyAfterSound(0);
		Object.DontDestroyOnLoad(speakers[0]);
	}

	public void RecenterZoomer(Wrapper target)
	{
		target.SetParent(null);
		float x = Interface.env.Cam.GetX();
		float y = Interface.env.Cam.GetY();
		SetPosition(x, y);
		target.SetParent(gears[0].transform);
	}

	public void ZoomBobble(int direction, float multiplier)
	{
		gears[0].TriggerAnim("zoomBobble" + direction, GetSpeed() / 4f * multiplier);
	}

	public void Await()
	{
		gears[0].TriggerAnim("awaiting");
	}

	public void SetFeedbacks(Feedback[] Feedbacks_new)
	{
		ClearFeedbacks();
		foreach (Feedback item in Feedbacks_new)
		{
			Feedbacks.Add(item);
		}
	}

	public void IncreaseActiveFeedback()
	{
		activeFeedback = ((activeFeedback + 1 < Feedbacks.Count) ? (activeFeedback + 1) : 0);
	}

	public void ClearFeedbacks()
	{
		activeFeedback = 0;
		Feedbacks.Clear();
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}

	public bool CheckIsThereFeedbacks()
	{
		if (Feedbacks.Count <= 0)
		{
			return false;
		}
		return true;
	}

	public Feedback GetActiveFeedback()
	{
		if (Feedbacks.Count > 0)
		{
			return Feedbacks[activeFeedback];
		}
		return null;
	}
}
