using System.Collections;
using System.IO;
using UnityEngine;

public class CommunityMenu : Wrapper
{
	[Header("Children")]
	public ScrollingBar ScrollingBar;

	public Wrapper Scroller;

	public LevelRow[] LevelRows;

	public Fader Fader;

	[Header("Fragments")]
	public Fragment activator;

	public textboxFragment title;

	public textboxFragment info;

	public textboxFragment[] labels;

	public spriteFragment[] prompts;

	[Header("Props")]
	private bool isActivated;

	private int highlightNum;

	private int rowCount = 1;

	private int pageNum = 1;

	private int pageTotal = 1;

	private int totalLevels;

	private float scrollingBarIncrement;

	private float scrollerIncrement;

	private float scrollingBarPosition;

	private float scrollerPosition;

	private Coroutine deactivating;

	private Coroutine navigating;

	private Coroutine configingRows;

	protected override void Awake()
	{
		activator.Awake();
		title.Initiate();
		info.Initiate();
		labels[0].Initiate();
		labels[1].Initiate();
		prompts[0].Initiate();
		prompts[1].Initiate();
		SetupFragments();
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(navigating);
		CancelCoroutine(configingRows);
		RenderChildren(toggle: true, 1);
		SetParentAndReposition(Interface.env.Cam.GetSliderTransform());
		SetLocalZ(1f);
		isActivated = true;
		highlightNum = 0;
		rowCount = 1;
		pageNum = 1;
		pageTotal = 1;
		totalLevels = 0;
		scrollingBarIncrement = 0f;
		scrollerIncrement = 0f;
		scrollingBarPosition = 0f;
		scrollerPosition = 0f;
		activator.TriggerAnim("in");
		title.SetState(0);
		labels[0].SetState(0);
		speakers[0].TriggerSound(0);
		info.SetText("");
		ScrollingBar.Activate();
		ScrollingBar.SetLocalY(scrollingBarPosition);
		Scroller.SetLocalY(scrollerPosition);
		LevelRows[0].ActivateAsBanner();
		Fader.Activate();
		SteamWorkshop.mgr.DownloadSubscribedItems();
		Interface.env.Disable();
		ConfigLabels();
		ConfigContent(isSpinner: true);
	}

