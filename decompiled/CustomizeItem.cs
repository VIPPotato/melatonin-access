using UnityEngine;

public class CustomizeItem : Wrapper
{
	[Header("Fragments")]
	public textboxFragment title;

	public spriteFragment thumbnail;

	[Header("Props")]
	private string code;

	private char dataType;

	private bool isActivated;

	protected override void Awake()
	{
		RenderChildren(toggle: false);
		title.Initiate();
		thumbnail.Initiate();
	}

	public void Show(char newDataType, string newCode)
	{
		code = newCode;
		dataType = newDataType;
		isActivated = true;
		RenderChildren(toggle: true);
		title.SetStateByName(dataType + "_" + code);
		if (title.GetCharacterCount() >= 15)
		{
			title.SetFontSize(2.4f);
		}
		else
		{
			title.SetFontSize(2.8f);
		}
		thumbnail.SetStateByName(dataType + "_" + code);
	}

	public void Hide()
	{
		isActivated = false;
		RenderChildren(toggle: false);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public string GetCode()
	{
		return code;
	}

	public char GetDataType()
	{
		return dataType;
	}
}
