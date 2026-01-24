using UnityEngine;

public class Number : Wrapper
{
	[Header("Fragments")]
	public spriteFragment truncater;

	private bool isActivated;

	private int num;

	private string letter;

	private float timeTilDeactivate;

	private Coroutine deactivating;

	protected override void Awake()
	{
		truncater.Initiate();
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Activate(int slideNum, int speedMultiplier)
	{
		isActivated = true;
		RenderChildren(toggle: true);
		ConfigureDigits();
		ConfigureTruncator();
		gears[1].TriggerAnim("activate" + slideNum, InfluencerLand.env.GetSpeed() * (float)speedMultiplier);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		ConfigureDigits();
		ConfigureTruncator();
		gears[1].TriggerAnim("shown");
	}

	public void Hide()
	{
		isActivated = false;
		RenderChildren(toggle: false);
	}

	public void IncreaseNumber()
	{
		num++;
		if (isActivated)
		{
			ConfigureDigits();
		}
	}

	public void SetNumber(int newNum)
	{
		if (newNum != num)
		{
			num = newNum;
			if (isActivated)
			{
				ConfigureDigits();
			}
		}
	}

	public void SetLetter(string newLetter)
	{
		if (newLetter != letter)
		{
			letter = newLetter;
			if (isActivated)
			{
				ConfigureTruncator();
			}
		}
	}

	private void ConfigureDigits()
	{
		string text = num.ToString();
		if (text.Length == 1)
		{
			gears[0].TriggerAnim("1digit");
			sprites[0].TriggerAnim("0");
			sprites[1].TriggerAnim(text);
		}
		else if (text.Length == 2)
		{
			gears[0].TriggerAnim("2digit");
			sprites[0].TriggerAnim(text[0].ToString() ?? "");
			sprites[1].TriggerAnim(text[1].ToString() ?? "");
		}
		else if (text.Length == 3)
		{
			gears[0].TriggerAnim("3digit");
			sprites[0].TriggerAnim(text[0].ToString() ?? "");
			sprites[1].TriggerAnim(text[1].ToString() ?? "");
			sprites[2].TriggerAnim(text[2].ToString() ?? "");
		}
		else if (text.Length == 4)
		{
			gears[0].TriggerAnim("4digit");
			sprites[0].TriggerAnim(text[0].ToString() ?? "");
			sprites[1].TriggerAnim(text[1].ToString() ?? "");
			sprites[2].TriggerAnim(text[2].ToString() ?? "");
			sprites[3].TriggerAnim(text[3].ToString() ?? "");
		}
	}

	private void ConfigureTruncator()
	{
		if (letter != "" && letter != null)
		{
			if (SaveManager.GetLang() == 1)
			{
				if (letter == "k")
				{
					truncater.SetState(3);
				}
				else if (letter == "m")
				{
					truncater.SetState(4);
				}
				else
				{
					truncater.SetState(9);
				}
			}
			else if (SaveManager.GetLang() == 2)
			{
				if (letter == "k")
				{
					truncater.SetState(5);
				}
				else if (letter == "m")
				{
					truncater.SetState(6);
				}
				else
				{
					truncater.SetState(9);
				}
			}
			else if (SaveManager.GetLang() == 3)
			{
				if (letter == "k")
				{
					truncater.SetState(3);
				}
				else if (letter == "m")
				{
					truncater.SetState(6);
				}
				else
				{
					truncater.SetState(9);
				}
			}
			else if (SaveManager.GetLang() == 4)
			{
				if (letter == "k")
				{
					truncater.SetState(7);
				}
				else if (letter == "m")
				{
					truncater.SetState(8);
				}
				else
				{
					truncater.SetState(10);
				}
			}
			else
			{
				truncater.SetStateByName(letter);
			}
			truncater.ToggleSpriteRenderer(toggle: true);
		}
		else
		{
			truncater.ToggleSpriteRenderer(toggle: false);
		}
	}

	public int GetNum()
	{
		return num;
	}

	public string GetLetter()
	{
		return letter;
	}
}
