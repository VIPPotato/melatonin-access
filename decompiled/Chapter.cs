using System.Collections;
using UnityEngine;

public class Chapter : Custom
{
	public static Chapter dir;

	private bool isTransitioning;

	private bool isCutsceneIntro;

	private bool isCutsceneOutro;

	private Coroutine intro;

	private Coroutine continuing;

	private static int activeChapterNum;

	private static string activeDreamName = "";

	private static bool isEnteringWithIntro;

	private static bool isEnteringWithOutro;

	private static bool isEnteringFromDream;

	private static bool isRemixSeen;

	private static bool isFinalSeen;

	private void Awake()
	{
		dir = this;
	}

	private void Start()
	{
		switch (SceneMonitor.mgr.GetActiveSceneName())
		{
		case "Chapter_1":
			activeChapterNum = 1;
			break;
		case "Chapter_2":
			activeChapterNum = 2;
			break;
		case "Chapter_3":
			activeChapterNum = 3;
			break;
		case "Chapter_4":
			activeChapterNum = 4;
			break;
		case "Chapter_5":
			activeChapterNum = 5;
			break;
		}
		LivingRoom.env.Show();
		Technician.mgr.ToggleVsync(toggle: true);
		Interface.env.Letterbox.Show();
		MusicBox.ResetCustomSongClip();
		if (SaveManager.mgr.GetChapterNum() < activeChapterNum)
		{
			ResetCache();
			EnterWithIntro();
		}
		else if ((CheckIsOnSavedChapter() && CheckIsChapterComplete() && !SaveManager.mgr.CheckIsGameComplete() && Builder.mgr.CheckIsFullGame()) || isEnteringWithOutro)
		{
			ResetCache();
			ExitToNextChapter();
		}
		else if (isEnteringWithIntro)
		{
			ResetCache();
			EnterWithIntro();
		}
		else if (isEnteringFromDream)
		{
			isEnteringFromDream = false;
			EnterFromDream();
		}
		else
		{
			ResetCache();
			EnterWithContinue();
		}
	}

	private void Update()
	{
		if (isTransitioning && ControlHandler.mgr.CheckIsActionPressed() && Time.timeScale > 0f)
		{
			SkipTransition();
		}
	}

	private void EnterWithIntro()
	{
		intro = StartCoroutine(EnteringWithIntro());
	}

