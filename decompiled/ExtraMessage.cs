using UnityEngine;

public class ExtraMessage : Wrapper
{
	[Header("Fragments")]
	public Fragment slider;

	public textboxFragment message;

	private bool isActivated;

	protected override void Awake()
	{
		slider.Awake();
		message.Initiate();
		message.ToggleIsRealTimeFader(toggle: true);
	}

	public void Activate(int stateNum)
	{
		isActivated = true;
		slider.TriggerAnim("in");
		message.FadeInText(1f, 0.33f);
		message.SetState(stateNum);
		if (stateNum == 0)
		{
			SetLocalY(0f);
			if (SaveManager.GetLang() == 7)
			{
				message.SetFontSize(3.6f);
			}
			else
			{
				message.SetFontSize(4f);
			}
		}
		else
		{
			SetLocalY(0.7f);
			if (SaveManager.GetLang() == 7)
			{
				message.SetFontSize(3.6f);
			}
			else
			{
				message.SetFontSize(4f);
			}
		}
	}

	public void Deactivate()
	{
		isActivated = false;
		message.FadeOutText(1f, 0.33f);
	}

	public void Hide()
	{
		isActivated = false;
		message.ToggleMeshRenderer(toggle: false);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
