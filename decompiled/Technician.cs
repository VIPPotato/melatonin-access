using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Technician : Custom
{
	public static Technician mgr;

	private bool isWarpingTime;

	private bool isSettingResolution;

	private float tempTimeScale;

	private float warpSpeed = 1f;

	private float warpedTime;

	private float timer;

	private Coroutine warpingTime;

	private Coroutine fadingAudioListener;

	private List<int> availableWidths = new List<int>();

	private int[] compatibleWidths = new int[6] { 960, 1280, 1600, 1920, 2560, 3840 };

	private static bool isResInitiated;

	public AudioMixer mixer;

	private void Awake()
	{
		mgr = this;
		Time.timeScale = 1f;
		AudioListener.pause = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		timer += Time.unscaledDeltaTime;
		if (timer > 0.1f)
		{
			if ((float)Screen.height / (float)Screen.width != 0.5625f && !ControlHandler.mgr.GetMouse().leftButton.isPressed)
			{
				SetDefaultResolution();
			}
			timer = 0f;
		}
	}

	public void FadeInAudioListener()
	{
		CancelCoroutine(fadingAudioListener);
		fadingAudioListener = StartCoroutine(FadingInAudioListener());
	}

	private IEnumerator FadingInAudioListener()
	{
		AudioListener.volume = 0f;
		while (AudioListener.volume < 1f)
		{
			AudioListener.volume += 0.5f * Time.unscaledDeltaTime;
			yield return null;
		}
		AudioListener.volume = 1f;
	}

	public void FadeOutAudioListener(float rate)
	{
		CancelCoroutine(fadingAudioListener);
		fadingAudioListener = StartCoroutine(FadingOutAudioListener(rate));
	}

	private IEnumerator FadingOutAudioListener(float rate)
	{
		while (AudioListener.volume > 0f)
		{
			AudioListener.volume -= rate * Time.unscaledDeltaTime;
			yield return null;
		}
	}

	public void PauseTime()
	{
		tempTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;
	}

	public void UnpauseTime()
	{
		StartCoroutine(UnpausingTime());
	}

	private IEnumerator UnpausingTime()
	{
		yield return null;
		Time.timeScale = tempTimeScale;
		AudioListener.pause = false;
	}

	public void WarpTime(float newWarpSpeed)
	{
		CancelCoroutine(warpingTime);
		warpingTime = StartCoroutine(WarpingTime(newWarpSpeed));
	}

	private IEnumerator WarpingTime(float newWarpSpeed)
	{
		warpSpeed = newWarpSpeed;
		Time.timeScale = warpSpeed;
		if (MusicBox.env != null)
		{
			MusicBox.env.SetSongSpeed(warpSpeed);
			Interface.env.AccuracyChecker.Show();
		}
		if (warpSpeed != 1f)
		{
			isWarpingTime = true;
			float prevSpedTime = warpedTime;
			float timeStarted = (float)AudioSettings.dspTime;
			while (isWarpingTime)
			{
				warpedTime = prevSpedTime + ((float)AudioSettings.dspTime - timeStarted) * (warpSpeed - 1f);
				yield return null;
			}
		}
		else
		{
			isWarpingTime = false;
			if (MusicBox.env != null)
			{
				Interface.env.AccuracyChecker.Hide();
			}
		}
	}

	public void SetMixerPrefs()
	{
		float value = ((SaveManager.mgr.GetMaster() > 0) ? (Mathf.Log10((float)SaveManager.mgr.GetMaster() / 10f) * 20f) : (-80f));
		float value2 = ((SaveManager.mgr.GetMusic() > 0) ? (Mathf.Log10((float)SaveManager.mgr.GetMusic() / 10f) * 20f) : (-80f));
		float value3 = ((SaveManager.mgr.GetSfx() > 0) ? (Mathf.Log10((float)SaveManager.mgr.GetSfx() / 10f) * 20f) : (-80f));
		float value4 = ((SaveManager.mgr.GetMetronome() > 0) ? (Mathf.Log10((float)SaveManager.mgr.GetMetronome() / 10f) * 20f) : (-80f));
		mixer.SetFloat("masterVol", value);
		mixer.SetFloat("musicVol", value2);
		mixer.SetFloat("sfxVol", value3);
		mixer.SetFloat("metronomeVol", value4);
	}

	public void SetAudioListener(float value)
	{
		AudioListener.volume = value;
	}

	public void ToggleVsync(bool toggle)
	{
		QualitySettings.vSyncCount = (toggle ? 1 : 0);
	}

	public void IncreaseVolume(int index)
	{
		switch (index)
		{
		case 0:
		{
			int num2 = ((SaveManager.mgr.GetMaster() + 1 <= 10) ? (SaveManager.mgr.GetMaster() + 1) : 0);
			float value2 = ((num2 > 0) ? (Mathf.Log10((float)num2 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMaster(num2);
			mixer.SetFloat("masterVol", value2);
			break;
		}
		case 1:
		{
			int num4 = ((SaveManager.mgr.GetMusic() + 1 <= 10) ? (SaveManager.mgr.GetMusic() + 1) : 0);
			float value4 = ((num4 > 0) ? (Mathf.Log10((float)num4 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMusic(num4);
			mixer.SetFloat("musicVol", value4);
			break;
		}
		case 2:
		{
			int num3 = ((SaveManager.mgr.GetSfx() + 1 <= 10) ? (SaveManager.mgr.GetSfx() + 1) : 0);
			float value3 = ((num3 > 0) ? (Mathf.Log10((float)num3 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetSfx(num3);
			mixer.SetFloat("sfxVol", value3);
			break;
		}
		case 3:
		{
			int num = ((SaveManager.mgr.GetMetronome() + 1 <= 10) ? (SaveManager.mgr.GetMetronome() + 1) : 0);
			float value = ((num > 0) ? (Mathf.Log10((float)num / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMetronome(num);
			mixer.SetFloat("metronomeVol", value);
			break;
		}
		}
	}

	public void DecreaseVolume(int index)
	{
		switch (index)
		{
		case 0:
		{
			int num2 = ((SaveManager.mgr.GetMaster() - 1 < 0) ? 10 : (SaveManager.mgr.GetMaster() - 1));
			float value2 = ((num2 > 0) ? (Mathf.Log10((float)num2 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMaster(num2);
			mixer.SetFloat("masterVol", value2);
			break;
		}
		case 1:
		{
			int num4 = ((SaveManager.mgr.GetMusic() - 1 < 0) ? 10 : (SaveManager.mgr.GetMusic() - 1));
			float value4 = ((num4 > 0) ? (Mathf.Log10((float)num4 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMusic(num4);
			mixer.SetFloat("musicVol", value4);
			break;
		}
		case 2:
		{
			int num3 = ((SaveManager.mgr.GetSfx() - 1 < 0) ? 10 : (SaveManager.mgr.GetSfx() - 1));
			float value3 = ((num3 > 0) ? (Mathf.Log10((float)num3 / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetSfx(num3);
			mixer.SetFloat("sfxVol", value3);
			break;
		}
		case 3:
		{
			int num = ((SaveManager.mgr.GetMetronome() - 1 < 0) ? 10 : (SaveManager.mgr.GetMetronome() - 1));
			float value = ((num > 0) ? (Mathf.Log10((float)num / 10f) * 20f) : (-80f));
			SaveManager.mgr.SetMetronome(num);
			mixer.SetFloat("metronomeVol", value);
			break;
		}
		}
	}

	public void SwitchFullscreenMode()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	private void ListAvailableWidths()
	{
		availableWidths.Clear();
		for (int i = 0; i < compatibleWidths.Length; i++)
		{
			if (compatibleWidths[i] <= Screen.currentResolution.width)
			{
				availableWidths.Add(compatibleWidths[i]);
			}
		}
	}

	public void InitiateResolution()
	{
		StartCoroutine(InitiatingResolution());
	}

	private IEnumerator InitiatingResolution()
	{
		bool isFullscreen = false;
		if (Screen.fullScreen)
		{
			isFullscreen = true;
			Screen.fullScreen = false;
			yield return null;
		}
		ListAvailableWidths();
		bool flag = true;
		for (int i = 0; i < availableWidths.Count; i++)
		{
			if (Screen.width == availableWidths[i] && Screen.height == Mathf.RoundToInt((float)availableWidths[i] * 0.5625f))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			if (availableWidths[availableWidths.Count - 1] > 1920)
			{
				Screen.SetResolution(1920, 1080, isFullscreen);
			}
			else
			{
				Screen.SetResolution(availableWidths[availableWidths.Count - 1], Mathf.RoundToInt((float)availableWidths[availableWidths.Count - 1] * 0.5625f), isFullscreen);
			}
		}
		else
		{
			Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
		}
		isResInitiated = true;
	}

	private void SetDefaultResolution()
	{
		StartCoroutine(SettingDefaultResolution());
	}

	private IEnumerator SettingDefaultResolution()
	{
		if (!isResInitiated || isSettingResolution)
		{
			yield break;
		}
		isSettingResolution = true;
		bool isFullscreen = false;
		if (Screen.fullScreen)
		{
			isFullscreen = true;
			Screen.fullScreen = false;
			yield return null;
		}
		ListAvailableWidths();
		if (availableWidths.Count > 0)
		{
			if (availableWidths[availableWidths.Count - 1] > 1920)
			{
				Screen.SetResolution(1920, 1080, isFullscreen);
			}
			else
			{
				Screen.SetResolution(availableWidths[availableWidths.Count - 1], Mathf.RoundToInt((float)availableWidths[availableWidths.Count - 1] * 0.5625f), isFullscreen);
			}
			yield return null;
		}
		isSettingResolution = false;
	}

	public void IncreaseResolution()
	{
		StartCoroutine(IncreasingResolution());
	}

	private IEnumerator IncreasingResolution()
	{
		if (isSettingResolution || Screen.width == 3840)
		{
			yield break;
		}
		isSettingResolution = true;
		bool isFullscreen = false;
		if (Screen.fullScreen)
		{
			isFullscreen = true;
			Screen.fullScreen = false;
			yield return null;
		}
		ListAvailableWidths();
		if (availableWidths.Count > 1)
		{
			for (int i = 0; i < availableWidths.Count; i++)
			{
				if (availableWidths[i] == Screen.width)
				{
					if (i != availableWidths.Count - 1)
					{
						Screen.SetResolution(availableWidths[i + 1], Mathf.RoundToInt((float)availableWidths[i + 1] * 0.5625f), isFullscreen);
					}
					else
					{
						Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
					}
					break;
				}
			}
		}
		else
		{
			Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
		}
		yield return null;
		isSettingResolution = false;
	}

	public void DecreaseResolution()
	{
		StartCoroutine(DecreasingResolution());
	}

	private IEnumerator DecreasingResolution()
	{
		if (isSettingResolution || Screen.width == 960)
		{
			yield break;
		}
		isSettingResolution = true;
		bool isFullscreen = false;
		if (Screen.fullScreen)
		{
			isFullscreen = true;
			Screen.fullScreen = false;
			yield return null;
		}
		ListAvailableWidths();
		if (availableWidths.Count > 1)
		{
			for (int i = 0; i < availableWidths.Count; i++)
			{
				if (availableWidths[i] == Screen.width)
				{
					if (i != 0)
					{
						Screen.SetResolution(availableWidths[i - 1], Mathf.RoundToInt((float)availableWidths[i - 1] * 0.5625f), isFullscreen);
					}
					else
					{
						Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
					}
					break;
				}
			}
		}
		else
		{
			Screen.SetResolution(Screen.width, Screen.height, isFullscreen);
		}
		yield return null;
		isSettingResolution = false;
	}

	public float GetDspTime()
	{
		return (float)AudioSettings.dspTime + warpedTime;
	}

	public float GetWarpSpeed()
	{
		return warpSpeed;
	}
}
