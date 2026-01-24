using UnityEngine;

public class BarSlot : Wrapper
{
	[Header("Fragments")]
	public textboxFragment barNum;

	public textboxFragment[] labels;

	public spriteFragment[] thumbnails;

	public bool[] denials = new bool[4];

	private char initiatedDataType;

	private string[] codes = new string[4] { ".", ".", ".", "." };

	protected override void Awake()
	{
		barNum.Initiate();
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		spriteFragment[] array2 = thumbnails;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Initiate();
		}
		RenderChildren(toggle: false);
	}

	public void Show(int index)
	{
		RenderChildren(toggle: true);
		SetLocalPosition(9.34f * (float)index, 0f);
		barNum.SetText((index + 1).ToString() ?? "");
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetText("");
		}
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void InitiateCode(int slotNum, string codeInitiated, char newInitiatedDataType)
	{
		codes[slotNum] = codeInitiated;
		initiatedDataType = newInitiatedDataType;
		char c = initiatedDataType;
		if (LvlEditor.dir.CheckIsRemix())
		{
			string text = codes[slotNum];
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == 's')
				{
					c = text[i + 1];
					break;
				}
			}
		}
		if (denials[slotNum])
		{
			thumbnails[slotNum].SetStateByName("denied");
		}
		else if (codes[slotNum].Contains("d"))
		{
			string text2 = codes[slotNum];
			for (int j = 0; j < text2.Length; j++)
			{
				if (text2[j] == 'd')
				{
					thumbnails[slotNum].SetStateByName(c + "_d" + text2[j + 1]);
					if (LvlEditor.dir.CheckIsRemix())
					{
						labels[slotNum].SetStateByName(c.ToString() ?? "");
					}
					break;
				}
			}
		}
		else
		{
			thumbnails[slotNum].SetStateByName("empty");
		}
	}

	public void SetCode(int slotNum, string codeAdded, char dataType)
	{
		if (LvlEditor.dir.CheckIsRemix() && !codeAdded.Contains("t"))
		{
			labels[slotNum].SetStateByName(dataType.ToString() ?? "");
			if (codes[slotNum] == ".")
			{
				codes[slotNum] = "s" + dataType;
			}
			else if (codes[slotNum].Contains("s"))
			{
				string text = codes[slotNum];
				for (int i = 0; i < text.Length; i++)
				{
					if (text[i] != 's')
					{
						continue;
					}
					if (text[i + 1] != dataType)
					{
						if (codes[slotNum].Contains("t"))
						{
							for (int j = 0; j < text.Length; j++)
							{
								if (text[j] == 't')
								{
									codes[slotNum] = "s" + dataType + "t" + text[j + 1];
									break;
								}
							}
						}
						else
						{
							codes[slotNum] = "s" + dataType;
						}
					}
					else
					{
						codes[slotNum] = codes[slotNum].Replace("s" + text[i + 1], "s" + dataType);
					}
					break;
				}
			}
			else
			{
				codes[slotNum] = codes[slotNum] + "s" + dataType;
			}
		}
		if (codes[slotNum] == ".")
		{
			codes[slotNum] = codeAdded;
		}
		else if (codeAdded.Contains("d") && codes[slotNum].Contains("d"))
		{
			string text2 = codes[slotNum];
			for (int k = 0; k < text2.Length; k++)
			{
				if (text2[k] == 'd')
				{
					codes[slotNum] = codes[slotNum].Replace("d" + text2[k + 1], codeAdded);
					break;
				}
			}
		}
		else if (codeAdded.Contains("u") && codes[slotNum].Contains("u"))
		{
			string text3 = codes[slotNum];
			for (int l = 0; l < text3.Length; l++)
			{
				if (text3[l] == 'u')
				{
					codes[slotNum] = codes[slotNum].Replace("u" + text3[l + 1], codeAdded);
					break;
				}
			}
		}
		else if (codeAdded.Contains("e") && codes[slotNum].Contains("e"))
		{
			string text4 = codes[slotNum];
			for (int m = 0; m < text4.Length; m++)
			{
				if (text4[m] == 'e')
				{
					codes[slotNum] = codes[slotNum].Replace("e" + text4[m + 1], codeAdded);
					break;
				}
			}
		}
		else if (codeAdded.Contains("t") && codes[slotNum].Contains("t"))
		{
			string text5 = codes[slotNum];
			for (int n = 0; n < text5.Length; n++)
			{
				if (text5[n] == 't')
				{
					codes[slotNum] = codes[slotNum].Replace("t" + text5[n + 1], codeAdded);
					break;
				}
			}
		}
		else
		{
			codes[slotNum] += codeAdded;
		}
		thumbnails[slotNum].SetStateByName(dataType + "_" + codeAdded);
	}

	public void RemoveCode(int slotNum, char charType)
	{
		if (!codes[slotNum].Contains(charType.ToString() ?? ""))
		{
			return;
		}
		string text = codes[slotNum];
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != charType)
			{
				continue;
			}
			codes[slotNum] = codes[slotNum].Replace(charType.ToString() + text[i + 1], "");
			if (LvlEditor.dir.CheckIsRemix())
			{
				labels[slotNum].SetText("");
				if (codes[slotNum] == "" || (codes[slotNum].Contains("s") && codes[slotNum].Length == 2))
				{
					codes[slotNum] = ".";
				}
				else
				{
					if (!codes[slotNum].Contains("t") || !codes[slotNum].Contains("s") || codes[slotNum].Length != 4)
					{
						break;
					}
					for (int j = 0; j < text.Length; j++)
					{
						if (text[j] == 's')
						{
							codes[slotNum] = codes[slotNum].Replace("s" + text[j + 1], "");
							break;
						}
					}
				}
			}
			else if (codes[slotNum] == "")
			{
				codes[slotNum] = ".";
			}
			break;
		}
		thumbnails[slotNum].SetStateByName("empty");
	}

	public void SetThumbnails(char charType)
	{
		for (int i = 0; i < 4; i++)
		{
			thumbnails[i].SetStateByName("empty");
			if (LvlEditor.dir.CheckIsRemix())
			{
				labels[i].SetText("");
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (denials[j] && charType != 't')
			{
				thumbnails[j].SetStateByName("denied");
			}
			else
			{
				if (!codes[j].Contains(charType.ToString() ?? ""))
				{
					continue;
				}
				string text = codes[j];
				char c = initiatedDataType;
				if (LvlEditor.dir.CheckIsRemix() && charType != 't')
				{
					for (int k = 0; k < text.Length; k++)
					{
						if (text[k] == 's')
						{
							c = text[k + 1];
							break;
						}
					}
				}
				for (int l = 0; l < text.Length; l++)
				{
					if (text[l] == charType)
					{
						thumbnails[j].SetStateByName(c + "_" + charType + text[l + 1]);
						if (LvlEditor.dir.CheckIsRemix() && charType != 't')
						{
							labels[j].SetStateByName(c.ToString() ?? "");
						}
						break;
					}
				}
			}
		}
	}

	public bool CheckIsBeatSlotDenied(int slotNum)
	{
		return denials[slotNum];
	}

	public string GetCode(int slotNum)
	{
		return codes[slotNum];
	}
}
