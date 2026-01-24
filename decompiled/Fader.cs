using System.Collections;
using UnityEngine;

public class Fader : Wrapper
{
	[Header("Props")]
	public bool isHalfFaded;

	public int color;

	public float speed = 1f;

	public bool isRealTime;

	private bool isActivated;

	private float timeTilDeactivate;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		timeTilDeactivate = sprites[0].GetAnimDuration("fadeOut");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		isActivated = true;
		RenderChildren(toggle: true);
		if (isRealTime)
		{
			sprites[0].SetAnimatorUnscaledTime();
		}
		RefreshColor();
		string animName = (isHalfFaded ? "fadeInToHalf" : "fadeIn");
		sprites[0].TriggerAnim(animName, speed);
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		RefreshColor();
		string animName = (isHalfFaded ? "fadeOutFromHalf" : "fadeOut");
		sprites[0].TriggerAnim(animName, speed);
		if (isRealTime)
		{
			yield return new WaitForSecondsRealtime(timeTilDeactivate / speed);
		}
		else
		{
			yield return new WaitForSeconds(timeTilDeactivate / speed);
		}
		RenderChildren(toggle: false);
	}

	public void Cross()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Crossing());
	}

	private IEnumerator Crossing()
	{
		RenderChildren(toggle: true);
		isActivated = false;
		RefreshColor();
		sprites[0].TriggerAnim("crossfade", speed);
		if (isRealTime)
		{
			yield return new WaitForSecondsRealtime(timeTilDeactivate / speed);
		}
		else
		{
			yield return new WaitForSeconds(timeTilDeactivate / speed);
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		isActivated = true;
		RefreshColor();
		string animName = (isHalfFaded ? "fadedInToHalf" : "fadedIn");
		sprites[0].TriggerAnim(animName, speed);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
		isActivated = false;
	}

	public void SetIsHalfFaded(bool toggle)
	{
		isHalfFaded = toggle;
	}

	public void SetColor(int newColor)
	{
		if (color != newColor)
		{
			color = newColor;
			if (isActivated)
			{
				RefreshColor();
			}
		}
	}

	private void RefreshColor()
	{
		if (color == 0)
		{
			sprites[0].SetSpriteColor(new Color(7f / 51f, 0.08627451f, 0.14901961f));
		}
		else if (color == 1)
		{
			sprites[0].SetSpriteColor(new Color(1f, 1f, 1f));
		}
	}

	public void SetSpeed(float newSpeed)
	{
		if (speed != newSpeed)
		{
			speed = newSpeed;
			if (isActivated)
			{
				sprites[0].SetCurrentAnimSpeed(speed);
			}
		}
	}

	public void AllowUnscaledTime()
	{
		sprites[0].SetAnimatorUnscaledTime();
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
