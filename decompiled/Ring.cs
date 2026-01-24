using System.Collections;
using UnityEngine;

public class Ring : Wrapper
{
	[Header("Fragments")]
	public Fragment positioner;

	public Fragment prompt;

	public Fragment ringCircle;

	private bool isHalfAddedToHit;

	private bool isHalfDistance;

	private bool isBlind;

	private int easeType;

	private int direction;

	private int rotation;

	private float beatsTilHit;

	private float distance;

	private float initLocalZ;

	private float timeStarted;

	private string transformAnim;

	private string buttonName;

	private Coroutine crossingIn;

	private Coroutine deactivating;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		positioner.Awake();
		prompt.Awake();
		ringCircle.Awake();
		initLocalZ = GetLocalZ();
		RenderChildren(toggle: false);
	}

	public void CrossInDelayed(float newTimeStarted, float newBeatsTilHit, int newDirection, bool newIsHalfAddedToHit)
	{
		CancelCoroutine(crossingIn);
		CancelCoroutine(deactivating);
		timeStarted = newTimeStarted;
		beatsTilHit = newBeatsTilHit;
		direction = newDirection;
		isHalfAddedToHit = newIsHalfAddedToHit;
		RenderChildren(toggle: true);
		Configure();
		if (easeType == 0)
		{
			crossingIn = StartCoroutine(CrossingInLinearDelayed());
		}
		else
		{
			crossingIn = StartCoroutine(CrossingInEasedDelayed());
		}
	}

	private IEnumerator CrossingInLinearDelayed()
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Activate();
		Vector3 startLocalPosition = GetLocalPosition();
		Vector3 endLocalPosition = new Vector3(0f, 0f, GetLocalZ());
		float duration = MusicBox.env.GetSecsPerBeat() * beatsTilHit;
		while (MusicBox.env.GetSongTime() - checkpoint < duration)
		{
			base.transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (MusicBox.env.GetSongTime() - checkpoint) / duration);
			yield return null;
		}
		if (!isBlind)
		{
			Deactivate(0.11667f);
		}
	}

	private IEnumerator CrossingInEasedDelayed()
	{
		bool isMoving = false;
		float checkpoint = 0f;
		while (beatsTilHit > 0.5f)
		{
			if (checkpoint == 0f)
			{
				checkpoint = timeStarted + 0.11667f;
			}
			else
			{
				checkpoint += MusicBox.env.GetSecsPerBeat();
			}
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			if (!isMoving)
			{
				prompt.ToggleAnimator(toggle: false);
				isMoving = true;
				Activate();
			}
			if (direction == 0)
			{
				SetLocalPosition(0f, beatsTilHit * distance * -1f);
			}
			else if (direction == 1)
			{
				SetLocalPosition(beatsTilHit * distance * -1f, 0f);
			}
			else if (direction == 2)
			{
				SetLocalPosition(beatsTilHit * distance, 0f);
			}
			else if (direction == 3)
			{
				SetLocalPosition(0f, beatsTilHit * distance);
			}
			positioner.TriggerAnim(transformAnim, GetSpeed());
			beatsTilHit -= 1f;
			yield return null;
		}
		if (beatsTilHit == 0.5f)
		{
			if (checkpoint == 0f)
			{
				checkpoint = timeStarted + 0.11667f;
			}
			else
			{
				checkpoint += MusicBox.env.GetSecsPerBeat();
			}
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			if (direction == 0)
			{
				SetLocalPosition(0f, beatsTilHit * distance * -1f);
			}
			else if (direction == 1)
			{
				SetLocalPosition(beatsTilHit * distance * -1f, 0f);
			}
			else if (direction == 2)
			{
				SetLocalPosition(beatsTilHit * distance, 0f);
			}
			else if (direction == 3)
			{
				SetLocalPosition(0f, beatsTilHit * distance);
			}
			positioner.TriggerAnim(transformAnim + "Half", GetSpeed() * 2f);
		}
		if (!isBlind)
		{
			if (isHalfAddedToHit)
			{
				checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
			}
			else
			{
				checkpoint += MusicBox.env.GetSecsPerBeat();
			}
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			Deactivate(0.11667f);
		}
	}

	private void Activate()
	{
		StartCoroutine(Activating());
	}

	private IEnumerator Activating()
	{
		ringCircle.SetSpriteAlpha(0f);
		prompt.SetSpriteAlpha(0f);
		float elapsed = 0f;
		float duration = 0.11667f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			ringCircle.SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			prompt.SetSpriteAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
			yield return null;
		}
		if (isBlind)
		{
			Deactivate(MusicBox.env.GetSecsPerBeat() * beatsTilHit / 2f - duration);
		}
	}

	private void Deactivate(float duration)
	{
		deactivating = StartCoroutine(Deactivating(duration));
	}

	private IEnumerator Deactivating(float duration)
	{
		ringCircle.SetSpriteAlpha(1f);
		prompt.SetSpriteAlpha(1f);
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			ringCircle.SetSpriteAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
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
		beatsTilHit = (isHalfAddedToHit ? (beatsTilHit + 0.5f) : beatsTilHit);
		distance = (isHalfDistance ? 1.5f : 3f);
		switch (direction)
		{
		case 0:
			if (ControlHandler.mgr.GetCtrlType() == 1)
			{
				buttonName = "gamepadA";
			}
			else if (ControlHandler.mgr.GetCtrlType() == 2)
			{
				buttonName = "gamepadCROSS";
			}
			else
			{
				buttonName = "key" + SaveManager.mgr.GetActionKey();
			}
			rotation = 0;
			SetLocalPosition(0f, beatsTilHit * distance * -1f);
			break;
		case 1:
			if (ControlHandler.mgr.GetCtrlType() == 1 || ControlHandler.mgr.GetCtrlType() == 2)
			{
				buttonName = "gamepadL";
			}
			else if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				buttonName = "keyA";
			}
			else
			{
				buttonName = "keyLEFT";
			}
			rotation = -90;
			SetLocalPosition(beatsTilHit * distance * -1f, 0f);
			break;
		case 2:
			if (ControlHandler.mgr.GetCtrlType() == 1 || ControlHandler.mgr.GetCtrlType() == 2)
			{
				buttonName = "gamepadR";
			}
			else if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				buttonName = "keyD";
			}
			else
			{
				buttonName = "keyRIGHT";
			}
			rotation = 90;
			SetLocalPosition(beatsTilHit * distance, 0f);
			break;
		case 3:
			if (ControlHandler.mgr.GetCtrlType() == 1)
			{
				buttonName = "gamepadA";
			}
			else if (ControlHandler.mgr.GetCtrlType() == 2)
			{
				buttonName = "gamepadCROSS";
			}
			else if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				buttonName = "keyW";
			}
			else
			{
				buttonName = "keyUP";
			}
			rotation = 180;
			SetLocalPosition(0f, beatsTilHit * distance);
			break;
		}
		SetLocalEulerAngles(0f, 0f, rotation);
		ringCircle.SetSpriteAlpha(0f);
		prompt.SetSpriteAlpha(0f);
		prompt.ToggleAnimator(toggle: true);
		prompt.TriggerAnim(buttonName);
		prompt.SetLocalEulerAngles(0f, 0f, rotation * -1);
		positioner.TriggerAnim("awaiting");
		if (easeType > 0)
		{
			transformAnim = ((distance == 3f) ? (easeType + "slide") : (easeType + "slideHalf"));
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

	public void SetEaseType(int newEaseType)
	{
		easeType = newEaseType;
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}
}
