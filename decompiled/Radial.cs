using System.Collections;
using UnityEngine;

public class Radial : Wrapper
{
	[Header("Fragments")]
	public Fragment positionerTransform;

	public Fragment positioner;

	public Fragment prompt;

	public Fragment middle;

	public Fragment[] halfCircles;

	private float distance;

	private float beatsTilHold;

	private float beatsTilRelease;

	private float beatsBetween;

	private float initMiddleHeight;

	private float timeStarted;

	private string transformAnim;

	private bool isHalfAddedToHold;

	private bool isHalfAddedToRelase;

	private bool isHalfDistance;

	private bool isBlind;

	private Coroutine crossingIn;

	private Coroutine deactivating;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		positionerTransform.Awake();
		positioner.Awake();
		prompt.Awake();
		middle.Awake();
		halfCircles[0].Awake();
		halfCircles[1].Awake();
		initMiddleHeight = middle.GetLocalHeight();
		RenderChildren(toggle: false);
	}

	public void CrossInDelayed(float newTimeStarted, float newBeatsTilHold, float newBeatsTilRelease, bool newIsHalfAddedToHold, bool newIsHalfAddedToRelease)
	{
		CancelCoroutine(crossingIn);
		CancelCoroutine(deactivating);
		timeStarted = newTimeStarted;
		beatsTilHold = newBeatsTilHold;
		beatsTilRelease = newBeatsTilRelease;
		isHalfAddedToHold = newIsHalfAddedToHold;
		isHalfAddedToRelase = newIsHalfAddedToRelease;
		RenderChildren(toggle: true);
		Configure();
		crossingIn = StartCoroutine(CrossingInDelayed());
	}

	private IEnumerator CrossingInDelayed()
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Activate();
		Vector3 startLocalPosition = positionerTransform.GetLocalPosition();
		Vector3 endLocalPosition = new Vector3(0f, beatsBetween * distance, GetLocalZ());
		float duration = MusicBox.env.GetSecsPerBeat() * beatsTilRelease;
		while (MusicBox.env.GetSongTime() - checkpoint < duration)
		{
			positionerTransform.transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (MusicBox.env.GetSongTime() - checkpoint) / duration);
			yield return null;
		}
		if (!isBlind)
		{
			Deactivate(0.11667f);
		}
	}

	private void Activate()
	{
		StartCoroutine(Activating());
	}

	private IEnumerator Activating()
	{
		middle.SetSpriteAlpha(0f);
		halfCircles[1].SetSpriteAlpha(0f);
		halfCircles[0].SetSpriteAlpha(0f);
		prompt.SetSpriteAlpha(0f);
		float elapsed = 0f;
		float duration = 0.11667f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			middle.SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			halfCircles[1].SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			halfCircles[0].SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			prompt.SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			yield return null;
		}
		if (isBlind)
		{
			Deactivate(MusicBox.env.GetSecsPerBeat() * beatsTilRelease / 2f - duration);
		}
	}

	private void Deactivate(float duration)
	{
		deactivating = StartCoroutine(Deactivating(duration));
	}

	private IEnumerator Deactivating(float duration)
	{
		middle.SetSpriteAlpha(1f);
		halfCircles[1].SetSpriteAlpha(1f);
		halfCircles[0].SetSpriteAlpha(1f);
		prompt.SetSpriteAlpha(1f);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			middle.SetSpriteAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
			halfCircles[1].SetSpriteAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
			halfCircles[0].SetSpriteAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
			prompt.SetSpriteAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
			yield return null;
		}
		CancelCoroutine(crossingIn);
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(crossingIn);
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	private void Configure()
	{
		beatsTilHold = (isHalfAddedToHold ? (beatsTilHold + 0.5f) : beatsTilHold);
		beatsTilRelease = (isHalfAddedToRelase ? (beatsTilRelease + 0.5f) : beatsTilRelease);
		beatsBetween = beatsTilRelease - beatsTilHold;
		distance = (isHalfDistance ? 1.5f : 3f);
		positionerTransform.SetLocalPosition(0f, beatsTilHold * distance * -1f);
		positioner.TriggerAnim("awaiting");
		middle.SetLocalScale(1f, beatsBetween * initMiddleHeight);
		halfCircles[1].SetLocalPosition(0f, beatsBetween * distance * -1f);
		middle.SetSpriteAlpha(0f);
		halfCircles[1].SetSpriteAlpha(0f);
		halfCircles[0].SetSpriteAlpha(0f);
		prompt.SetSpriteAlpha(0f);
		prompt.ToggleAnimator(toggle: true);
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadCROSS");
		}
		else
		{
			prompt.TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		}
	}

	public void ToggleIsBlind(bool toggle)
	{
		isBlind = toggle;
	}

	public void ToggleIsHalfDistance(bool toggle)
	{
		isHalfDistance = toggle;
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}
}