	private IEnumerator EnteringWithIntro()
	{
		isCutsceneIntro = true;
		Technician.mgr.FadeInAudioListener();
		RealWorld.env.Fader.Show();
		RealWorld.env.Fader.SetColor(0);
		RealWorld.env.Fader.SetSpeed(1f);
		RealWorld.env.Fader.Deactivate();
		Map.env.Neighbourhood.McMap.ToggleIsAsleep(toggle: true);
		if (activeChapterNum == 1)
		{
			RealWorld.env.TitleCard.Show(0);
			LivingRoom.env.PlayAmbience(0, 0f);
			LivingRoom.env.SetComposition("awaiting");
			yield return new WaitForSeconds(4.5f);
			LivingRoom.env.SetCharacter("idling");
			RealWorld.env.TitleCard.Hide();
			yield return new WaitForSeconds(3.33f);
			LivingRoom.env.SetComposition("parallaxFromAwaiting");
			yield return new WaitForSeconds(2.11f);
			LivingRoom.env.SetCharacter("blink");
			yield return new WaitForSeconds(5f);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.SetSpeed(1f);
			RealWorld.env.Fader.Activate();
			LivingRoom.env.FadeOutAmbience(0);
			yield return new WaitForSeconds(1.5f);
			RealWorld.env.Fader.Deactivate();
			LivingRoom.env.PlayAmbience(0, 30f);
			LivingRoom.env.FadeInAmbience(0);
			LivingRoom.env.SetCharacter("sleeping");
			yield return new WaitForSeconds(2f);
			LivingRoom.env.FadeOutAmbience(0);
			LivingRoom.env.SetComposition("centerFromParallax");
			Map.env.PlayMusic();
			yield return new WaitForSeconds(2.5f);
			isCutsceneIntro = false;
			SaveChapter();
			LivingRoom.env.SetComposition("transitionFromCentered");
			yield return new WaitForSeconds(1.35f);
			Map.env.Activate();
		}
		else if (activeChapterNum == 2)
		{
			RealWorld.env.TitleCard.Show(1);
			LivingRoom.env.PlayAmbience(0, 27.75f);
			LivingRoom.env.SetComposition("awaiting");
			LivingRoom.env.SetCharacter("idling");
			yield return new WaitForSeconds(4.5f);
			RealWorld.env.TitleCard.Hide();
			yield return new WaitForSeconds(3.33f);
			LivingRoom.env.SetComposition("parallaxFromAwaiting");
			yield return new WaitForSeconds(4.5f);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.Show();
			LivingRoom.env.SetCharacter("sleeping");
			LivingRoom.env.StopAmbience();
			yield return new WaitForSeconds(2f);
			LivingRoom.env.PlayAmbience(0, 0.6f);
			RealWorld.env.Fader.Hide();
			yield return new WaitForSeconds(2.5f);
			LivingRoom.env.FadeOutAmbience(0);
			LivingRoom.env.SetComposition("centerFromParallax");
			Map.env.PlayMusic();
			yield return new WaitForSeconds(2.5f);
			isCutsceneIntro = false;
			SaveChapter();
			LivingRoom.env.SetComposition("transitionFromCentered");
			yield return new WaitForSeconds(1.35f);
			Map.env.Activate();
		}
		else if (activeChapterNum == 3)
		{
			RealWorld.env.TitleCard.Show(2);
			LivingRoom.env.PlayAmbience(0, 1f);
			LivingRoom.env.SetComposition("awaiting");
			yield return new WaitForSeconds(4.5f);
			LivingRoom.env.SetCharacter("idling");
			RealWorld.env.TitleCard.Hide();
			yield return new WaitForSeconds(3.33f);
			LivingRoom.env.SetComposition("parallaxFromAwaiting");
			yield return new WaitForSeconds(5f);
			LivingRoom.env.FadeOutAmbience(0);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.SetSpeed(0.4f);
			RealWorld.env.Fader.Cross();
			yield return new WaitForSeconds(1.25f);
			LivingRoom.env.SetCharacter("sleeping");
			yield return new WaitForSeconds(1.75f);
			LivingRoom.env.SetComposition("centerFromParallax");
			Map.env.PlayMusic();
			yield return new WaitForSeconds(2.5f);
			isCutsceneIntro = false;
			SaveChapter();
			LivingRoom.env.SetComposition("transitionFromCentered");
			yield return new WaitForSeconds(1.35f);
			Map.env.Activate();
		}
		else if (activeChapterNum == 4)
		{
			RealWorld.env.TitleCard.Show(3);
			LivingRoom.env.PlayAmbience(0, 0f);
			LivingRoom.env.SetComposition("awaiting");
			yield return new WaitForSeconds(4.5f);
			LivingRoom.env.SetCharacter("idling");
			yield return new WaitForSeconds(0.2f);
			RealWorld.env.TitleCard.Hide();
			yield return new WaitForSeconds(3.33f);
			LivingRoom.env.SetComposition("parallaxFromAwaiting");
			yield return new WaitForSeconds(3f);
			LivingRoom.env.SetCharacterTrigger("sleep");
			yield return new WaitForSeconds(2f);
			LivingRoom.env.SetComposition("centerFromParallax");
			yield return new WaitForSeconds(1.5f);
			Map.env.PlayMusic();
			yield return new WaitForSeconds(2.5f);
			isCutsceneIntro = false;
			SaveChapter();
			LivingRoom.env.SetComposition("transitionFromCentered");
			yield return new WaitForSeconds(1.35f);
			Map.env.Neighbourhood.McMap.ToggleIsStartMirrored(toggle: true);
			Map.env.Activate();
		}
		else if (activeChapterNum == 5)
		{
			RealWorld.env.TitleCard.Show(4);
			LivingRoom.env.PlayAmbience(0, 0f);
			LivingRoom.env.SetComposition("awaiting");
			yield return new WaitForSeconds(4.5f);
			LivingRoom.env.SetCharacter("sleeping");
			LivingRoom.env.FadeOutAmbience(0);
			RealWorld.env.TitleCard.Deactivate();
			yield return new WaitForSeconds(3.33f);
			LivingRoom.env.SetComposition("parallaxFromAwaiting");
			yield return new WaitForSeconds(3.5f);
			LivingRoom.env.SetComposition("centerFromParallax");
			Map.env.PlayMusic();
			yield return new WaitForSeconds(2.5f);
			isCutsceneIntro = false;
			SaveChapter();
			LivingRoom.env.SetComposition("transitionFromCentered");
			yield return new WaitForSeconds(1.35f);
			Map.env.Activate();
		}
	}

