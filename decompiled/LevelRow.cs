using System.Collections;
using UnityEngine;

public class LevelRow : Wrapper
{
	[Header("Fragments")]
	public spriteFragment banner;

	public textboxFragment title;

	public textboxFragment subtitle;

	public textboxFragment tags;

	public Fragment highlight;

	public spriteFragment fill;

	private string path;

	private float offsetY;

	private float titleInitLocalX;

	private float subtitleInitLocalX;

	private float tagsInitLocalX;

	private int index;

	private Coroutine deactivating;

	protected override void Awake()
	{
		banner.Initiate();
		title.Initiate();
		subtitle.Initiate();
		tags.Initiate();
		highlight.Awake();
		fill.Initiate();
		offsetY = GetLocalY();
		titleInitLocalX = title.GetLocalX();
		subtitleInitLocalX = subtitle.GetLocalX();
		tagsInitLocalX = tags.GetLocalX();
		RenderChildren(toggle: false);
	}

	public void ActivateAsBanner()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		highlight.FadeInSprite(1f, 0.33f);
		banner.FadeInSprite(1f, 0.33f);
		title.ToggleMeshRenderer(toggle: false);
		subtitle.ToggleMeshRenderer(toggle: false);
		tags.ToggleMeshRenderer(toggle: false);
		fill.ToggleSpriteRenderer(toggle: false);
	}

	public void Activate(string titleText, string subtitleText, string tagsText, string newPath, int newIndex)
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		banner.ToggleSpriteRenderer(toggle: false);
		title.SetText(titleText);
		subtitle.SetText(subtitleText);
		tags.SetText(tagsText.Replace(",", "<br>"));
		path = newPath;
		index = newIndex;
		SetLocalY(offsetY + (float)index * 1.699f * -1f);
		highlight.ToggleSpriteRenderer(toggle: false);
		if (index % 2 == 0)
		{
			fill.ToggleSpriteRenderer(toggle: false);
		}
		else
		{
			fill.ToggleSpriteRenderer(toggle: true);
		}
		if (index < 6)
		{
			title.FadeInText(1f, 0.33f);
			subtitle.FadeInText(1f, 0.33f);
			tags.FadeInText(1f, 0.33f);
			if (index % 2 == 1)
			{
				fill.FadeInSprite(1f, 0.33f);
			}
		}
		else
		{
			title.SetFontAlpha(1f);
			subtitle.SetFontAlpha(1f);
			tags.SetFontAlpha(1f);
			if (index % 2 == 1)
			{
				fill.SetSpriteAlpha(1f);
			}
		}
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		title.FadeOutText(1f, 0.33f);
		subtitle.FadeOutText(1f, 0.33f);
		tags.FadeOutText(1f, 0.33f);
		if (index % 2 == 1)
		{
			fill.FadeOutSprite(1f, 0.33f);
		}
		if (highlight.CheckIsSpriteRendered())
		{
			highlight.FadeOutSprite(1f, 0.33f);
		}
		yield return new WaitForSecondsRealtime(0.34f);
		RenderChildren(toggle: false);
	}

	public void DeactivateAsBanner()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(DeactivatingAsBanner());
	}

	private IEnumerator DeactivatingAsBanner()
	{
		if (highlight.CheckIsSpriteRendered())
		{
			highlight.FadeOutSprite(1f, 0.33f);
		}
		banner.FadeOutSprite(1f, 0.33f);
		yield return new WaitForSecondsRealtime(0.34f);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (base.transform.position.y > 5.7f || base.transform.position.y < -4.85f)
		{
			if (title.GetLocalX() != 999f)
			{
				title.SetLocalX(-999f);
				subtitle.SetLocalX(-999f);
				tags.SetLocalX(-999f);
			}
		}
		else if (title.GetLocalX() != titleInitLocalX)
		{
			title.SetLocalX(titleInitLocalX);
			subtitle.SetLocalX(subtitleInitLocalX);
			tags.SetLocalX(tagsInitLocalX);
		}
	}

	public void ToggleHighlight(bool toggle)
	{
		highlight.ToggleSpriteRenderer(toggle);
		if (toggle)
		{
			highlight.SetSpriteAlpha(1f);
		}
	}

	public string GetPath()
	{
		return path;
	}
}
