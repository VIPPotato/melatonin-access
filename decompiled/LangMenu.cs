using System.Collections;
using UnityEngine;

public class LangMenu : Wrapper
{
	[Header("Children")]
	public Fader Fader;

	public ScrollingBar ScrollingBar;

	public Wrapper Scroller;

	[Header("Fragments")]
	public Fragment activator;

	public textboxFragment title;

	public textboxFragment info;

	public textboxFragment[] labels;

	public Fragment[] prompts;

	public textboxFragment[] langs;

	public Fragment highlight;

	public spriteFragment checkmark;

	public Fragment speaker;

	[Header("Props")]
	public float scrollingBarIncrement = 1f;

	public float scrollerIncrement = 1f;

	private int highlightNum;

	private float scrollingBarPosition;

	private float scrollerPosition;

	private Coroutine deactivating;

	private Coroutine navigating;

	protected override void Awake()
	{
		activator.Awake();
		title.Initiate();
		info.Initiate();
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		Fragment[] array2 = prompts;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Awake();
		}
		array = langs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		highlight.Awake();
		checkmark.Initiate();
		speaker.Awake();
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 1);
		activator.TriggerAnim("in");
		highlight.SetLocalY(langs[0].GetLocalY() + 0.02f);
		highlight.FadeInSprite(1f, 0.33f);
		checkmark.SetLocalY(langs[SaveManager.GetLang()].GetLocalY() + 0.02f);
		checkmark.FadeInSprite(1f, 0.33f);
		textboxFragment[] array = langs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FadeInText(1f, 0.33f);
		}
		ConfigLabels();
		speaker.TriggerSound(0);
		ScrollingBar.Activate();
		ScrollingBar.SetLocalY(0f);
		Scroller.SetLocalY(0f);
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompts[0].TriggerAnim("gamepadB");
			prompts[1].TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompts[0].TriggerAnim("gamepadCIRCLE");
			prompts[1].TriggerAnim("gamepadCROSS");
		}
		else
		{
			prompts[0].TriggerAnim("keyESC");
			prompts[1].TriggerAnim("keySPACE");
		}
		Fader.Activate();
		langs[langs.Length - 1].ToggleMeshRenderer(toggle: false);
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		activator.TriggerAnim("inReversed");
		speaker.TriggerSound(1);
		highlight.FadeOutSprite(1f, 0.33f);
		checkmark.FadeOutSprite(1f, 0.33f);
		textboxFragment[] array = langs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FadeOutText(1f, 0.33f);
		}
		Fader.Deactivate();
		ScrollingBar.Deactivate();
		scrollingBarPosition = 0f;
		scrollerPosition = 0f;
		highlightNum = 0;
		yield return new WaitForSeconds(0.5f);
		RenderChildren(toggle: false, 1);
	}

	public void Select()
	{
		checkmark.SetLocalY(langs[highlightNum].GetLocalY() + 0.02f);
		speaker.TriggerSound(3);
		SaveManager.mgr.SetLang(highlightNum);
		title.SetState(0);
		info.SetState(0);
		ConfigLabels();
	}

	public void Descend()
	{
		CancelCoroutine(navigating);
		navigating = StartCoroutine(Descending());
	}

	private IEnumerator Descending()
	{
		if (highlightNum >= langs.Length - 1)
		{
			yield break;
		}
		speaker.TriggerSound(2);
		scrollingBarPosition -= scrollingBarIncrement;
		scrollerPosition += scrollerIncrement;
		ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
		Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
		highlightNum++;
		highlight.SetLocalY(langs[highlightNum].GetLocalY() + 0.02f);
		if (highlightNum == 5)
		{
			langs[0].ToggleMeshRenderer(toggle: false);
			langs[langs.Length - 1].ToggleMeshRenderer(toggle: true);
			langs[langs.Length - 1].SetFontAlpha(1f);
		}
		yield return new WaitForSeconds(0.33f);
		while (ControlHandler.mgr.CheckIsDownPressing() && highlightNum != langs.Length - 1)
		{
			yield return new WaitForSeconds(0.1f);
			speaker.TriggerSound(2);
			scrollingBarPosition -= scrollingBarIncrement;
			scrollerPosition += scrollerIncrement;
			ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
			Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
			highlightNum++;
			highlight.SetLocalY(langs[highlightNum].GetLocalY() + 0.02f);
			if (highlightNum == 5)
			{
				langs[0].ToggleMeshRenderer(toggle: false);
				langs[langs.Length - 1].ToggleMeshRenderer(toggle: true);
				langs[langs.Length - 1].SetFontAlpha(1f);
			}
			yield return null;
		}
	}

	public void Ascend()
	{
		CancelCoroutine(navigating);
		navigating = StartCoroutine(Ascending());
	}

	private IEnumerator Ascending()
	{
		if (highlightNum <= 0)
		{
			yield break;
		}
		speaker.TriggerSound(2);
		scrollingBarPosition += scrollingBarIncrement;
		scrollerPosition -= scrollerIncrement;
		ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
		Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
		highlightNum--;
		highlight.SetLocalY(langs[highlightNum].GetLocalY() + 0.02f);
		if (highlightNum == 4)
		{
			langs[0].ToggleMeshRenderer(toggle: true);
			langs[0].SetFontAlpha(1f);
			langs[langs.Length - 1].ToggleMeshRenderer(toggle: false);
		}
		yield return new WaitForSeconds(0.33f);
		while (ControlHandler.mgr.CheckIsUpPressing() && highlightNum != 0)
		{
			yield return new WaitForSeconds(0.1f);
			speaker.TriggerSound(2);
			scrollingBarPosition += scrollingBarIncrement;
			scrollerPosition -= scrollerIncrement;
			ScrollingBar.MoveToLocalTarget(new Vector3(0f, scrollingBarPosition, 0f), 12f, isEasingIn: false);
			Scroller.MoveToLocalTarget(new Vector3(0f, scrollerPosition, 0f), 8f, isEasingIn: false);
			highlightNum--;
			highlight.SetLocalY(langs[highlightNum].GetLocalY() + 0.02f);
			if (highlightNum == 4)
			{
				langs[0].ToggleMeshRenderer(toggle: true);
				langs[0].SetFontAlpha(1f);
				langs[langs.Length - 1].ToggleMeshRenderer(toggle: false);
			}
			yield return null;
		}
	}

	private void ConfigLabels()
	{
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
}