	private void EnterWithContinue()
	{
		continuing = StartCoroutine(EnteringWithContinue());
	}

	private IEnumerator EnteringWithContinue()
	{
		isTransitioning = true;
		if (activeChapterNum == 5)
		{
			Map.env.Neighbourhood.McMap.ToggleIsAsleep(toggle: true);
		}
		Technician.mgr.FadeInAudioListener();
		RealWorld.env.Fader.Show();
		RealWorld.env.Fader.SetColor(0);
		RealWorld.env.Fader.SetSpeed(1f);
		RealWorld.env.Fader.Deactivate();
		LivingRoom.env.SetCharacter("sleeping");
		yield return new WaitForSeconds(0.5f);
		LivingRoom.env.SetComposition("centerFromAwaiting");
		Map.env.PlayMusic();
		yield return new WaitForSeconds(2.5f);
		LivingRoom.env.SetComposition("transitionFromCentered");
		yield return new WaitForSeconds(1.35f);
		isTransitioning = false;
		Map.env.Activate();
	}

	private void EnterFromDream()
	{
		StartCoroutine(EnteringFromDream());
	}

	private IEnumerator EnteringFromDream()
	{
		Technician.mgr.SetAudioListener(1f);
		RealWorld.env.TransitionFromDream();
		Interface.env.FeatherBorder.Show();
		LivingRoom.env.SetComposition("transitioned");
		LivingRoom.env.SetCharacter("sleeping");
		yield return new WaitForSeconds(1.35f);
		Interface.env.FeatherBorder.Deactivate();
		Map.env.PlayMusic();
		Map.env.Activate();
	}

	public void ExitToDream(string sceneName)
	{
		StartCoroutine(ExitingToDream(sceneName));
	}

	private IEnumerator ExitingToDream(string sceneName)
	{
		isEnteringFromDream = true;
		Interface.env.FeatherBorder.Activate();
		Interface.env.Letterbox.Activate();
		Interface.env.Cam.MoveToTarget(new Vector3(0f, 0f, 0f), 1f);
		Map.env.FadeOutMusic();
		SceneMonitor.mgr.PreloadScene(sceneName);
		yield return new WaitForSeconds(1.67f);
		RealWorld.env.PlayTransitionSound();
		yield return new WaitForSeconds(0.72f);
		RealWorld.env.TransitionToDream();
		yield return new WaitForSeconds(0.8f);
		SceneMonitor.mgr.LoadScene();
	}

	private void ExitToNextChapter()
	{
		StartCoroutine(ExitingToNextChapter());
	}

