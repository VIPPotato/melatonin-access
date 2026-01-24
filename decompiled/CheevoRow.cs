using UnityEngine;

public class CheevoRow : Wrapper
{
	[Header("Fragments")]
	public Fragment activator;

	public textboxFragment title;

	public textboxFragment description;

	public spriteFragment checkmark;

	public Fragment highlight;

	public Fragment thumbnail;

	private float titleInitLocalX;

	private float descriptionInitLocalX;

	protected override void Awake()
	{
		activator.Awake();
		title.Initiate();
		description.Initiate();
		checkmark.Initiate();
		highlight.Awake();
		if (thumbnail != null)
		{
			thumbnail.Awake();
		}
		titleInitLocalX = title.GetLocalX();
		descriptionInitLocalX = description.GetLocalX();
	}

	public void Activate()
	{
		activator.TriggerAnim("in");
	}

	public void Deactivate()
	{
		activator.TriggerAnim("inReversed");
		highlight.ToggleSpriteRenderer(toggle: false);
	}

	private void Update()
	{
		if (base.transform.position.y > 5.54f || base.transform.position.y < -4.6f)
		{
			if (title.GetLocalX() != 999f)
			{
				title.SetLocalX(-999f);
				description.SetLocalX(-999f);
			}
		}
		else if (title.GetLocalX() != titleInitLocalX)
		{
			title.SetLocalX(titleInitLocalX);
			description.SetLocalX(descriptionInitLocalX);
		}
	}

	public void Check()
	{
		checkmark.SetState(1);
	}

	public void SetTitle(int stateNum)
	{
		title.SetState(stateNum);
	}

	public void SetDescription(int stateNum)
	{
		description.SetState(stateNum);
		if (SaveManager.GetLang() == 3)
		{
			description.SetFontSize(2.8f);
		}
		else if (SaveManager.GetLang() == 5)
		{
			description.SetFontSize(3.2f);
		}
		else if (SaveManager.GetLang() == 6)
		{
			description.SetFontSize(3.2f);
		}
		else if (SaveManager.GetLang() == 7)
		{
			description.SetFontSize(3.2f);
		}
		else if (SaveManager.GetLang() == 8)
		{
			description.SetFontSize(3.2f);
		}
		else if (SaveManager.GetLang() == 9)
		{
			description.SetFontSize(3.2f);
		}
		else
		{
			description.SetFontSize(3.4f);
		}
	}

	public void SetTitleText(string text)
	{
		title.SetText(text);
	}

	public void SetDescriptionText(string text)
	{
		description.SetText(text);
	}

	public void SetThumbnail(string thumbnailName)
	{
		thumbnail.TriggerAnim(thumbnailName);
	}

	public void ToggleHighlight(bool toggle)
	{
		highlight.ToggleSpriteRenderer(toggle);
	}
}
