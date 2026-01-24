using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class SaveManager : Custom
{
	public static SaveManager mgr;

	private bool isSavingPlayerDataStacked;

	private bool isSavingPlayerDataStackedQueued;

	private bool isSavingEditorDataStacked;

	private bool isSavingEditorDataStackedQueued;

	private Coroutine savingPlayerDataStacked;

	private Coroutine savingPlayerDataStackedQueued;

	private Coroutine savingEditorDataStacked;

	private Coroutine savingEditorDataStackedQueued;

	private static playerData playerData = new playerData();

	private static editorData editorData = new editorData();

	private void Awake()
	{
		mgr = this;
	}

	public void LoadPlayerData()
	{
		try
		{
			if (new FileInfo("save.json").Length == 0L)
			{
				throw new Exception();
			}
			playerData = JsonUtility.FromJson<playerData>(File.ReadAllText("save.json"));
		}
		catch
		{
			try
			{
				if (new FileInfo("backup.json").Length == 0L)
				{
					throw new Exception();
				}
				playerData = JsonUtility.FromJson<playerData>(File.ReadAllText("backup.json"));
				File.WriteAllText("save.json", JsonUtility.ToJson(playerData));
			}
			catch
			{
				playerData = new playerData();
				File.WriteAllText("save.json", JsonUtility.ToJson(playerData));
			}
		}
	}

	public void LoadEditorData(string filePath = "")
	{
		string editorName = LvlEditor.GetEditorName();
		try
		{
			if (new FileInfo(filePath + editorName + ".json").Length == 0L)
			{
				throw new Exception();
			}
			editorData = JsonUtility.FromJson<editorData>(File.ReadAllText(filePath + editorName + ".json"));
		}
		catch
		{
			editorData = new editorData();
			File.WriteAllText(filePath + editorName + ".json", JsonUtility.ToJson(editorData));
		}
	}

	public void SavePlayerData()
	{
		isSavingPlayerDataStacked = false;
		isSavingPlayerDataStackedQueued = false;
		CancelCoroutine(savingPlayerDataStacked);
		CancelCoroutine(savingPlayerDataStackedQueued);
		File.WriteAllText("saveTemp.json", JsonUtility.ToJson(playerData));
		if (File.Exists("backup.json"))
		{
			File.Delete("backup.json");
		}
		if (File.Exists("save.json"))
		{
			File.Move("save.json", "backup.json");
		}
		File.Move("saveTemp.json", "save.json");
	}

	private void SavePlayerDataStacked()
	{
		if (isSavingPlayerDataStacked && !isSavingPlayerDataStackedQueued)
		{
			savingPlayerDataStackedQueued = StartCoroutine(SavingPlayerDataStackedQueued());
		}
		else if (!isSavingPlayerDataStacked)
		{
			savingPlayerDataStacked = StartCoroutine(SavingPlayerDataStacked());
		}
	}

	private IEnumerator SavingPlayerDataStackedQueued()
	{
		isSavingPlayerDataStackedQueued = true;
		yield return new WaitUntil(() => !isSavingPlayerDataStacked);
		isSavingPlayerDataStackedQueued = false;
		savingPlayerDataStacked = StartCoroutine(SavingPlayerDataStacked());
	}

	private IEnumerator SavingPlayerDataStacked()
	{
		isSavingPlayerDataStacked = true;
		File.WriteAllText("saveTemp.json", JsonUtility.ToJson(playerData));
		if (File.Exists("backup.json"))
		{
			File.Delete("backup.json");
		}
		if (File.Exists("save.json"))
		{
			File.Move("save.json", "backup.json");
		}
		File.Move("saveTemp.json", "save.json");
		yield return new WaitForSecondsRealtime(2f);
		isSavingPlayerDataStacked = false;
	}

	public void SaveEditorData()
	{
		isSavingEditorDataStacked = false;
		isSavingEditorDataStackedQueued = false;
		CancelCoroutine(savingEditorDataStacked);
		CancelCoroutine(savingEditorDataStackedQueued);
		File.WriteAllText(LvlEditor.GetEditorName() + ".json", JsonUtility.ToJson(editorData));
	}

	private void SaveEditorDataStacked()
	{
		if (isSavingEditorDataStacked && !isSavingEditorDataStackedQueued)
		{
			savingEditorDataStackedQueued = StartCoroutine(SavingEditorDataStackedQueued());
		}
		else if (!isSavingEditorDataStacked)
		{
			savingEditorDataStacked = StartCoroutine(SavingEditorDataStacked());
		}
	}

	private IEnumerator SavingEditorDataStackedQueued()
	{
		isSavingEditorDataStackedQueued = true;
		yield return new WaitUntil(() => !isSavingEditorDataStacked);
		isSavingEditorDataStackedQueued = false;
		savingEditorDataStacked = StartCoroutine(SavingEditorDataStacked());
	}

	private IEnumerator SavingEditorDataStacked()
	{
		isSavingEditorDataStacked = true;
		File.WriteAllText(LvlEditor.GetEditorName() + ".json", JsonUtility.ToJson(editorData));
		yield return new WaitForSecondsRealtime(2f);
		isSavingEditorDataStacked = false;
	}

	public void ClearPlayerData()
	{
		playerData = new playerData();
		if (File.Exists("save.json"))
		{
			File.WriteAllText("save.json", JsonUtility.ToJson(playerData));
		}
		if (File.Exists("backup.json"))
		{
			File.WriteAllText("backup.json", JsonUtility.ToJson(playerData));
		}
	}

	public void ToggleIsVisualAssisting(bool toggle)
	{
		playerData.isVisualAssisting = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleAudioAssisting(bool toggle)
	{
		playerData.isAudioAssisting = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsBiggerHitWindows(bool toggle)
	{
		playerData.isBiggerHitWindows = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsEasyScoring(bool toggle)
	{
		playerData.isEasyScoring = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsVibrationDisabled(bool toggle)
	{
		playerData.isVibrationDisabled = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsGameComplete(bool toggle)
	{
		playerData.igc = toggle;
		SavePlayerData();
	}

	public void ToggleIsDirectionKeysAlt(bool toggle)
	{
		playerData.isDirectionKeysAlt = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsTp(bool toggle)
	{
		playerData.isTp = toggle;
		SavePlayerData();
	}

	public void ToggleIsCreator(bool toggle)
	{
		playerData.isCreator = toggle;
		SavePlayerData();
	}

	public void ToggleIsWarmth(bool toggle)
	{
		playerData.isWarmth = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsPerfectsOnly(bool toggle)
	{
		playerData.isPerfectsOnly = toggle;
		SavePlayerDataStacked();
	}

	public void ToggleIsVsynced(bool toggle)
	{
		playerData.isVsynced = toggle;
		SavePlayerDataStacked();
	}

	public void SetActionKey(string key)
	{
		playerData.actionKey = key;
		SavePlayerDataStacked();
	}

	public void SetLang(int newLang)
	{
		playerData.lang = newLang;
		SavePlayerDataStacked();
	}

	public void SetScreenshake(int newScreenshake)
	{
		playerData.screenshake = newScreenshake;
		SavePlayerDataStacked();
	}

	public void SetCalibrationOffsetMs(int newCalibrationOffsetMs)
	{
		playerData.calibrationOffsetMs = newCalibrationOffsetMs;
		SavePlayerDataStacked();
	}

	public void SetAudioOffsetMs(int newAudioOffsetMs)
	{
		playerData.audioOffsetMs = newAudioOffsetMs;
		SavePlayerDataStacked();
	}

	public void SetMaster(int newVolume)
	{
		playerData.master = newVolume;
		SavePlayerDataStacked();
	}

	public void SetMusic(int newVolume)
	{
		playerData.music = newVolume;
		SavePlayerDataStacked();
	}

	public void SetSfx(int newVolume)
	{
		playerData.sfx = newVolume;
		SavePlayerDataStacked();
	}

	public void SetMetronome(int newVolume)
	{
		playerData.metronome = newVolume;
		SavePlayerDataStacked();
	}

	public void SetContrast(int newContrast)
	{
		playerData.contrast = newContrast;
		SavePlayerDataStacked();
	}

	public void SetChapterNum(int newChapterNum)
	{
		playerData.cn = newChapterNum;
		SavePlayerData();
	}

	public void SetScore(string dreamName, int newScore)
	{
		switch (dreamName)
		{
		case "Dream_food":
			playerData.fd = newScore;
			break;
		case "Dream_foodAlt":
			playerData.fdAlt = newScore;
			break;
		case "Dream_shopping":
			playerData.sp = newScore;
			break;
		case "Dream_shoppingAlt":
			playerData.spAlt = newScore;
			break;
		case "Dream_tech":
			playerData.tc = newScore;
			break;
		case "Dream_techAlt":
			playerData.tcAlt = newScore;
			break;
		case "Dream_followers":
			playerData.fw = newScore;
			break;
		case "Dream_followersAlt":
			playerData.fwAlt = newScore;
			break;
		case "Dream_indulgence":
			playerData.id = newScore;
			break;
		case "Dream_indulgenceAlt":
			playerData.idAlt = newScore;
			break;
		case "Dream_exercise":
			playerData.ec = newScore;
			break;
		case "Dream_exerciseAlt":
			playerData.ecAlt = newScore;
			break;
		case "Dream_career":
			playerData.cr = newScore;
			break;
		case "Dream_careerAlt":
			playerData.crAlt = newScore;
			break;
		case "Dream_money":
			playerData.sv = newScore;
			break;
		case "Dream_moneyAlt":
			playerData.svAlt = newScore;
			break;
		case "Dream_dating":
			playerData.dt = newScore;
			break;
		case "Dream_datingAlt":
			playerData.dtAlt = newScore;
			break;
		case "Dream_pressure":
			playerData.pr = newScore;
			break;
		case "Dream_pressureAlt":
			playerData.prAlt = newScore;
			break;
		case "Dream_time":
			playerData.ft = newScore;
			break;
		case "Dream_timeAlt":
			playerData.ftAlt = newScore;
			break;
		case "Dream_mind":
			playerData.mn = newScore;
			break;
		case "Dream_mindAlt":
			playerData.mnAlt = newScore;
			break;
		case "Dream_space":
			playerData.wr = newScore;
			break;
		case "Dream_spaceAlt":
			playerData.wrAlt = newScore;
			break;
		case "Dream_nature":
			playerData.nr = newScore;
			break;
		case "Dream_natureAlt":
			playerData.nrAlt = newScore;
			break;
		case "Dream_meditation":
			playerData.md = newScore;
			break;
		case "Dream_meditationAlt":
			playerData.mdAlt = newScore;
			break;
		case "Dream_stress":
			playerData.fr = newScore;
			break;
		case "Dream_stressAlt":
			playerData.frAlt = newScore;
			break;
		case "Dream_desires":
			playerData.me = newScore;
			break;
		case "Dream_desiresAlt":
			playerData.meAlt = newScore;
			break;
		case "Dream_past":
			playerData.ps = newScore;
			break;
		case "Dream_pastAlt":
			playerData.psAlt = newScore;
			break;
		case "Dream_future":
			playerData.fu = newScore;
			break;
		case "Dream_futureAlt":
			playerData.fuAlt = newScore;
			break;
		case "Dream_setbacks":
			playerData.ax = newScore;
			break;
		case "Dream_setbacksAlt":
			playerData.axAlt = newScore;
			break;
		case "Dream_final":
			playerData.fn = newScore;
			break;
		case "Dream_finalAlt":
			playerData.fnAlt = newScore;
			break;
		}
		SavePlayerData();
	}

	public void SetEditorDataCode(int phrase, int bar, int beat, string code)
	{
		switch (phrase)
		{
		case 1:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar1[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar2[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar3[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar4[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar5[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar6[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar7[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar8[beat - 1] = code;
				break;
			}
			break;
		case 2:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar9[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar10[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar11[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar12[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar13[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar14[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar15[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar16[beat - 1] = code;
				break;
			}
			break;
		case 3:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar17[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar18[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar19[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar20[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar21[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar22[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar23[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar24[beat - 1] = code;
				break;
			}
			break;
		case 4:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar25[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar26[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar27[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar28[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar29[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar30[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar31[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar32[beat - 1] = code;
				break;
			}
			break;
		case 5:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar33[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar34[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar35[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar36[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar37[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar38[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar39[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar40[beat - 1] = code;
				break;
			}
			break;
		case 6:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar41[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar42[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar43[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar44[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar45[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar46[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar47[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar48[beat - 1] = code;
				break;
			}
			break;
		case 7:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar49[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar50[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar51[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar52[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar53[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar54[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar55[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar56[beat - 1] = code;
				break;
			}
			break;
		case 8:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar57[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar58[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar59[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar60[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar61[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar62[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar63[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar64[beat - 1] = code;
				break;
			}
			break;
		case 9:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar65[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar66[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar67[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar68[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar69[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar70[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar71[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar72[beat - 1] = code;
				break;
			}
			break;
		case 10:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar73[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar74[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar75[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar76[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar77[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar78[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar79[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar80[beat - 1] = code;
				break;
			}
			break;
		case 11:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar81[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar82[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar83[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar84[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar85[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar86[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar87[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar88[beat - 1] = code;
				break;
			}
			break;
		case 12:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar89[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar90[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar91[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar92[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar93[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar94[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar95[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar96[beat - 1] = code;
				break;
			}
			break;
		case 13:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar97[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar98[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar99[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar100[beat - 1] = code;
				break;
			case 5:
				editorData.customCodedBar101[beat - 1] = code;
				break;
			case 6:
				editorData.customCodedBar102[beat - 1] = code;
				break;
			case 7:
				editorData.customCodedBar103[beat - 1] = code;
				break;
			case 8:
				editorData.customCodedBar104[beat - 1] = code;
				break;
			}
			break;
		case 14:
			switch (bar)
			{
			case 1:
				editorData.customCodedBar105[beat - 1] = code;
				break;
			case 2:
				editorData.customCodedBar106[beat - 1] = code;
				break;
			case 3:
				editorData.customCodedBar107[beat - 1] = code;
				break;
			case 4:
				editorData.customCodedBar108[beat - 1] = code;
				break;
			}
			break;
		}
		SaveEditorDataStacked();
	}

	public void SetEditorCustomSongTempo(float newCustomSongTempo)
	{
		editorData.customSongTempo = newCustomSongTempo;
		SaveEditorDataStacked();
	}

	public void SetEditorCustomSongLength(int newCustomSongLength)
	{
		editorData.customSongLength = newCustomSongLength;
		SaveEditorDataStacked();
	}

	public void SetEditorCustomSongSyncOffset(int newCustomSyncOffset)
	{
		editorData.customSongSyncOffset = newCustomSyncOffset;
		SaveEditorDataStacked();
	}

	public void ResetCustomCodedBars()
	{
		float customSongTempo = editorData.customSongTempo;
		int customSongLength = editorData.customSongLength;
		int customSongSyncOffset = editorData.customSongSyncOffset;
		editorData = new editorData();
		editorData.customSongTempo = customSongTempo;
		editorData.customSongLength = customSongLength;
		editorData.customSongSyncOffset = customSongSyncOffset;
		SaveEditorData();
	}

	private int ConvertScoreToEarned(int starScore)
	{
		if (starScore < 4)
		{
			return starScore;
		}
		return 3;
	}

	public bool CheckIsSavingPlayerDataStacked()
	{
		if (isSavingPlayerDataStacked || isSavingPlayerDataStackedQueued)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsSavingEditorDataStacked()
	{
		if (isSavingEditorDataStacked || isSavingEditorDataStackedQueued)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsGameComplete()
	{
		return playerData.igc;
	}

	public bool CheckIsVisualAssisting()
	{
		return playerData.isVisualAssisting;
	}

	public bool CheckIsAudioAssisting()
	{
		return playerData.isAudioAssisting;
	}

	public bool CheckIsBiggerHitWindows()
	{
		return playerData.isBiggerHitWindows;
	}

	public bool CheckIsEasyScoring()
	{
		return playerData.isEasyScoring;
	}

	public bool CheckIsVibrationDisabled()
	{
		return playerData.isVibrationDisabled;
	}

	public bool CheckIsDirectionKeysAlt()
	{
		return playerData.isDirectionKeysAlt;
	}

	public bool CheckIsTp()
	{
		return playerData.isTp;
	}

	public bool CheckIsCreator()
	{
		return playerData.isCreator;
	}

	public bool CheckIsWarmth()
	{
		return playerData.isWarmth;
	}

	public bool CheckIsPerfectsOnly()
	{
		return playerData.isPerfectsOnly;
	}

	public bool CheckIsVsynced()
	{
		return playerData.isVsynced;
	}

	public string GetActionKey()
	{
		return playerData.actionKey;
	}

	public static int GetLang()
	{
		return playerData.lang;
	}

	public int GetScreenshake()
	{
		return playerData.screenshake;
	}

	public int GetCalibrationOffsetMs()
	{
		return playerData.calibrationOffsetMs;
	}

	public int GetAudioOffsetMs()
	{
		return playerData.audioOffsetMs;
	}

	public int GetMaster()
	{
		return playerData.master;
	}

	public int GetMusic()
	{
		return playerData.music;
	}

	public int GetSfx()
	{
		return playerData.sfx;
	}

	public int GetMetronome()
	{
		return playerData.metronome;
	}

	public int GetContrast()
	{
		return playerData.contrast;
	}

	public int GetChapterNum()
	{
		return playerData.cn;
	}

	public int GetChapterEarnedStars(int chapterNum)
	{
		return chapterNum switch
		{
			1 => ConvertScoreToEarned(playerData.fd) + ConvertScoreToEarned(playerData.sp) + ConvertScoreToEarned(playerData.tc) + ConvertScoreToEarned(playerData.fw) + ConvertScoreToEarned(playerData.id), 
			2 => ConvertScoreToEarned(playerData.ec) + ConvertScoreToEarned(playerData.cr) + ConvertScoreToEarned(playerData.sv) + ConvertScoreToEarned(playerData.dt) + ConvertScoreToEarned(playerData.pr), 
			3 => ConvertScoreToEarned(playerData.ft) + ConvertScoreToEarned(playerData.mn) + ConvertScoreToEarned(playerData.wr) + ConvertScoreToEarned(playerData.nr) + ConvertScoreToEarned(playerData.md), 
			4 => ConvertScoreToEarned(playerData.fr) + ConvertScoreToEarned(playerData.me) + ConvertScoreToEarned(playerData.ps) + ConvertScoreToEarned(playerData.fu) + ConvertScoreToEarned(playerData.ax), 
			5 => ConvertScoreToEarned(playerData.fn), 
			_ => 0, 
		};
	}

	public int GetChapterEarnedRings(int chapterNum)
	{
		return chapterNum switch
		{
			1 => ConvertScoreToEarned(playerData.fdAlt) + ConvertScoreToEarned(playerData.spAlt) + ConvertScoreToEarned(playerData.tcAlt) + ConvertScoreToEarned(playerData.fwAlt) + ConvertScoreToEarned(playerData.idAlt), 
			2 => ConvertScoreToEarned(playerData.ecAlt) + ConvertScoreToEarned(playerData.crAlt) + ConvertScoreToEarned(playerData.svAlt) + ConvertScoreToEarned(playerData.dtAlt) + ConvertScoreToEarned(playerData.prAlt), 
			3 => ConvertScoreToEarned(playerData.ftAlt) + ConvertScoreToEarned(playerData.mnAlt) + ConvertScoreToEarned(playerData.wrAlt) + ConvertScoreToEarned(playerData.nrAlt) + ConvertScoreToEarned(playerData.mdAlt), 
			4 => ConvertScoreToEarned(playerData.frAlt) + ConvertScoreToEarned(playerData.meAlt) + ConvertScoreToEarned(playerData.psAlt) + ConvertScoreToEarned(playerData.fuAlt) + ConvertScoreToEarned(playerData.axAlt), 
			5 => ConvertScoreToEarned(playerData.fnAlt), 
			_ => 0, 
		};
	}

	public int GetChapterEarnedPerfects(int chapterNum)
	{
		int num = 0;
		switch (chapterNum)
		{
		case 1:
			if (playerData.fd == 4)
			{
				num++;
			}
			if (playerData.fdAlt == 4)
			{
				num++;
			}
			if (playerData.sp == 4)
			{
				num++;
			}
			if (playerData.spAlt == 4)
			{
				num++;
			}
			if (playerData.tc == 4)
			{
				num++;
			}
			if (playerData.tcAlt == 4)
			{
				num++;
			}
			if (playerData.fw == 4)
			{
				num++;
			}
			if (playerData.fwAlt == 4)
			{
				num++;
			}
			if (playerData.id == 4)
			{
				num++;
			}
			if (playerData.idAlt == 4)
			{
				num++;
			}
			break;
		case 2:
			if (playerData.ec == 4)
			{
				num++;
			}
			if (playerData.ecAlt == 4)
			{
				num++;
			}
			if (playerData.cr == 4)
			{
				num++;
			}
			if (playerData.crAlt == 4)
			{
				num++;
			}
			if (playerData.sv == 4)
			{
				num++;
			}
			if (playerData.svAlt == 4)
			{
				num++;
			}
			if (playerData.dt == 4)
			{
				num++;
			}
			if (playerData.dtAlt == 4)
			{
				num++;
			}
			if (playerData.pr == 4)
			{
				num++;
			}
			if (playerData.prAlt == 4)
			{
				num++;
			}
			break;
		case 3:
			if (playerData.ft == 4)
			{
				num++;
			}
			if (playerData.ftAlt == 4)
			{
				num++;
			}
			if (playerData.mn == 4)
			{
				num++;
			}
			if (playerData.mnAlt == 4)
			{
				num++;
			}
			if (playerData.wr == 4)
			{
				num++;
			}
			if (playerData.wrAlt == 4)
			{
				num++;
			}
			if (playerData.nr == 4)
			{
				num++;
			}
			if (playerData.nrAlt == 4)
			{
				num++;
			}
			if (playerData.md == 4)
			{
				num++;
			}
			if (playerData.mdAlt == 4)
			{
				num++;
			}
			break;
		case 4:
			if (playerData.fr == 4)
			{
				num++;
			}
			if (playerData.frAlt == 4)
			{
				num++;
			}
			if (playerData.me == 4)
			{
				num++;
			}
			if (playerData.meAlt == 4)
			{
				num++;
			}
			if (playerData.ps == 4)
			{
				num++;
			}
			if (playerData.psAlt == 4)
			{
				num++;
			}
			if (playerData.fu == 4)
			{
				num++;
			}
			if (playerData.fuAlt == 4)
			{
				num++;
			}
			if (playerData.ax == 4)
			{
				num++;
			}
			if (playerData.axAlt == 4)
			{
				num++;
			}
			break;
		default:
			if (playerData.fn == 4)
			{
				num++;
			}
			if (playerData.fnAlt == 4)
			{
				num++;
			}
			break;
		}
		return num;
	}

	public int GetTotalEarnedStars()
	{
		return GetChapterEarnedStars(1) + GetChapterEarnedStars(2) + GetChapterEarnedStars(3) + GetChapterEarnedStars(4) + GetChapterEarnedStars(5);
	}

	public int GetTotalEarnedRings()
	{
		return GetChapterEarnedRings(1) + GetChapterEarnedRings(2) + GetChapterEarnedRings(3) + GetChapterEarnedRings(4) + GetChapterEarnedRings(5);
	}

	public int GetTotalPerfects()
	{
		int num = 0;
		if (playerData.fd == 4)
		{
			num++;
		}
		if (playerData.fdAlt == 4)
		{
			num++;
		}
		if (playerData.sp == 4)
		{
			num++;
		}
		if (playerData.spAlt == 4)
		{
			num++;
		}
		if (playerData.tc == 4)
		{
			num++;
		}
		if (playerData.tcAlt == 4)
		{
			num++;
		}
		if (playerData.fw == 4)
		{
			num++;
		}
		if (playerData.fwAlt == 4)
		{
			num++;
		}
		if (playerData.id == 4)
		{
			num++;
		}
		if (playerData.idAlt == 4)
		{
			num++;
		}
		if (playerData.ec == 4)
		{
			num++;
		}
		if (playerData.ecAlt == 4)
		{
			num++;
		}
		if (playerData.cr == 4)
		{
			num++;
		}
		if (playerData.crAlt == 4)
		{
			num++;
		}
		if (playerData.sv == 4)
		{
			num++;
		}
		if (playerData.svAlt == 4)
		{
			num++;
		}
		if (playerData.dt == 4)
		{
			num++;
		}
		if (playerData.dtAlt == 4)
		{
			num++;
		}
		if (playerData.pr == 4)
		{
			num++;
		}
		if (playerData.prAlt == 4)
		{
			num++;
		}
		if (playerData.ft == 4)
		{
			num++;
		}
		if (playerData.ftAlt == 4)
		{
			num++;
		}
		if (playerData.mn == 4)
		{
			num++;
		}
		if (playerData.mnAlt == 4)
		{
			num++;
		}
		if (playerData.wr == 4)
		{
			num++;
		}
		if (playerData.wrAlt == 4)
		{
			num++;
		}
		if (playerData.nr == 4)
		{
			num++;
		}
		if (playerData.nrAlt == 4)
		{
			num++;
		}
		if (playerData.md == 4)
		{
			num++;
		}
		if (playerData.mdAlt == 4)
		{
			num++;
		}
		if (playerData.fr == 4)
		{
			num++;
		}
		if (playerData.frAlt == 4)
		{
			num++;
		}
		if (playerData.me == 4)
		{
			num++;
		}
		if (playerData.meAlt == 4)
		{
			num++;
		}
		if (playerData.ps == 4)
		{
			num++;
		}
		if (playerData.psAlt == 4)
		{
			num++;
		}
		if (playerData.fu == 4)
		{
			num++;
		}
		if (playerData.fuAlt == 4)
		{
			num++;
		}
		if (playerData.ax == 4)
		{
			num++;
		}
		if (playerData.axAlt == 4)
		{
			num++;
		}
		if (playerData.fn == 4)
		{
			num++;
		}
		if (playerData.fnAlt == 4)
		{
			num++;
		}
		return num;
	}

	public int GetScore(string dreamName)
	{
		return dreamName switch
		{
			"Dream_food" => playerData.fd, 
			"Dream_foodAlt" => playerData.fdAlt, 
			"Dream_shopping" => playerData.sp, 
			"Dream_shoppingAlt" => playerData.spAlt, 
			"Dream_tech" => playerData.tc, 
			"Dream_techAlt" => playerData.tcAlt, 
			"Dream_followers" => playerData.fw, 
			"Dream_followersAlt" => playerData.fwAlt, 
			"Dream_indulgence" => playerData.id, 
			"Dream_indulgenceAlt" => playerData.idAlt, 
			"Dream_exercise" => playerData.ec, 
			"Dream_exerciseAlt" => playerData.ecAlt, 
			"Dream_career" => playerData.cr, 
			"Dream_careerAlt" => playerData.crAlt, 
			"Dream_money" => playerData.sv, 
			"Dream_moneyAlt" => playerData.svAlt, 
			"Dream_dating" => playerData.dt, 
			"Dream_datingAlt" => playerData.dtAlt, 
			"Dream_pressure" => playerData.pr, 
			"Dream_pressureAlt" => playerData.prAlt, 
			"Dream_time" => playerData.ft, 
			"Dream_timeAlt" => playerData.ftAlt, 
			"Dream_mind" => playerData.mn, 
			"Dream_mindAlt" => playerData.mnAlt, 
			"Dream_space" => playerData.wr, 
			"Dream_spaceAlt" => playerData.wrAlt, 
			"Dream_nature" => playerData.nr, 
			"Dream_natureAlt" => playerData.nrAlt, 
			"Dream_meditation" => playerData.md, 
			"Dream_meditationAlt" => playerData.mdAlt, 
			"Dream_stress" => playerData.fr, 
			"Dream_stressAlt" => playerData.frAlt, 
			"Dream_desires" => playerData.me, 
			"Dream_desiresAlt" => playerData.meAlt, 
			"Dream_past" => playerData.ps, 
			"Dream_pastAlt" => playerData.psAlt, 
			"Dream_future" => playerData.fu, 
			"Dream_futureAlt" => playerData.fuAlt, 
			"Dream_setbacks" => playerData.ax, 
			"Dream_setbacksAlt" => playerData.axAlt, 
			"Dream_final" => playerData.fn, 
			"Dream_finalAlt" => playerData.fnAlt, 
			_ => 0, 
		};
	}

	public editorData GetEditorData()
	{
		return editorData;
	}

	public string[] GetEditorDataBar(int bar)
	{
		return bar switch
		{
			1 => editorData.customCodedBar1, 
			2 => editorData.customCodedBar2, 
			3 => editorData.customCodedBar3, 
			4 => editorData.customCodedBar4, 
			5 => editorData.customCodedBar5, 
			6 => editorData.customCodedBar6, 
			7 => editorData.customCodedBar7, 
			8 => editorData.customCodedBar8, 
			9 => editorData.customCodedBar9, 
			10 => editorData.customCodedBar10, 
			11 => editorData.customCodedBar11, 
			12 => editorData.customCodedBar12, 
			13 => editorData.customCodedBar13, 
			14 => editorData.customCodedBar14, 
			15 => editorData.customCodedBar15, 
			16 => editorData.customCodedBar16, 
			17 => editorData.customCodedBar17, 
			18 => editorData.customCodedBar18, 
			19 => editorData.customCodedBar19, 
			20 => editorData.customCodedBar20, 
			21 => editorData.customCodedBar21, 
			22 => editorData.customCodedBar22, 
			23 => editorData.customCodedBar23, 
			24 => editorData.customCodedBar24, 
			25 => editorData.customCodedBar25, 
			26 => editorData.customCodedBar26, 
			27 => editorData.customCodedBar27, 
			28 => editorData.customCodedBar28, 
			29 => editorData.customCodedBar29, 
			30 => editorData.customCodedBar30, 
			31 => editorData.customCodedBar31, 
			32 => editorData.customCodedBar32, 
			33 => editorData.customCodedBar33, 
			34 => editorData.customCodedBar34, 
			35 => editorData.customCodedBar35, 
			36 => editorData.customCodedBar36, 
			37 => editorData.customCodedBar37, 
			38 => editorData.customCodedBar38, 
			39 => editorData.customCodedBar39, 
			40 => editorData.customCodedBar40, 
			41 => editorData.customCodedBar41, 
			42 => editorData.customCodedBar42, 
			43 => editorData.customCodedBar43, 
			44 => editorData.customCodedBar44, 
			45 => editorData.customCodedBar45, 
			46 => editorData.customCodedBar46, 
			47 => editorData.customCodedBar47, 
			48 => editorData.customCodedBar48, 
			49 => editorData.customCodedBar49, 
			50 => editorData.customCodedBar50, 
			51 => editorData.customCodedBar51, 
			52 => editorData.customCodedBar52, 
			53 => editorData.customCodedBar53, 
			54 => editorData.customCodedBar54, 
			55 => editorData.customCodedBar55, 
			56 => editorData.customCodedBar56, 
			57 => editorData.customCodedBar57, 
			58 => editorData.customCodedBar58, 
			59 => editorData.customCodedBar59, 
			60 => editorData.customCodedBar60, 
			61 => editorData.customCodedBar61, 
			62 => editorData.customCodedBar62, 
			63 => editorData.customCodedBar63, 
			64 => editorData.customCodedBar64, 
			65 => editorData.customCodedBar65, 
			66 => editorData.customCodedBar66, 
			67 => editorData.customCodedBar67, 
			68 => editorData.customCodedBar68, 
			69 => editorData.customCodedBar69, 
			70 => editorData.customCodedBar70, 
			71 => editorData.customCodedBar71, 
			72 => editorData.customCodedBar72, 
			73 => editorData.customCodedBar73, 
			74 => editorData.customCodedBar74, 
			75 => editorData.customCodedBar75, 
			76 => editorData.customCodedBar76, 
			77 => editorData.customCodedBar77, 
			78 => editorData.customCodedBar78, 
			79 => editorData.customCodedBar79, 
			80 => editorData.customCodedBar80, 
			81 => editorData.customCodedBar81, 
			82 => editorData.customCodedBar82, 
			83 => editorData.customCodedBar83, 
			84 => editorData.customCodedBar84, 
			85 => editorData.customCodedBar85, 
			86 => editorData.customCodedBar86, 
			87 => editorData.customCodedBar87, 
			88 => editorData.customCodedBar88, 
			89 => editorData.customCodedBar89, 
			90 => editorData.customCodedBar90, 
			91 => editorData.customCodedBar91, 
			92 => editorData.customCodedBar92, 
			93 => editorData.customCodedBar93, 
			94 => editorData.customCodedBar94, 
			95 => editorData.customCodedBar95, 
			96 => editorData.customCodedBar96, 
			97 => editorData.customCodedBar97, 
			98 => editorData.customCodedBar98, 
			99 => editorData.customCodedBar99, 
			100 => editorData.customCodedBar100, 
			101 => editorData.customCodedBar101, 
			102 => editorData.customCodedBar102, 
			103 => editorData.customCodedBar103, 
			104 => editorData.customCodedBar104, 
			105 => editorData.customCodedBar105, 
			106 => editorData.customCodedBar106, 
			107 => editorData.customCodedBar107, 
			_ => editorData.customCodedBar108, 
		};
	}

	public bool CheckStarPrecisionAchievement()
	{
		if (playerData.fd >= 3)
		{
			return true;
		}
		if (playerData.sp >= 3)
		{
			return true;
		}
		if (playerData.tc >= 3)
		{
			return true;
		}
		if (playerData.fw >= 3)
		{
			return true;
		}
		if (playerData.id >= 3)
		{
			return true;
		}
		if (playerData.ec >= 3)
		{
			return true;
		}
		if (playerData.cr >= 3)
		{
			return true;
		}
		if (playerData.sv >= 3)
		{
			return true;
		}
		if (playerData.dt >= 3)
		{
			return true;
		}
		if (playerData.pr >= 3)
		{
			return true;
		}
		if (playerData.ft >= 3)
		{
			return true;
		}
		if (playerData.mn >= 3)
		{
			return true;
		}
		if (playerData.wr >= 3)
		{
			return true;
		}
		if (playerData.nr >= 3)
		{
			return true;
		}
		if (playerData.md >= 3)
		{
			return true;
		}
		if (playerData.fr >= 3)
		{
			return true;
		}
		if (playerData.me >= 3)
		{
			return true;
		}
		if (playerData.ps >= 3)
		{
			return true;
		}
		if (playerData.fu >= 3)
		{
			return true;
		}
		if (playerData.ax >= 3)
		{
			return true;
		}
		if (playerData.fn >= 3)
		{
			return true;
		}
		return false;
	}

	public bool CheckStarPerfectionistAchievement()
	{
		if (playerData.fd == 4)
		{
			return true;
		}
		if (playerData.sp == 4)
		{
			return true;
		}
		if (playerData.tc == 4)
		{
			return true;
		}
		if (playerData.fw == 4)
		{
			return true;
		}
		if (playerData.id == 4)
		{
			return true;
		}
		if (playerData.ec == 4)
		{
			return true;
		}
		if (playerData.cr == 4)
		{
			return true;
		}
		if (playerData.sv == 4)
		{
			return true;
		}
		if (playerData.dt == 4)
		{
			return true;
		}
		if (playerData.pr == 4)
		{
			return true;
		}
		if (playerData.ft == 4)
		{
			return true;
		}
		if (playerData.mn == 4)
		{
			return true;
		}
		if (playerData.wr == 4)
		{
			return true;
		}
		if (playerData.nr == 4)
		{
			return true;
		}
		if (playerData.md == 4)
		{
			return true;
		}
		if (playerData.fr == 4)
		{
			return true;
		}
		if (playerData.me == 4)
		{
			return true;
		}
		if (playerData.ps == 4)
		{
			return true;
		}
		if (playerData.fu == 4)
		{
			return true;
		}
		if (playerData.ax == 4)
		{
			return true;
		}
		if (playerData.fn == 4)
		{
			return true;
		}
		return false;
	}

	public bool CheckStargazerAchievement()
	{
		if (GetTotalEarnedStars() == 63)
		{
			return true;
		}
		return false;
	}

	public bool CheckRingPrecisionAchievement()
	{
		if (playerData.fdAlt >= 3)
		{
			return true;
		}
		if (playerData.spAlt >= 3)
		{
			return true;
		}
		if (playerData.tcAlt >= 3)
		{
			return true;
		}
		if (playerData.fwAlt >= 3)
		{
			return true;
		}
		if (playerData.idAlt >= 3)
		{
			return true;
		}
		if (playerData.ecAlt >= 3)
		{
			return true;
		}
		if (playerData.crAlt >= 3)
		{
			return true;
		}
		if (playerData.svAlt >= 3)
		{
			return true;
		}
		if (playerData.dtAlt >= 3)
		{
			return true;
		}
		if (playerData.prAlt >= 3)
		{
			return true;
		}
		if (playerData.ftAlt >= 3)
		{
			return true;
		}
		if (playerData.mnAlt >= 3)
		{
			return true;
		}
		if (playerData.wrAlt >= 3)
		{
			return true;
		}
		if (playerData.nrAlt >= 3)
		{
			return true;
		}
		if (playerData.mdAlt >= 3)
		{
			return true;
		}
		if (playerData.frAlt >= 3)
		{
			return true;
		}
		if (playerData.meAlt >= 3)
		{
			return true;
		}
		if (playerData.psAlt >= 3)
		{
			return true;
		}
		if (playerData.fuAlt >= 3)
		{
			return true;
		}
		if (playerData.axAlt >= 3)
		{
			return true;
		}
		if (playerData.fnAlt >= 3)
		{
			return true;
		}
		return false;
	}

	public bool CheckRingPerfectionistAchievement()
	{
		if (playerData.fdAlt == 4)
		{
			return true;
		}
		if (playerData.spAlt == 4)
		{
			return true;
		}
		if (playerData.tcAlt == 4)
		{
			return true;
		}
		if (playerData.fwAlt == 4)
		{
			return true;
		}
		if (playerData.idAlt == 4)
		{
			return true;
		}
		if (playerData.ecAlt == 4)
		{
			return true;
		}
		if (playerData.crAlt == 4)
		{
			return true;
		}
		if (playerData.svAlt == 4)
		{
			return true;
		}
		if (playerData.dtAlt == 4)
		{
			return true;
		}
		if (playerData.prAlt == 4)
		{
			return true;
		}
		if (playerData.ftAlt == 4)
		{
			return true;
		}
		if (playerData.mnAlt == 4)
		{
			return true;
		}
		if (playerData.wrAlt == 4)
		{
			return true;
		}
		if (playerData.nrAlt == 4)
		{
			return true;
		}
		if (playerData.mdAlt == 4)
		{
			return true;
		}
		if (playerData.frAlt == 4)
		{
			return true;
		}
		if (playerData.meAlt == 4)
		{
			return true;
		}
		if (playerData.psAlt == 4)
		{
			return true;
		}
		if (playerData.fuAlt == 4)
		{
			return true;
		}
		if (playerData.axAlt == 4)
		{
			return true;
		}
		if (playerData.fnAlt == 4)
		{
			return true;
		}
		return false;
	}

	public bool CheckRingCollectorAchievement()
	{
		if (GetTotalEarnedRings() == 63)
		{
			return true;
		}
		return false;
	}
}