	private IEnumerator ExitingToNextChapter()
	{
		isCutsceneOutro = true;
		Technician.mgr.SetAudioListener(1f);
		RealWorld.env.TransitionFromDream();
		Interface.env.FeatherBorder.Show();
		if (activeChapterNum == 1)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("indulgence");
			}
			LivingRoom.env.PlayAmbience(1, 0f);
			LivingRoom.env.FadeInAmbience(1);
			LivingRoom.env.SetComposition("transitioned");
			LivingRoom.env.SetCharacter("wake");
			LivingRoom.env.PauseCharacter();
			yield return new WaitForSeconds(1.25f);
			LivingRoom.env.SetComposition("awaitFromTransitioned");
			Interface.env.FeatherBorder.Deactivate();
			yield return new WaitForSeconds(3f);
			LivingRoom.env.SetCharacter("wake");
			yield return new WaitForSeconds(LivingRoom.env.GetCharacterAnimDuration("wake") - 1f / 6f);
			LivingRoom.env.StopAmbience();
			LivingRoom.env.PlaySoundEffect(0);
			yield return new WaitForSeconds(1f / 6f);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.Show();
			SceneMonitor.mgr.PreloadScene("Chapter_2");
		}
		else if (activeChapterNum == 2)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("under_pressure");
			}
			LivingRoom.env.PlayAmbience(1, 0f);
			LivingRoom.env.FadeInAmbience(1);
			LivingRoom.env.SetComposition("transitioned");
			LivingRoom.env.SetCharacter("wake1");
			LivingRoom.env.PauseCharacter();
			yield return new WaitForSeconds(1.25f);
			LivingRoom.env.SetComposition("awaitFromTransitioned");
			Interface.env.FeatherBorder.Deactivate();
			yield return new WaitForSeconds(3.5f);
			LivingRoom.env.PlaySoundEffect(0);
			yield return new WaitForSeconds(0.1f);
			LivingRoom.env.SetCharacter("wake1");
			yield return new WaitForSeconds(3f);
			LivingRoom.env.SetCharacter("wake2");
			LivingRoom.env.PlaySoundEffect(1);
			LivingRoom.env.StopSoundEffect(0);
			yield return new WaitForSeconds(1.5f);
			LivingRoom.env.StopAmbience();
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.Show();
			SceneMonitor.mgr.PreloadScene("Chapter_3");
		}
		else if (activeChapterNum == 3)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("meditation");
			}
			LivingRoom.env.PlayAmbience(1, 0f);
			LivingRoom.env.FadeInAmbience(1);
			LivingRoom.env.SetComposition("transitioned");
			LivingRoom.env.SetCharacter("wake");
			LivingRoom.env.PauseCharacter();
			yield return new WaitForSeconds(1.25f);
			LivingRoom.env.SetComposition("awaitFromTransitioned");
			Interface.env.FeatherBorder.Deactivate();
			yield return new WaitForSeconds(3.7f);
			LivingRoom.env.StopAmbience();
			LivingRoom.env.Static();
			yield return new WaitForSeconds(1f);
			LivingRoom.env.SetCharacter("wake");
			yield return new WaitForSeconds(LivingRoom.env.GetCharacterAnimDuration("wake"));
			LivingRoom.env.StopAmbience();
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.Show();
			SceneMonitor.mgr.PreloadScene("Chapter_4");
		}
		else if (activeChapterNum == 4)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("setbacks");
			}
			LivingRoom.env.PlayAmbience(1, 0f);
			LivingRoom.env.SetCharacter("sleeping");
			LivingRoom.env.SetComposition("transitioned");
			yield return new WaitForSeconds(1.5f);
			LivingRoom.env.SetCharacterTrigger("shuteye");
			yield return new WaitForSeconds(0.67f);
			LivingRoom.env.SetComposition("awaitFromTransitioned");
			Interface.env.FeatherBorder.Deactivate();
			yield return new WaitForSeconds(5.5f);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.SetSpeed(0.5f);
			RealWorld.env.Fader.Activate();
			Technician.mgr.FadeOutAudioListener(0.5f);
			SceneMonitor.mgr.PreloadScene("Chapter_5");
			yield return new WaitForSeconds(3f);
		}
		else if (activeChapterNum == 5)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("new_day");
			}
			LivingRoom.env.PlayAmbience(0, 0f);
			LivingRoom.env.SetCharacter("wake");
			LivingRoom.env.SetComposition("transitioned");
			yield return new WaitForSeconds(2f);
			LivingRoom.env.SetComposition("parallaxFromTransitioned");
			Interface.env.FeatherBorder.Deactivate();
			yield return new WaitForSeconds(5.8f);
			LivingRoom.env.SetComposition("windowFromParallaxed");
			yield return new WaitForSeconds(0.67f);
			LivingRoom.env.SetCharacter("look");
			yield return new WaitForSeconds(4f);
			RealWorld.env.Fader.SetColor(0);
			RealWorld.env.Fader.SetSpeed(0.25f);
			RealWorld.env.Fader.Activate();
			SceneMonitor.mgr.PreloadScene("Credits");
			Technician.mgr.FadeOutAudioListener(0.5f);
			yield return new WaitForSeconds(6f);
			SaveManager.mgr.ToggleIsGameComplete(toggle: true);
		}
		yield return new WaitForSeconds(2f);
		SceneMonitor.mgr.LoadScene();
	}

	public void SkipTransition()
	{
		isTransitioning = false;
		CancelCoroutine(continuing);
		LivingRoom.env.SetComposition("transitioned");
		Map.env.PlayMusic();
		Map.env.Activate();
	}

	public void SaveChapter()
	{
		if (SaveManager.mgr.GetChapterNum() < activeChapterNum)
		{
			SaveManager.mgr.SetChapterNum(activeChapterNum);
		}
	}

	public void TrailerZoom()
	{
		CancelCoroutine(intro);
		LivingRoom.env.SetComposition("zoom");
		LivingRoom.env.SetCharacter("sleeping");
		Interface.env.Letterbox.Hide();
	}

	private void ResetCache()
	{
		isEnteringFromDream = false;
		isEnteringWithIntro = false;
		isEnteringWithOutro = false;
		activeDreamName = "";
		isRemixSeen = false;
		isFinalSeen = false;
	}

	public static void ClearAllCache()
	{
		activeChapterNum = 0;
		activeDreamName = "";
		isEnteringFromDream = false;
		isEnteringWithIntro = false;
		isEnteringWithOutro = false;
		isRemixSeen = false;
		isFinalSeen = false;
	}

	public void SetActiveDreamName(string newName)
	{
		activeDreamName = newName;
	}

	public void ToggleIsRemixSeen(bool toggle)
	{
		isRemixSeen = toggle;
	}

	public void ToggleIsFinalSeen(bool toggle)
	{
		isFinalSeen = toggle;
	}

	public void ResetActiveDreamName()
	{
		activeDreamName = "";
	}

	public static void ToggleIsEnteringWithIntro(bool toggle)
	{
		isEnteringWithIntro = toggle;
	}

	public static void ToggleIsEnteringWithOutro(bool toggle)
	{
		isEnteringWithOutro = toggle;
	}

	private bool CheckIsChapterComplete()
	{
		if (activeChapterNum == 1 && SaveManager.mgr.GetScore("Dream_indulgence") >= 2)
		{
			return true;
		}
		if (activeChapterNum == 2 && SaveManager.mgr.GetScore("Dream_pressure") >= 2)
		{
			return true;
		}
		if (activeChapterNum == 3 && SaveManager.mgr.GetScore("Dream_meditation") >= 2)
		{
			return true;
		}
		if (activeChapterNum == 4 && SaveManager.mgr.GetScore("Dream_setbacks") >= 2)
		{
			return true;
		}
		if (activeChapterNum == 5 && SaveManager.mgr.GetScore("Dream_final") >= 2)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsRemixSeen()
	{
		if (CheckIsChapterComplete())
		{
			return true;
		}
		return isRemixSeen;
	}

	public bool CheckIsFinalSeen()
	{
		return isFinalSeen;
	}

	public bool CheckIsCutsceneIntro()
	{
		return isCutsceneIntro;
	}

	public bool CheckIsCutsceneOutro()
	{
		return isCutsceneOutro;
	}

	public bool CheckIsOnSavedChapter()
	{
		if (SaveManager.mgr.GetChapterNum() == activeChapterNum)
		{
			return true;
		}
		return false;
	}

	public string GetActiveDreamName()
	{
		return activeDreamName;
	}

	public static int GetActiveChapterNum()
	{
		return activeChapterNum;
	}
}
