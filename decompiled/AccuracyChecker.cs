using System.Collections;
using UnityEngine;

public class AccuracyChecker : Wrapper
{
	private bool isTiming;

	private bool isActivated;

	private Coroutine timing;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		RenderChildren(toggle: true);
		sprites[0].SetSpriteColor(new Color(0.8f, 0.8f, 0.8f));
	}

	public void Hide()
	{
		isActivated = false;
		isTiming = false;
		SetParentAndReposition(Interface.env.transform);
		CancelCoroutine(timing);
		RenderChildren(toggle: false);
	}

	public void Time(float timeStarted)
	{
		CancelCoroutine(timing);
		timing = StartCoroutine(Timing(timeStarted));
	}

	private IEnumerator Timing(float timeStarted)
	{
		isTiming = true;
		while (isTiming)
		{
			float num = MusicBox.env.GetSongTime() - timeStarted;
			textboxes[0].SetText(num.ToString("F3"));
			yield return null;
		}
	}

	public void EndTimer()
	{
		isTiming = false;
		CancelCoroutine(timing);
	}

	public void SetAccuracy(float eventAccuracy)
	{
		if (eventAccuracy == 1f)
		{
			textboxes[0].SetText("");
			sprites[0].SetSpriteColor(new Color(0.53f, 1f, 0.92f));
		}
		else if (eventAccuracy == 0.332f || eventAccuracy == 0.333f)
		{
			textboxes[0].SetText("");
			if (eventAccuracy == 0.333f)
			{
				sprites[0].SetSpriteColor(new Color(1f, 0.64f, 0.55f));
			}
			else
			{
				sprites[0].SetSpriteColor(new Color(1f, 0.92f, 0.6f));
			}
		}
		else
		{
			sprites[0].SetSpriteColor(new Color(0.8f, 0.8f, 0.8f));
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
