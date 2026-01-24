using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Interface : Wrapper
{
	public static Interface env;

	[Header("Children")]
	public Cam Cam;

	public Spinner Spinner;

	public CommunityMenu CommunityMenu;

	public Letterbox Letterbox;

	public AccuracyChecker AccuracyChecker;

	public Submenu Submenu;

	public Results Results;

	public WipeScreen WipeScreen;

	public FeatherBorder FeatherBorder;

	public ScoreMeter ScoreMeter;

	public SideLabel SideLabel;

	public Circle Circle;

	public Fader Fader;

	[Header("Props")]
	public PostProcessVolume fxController;

	private ChromaticAberration aberration;

	private ColorGrading colorGrading;

	private bool isEnabled = true;

	private Coroutine abberating;

	private float initAbberation;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		fxController.profile.TryGetSettings<ChromaticAberration>(out aberration);
		initAbberation = aberration.intensity.value;
	}

	private void Start()
	{
		if (SceneMonitor.mgr.GetActiveSceneName() != "BootUp")
		{
			fxController.profile.TryGetSettings<ColorGrading>(out colorGrading);
			float num = (float)(SaveManager.mgr.GetContrast() + 6) / 40f;
			colorGrading.gamma.value = new Vector4(1f, 1f, 1f, -0.1f - num);
		}
	}

	private void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		if (!Submenu.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsStartPressed())
			{
				OpenSubmenu();
			}
			else if (TitleWorld.env != null && TitleWorld.env.DreamOcean.CheckIsActivated() && ControlHandler.mgr.CheckIsActionPressed())
			{
				OpenSubmenu();
			}
		}
		else if (ControlHandler.mgr.CheckIsStartPressed() || ControlHandler.mgr.CheckIsCancelPressed())
		{
			if (Submenu.GetMenuType() != 0)
			{
				Submenu.SetPreviousMenu();
			}
			else
			{
				CloseSubmenu();
			}
		}
	}

	private void OpenSubmenu()
	{
		if (EditorUI.env == null)
		{
			DisableAberration();
		}
		Cam.SlideCameraLeft();
		Submenu.Activate();
		if (Dream.dir != null)
		{
			Dream.dir.ToggleIsControllable(toggle: false);
			Technician.mgr.ToggleVsync(toggle: true);
		}
		if (TitleWorld.env != null)
		{
			if (TitleWorld.env.DreamOcean.CheckIsActivated())
			{
				TitleWorld.env.DeactivateInterfaces();
			}
		}
		else
		{
			Technician.mgr.PauseTime();
		}
	}

	public void CloseSubmenu()
	{
		if (EditorUI.env == null)
		{
			EnableAberration();
		}
		Cam.UnslideCameraLeft();
		Submenu.Deactivate();
		if (Dream.dir != null)
		{
			Dream.dir.ToggleIsControllable(toggle: true);
			Dream.dir.TriggerActionReleased();
			Technician.mgr.ToggleVsync(SaveManager.mgr.CheckIsVsynced());
		}
		if (TitleWorld.env != null)
		{
			if (TitleWorld.env.DreamOcean.CheckIsActivated())
			{
				TitleWorld.env.ActivateInterfaces();
			}
		}
		else
		{
			Technician.mgr.UnpauseTime();
		}
	}

	public void ExitTo(string sceneName)
	{
		StartCoroutine(ExitingTo(sceneName));
	}

	private IEnumerator ExitingTo(string sceneName)
	{
		if (TitleScreen.dir != null)
		{
			TitleScreen.dir.ToggleIsDirecting(toggle: false);
		}
		Fader.SetParentAndReposition(env.Cam.GetOuterTransform());
		Fader.Activate();
		Fader.AllowUnscaledTime();
		if (Submenu.CheckIsActivated())
		{
			Cam.UnslideCameraLeft();
			Submenu.Deactivate();
		}
		Technician.mgr.FadeOutAudioListener(0.5f);
		SceneMonitor.mgr.PreloadScene(sceneName);
		yield return new WaitForSecondsRealtime(1.75f);
		SceneMonitor.mgr.LoadScene();
	}

	public void ExitGame()
	{
		StartCoroutine(ExitingGame());
	}

	private IEnumerator ExitingGame()
	{
		if (TitleScreen.dir != null)
		{
			TitleScreen.dir.ToggleIsDirecting(toggle: false);
		}
		Disable();
		if (SaveManager.mgr.CheckIsSavingPlayerDataStacked())
		{
			SaveManager.mgr.SavePlayerData();
		}
		if (SaveManager.mgr.CheckIsSavingEditorDataStacked())
		{
			SaveManager.mgr.SaveEditorData();
		}
		Fader.SetParentAndReposition(env.Cam.GetOuterTransform());
		Fader.Activate();
		Fader.AllowUnscaledTime();
		if (Submenu.CheckIsActivated())
		{
			Cam.UnslideCameraLeft();
			Submenu.Deactivate();
		}
		Spinner.Activate();
		Technician.mgr.FadeOutAudioListener(0.5f);
		yield return new WaitForSecondsRealtime(1.67f);
		Spinner.Deactivate();
		yield return new WaitForSecondsRealtime(0.33f);
		Application.Quit();
	}

	public void EnableAberration()
	{
		CancelCoroutine(abberating);
		abberating = StartCoroutine(EnablingAberration());
	}

	private IEnumerator EnablingAberration()
	{
		fxController.profile.TryGetSettings<ChromaticAberration>(out aberration);
		aberration.intensity.value = 0f;
		while (aberration.intensity.value < initAbberation)
		{
			aberration.intensity.value = aberration.intensity.value + Time.unscaledDeltaTime / 10f;
			yield return null;
		}
		aberration.intensity.value = initAbberation;
	}

	public void DisableAberration()
	{
		CancelCoroutine(abberating);
		abberating = StartCoroutine(DisablingAberration());
	}

	private IEnumerator DisablingAberration()
	{
		fxController.profile.TryGetSettings<ChromaticAberration>(out aberration);
		aberration.intensity.value = initAbberation;
		while (aberration.intensity.value > 0f)
		{
			aberration.intensity.value = aberration.intensity.value - Time.unscaledDeltaTime / 10f;
			yield return null;
		}
		aberration.intensity.value = 0f;
	}

	public void IncreaseContrast()
	{
		int contrast = SaveManager.mgr.GetContrast();
		int contrast2 = ((contrast + 1 > 10) ? 10 : (contrast + 1));
		SaveManager.mgr.SetContrast(contrast2);
		fxController.profile.TryGetSettings<ColorGrading>(out colorGrading);
		float num = (float)(SaveManager.mgr.GetContrast() + 6) / 40f;
		colorGrading.gamma.value = new Vector4(1f, 1f, 1f, -0.1f - num);
	}

	public void DecreaseContrast()
	{
		int contrast = SaveManager.mgr.GetContrast();
		int contrast2 = ((contrast - 1 >= 0) ? (contrast - 1) : 0);
		SaveManager.mgr.SetContrast(contrast2);
		fxController.profile.TryGetSettings<ColorGrading>(out colorGrading);
		float num = (float)(SaveManager.mgr.GetContrast() + 6) / 40f;
		colorGrading.gamma.value = new Vector4(1f, 1f, 1f, -0.1f - num);
	}

	public void ToggleWarmth(bool toggle)
	{
		fxController.profile.TryGetSettings<ColorGrading>(out colorGrading);
		if (toggle)
		{
			colorGrading.temperature.overrideState = true;
			colorGrading.saturation.overrideState = true;
			colorGrading.temperature.value = 5f;
			colorGrading.saturation.value = 12f;
		}
		else
		{
			colorGrading.temperature.overrideState = false;
			colorGrading.saturation.overrideState = false;
			colorGrading.temperature.value = 0f;
			colorGrading.saturation.value = 0f;
		}
	}

	public void Disable()
	{
		isEnabled = false;
		Submenu.Disable();
	}

	public void Enable()
	{
		isEnabled = true;
		Submenu.Enable();
	}

	public bool CheckIsEnabled()
	{
		return isEnabled;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}
}
