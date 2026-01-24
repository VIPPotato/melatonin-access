using System.Collections;
using UnityEngine;

public class ScoreBubble : Wrapper
{
	[Header("Fragments")]
	public Fragment activator;

	public Fragment starDisplay;

	public Fragment ringDisplay;

	private bool isEnabled;

	private int starScore;

	private int ringScore;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		activator.Awake();
		starDisplay.Awake();
		ringDisplay.Awake();
		timeTilOut = activator.GetAnimDuration("fadeOut");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		activator.TriggerAnim("fadeIn");
		starDisplay.SetLocalY(0.733f);
		starDisplay.TriggerAnim(starScore.ToString() ?? "");
		ringDisplay.ToggleSpriteRenderer(toggle: true);
		ringDisplay.TriggerAnim(ringScore.ToString() ?? "");
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		activator.TriggerAnim("fadeOut");
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		isEnabled = true;
		activator.TriggerAnim("fadedIn");
		starDisplay.SetLocalY(0.733f);
		starDisplay.TriggerAnim(starScore.ToString() ?? "");
		ringDisplay.ToggleSpriteRenderer(toggle: true);
		ringDisplay.TriggerAnim(ringScore.ToString() ?? "");
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		if (Map.env.Neighbourhood.McMap.GetColliderPoint() < GetY() - 0.67f)
		{
			if (GetLocalZ() != 0f)
			{
				SetLocalZ(0f);
			}
		}
		else
		{
			SetLocalZ(-29f);
		}
	}

	public void SetNumStars(int value)
	{
		starScore = value;
	}

	public void SetNumRings(int value)
	{
		ringScore = value;
	}
}
