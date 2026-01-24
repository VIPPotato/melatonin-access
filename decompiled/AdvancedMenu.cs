using System.Collections;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

public class AdvancedMenu : Wrapper
{
	[Header("Fragments")]
	public Fragment speaker;

	public Fragment activator;

	public Fragment[] groups;

	public textboxFragment[] titles;

	public textboxFragment key;

	public textboxFragment explanation;

	public textboxFragment[] labels_music;

	public textboxFragment[] labels_share;

	public textboxFragment[] numbers;

	public Fragment[] buttons_music;

	public Fragment[] buttons_share;

	public textboxFragment[] tips;

	public spriteFragment[] tooltips;

	public spriteFragment[] prompts;

	public textboxFragment[] buttonLabels;

	public spriteFragment solid;

	public textboxFragment feedback;

	private float timeTilOut;

	private float timer;

	private bool isActivated;

	private bool isCustomized;

	private bool isNamed;

	private bool isImporting;

	private bool isUpdateDisabled;

	private string editorName;

	private string songFileName;

	private int rowNum;

	private int tabNum;

	private Coroutine deactivating;

	private Coroutine increasing;

	protected override void Awake()
	{
		speaker.Awake();
		activator.Awake();
		textboxFragment[] array = titles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		key.Initiate();
		explanation.Initiate();
		array = labels_music;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		array = labels_share;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		array = numbers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		Fragment[] array2 = buttons_music;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Awake();
		}
		array2 = buttons_share;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Awake();
		}
		array = tips;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		spriteFragment[] array3 = tooltips;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].Initiate();
		}
		array3 = prompts;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].Initiate();
		}
		array = buttonLabels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		solid.Initiate();
		feedback.Initiate();
		timeTilOut = activator.GetAnimDuration("inReversed");
		RenderChildren(toggle: false);
	}

	public void Initiate()
	{
		MusicBox.ResetCustomSongClip();
		key.SetText(key.GetText().Replace("[]", LvlEditor.dir.GetKey()));
		editorName = SceneMonitor.mgr.GetActiveSceneName();
		if (CheckIfSongExists())
		{
			ToggleIsCustomized(toggle: true);
		}
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(increasing);
		RenderChildren(toggle: true);
		isActivated = true;
		timer = 0f;
		rowNum = 0;
		tabNum = 0;
		groups[0].SetLocalX(0f);
		groups[1].SetLocalX(999f);
		activator.TriggerAnim("in");
		key.FadeInText(1f, 0.33f);
		Fragment[] array = buttons_music;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("inactive");
		}
		buttons_music[0].TriggerAnim("active");
		buttons_music[0].FadeInSprite(1f, 0.33f);
		labels_music[0].FadeInText(1f, 0.33f);
		textboxFragment[] array2;
		if (isCustomized)
		{
			buttons_music[1].FadeInSprite(1f, 0.33f);
			buttons_music[2].FadeInSprite(1f, 0.33f);
			buttons_music[3].FadeInSprite(1f, 0.33f);
			buttons_music[4].FadeInSprite(1f, 0.33f);
			labels_music[0].SetState(1);
			labels_music[1].FadeInText(1f, 0.33f);
			labels_music[2].FadeInText(1f, 0.33f);
			labels_music[3].FadeInText(1f, 0.33f);
			labels_music[4].FadeInText(1f, 0.33f);
			array2 = numbers;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].FadeInText(1f, 0.33f);
			}
		}
		else
		{
			buttons_music[1].FadeInSprite(0.5f, 0.33f);
			buttons_music[2].FadeInSprite(0.5f, 0.33f);
			buttons_music[3].FadeInSprite(0.5f, 0.33f);
			buttons_music[4].FadeInSprite(0.5f, 0.33f);
			labels_music[0].SetState(0);
			labels_music[1].FadeInText(0.5f, 0.33f);
			labels_music[2].FadeInText(0.5f, 0.33f);
			labels_music[3].FadeInText(0.5f, 0.33f);
			labels_music[4].FadeInText(0.5f, 0.33f);
			array2 = numbers;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].FadeInText(0.5f, 0.33f);
			}
		}
		array2 = tips;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].FadeInText(0.5f, 0.33f);
		}
		spriteFragment[] array3 = tooltips;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].FadeInSprite(0.5f, 0.33f);
		}
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompts[0].SetState(1);
			tips[0].SetState(1);
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompts[0].SetState(2);
			tips[0].SetState(1);
		}
		else
		{
			prompts[0].SetState(0);
			tips[0].SetState(0);
		}
		explanation.SetFontAlpha(1f);
		labels_share[0].SetFontAlpha(0.5f);
		labels_share[1].SetFontAlpha(1f);
		buttons_share[0].SetSpriteAlpha(1f);
		buttons_share[1].SetSpriteAlpha(1f);
		Interface.env.Disable();
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(increasing);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		activator.TriggerAnim("inReversed");
		if (tabNum == 0)
		{
			key.FadeOutText(1f, 0.33f);
			buttons_music[0].FadeOutSprite(1f, 0.33f);
			labels_music[0].FadeOutText(1f, 0.33f);
			textboxFragment[] array;
			if (isCustomized)
			{
				buttons_music[1].FadeOutSprite(1f, 0.33f);
				buttons_music[2].FadeOutSprite(1f, 0.33f);
				buttons_music[3].FadeOutSprite(1f, 0.33f);
				buttons_music[4].FadeOutSprite(1f, 0.33f);
				labels_music[1].FadeOutText(1f, 0.33f);
				labels_music[2].FadeOutText(1f, 0.33f);
				labels_music[3].FadeOutText(1f, 0.33f);
				labels_music[4].FadeOutText(1f, 0.33f);
				array = numbers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].FadeOutText(1f, 0.33f);
				}
			}
			else
			{
				buttons_music[1].FadeOutSprite(0.5f, 0.33f);
				buttons_music[2].FadeOutSprite(0.5f, 0.33f);
				buttons_music[3].FadeOutSprite(0.5f, 0.33f);
				buttons_music[4].FadeOutSprite(0.5f, 0.33f);
				labels_music[1].FadeOutText(0.5f, 0.33f);
				labels_music[2].FadeOutText(0.5f, 0.33f);
				labels_music[3].FadeOutText(0.5f, 0.33f);
				labels_music[4].FadeOutText(0.5f, 0.33f);
				array = numbers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].FadeOutText(0.5f, 0.33f);
				}
			}
			array = tips;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FadeOutText(0.5f, 0.33f);
			}
			spriteFragment[] array2 = tooltips;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].FadeOutSprite(0.5f, 0.33f);
			}
		}
		else
		{
			explanation.FadeOutText(1f, 0.33f);
			textboxFragment[] array = labels_share;
			foreach (textboxFragment obj in array)
			{
				obj.FadeOutText(obj.GetFontAlpha(), 0.33f);
			}
			buttons_share[0].FadeOutSprite(1f, 0.33f);
			buttons_share[1].FadeOutSprite(1f, 0.33f);
		}
		yield return null;
		Interface.env.Enable();
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (!isImporting && !isUpdateDisabled)
		{
			timer += Time.deltaTime;
			if (timer > 4f && !isImporting)
			{
				LookForSong();
				timer = 0f;
			}
		}
	}

	private void LookForSong()
	{
		if (!isCustomized && CheckIfSongExists())
		{
			ToggleIsCustomized(toggle: true);
		}
		else if (isCustomized && !CheckIfSongExists())
		{
			ToggleIsCustomized(toggle: false);
		}
	}

	public void SwapTab()
	{
		rowNum = 0;
		tabNum++;
		if (tabNum > 1)
		{
			tabNum = 0;
		}
		if (tabNum == 0)
		{
			groups[0].SetLocalX(0f);
			groups[1].SetLocalX(999f);
			ConfigMusicTab();
		}
		else
		{
			groups[0].SetLocalX(999f);
			groups[1].SetLocalX(0f);
			ConfigShareTab();
		}
	}

	public void Select()
	{
		StartCoroutine(Selecting());
	}

	private IEnumerator Selecting()
	{
		if (tabNum == 0)
		{
			if (rowNum == 0)
			{
				PickAndCopySongFile();
			}
			else if (rowNum == 4)
			{
				if (File.Exists(editorName + ".mp3"))
				{
					File.Delete(editorName + ".mp3");
				}
				if (File.Exists(editorName + ".wav"))
				{
					File.Delete(editorName + ".wav");
				}
				if (File.Exists(editorName + ".ogg"))
				{
					File.Delete(editorName + ".ogg");
				}
				ToggleIsCustomized(toggle: false);
				MusicBox.ResetCustomSongClip();
			}
		}
		else
		{
			if (tabNum != 1)
			{
				yield break;
			}
			if (rowNum == 0)
			{
				if (!isNamed)
				{
					isNamed = true;
					labels_share[0].SetText("|");
				}
				else
				{
					labels_share[0].SetText(labels_share[0].GetText() + "|");
				}
				labels_share[0].SetFontAlpha(1f);
				solid.ToggleSpriteRenderer(toggle: true);
				LvlEditor.dir.ToggleIsEnabled(toggle: false);
				ControlHandler.mgr.GetKeyboard().onTextInput += InputLetter;
			}
			if (rowNum == 1)
			{
				LvlEditor.dir.ToggleIsEnabled(toggle: false);
				SteamWorkshop.mgr.Upload(labels_share[0].GetText());
				feedback.ToggleMeshRenderer(toggle: false);
				labels_share[1].SetFontAlpha(0.33f);
				labels_share[1].SetState(1);
				yield return new WaitUntil(() => !SteamWorkshop.mgr.CheckIsUploading());
				feedback.ToggleMeshRenderer(toggle: true);
				if (SteamWorkshop.mgr.CheckIsLastUploadSuccessful())
				{
					feedback.SetState(0);
				}
				else
				{
					feedback.SetState(1);
				}
				LvlEditor.dir.ToggleIsEnabled(toggle: true);
				labels_share[1].SetState(0);
				labels_share[1].SetFontAlpha(1f);
			}
		}
	}

	public void NextRow()
	{
		if (tabNum == 0 && isCustomized)
		{
			rowNum++;
			if (rowNum >= buttons_music.Length)
			{
				rowNum = 0;
			}
			ConfigMusicTab();
		}
		else if (tabNum == 1)
		{
			rowNum++;
			if (rowNum >= buttons_share.Length)
			{
				rowNum = 0;
			}
			ConfigShareTab();
		}
	}

	public void PrevRow()
	{
		if (tabNum == 0 && isCustomized)
		{
			rowNum--;
			if (rowNum < 0)
			{
				rowNum = buttons_music.Length - 1;
			}
			ConfigMusicTab();
		}
		else if (tabNum == 1)
		{
			rowNum--;
			if (rowNum < 0)
			{
				rowNum = buttons_share.Length - 1;
			}
			ConfigShareTab();
		}
	}

	public void Increase()
	{
		CancelCoroutine(increasing);
		increasing = StartCoroutine(Increasing());
	}

	private IEnumerator Increasing()
	{
		if (tabNum != 0 || !isCustomized)
		{
			yield break;
		}
		string text = "";
		int num = 0;
		float decimalNum = 0f;
		if (rowNum == 1)
		{
			text = numbers[0].GetText();
		}
		else if (rowNum == 2)
		{
			text = numbers[1].GetText();
		}
		else if (rowNum == 3)
		{
			text = numbers[2].GetText();
		}
		text = text.Replace("< ", "");
		text = text.Replace(" >", "");
		if (rowNum == 1)
		{
			decimalNum = float.Parse(text);
			decimalNum += 1f;
			if (decimalNum > 200f)
			{
				decimalNum = 200f;
			}
			numbers[0].SetText("< " + decimalNum + " >");
			SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
		}
		else if (rowNum == 2)
		{
			num = int.Parse(text);
			num++;
			if (num > 108)
			{
				num = 108;
			}
			numbers[1].SetText("< " + num + " >");
			LvlEditor.dir.SetBarsLength(num);
			SaveManager.mgr.SetEditorCustomSongLength(num);
		}
		else if (rowNum == 3)
		{
			num = int.Parse(text);
			num++;
			numbers[2].SetText("< " + num + " >");
			SaveManager.mgr.SetEditorCustomSongSyncOffset(num);
		}
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsRightPressing())
		{
			if (rowNum == 1)
			{
				decimalNum += 1f;
				if (decimalNum > 200f)
				{
					break;
				}
				numbers[0].SetText("< " + decimalNum + " >");
				SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
				EditorUI.env.PlayToggleSfx();
			}
			else if (rowNum == 2)
			{
				num++;
				if (num > 108)
				{
					break;
				}
				numbers[1].SetText("< " + num + " >");
				LvlEditor.dir.SetBarsLength(num);
				SaveManager.mgr.SetEditorCustomSongLength(num);
				EditorUI.env.PlayToggleSfx();
			}
			else if (rowNum == 3)
			{
				num++;
				numbers[2].SetText("< " + num + " >");
				SaveManager.mgr.SetEditorCustomSongSyncOffset(num);
				EditorUI.env.PlayToggleSfx();
			}
			numMoves++;
			if (numMoves >= 32)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
		}
	}

	public void Decrease()
	{
		CancelCoroutine(increasing);
		increasing = StartCoroutine(Decreasing());
	}

	private IEnumerator Decreasing()
	{
		if (tabNum != 0 || !isCustomized)
		{
			yield break;
		}
		string text = "";
		int num = 0;
		float decimalNum = 0f;
		if (rowNum == 1)
		{
			text = numbers[0].GetText();
		}
		else if (rowNum == 2)
		{
			text = numbers[1].GetText();
		}
		else if (rowNum == 3)
		{
			text = numbers[2].GetText();
		}
		text = text.Replace("< ", "");
		text = text.Replace(" >", "");
		if (rowNum == 1)
		{
			decimalNum = float.Parse(text);
			decimalNum -= 1f;
			if (decimalNum < 1f)
			{
				decimalNum = 1f;
			}
			numbers[0].SetText("< " + decimalNum + " >");
			SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
		}
		else if (rowNum == 2)
		{
			num = int.Parse(text);
			num--;
			if (num < 1)
			{
				num = 1;
			}
			numbers[1].SetText("< " + num + " >");
			LvlEditor.dir.SetBarsLength(num);
			SaveManager.mgr.SetEditorCustomSongLength(num);
		}
		else if (rowNum == 3)
		{
			num = int.Parse(text);
			num--;
			numbers[2].SetText("< " + num + " >");
			SaveManager.mgr.SetEditorCustomSongSyncOffset(num);
		}
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsLeftPressing())
		{
			if (rowNum == 1)
			{
				decimalNum -= 1f;
				if (decimalNum < 1f)
				{
					break;
				}
				numbers[0].SetText("< " + decimalNum + " >");
				SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
				EditorUI.env.PlayToggleSfx();
			}
			else if (rowNum == 2)
			{
				num--;
				if (num < 1)
				{
					break;
				}
				numbers[1].SetText("< " + num + " >");
				LvlEditor.dir.SetBarsLength(num);
				SaveManager.mgr.SetEditorCustomSongLength(num);
				EditorUI.env.PlayToggleSfx();
			}
			else if (rowNum == 3)
			{
				num--;
				numbers[2].SetText("< " + num + " >");
				SaveManager.mgr.SetEditorCustomSongSyncOffset(num);
				EditorUI.env.PlayToggleSfx();
			}
			numMoves++;
			if (numMoves >= 32)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
		}
	}

	public void Increment()
	{
		CancelCoroutine(increasing);
		increasing = StartCoroutine(Incrementing());
	}

	private IEnumerator Incrementing()
	{
		if (tabNum == 0 && rowNum == 1)
		{
			string text = numbers[0].GetText();
			text = text.Replace("< ", "");
			text = text.Replace(" >", "");
			float decimalNum = float.Parse(text) * 100f;
			int num = Mathf.RoundToInt(decimalNum);
			num++;
			decimalNum = (float)num / 100f;
			if (decimalNum > 200f)
			{
				decimalNum = 200f;
			}
			numbers[0].SetText("< " + decimalNum + " >");
			SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
			yield return new WaitForSeconds(0.33f);
			int numMoves = 0;
			while (ControlHandler.mgr.CheckIsActionRightPressing())
			{
				decimalNum *= 100f;
				num = Mathf.RoundToInt(decimalNum);
				num++;
				decimalNum = (float)num / 100f;
				if (decimalNum > 200f)
				{
					break;
				}
				numbers[0].SetText("< " + decimalNum + " >");
				SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
				EditorUI.env.PlayToggleSfx();
				numMoves++;
				if (numMoves >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
		else if (tabNum == 0 && rowNum == 2)
		{
			string text2 = "";
			if (rowNum == 1)
			{
				text2 = numbers[0].GetText();
			}
			else if (rowNum == 2)
			{
				text2 = numbers[1].GetText();
			}
			else if (rowNum == 3)
			{
				text2 = numbers[2].GetText();
			}
			text2 = text2.Replace("< ", "");
			text2 = text2.Replace(" >", "");
			int numMoves = int.Parse(text2);
			numMoves += 10;
			if (numMoves > 108)
			{
				numMoves = 108;
			}
			numbers[1].SetText("< " + numMoves + " >");
			LvlEditor.dir.SetBarsLength(numMoves);
			SaveManager.mgr.SetEditorCustomSongLength(numMoves);
			yield return new WaitForSeconds(0.33f);
			int numMoves2 = 0;
			while (ControlHandler.mgr.CheckIsRightPressing())
			{
				numMoves += 10;
				if (numMoves > 108)
				{
					break;
				}
				numbers[1].SetText("< " + numMoves + " >");
				LvlEditor.dir.SetBarsLength(numMoves);
				SaveManager.mgr.SetEditorCustomSongLength(numMoves);
				EditorUI.env.PlayToggleSfx();
				numMoves2++;
				if (numMoves2 >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves2 >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
		else
		{
			if (tabNum != 0 || rowNum != 3)
			{
				yield break;
			}
			string text3 = "";
			if (rowNum == 1)
			{
				text3 = numbers[0].GetText();
			}
			else if (rowNum == 2)
			{
				text3 = numbers[1].GetText();
			}
			else if (rowNum == 3)
			{
				text3 = numbers[2].GetText();
			}
			text3 = text3.Replace("< ", "");
			text3 = text3.Replace(" >", "");
			int numMoves2 = int.Parse(text3);
			numMoves2 += 10;
			numbers[2].SetText("< " + numMoves2 + " >");
			SaveManager.mgr.SetEditorCustomSongSyncOffset(numMoves2);
			yield return new WaitForSeconds(0.33f);
			int numMoves = 0;
			while (ControlHandler.mgr.CheckIsRightPressing())
			{
				numMoves2 += 10;
				numbers[2].SetText("< " + numMoves2 + " >");
				SaveManager.mgr.SetEditorCustomSongSyncOffset(numMoves2);
				EditorUI.env.PlayToggleSfx();
				numMoves++;
				if (numMoves >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
	}

	public void Diminish()
	{
		CancelCoroutine(increasing);
		increasing = StartCoroutine(Diminishing());
	}

	private IEnumerator Diminishing()
	{
		if (tabNum == 0 && rowNum == 1)
		{
			string text = numbers[0].GetText();
			text = text.Replace("< ", "");
			text = text.Replace(" >", "");
			float decimalNum = float.Parse(text) * 100f;
			int num = Mathf.RoundToInt(decimalNum);
			num--;
			decimalNum = (float)num / 100f;
			if (decimalNum < 1f)
			{
				decimalNum = 1f;
			}
			numbers[0].SetText("< " + decimalNum + " >");
			SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
			yield return new WaitForSeconds(0.33f);
			int numMoves = 0;
			while (ControlHandler.mgr.CheckIsActionLeftPressing())
			{
				decimalNum *= 100f;
				num = Mathf.RoundToInt(decimalNum);
				num--;
				decimalNum = (float)num / 100f;
				if (decimalNum < 1f)
				{
					break;
				}
				numbers[0].SetText("< " + decimalNum + " >");
				SaveManager.mgr.SetEditorCustomSongTempo(decimalNum);
				EditorUI.env.PlayToggleSfx();
				numMoves++;
				if (numMoves >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
		else if (tabNum == 0 && rowNum == 2)
		{
			string text2 = "";
			if (rowNum == 1)
			{
				text2 = numbers[0].GetText();
			}
			else if (rowNum == 2)
			{
				text2 = numbers[1].GetText();
			}
			else if (rowNum == 3)
			{
				text2 = numbers[2].GetText();
			}
			text2 = text2.Replace("< ", "");
			text2 = text2.Replace(" >", "");
			int numMoves = int.Parse(text2);
			numMoves -= 10;
			if (numMoves < 1)
			{
				numMoves = 1;
			}
			numbers[1].SetText("< " + numMoves + " >");
			LvlEditor.dir.SetBarsLength(numMoves);
			SaveManager.mgr.SetEditorCustomSongLength(numMoves);
			yield return new WaitForSeconds(0.33f);
			int numMoves2 = 0;
			while (ControlHandler.mgr.CheckIsLeftPressing())
			{
				numMoves -= 10;
				if (numMoves < 1)
				{
					break;
				}
				numbers[1].SetText("< " + numMoves + " >");
				LvlEditor.dir.SetBarsLength(numMoves);
				SaveManager.mgr.SetEditorCustomSongLength(numMoves);
				EditorUI.env.PlayToggleSfx();
				numMoves2++;
				if (numMoves2 >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves2 >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
		else
		{
			if (tabNum != 0 || rowNum != 3)
			{
				yield break;
			}
			string text3 = "";
			if (rowNum == 1)
			{
				text3 = numbers[0].GetText();
			}
			else if (rowNum == 2)
			{
				text3 = numbers[1].GetText();
			}
			else if (rowNum == 3)
			{
				text3 = numbers[2].GetText();
			}
			text3 = text3.Replace("< ", "");
			text3 = text3.Replace(" >", "");
			int numMoves2 = int.Parse(text3);
			numMoves2 -= 10;
			numbers[2].SetText("< " + numMoves2 + " >");
			SaveManager.mgr.SetEditorCustomSongSyncOffset(numMoves2);
			yield return new WaitForSeconds(0.33f);
			int numMoves = 0;
			while (ControlHandler.mgr.CheckIsLeftPressing())
			{
				numMoves2 -= 10;
				numbers[2].SetText("< " + numMoves2 + " >");
				SaveManager.mgr.SetEditorCustomSongSyncOffset(numMoves2);
				EditorUI.env.PlayToggleSfx();
				numMoves++;
				if (numMoves >= 32)
				{
					yield return new WaitForSeconds(0.02f);
				}
				else if (numMoves >= 16)
				{
					yield return new WaitForSeconds(0.033f);
				}
				else
				{
					yield return new WaitForSeconds(0.067f);
				}
			}
		}
	}

	private void InputLetter(char character)
	{
		StartCoroutine(InputtingLetter(character));
	}

	private IEnumerator InputtingLetter(char character)
	{
		bool isPressed = ControlHandler.mgr.GetKeyboard().shiftKey.isPressed;
		switch (character)
		{
		case '\b':
			if (labels_share[0].GetText().Length > 1)
			{
				speaker.TriggerSound(0);
				string text2 = labels_share[0].GetText().Substring(0, labels_share[0].GetText().Length - 2);
				labels_share[0].SetText(text2 + "|");
			}
			yield break;
		case '\n':
		case '\r':
		case '\u001b':
		{
			solid.ToggleSpriteRenderer(toggle: false);
			EditorUI.env.PlaySelectSfx();
			ControlHandler.mgr.GetKeyboard().onTextInput -= InputLetter;
			string text = labels_share[0].GetText().Substring(0, labels_share[0].GetText().Length - 1);
			labels_share[0].SetText(text);
			labels_share[0].SetFontAlpha(0.5f);
			yield return null;
			LvlEditor.dir.ToggleIsEnabled(toggle: true);
			yield break;
		}
		}
		if (char.IsLetterOrDigit(character) || char.IsSymbol(character) || char.IsPunctuation(character) || character == ' ')
		{
			if (!isPressed && char.IsLetter(character))
			{
				character = char.ToLower(character);
			}
			if (labels_share[0].GetText().Length <= 30)
			{
				speaker.TriggerSound(0);
				string text3 = labels_share[0].GetText().Substring(0, labels_share[0].GetText().Length - 1);
				labels_share[0].SetText(text3 + character + "|");
			}
		}
	}

	private void PickAndCopySongFile()
	{
		Cursor.visible = true;
		ExtensionFilter[] extensions = new ExtensionFilter[1]
		{
			new ExtensionFilter("Sound Files", "mp3", "wav", "ogg")
		};
		string[] array = StandaloneFileBrowser.OpenFilePanel("Select a song file", "", extensions, multiselect: false);
		if (array.Length != 0)
		{
			if (array[0][array[0].Length - 1] == '3')
			{
				songFileName = editorName + ".mp3";
			}
			else if (array[0][array[0].Length - 1] == 'v')
			{
				songFileName = editorName + ".wav";
			}
			else if (array[0][array[0].Length - 1] == 'g')
			{
				songFileName = editorName + ".ogg";
			}
			string text = Application.dataPath + "/" + songFileName;
			text = text.Replace("/Melatonin_Data", "");
			if (File.Exists(editorName + ".mp3"))
			{
				File.Delete(editorName + ".mp3");
			}
			if (File.Exists(editorName + ".wav"))
			{
				File.Delete(editorName + ".wav");
			}
			if (File.Exists(editorName + ".ogg"))
			{
				File.Delete(editorName + ".ogg");
			}
			File.Copy(array[0], text);
			ToggleIsCustomized(toggle: true);
		}
		Cursor.visible = false;
	}

	private void ImportSong()
	{
		StartCoroutine(ImportingSong());
	}

	private IEnumerator ImportingSong()
	{
		bool isTryAgain = false;
		string filePath;
		if (LvlEditor.GetDownloadFilePath() == "")
		{
			filePath = Application.dataPath + "/" + songFileName;
			filePath = filePath.Replace("/Melatonin_Data", "");
		}
		else
		{
			filePath = LvlEditor.GetDownloadFilePath() + songFileName;
		}
		isImporting = true;
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
		{
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
			{
				MonoBehaviour.print("Failed to import audio clip: " + www.error + ". Will try again with file:// added to path.");
				filePath = "file://" + filePath;
				isTryAgain = true;
			}
			else
			{
				AudioClip content = DownloadHandlerAudioClip.GetContent(www);
				if (content != null)
				{
					MusicBox.SetCustomSongClip(content);
				}
				else
				{
					MonoBehaviour.print("Audio clip issue");
				}
			}
		}
		if (isTryAgain)
		{
			using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN);
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
			{
				MonoBehaviour.print("Failed to import audio clip: " + www.error + ".");
			}
			else
			{
				AudioClip content2 = DownloadHandlerAudioClip.GetContent(www);
				if (content2 != null)
				{
					MusicBox.SetCustomSongClip(content2);
				}
				else
				{
					MonoBehaviour.print("Audio clip issue");
				}
			}
		}
		isImporting = false;
	}

	public void ToggleIsUpdateDisabled(bool toggle)
	{
		isUpdateDisabled = toggle;
	}

	private void ConfigMusicTab()
	{
		Fragment[] array = buttons_music;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("inactive");
		}
		textboxFragment[] array2 = tips;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetFontAlpha(0.5f);
		}
		spriteFragment[] array3 = tooltips;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].SetSpriteAlpha(0.5f);
		}
		buttons_music[rowNum].TriggerAnim("active");
		if (rowNum == 1)
		{
			tips[0].SetFontAlpha(1f);
			tooltips[0].SetSpriteAlpha(1f);
		}
		else if (rowNum == 2)
		{
			tips[1].SetFontAlpha(1f);
			tooltips[1].SetSpriteAlpha(1f);
		}
		else if (rowNum == 3)
		{
			tips[2].SetFontAlpha(1f);
			tooltips[2].SetSpriteAlpha(1f);
		}
		array2 = tips;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetFontAlpha(0.5f);
		}
		array3 = tooltips;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].SetSpriteAlpha(0.5f);
		}
		if (rowNum == 1)
		{
			tips[0].SetFontAlpha(1f);
			tooltips[0].SetSpriteAlpha(1f);
		}
		else if (rowNum == 2)
		{
			tips[1].SetFontAlpha(1f);
			tooltips[1].SetSpriteAlpha(1f);
		}
		else if (rowNum == 3)
		{
			tips[2].SetFontAlpha(1f);
			tooltips[2].SetSpriteAlpha(1f);
		}
	}

	private void ConfigShareTab()
	{
		labels_share[1].SetState(0);
		feedback.ToggleMeshRenderer(toggle: false);
		Fragment[] array = buttons_share;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("inactive");
		}
		buttons_share[rowNum].TriggerAnim("active");
	}

	private void ToggleIsCustomized(bool toggle)
	{
		rowNum = 0;
		if (toggle)
		{
			ImportSong();
			isCustomized = true;
			numbers[0].SetText("< " + SaveManager.mgr.GetEditorData().customSongTempo + " >");
			numbers[1].SetText("< " + SaveManager.mgr.GetEditorData().customSongLength + " >");
			numbers[2].SetText("< " + SaveManager.mgr.GetEditorData().customSongSyncOffset + " >");
			LvlEditor.dir.SetBarsLength(SaveManager.mgr.GetEditorData().customSongLength);
			if (isActivated)
			{
				buttons_music[1].SetSpriteAlpha(1f);
				buttons_music[2].SetSpriteAlpha(1f);
				buttons_music[3].SetSpriteAlpha(1f);
				buttons_music[4].SetSpriteAlpha(1f);
				labels_music[0].SetState(1);
				labels_music[1].SetFontAlpha(1f);
				labels_music[2].SetFontAlpha(1f);
				labels_music[3].SetFontAlpha(1f);
				labels_music[4].SetFontAlpha(1f);
				numbers[0].SetFontAlpha(1f);
				numbers[1].SetFontAlpha(1f);
				numbers[2].SetFontAlpha(1f);
			}
			return;
		}
		isCustomized = false;
		MusicBox.ResetCustomSongClip();
		LvlEditor.dir.RevertBarsLength();
		if (isActivated)
		{
			Fragment[] array = buttons_music;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TriggerAnim("inactive");
			}
			buttons_music[0].TriggerAnim("active");
			buttons_music[1].SetSpriteAlpha(0.5f);
			buttons_music[2].SetSpriteAlpha(0.5f);
			buttons_music[3].SetSpriteAlpha(0.5f);
			buttons_music[4].SetSpriteAlpha(0.5f);
			labels_music[0].SetState(0);
			labels_music[1].SetFontAlpha(0.5f);
			labels_music[2].SetFontAlpha(0.5f);
			labels_music[3].SetFontAlpha(0.5f);
			labels_music[4].SetFontAlpha(0.5f);
			numbers[0].SetFontAlpha(0.5f);
			numbers[1].SetFontAlpha(0.5f);
			numbers[2].SetFontAlpha(0.5f);
		}
	}

	private bool CheckIfSongExists()
	{
		if (File.Exists(LvlEditor.GetDownloadFilePath() + editorName + ".mp3"))
		{
			songFileName = editorName + ".mp3";
			return true;
		}
		if (File.Exists(LvlEditor.GetDownloadFilePath() + editorName + ".wav"))
		{
			songFileName = editorName + ".wav";
			return true;
		}
		if (File.Exists(LvlEditor.GetDownloadFilePath() + editorName + ".ogg"))
		{
			songFileName = editorName + ".ogg";
			return true;
		}
		songFileName = "";
		return false;
	}

	public int GetRowNum()
	{
		return rowNum;
	}

	public int GetTabNum()
	{
		return tabNum;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsCustomized()
	{
		return isCustomized;
	}

	public bool CheckIsImporting()
	{
		return isImporting;
	}
}
