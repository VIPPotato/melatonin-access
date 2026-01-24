using UnityEngine;

public class Feedback : Wrapper
{
	[Header("Fragments")]
	public Fragment[] aligners;

	public Fragment fader;

	public textboxFragment textbox;

	protected override void Awake()
	{
		Fragment[] array = aligners;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		fader.Awake();
		textbox.Initiate();
	}

	public void CrossIn(string text)
	{
		if (SaveManager.GetLang() == 0)
		{
			if (text == "early")
			{
				aligners[0].TriggerAnim("cross", 1.25f);
			}
			else if (text == "late")
			{
				aligners[1].TriggerAnim("cross", 1.25f);
			}
			else
			{
				aligners[2].TriggerAnim("cross", 1.25f);
			}
		}
		else if (text == "early")
		{
			fader.TriggerAnim("early", 1.25f);
			textbox.SetState(0);
		}
		else if (text == "late")
		{
			fader.TriggerAnim("late", 1.25f);
			textbox.SetState(1);
		}
		else
		{
			fader.TriggerAnim("perfect", 1.25f);
			textbox.SetState(2);
		}
	}

	public void Hide()
	{
		Fragment[] array = aligners;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("hidden");
		}
		fader.TriggerAnim("hidden");
	}
}