	public void Deactivate(bool isLoadingScene = false)
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(navigating);
		CancelCoroutine(configingRows);
		deactivating = StartCoroutine(Deactivating(isLoadingScene));
	}

	private IEnumerator Deactivating(bool isLoadingScene)
	{
		isActivated = false;
		if (ScrollingBar.CheckIsActivated())
		{
			ScrollingBar.Deactivate();
		}
		activator.TriggerAnim("inReversed");
		speakers[0].TriggerSound(1);
		Fader.Deactivate();
		LevelRows[0].DeactivateAsBanner();
		for (int i = 1; i < rowCount && i < LevelRows.Length; i++)
		{
			LevelRows[i].Deactivate();
		}
		if (!isLoadingScene)
		{
			yield return new WaitForSecondsRealtime(0.34f);
			SetParentAndReposition(null);
			SetLocalZ(0f);
			RenderChildren(toggle: false, 1);
			Interface.env.Enable();
		}
	}

	public void Descend()
	{
		CancelCoroutine(navigating);
		navigating = StartCoroutine(Descending());
	}

	private IEnumerator Descending()
	{
		if (highlightNum < rowCount - 1)
		{
			speakers[0].TriggerSound(2);
			scrollingBarPosition -= scrollingBarIncrement;
			scrollerPosition += scrollerIncrement;
			ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
			Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
			LevelRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum++;
			LevelRows[highlightNum].ToggleHighlight(toggle: true);
			yield return new WaitForSeconds(0.33f);
			while (ControlHandler.mgr.CheckIsDownPressing() && highlightNum != rowCount - 1)
			{
				yield return new WaitForSeconds(0.075f);
				speakers[0].TriggerSound(2);
				scrollingBarPosition -= scrollingBarIncrement;
				scrollerPosition += scrollerIncrement;
				ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
				Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
				LevelRows[highlightNum].ToggleHighlight(toggle: false);
				highlightNum++;
				LevelRows[highlightNum].ToggleHighlight(toggle: true);
				yield return null;
			}
		}
	}

	public void Ascend()
	{
		CancelCoroutine(navigating);
		navigating = StartCoroutine(Ascending());
	}

	private IEnumerator Ascending()
	{
		if (highlightNum > 0)
		{
			speakers[0].TriggerSound(2);
			scrollingBarPosition += scrollingBarIncrement;
			scrollerPosition -= scrollerIncrement;
			ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
			Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
			LevelRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum--;
			LevelRows[highlightNum].ToggleHighlight(toggle: true);
			yield return new WaitForSeconds(0.33f);
			while (ControlHandler.mgr.CheckIsUpPressing() && highlightNum != 0)
			{
				yield return new WaitForSeconds(0.075f);
				speakers[0].TriggerSound(2);
				scrollingBarPosition += scrollingBarIncrement;
				scrollerPosition -= scrollerIncrement;
				ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
				Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
				LevelRows[highlightNum].ToggleHighlight(toggle: false);
				highlightNum--;
				LevelRows[highlightNum].ToggleHighlight(toggle: true);
				yield return null;
			}
		}
	}

	public void Select()
	{
		if (highlightNum == 0)
		{
			SteamWorkshop.mgr.OpenWorkshopFrontPage();
			Deactivate();
		}
		else
		{
			LvlEditor.SetDownloadFilePath(LevelRows[highlightNum].GetPath());
			FileSystemInfo[] fileSystemInfos = new DirectoryInfo(LevelRows[highlightNum].GetPath()).GetFileSystemInfos("*.json");
			string sceneName = fileSystemInfos[0].Name.Replace(fileSystemInfos[0].Extension, "");
			Interface.env.ExitTo(sceneName);
			Deactivate(isLoadingScene: true);
		}
		Interface.env.Submenu.PlaySfx(1);
	}

	public void PrevPage()
	{
		if (pageTotal > 1)
		{
			speakers[0].TriggerSound(2);
			LevelRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum = 0;
			rowCount = 1;
			LevelRows[highlightNum].ToggleHighlight(toggle: true);
			pageNum--;
			if (pageNum == 0)
			{
				pageNum = pageTotal;
			}
			scrollingBarPosition = 0f;
			scrollerPosition = 0f;
			ScrollingBar.SetLocalY(scrollingBarPosition);
			Scroller.SetLocalY(scrollerPosition);
			ConfigContent(isSpinner: false);
		}
	}

	public void NextPage()
	{
		if (pageTotal > 1)
		{
			speakers[0].TriggerSound(2);
			LevelRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum = 0;
			rowCount = 1;
			LevelRows[highlightNum].ToggleHighlight(toggle: true);
			pageNum++;
			if (pageNum > pageTotal)
			{
				pageNum = 1;
			}
			scrollingBarPosition = 0f;
			scrollerPosition = 0f;
			ScrollingBar.SetLocalY(scrollingBarPosition);
			Scroller.SetLocalY(scrollerPosition);
			ConfigContent(isSpinner: false);
		}
	}

	private void ConfigLabels()
	{
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompts[0].SetState(1);
			prompts[1].SetState(1);
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompts[0].SetState(2);
			prompts[1].SetState(2);
		}
		else
		{
			prompts[0].SetState(0);
			prompts[1].SetState(0);
		}
		textboxFragment[] array = labels;
		foreach (textboxFragment obj in array)
		{
			obj.SetState(0);
			obj.SetFontSize(4f);
		}
		if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 8 || SaveManager.GetLang() == 9)
		{
			array = labels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetFontSize(3f);
			}
		}
		switch (SaveManager.GetLang())
		{
		case 0:
			prompts[0].SetLocalX(-2.206f);
			labels[0].SetLocalX(-1.716f);
			prompts[1].SetLocalX(0.827f);
			labels[1].SetLocalX(1.317f);
			break;
		case 1:
			prompts[0].SetLocalX(-2.01f);
			labels[0].SetLocalX(-1.52f);
			prompts[1].SetLocalX(0.94f);
			labels[1].SetLocalX(1.43f);
			break;
		case 2:
			prompts[0].SetLocalX(-2.01f);
			labels[0].SetLocalX(-1.52f);
			prompts[1].SetLocalX(0.94f);
			labels[1].SetLocalX(1.43f);
			break;
		case 3:
			prompts[0].SetLocalX(-2.22f);
			labels[0].SetLocalX(-1.73f);
			prompts[1].SetLocalX(0.94f);
			labels[1].SetLocalX(1.43f);
			break;
		case 4:
			prompts[0].SetLocalX(-2.01f);
			labels[0].SetLocalX(-1.52f);
			prompts[1].SetLocalX(0.94f);
			labels[1].SetLocalX(1.43f);
			break;
		case 5:
			prompts[0].SetLocalX(-2.03f);
			labels[0].SetLocalX(-1.607f);
			prompts[1].SetLocalX(0.94f);
			labels[1].SetLocalX(1.43f);
			break;
		case 6:
			prompts[0].SetLocalX(-2.049f);
			labels[0].SetLocalX(-1.58f);
			prompts[1].SetLocalX(0.646f);
			labels[1].SetLocalX(1.136f);
			break;
		case 7:
			prompts[0].SetLocalX(-2.26f);
			labels[0].SetLocalX(-1.81f);
			prompts[1].SetLocalX(0.71f);
			labels[1].SetLocalX(1.2f);
			break;
		case 8:
			prompts[0].SetLocalX(-2.03f);
			labels[0].SetLocalX(-1.607f);
			prompts[1].SetLocalX(0.66f);
			labels[1].SetLocalX(1.15f);
			break;
		case 9:
			prompts[0].SetLocalX(-2.03f);
			labels[0].SetLocalX(-1.607f);
			prompts[1].SetLocalX(0.75f);
			labels[1].SetLocalX(1.24f);
			break;
		}
	}

	private void ConfigContent(bool isSpinner)
	{
		CancelCoroutine(configingRows);
		configingRows = StartCoroutine(ConfigingRows(isSpinner));
	}

	private IEnumerator ConfigingRows(bool isSpinner)
	{
		for (int i = 1; i < LevelRows.Length; i++)
		{
			LevelRows[i].Hide();
		}
		if (isSpinner)
		{
			Interface.env.Spinner.Activate(Interface.env.Submenu.CheckIsActivated());
			yield return new WaitForSecondsRealtime(0.333f);
		}
		yield return new WaitUntil(() => !SteamWorkshop.mgr.CheckIsDownloading());
		Interface.env.Spinner.Deactivate();
		totalLevels = SteamWorkshop.mgr.GetSubscribedItems().Count;
		pageTotal = (int)Mathf.Ceil((float)totalLevels / 10f);
		for (int num = (pageNum - 1) * 10; num < totalLevels && num < pageNum * 10; num++)
		{
			rowCount++;
			LevelRows[num + 1 - (pageNum - 1) * 10].Activate(SteamWorkshop.mgr.GetSubscribedItems()[num].title, SteamWorkshop.mgr.GetSubscribedItems()[num].author, SteamWorkshop.mgr.GetSubscribedItems()[num].tags, SteamWorkshop.mgr.GetSubscribedItems()[num].path, num + 1 - (pageNum - 1) * 10);
		}
		int num2 = 5;
		float num3 = 1.699f;
		float num4 = 0.5f;
		if (rowCount > num2)
		{
			float num5 = (float)(rowCount - num2) * num3 + num4;
			scrollerIncrement = num5 / (float)rowCount;
		}
		else
		{
			scrollerIncrement = 0f;
		}
		float num6 = 5.6f;
		if (scrollerIncrement > 0f)
		{
			scrollingBarIncrement = num6 / (float)rowCount;
			ScrollingBar.Show();
		}
		else
		{
			scrollingBarIncrement = 0f;
			ScrollingBar.Hide();
		}
		info.SetState(0);
		info.SetText(info.GetText().Replace("x", pageNum.ToString() ?? ""));
		info.SetText(info.GetText().Replace("y", pageTotal.ToString() ?? ""));
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
