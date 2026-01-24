using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class spriteFragment : Custom
{
	[Header("Props")]
	public Sprite[] states;

	public List<string> stateNames = new List<string>();

	private bool isRealTimeFader;

	private SpriteRenderer spriteRenderer;

	private Coroutine fadingSprite;

	public void Initiate()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (states.Length != 0)
		{
			SetState(0);
		}
	}

	public void FadeInSprite(float newAlpha, float duration)
	{
		CancelCoroutine(fadingSprite);
		fadingSprite = StartCoroutine(FadingInSprite(newAlpha, duration));
	}

	private IEnumerator FadingInSprite(float newAlpha, float duration)
	{
		spriteRenderer.enabled = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(0f, newAlpha, elapsed / duration);
			spriteRenderer.color = color;
			yield return null;
		}
	}

	public void FadeOutSprite(float startAlpha, float duration)
	{
		CancelCoroutine(fadingSprite);
		fadingSprite = StartCoroutine(FadingOutSprite(startAlpha, duration));
	}

	private IEnumerator FadingOutSprite(float startAlpha, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
			spriteRenderer.color = color;
			yield return null;
		}
		spriteRenderer.enabled = false;
	}

	public void FadeToColor(Color newColor, float duration)
	{
		CancelCoroutine(fadingSprite);
		fadingSprite = StartCoroutine(FadingToColor(newColor, duration));
	}

	private IEnumerator FadingToColor(Color newColor, float duration)
	{
		spriteRenderer.enabled = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			spriteRenderer.color = Color.Lerp(spriteRenderer.color, newColor, elapsed / duration);
			yield return null;
		}
	}

	public void SetState(int num)
	{
		spriteRenderer.sprite = states[num];
	}

	public void SetStateByName(string name)
	{
		if (stateNames.Contains(name))
		{
			spriteRenderer.sprite = states[stateNames.IndexOf(name)];
		}
	}

	public void SetStateDelayedDelta(float delta, int num)
	{
		StartCoroutine(SettingStateDelayedDelta(delta, num));
	}

	private IEnumerator SettingStateDelayedDelta(float delta, int num)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		spriteRenderer.sprite = states[num];
	}

	public void SetSpriteColor(Color newColor)
	{
		CancelCoroutine(fadingSprite);
		spriteRenderer.color = newColor;
	}

	public void SetSpriteAlpha(float newAlpha)
	{
		CancelCoroutine(fadingSprite);
		Color color = spriteRenderer.color;
		color.a = newAlpha;
		spriteRenderer.color = color;
	}

	public void SetSpriteMaskInteraction(int interactionType)
	{
		switch (interactionType)
		{
		case 0:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
			break;
		case 1:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case 2:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		}
	}

	public void ToggleSpriteRenderer(bool toggle)
	{
		spriteRenderer.enabled = toggle;
	}

	public void ToggleSpriteFlip(bool toggle)
	{
		spriteRenderer.flipX = toggle;
	}

	public void ToggleIsRealTimeFader(bool toggle)
	{
		isRealTimeFader = toggle;
	}

	public bool CheckIsSpriteRendered()
	{
		return spriteRenderer.enabled;
	}

	public bool CheckIsSpriteFlipped()
	{
		return spriteRenderer.flipX;
	}

	public int GetState()
	{
		for (int i = 0; i < states.Length; i++)
		{
			if (spriteRenderer.sprite == states[i])
			{
				return i;
			}
		}
		MonoBehaviour.print("error: no states");
		return 0;
	}

	public string GetStateName()
	{
		for (int i = 0; i < states.Length; i++)
		{
			if (spriteRenderer.sprite == states[i])
			{
				return stateNames[i];
			}
		}
		MonoBehaviour.print("error: no states or no matching name");
		return "";
	}

	public Color GetSpriteColor()
	{
		return spriteRenderer.color;
	}

	public float GetSpriteAlpha()
	{
		return spriteRenderer.color.a;
	}
}
