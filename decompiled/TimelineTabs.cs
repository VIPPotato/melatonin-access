using UnityEngine;

public class TimelineTabs : Wrapper
{
	[Header("Fragments")]
	public textboxFragment[] labels;

	public Fragment[] tabs;

	private char charType = 'd';

	protected override void Awake()
	{
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		Fragment[] array2 = tabs;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Awake();
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		charType = 'd';
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFontAlpha(0.67f);
		}
		Fragment[] array2 = tabs;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].TriggerAnim("disabled");
		}
		labels[0].SetFontAlpha(1f);
		tabs[0].TriggerAnim("enabled");
		if (SaveManager.GetLang() == 3 || SaveManager.GetLang() == 6)
		{
			array = labels;
			foreach (textboxFragment obj in array)
			{
				obj.SetFontSize(3f);
				obj.SetLetterSpacing(-2f);
			}
		}
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void NextTab()
	{
		if (charType == 'd')
		{
			charType = 'u';
			labels[0].SetFontAlpha(0.67f);
			labels[1].SetFontAlpha(1f);
			tabs[0].TriggerAnim("disabled");
			tabs[1].TriggerAnim("enabled");
		}
		else if (charType == 'u')
		{
			charType = 'e';
			labels[1].SetFontAlpha(0.67f);
			labels[2].SetFontAlpha(1f);
			tabs[1].TriggerAnim("disabled");
			tabs[2].TriggerAnim("enabled");
		}
		else if (charType == 'e')
		{
			charType = 't';
			labels[2].SetFontAlpha(0.67f);
			labels[3].SetFontAlpha(1f);
			tabs[2].TriggerAnim("disabled");
			tabs[3].TriggerAnim("enabled");
		}
		else if (charType == 't')
		{
			charType = 'd';
			labels[0].SetFontAlpha(1f);
			tabs[0].TriggerAnim("enabled");
			labels[3].SetFontAlpha(0.67f);
			tabs[3].TriggerAnim("disabled");
		}
	}

	public void PrevTab()
	{
		if (charType == 'd')
		{
			charType = 't';
			tabs[3].TriggerAnim("enabled");
			labels[0].SetFontAlpha(0.67f);
			tabs[0].TriggerAnim("disabled");
		}
		else if (charType == 'u')
		{
			charType = 'd';
			labels[1].SetFontAlpha(0.67f);
			tabs[1].TriggerAnim("disabled");
			tabs[0].TriggerAnim("enabled");
		}
		else if (charType == 'e')
		{
			charType = 'u';
			labels[2].SetFontAlpha(0.67f);
			tabs[2].TriggerAnim("disabled");
			tabs[1].TriggerAnim("enabled");
		}
		else if (charType == 't')
		{
			charType = 'e';
			labels[3].SetFontAlpha(0.67f);
			tabs[3].TriggerAnim("disabled");
			tabs[2].TriggerAnim("enabled");
		}
	}

	public char GetCharType()
	{
		return charType;
	}
}
