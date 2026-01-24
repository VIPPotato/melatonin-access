using System.Collections;
using UnityEngine;

public class AchievementsMenu : Wrapper
{
	[Header("Children")]
	public ScrollingBar ScrollingBar;

	public Wrapper Scroller;

	public CheevoRow[] CheevoRows;

	public Fader Fader;

	[Header("Fragments")]
	public Fragment activator;

	public textboxFragment title;

	public textboxFragment info;

	public textboxFragment label;

	public Fragment prompt;

	public float scrollingBarIncrement = 1f;

	public float scrollerIncrement = 1f;

	private bool isActivated;

	private int highlightNum;

	private int completedNum;

	private float scrollingBarPosition;

	private float scrollerPosition;

	private Coroutine deactivating;

	private Coroutine navigating;

	protected override void Awake()
	{
		activator.Awake();
		title.Initiate();
		info.Initiate();
		label.Initiate();
		prompt.Awake();
		SetupFragments();
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 1);
		SetParentAndReposition(Interface.env.Cam.GetSliderTransform());
		SetLocalZ(1f);
		isActivated = true;
		activator.TriggerAnim("in");
		title.SetState(0);
		label.SetState(0);
		speakers[0].TriggerSound(0);
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadB");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadCIRCLE");
		}
		else
		{
			prompt.TriggerAnim("keyESC");
		}
		CheevoRows[highlightNum].ToggleHighlight(toggle: true);
		for (int i = 0; i < 5; i++)
		{
			CheevoRows[i].Activate();
			CheevoRows[i].SetTitleText("?????");
			CheevoRows[i].SetDescriptionText("?????");
		}
		for (int j = 5; j < CheevoRows.Length; j++)
		{
			CheevoRows[j].Activate();
			CheevoRows[j].SetTitle(j);
			CheevoRows[j].SetDescription(j);
		}
		if (SaveManager.mgr.GetChapterNum() > 1)
		{
			CheevoRows[0].Check();
			CheevoRows[0].SetTitle(0);
			CheevoRows[0].SetDescription(0);
			CheevoRows[0].SetThumbnail("indulgence");
			completedNum++;
		}
		if (SaveManager.mgr.GetChapterNum() > 2)
		{
			CheevoRows[1].Check();
			CheevoRows[1].SetTitle(1);
			CheevoRows[1].SetDescription(1);
			CheevoRows[1].SetThumbnail("under_pressure");
			completedNum++;
		}
		if (SaveManager.mgr.GetChapterNum() > 3)
		{
			CheevoRows[2].Check();
			CheevoRows[2].SetTitle(2);
			CheevoRows[2].SetDescription(2);
			CheevoRows[2].SetThumbnail("meditation");
			completedNum++;
		}
		if (SaveManager.mgr.GetChapterNum() > 4)
		{
			CheevoRows[3].Check();
			CheevoRows[3].SetTitle(3);
			CheevoRows[3].SetDescription(3);
			CheevoRows[3].SetThumbnail("setbacks");
			completedNum++;
		}
		if (SaveManager.mgr.CheckIsGameComplete())
		{
			CheevoRows[4].Check();
			CheevoRows[4].SetTitle(4);
			CheevoRows[4].SetDescription(4);
			CheevoRows[4].SetThumbnail("new_day");
			completedNum++;
		}
		if (SaveManager.mgr.CheckIsTp())
		{
			CheevoRows[5].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckStarPrecisionAchievement())
		{
			CheevoRows[6].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckStarPerfectionistAchievement())
		{
			CheevoRows[7].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckStargazerAchievement())
		{
			CheevoRows[8].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckRingPrecisionAchievement())
		{
			CheevoRows[9].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckRingPerfectionistAchievement())
		{
			CheevoRows[10].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckRingCollectorAchievement())
		{
			CheevoRows[11].Check();
			completedNum++;
		}
		if (SaveManager.mgr.GetTotalPerfects() >= 42)
		{
			CheevoRows[12].Check();
			completedNum++;
		}
		if (SaveManager.mgr.CheckIsCreator())
		{
			CheevoRows[13].Check();
			completedNum++;
		}
		info.SetText(Mathf.RoundToInt((float)completedNum / 14f * 100f) + "%");
		ScrollingBar.Activate();
		ScrollingBar.SetLocalY(0f);
		Scroller.SetLocalY(0f);
		Fader.Activate();
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
		highlightNum = 0;
		ScrollingBar.Deactivate();
		scrollingBarPosition = 0f;
		scrollerPosition = 0f;
		completedNum = 0;
		activator.TriggerAnim("inReversed");
		speakers[0].TriggerSound(1);
		Fader.Deactivate();
		CheevoRow[] cheevoRows = CheevoRows;
		for (int i = 0; i < cheevoRows.Length; i++)
		{
			cheevoRows[i].Deactivate();
		}
		yield return new WaitForSeconds(0.33f);
		SetParentAndReposition(null);
		SetLocalZ(0f);
		RenderChildren(toggle: false, 1);
		Interface.env.Enable();
	}

	public void Descend()
	{
		CancelCoroutine(navigating);
		navigating = StartCoroutine(Descending());
	}

	private IEnumerator Descending()
	{
		if (highlightNum < CheevoRows.Length - 1)
		{
			speakers[0].TriggerSound(2);
			scrollingBarPosition -= scrollingBarIncrement;
			scrollerPosition += scrollerIncrement;
			ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
			Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
			CheevoRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum++;
			CheevoRows[highlightNum].ToggleHighlight(toggle: true);
			yield return new WaitForSeconds(0.33f);
			while (ControlHandler.mgr.CheckIsDownPressing() && highlightNum != CheevoRows.Length - 1)
			{
				yield return new WaitForSeconds(0.075f);
				speakers[0].TriggerSound(2);
				scrollingBarPosition -= scrollingBarIncrement;
				scrollerPosition += scrollerIncrement;
				ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
				Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
				CheevoRows[highlightNum].ToggleHighlight(toggle: false);
				highlightNum++;
				CheevoRows[highlightNum].ToggleHighlight(toggle: true);
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
			CheevoRows[highlightNum].ToggleHighlight(toggle: false);
			highlightNum--;
			CheevoRows[highlightNum].ToggleHighlight(toggle: true);
			yield return new WaitForSeconds(0.33f);
			while (ControlHandler.mgr.CheckIsUpPressing() && highlightNum != 0)
			{
				yield return new WaitForSeconds(0.075f);
				speakers[0].TriggerSound(2);
				scrollingBarPosition += scrollingBarIncrement;
				scrollerPosition -= scrollerIncrement;
				ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
				Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
				CheevoRows[highlightNum].ToggleHighlight(toggle: false);
				highlightNum--;
				CheevoRows[highlightNum].ToggleHighlight(toggle: true);
				yield return null;
			}
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
