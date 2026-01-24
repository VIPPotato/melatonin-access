using System;
using System.Collections;
using UnityEngine;

public class CustomizeMenu : Wrapper
{
	[Serializable]
	public class data
	{
		public string code;
	}

	[Serializable]
	public class dataList
	{
		public char type;

		public data[] datas;
	}

	[Header("Children")]
	public ProgressDots ProgressDots;

	public Highlighter Highlighter;

	public CustomizeItem[] CustomizeItems;

	[Header("Fragments")]
	public textboxFragment title;

	public Fragment activator;

	[Header("Props")]
	public dataList[] dataLists;

	private bool isActivated;

	private int activeItemNum;

	private int activeList;

	private float timeTilDeactivate;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		title.Initiate();
		activator.Awake();
		timeTilDeactivate = activator.GetAnimDuration("deactivate");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		isActivated = true;
		activeItemNum = 0;
		Highlighter.Hide();
		RenderChildren(toggle: true);
		if (LvlEditor.dir.CheckIsRemix() && dataLists.Length > 1)
		{
			title.SetStateByName(dataLists[activeList].type.ToString() ?? "");
			ProgressDots.Show(dataLists.Length);
			ProgressDots.SetActiveDot(activeList);
		}
		activator.TriggerAnim("activate");
		for (int i = 0; i < CustomizeItems.Length; i++)
		{
			if (dataLists.Length != 0 && i < dataLists[activeList].datas.Length)
			{
				CustomizeItems[i].Show(dataLists[activeList].type, dataLists[activeList].datas[i].code);
			}
			else
			{
				CustomizeItems[i].Hide();
			}
		}
		if (dataLists.Length != 0 && dataLists[activeList].datas.Length != 0 && !Highlighter.CheckIsActivated())
		{
			Highlighter.Show();
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
		Interface.env.Disable();
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		activator.TriggerAnim("deactivate");
		yield return null;
		Interface.env.Enable();
		yield return new WaitForSeconds(timeTilDeactivate);
		CustomizeItem[] customizeItems = CustomizeItems;
		foreach (CustomizeItem customizeItem in customizeItems)
		{
			if (customizeItem.CheckIsActivated())
			{
				customizeItem.Hide();
			}
		}
		Highlighter.Hide();
		RenderChildren(toggle: false);
	}

	public void HighlightNextColumn()
	{
		if (activeItemNum != 2 && activeItemNum != 5 && activeItemNum != 8 && CustomizeItems[activeItemNum + 1].CheckIsActivated())
		{
			activeItemNum++;
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
		else
		{
			NextList();
		}
	}

	public void HighlightPrevColumn()
	{
		if (activeItemNum != 0 && activeItemNum != 3 && activeItemNum != 6 && CustomizeItems[activeItemNum - 1].CheckIsActivated())
		{
			activeItemNum--;
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
		else
		{
			PrevList();
		}
	}

	public void HighlightNextRow()
	{
		if (activeItemNum != 6 && activeItemNum != 7 && activeItemNum != 8 && CustomizeItems[activeItemNum + 3].CheckIsActivated())
		{
			activeItemNum += 3;
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
		else
		{
			NextList();
		}
	}

	public void HighlightPrevRow()
	{
		if (activeItemNum != 0 && activeItemNum != 1 && activeItemNum != 2 && CustomizeItems[activeItemNum - 3].CheckIsActivated())
		{
			activeItemNum -= 3;
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
		else
		{
			PrevList();
		}
	}

	public void NextList()
	{
		if (dataLists.Length <= 1)
		{
			return;
		}
		activeItemNum = 0;
		activeList++;
		if (activeList >= dataLists.Length)
		{
			activeList = 0;
		}
		ProgressDots.SetActiveDot(activeList);
		Highlighter.Hide();
		title.SetStateByName(dataLists[activeList].type.ToString() ?? "");
		for (int i = 0; i < CustomizeItems.Length; i++)
		{
			if (i < dataLists[activeList].datas.Length)
			{
				CustomizeItems[i].Show(dataLists[activeList].type, dataLists[activeList].datas[i].code);
			}
			else
			{
				CustomizeItems[i].Hide();
			}
		}
		if (dataLists[activeList].datas.Length != 0 && !Highlighter.CheckIsActivated())
		{
			Highlighter.Show();
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
	}

	public void PrevList()
	{
		if (dataLists.Length <= 1)
		{
			return;
		}
		activeItemNum = 0;
		activeList--;
		if (activeList < 0)
		{
			activeList = dataLists.Length - 1;
		}
		ProgressDots.SetActiveDot(activeList);
		Highlighter.Hide();
		title.SetStateByName(dataLists[activeList].type.ToString() ?? "");
		for (int i = 0; i < CustomizeItems.Length; i++)
		{
			if (i < dataLists[activeList].datas.Length)
			{
				CustomizeItems[i].Show(dataLists[activeList].type, dataLists[activeList].datas[i].code);
			}
			else
			{
				CustomizeItems[i].Hide();
			}
		}
		if (dataLists[activeList].datas.Length != 0 && !Highlighter.CheckIsActivated())
		{
			Highlighter.Show();
			Highlighter.SetLocalPosition(CustomizeItems[activeItemNum].GetLocalX(), CustomizeItems[activeItemNum].GetLocalY() - 0.22f);
		}
	}

	public bool CheckIsUsableMenu()
	{
		if (CustomizeItems[0].CheckIsActivated())
		{
			return true;
		}
		return false;
	}

	public bool CheckIsPaginatedMenu()
	{
		if (dataLists.Length > 1)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public CustomizeItem GetHighlightedCustomzieItem()
	{
		return CustomizeItems[activeItemNum];
	}
}
