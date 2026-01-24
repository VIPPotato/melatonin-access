using System.Collections;
using UnityEngine;

public class CalibrationTool : Wrapper
{
	public static CalibrationTool env;

	[Header("Children")]
	public PingBar PingBar;

	[Header("Fragments")]
	public textboxFragment title;

	public textboxFragment description;

	public textboxFragment offset;

	public textboxFragment number;

	public textboxFragment label;

	public Fragment speaker;

	public Fragment activator;

	public Fragment[] prompts;

	public Fragment doneButton;

	private bool isActivated;

	private float timeTilOut;

	private Coroutine deactivating;

	private Coroutine changingOffset;

	protected override void Awake()
	{
		env = this;
		title.Initiate();
		description.Initiate();
		offset.Initiate();
		number.Initiate();
		label.Initiate();
		speaker.Awake();
		activator.Awake();
		Fragment[] array = prompts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		doneButton.Awake();
		timeTilOut = activator.GetAnimDuration("out");
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(changingOffset);
		isActivated = true;
		RenderChildren(toggle: true, 1);
		SetParentAndReposition(Interface.env.Cam.GetSliderTransform());
		SetLocalZ(1f);
		if (ControlHandler.mgr.GetCtrlType() != 1 && ControlHandler.mgr.GetCtrlType() != 2)
		{
			SaveManager.mgr.GetActionKey();
		}
		title.SetState(0);
		description.SetState(0);
		offset.SetState(0);
		speaker.TriggerSound(2);
		number.SetText(SaveManager.mgr.GetCalibrationOffsetMs().ToString() ?? "");
		activator.TriggerAnim("in");
		RefreshPrompt();
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompts[1].TriggerAnim("gamepadB");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompts[1].TriggerAnim("gamepadCIRCLE");
		}
		else
		{
			prompts[1].TriggerAnim("keyESC");
		}
		PingBar.Activate();
		PingBar.Ping();
		Interface.env.Disable();
		TitleWorld.env.PauseMusic();
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		CancelCoroutine(changingOffset);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		speaker.TriggerSound(3);
		activator.TriggerAnim("out");
		PingBar.Deactivate();
		yield return new WaitForSeconds(timeTilOut);
		PingBar.Hide();
		SetParentAndReposition(null);
		RenderChildren(toggle: false, 1);
		Interface.env.Enable();
		TitleWorld.env.PlayMusic();
	}

	private void Update()
	{
		if (!isActivated)
		{
			return;
		}
		if (ControlHandler.mgr.CheckIsRightPressed())
		{
			if (SaveManager.mgr.GetCalibrationOffsetMs() < 50)
			{
				IncreaseOffset();
			}
			else
			{
				speaker.TriggerSound(1);
			}
		}
		else if (ControlHandler.mgr.CheckIsLeftPressed())
		{
			if ((float)SaveManager.mgr.GetCalibrationOffsetMs() > -50f)
			{
				DecreaseOffset();
			}
			else
			{
				speaker.TriggerSound(1);
			}
		}
		else if (ControlHandler.mgr.CheckIsCancelPressed())
		{
			Deactivate();
		}
	}

	public void IncreaseOffset()
	{
		CancelCoroutine(changingOffset);
		changingOffset = StartCoroutine(IncreasingOffset());
	}

	private IEnumerator IncreasingOffset()
	{
		int num = SaveManager.mgr.GetCalibrationOffsetMs() + 1;
		SaveManager.mgr.SetCalibrationOffsetMs(num);
		number.SetText(num.ToString() ?? "");
		if (Dream.dir != null)
		{
			Dream.dir.SetOffsetSeconds(num);
		}
		PingBar.RefreshWindow();
		speaker.TriggerSound(0);
		yield return new WaitForSeconds(0.5f);
		while (ControlHandler.mgr.CheckIsRightPressing() && SaveManager.mgr.GetCalibrationOffsetMs() < 50)
		{
			num = SaveManager.mgr.GetCalibrationOffsetMs() + 1;
			SaveManager.mgr.SetCalibrationOffsetMs(num);
			number.SetText(num.ToString() ?? "");
			if (Dream.dir != null)
			{
				Dream.dir.SetOffsetSeconds(num);
			}
			PingBar.RefreshWindow();
			speaker.TriggerSound(0);
			yield return new WaitForSeconds(0.08f);
			yield return null;
		}
	}

	public void DecreaseOffset()
	{
		CancelCoroutine(changingOffset);
		changingOffset = StartCoroutine(DecreasingOffset());
	}

	private IEnumerator DecreasingOffset()
	{
		int num = SaveManager.mgr.GetCalibrationOffsetMs() - 1;
		SaveManager.mgr.SetCalibrationOffsetMs(num);
		number.SetText(num.ToString() ?? "");
		if (Dream.dir != null)
		{
			Dream.dir.SetOffsetSeconds(num);
		}
		PingBar.RefreshWindow();
		speaker.TriggerSound(0);
		yield return new WaitForSeconds(0.5f);
		while (ControlHandler.mgr.CheckIsLeftPressing() && SaveManager.mgr.GetCalibrationOffsetMs() > -50)
		{
			num = SaveManager.mgr.GetCalibrationOffsetMs() - 1;
			SaveManager.mgr.SetCalibrationOffsetMs(num);
			number.SetText(num.ToString() ?? "");
			if (Dream.dir != null)
			{
				Dream.dir.SetOffsetSeconds(num);
			}
			PingBar.RefreshWindow();
			speaker.TriggerSound(0);
			yield return new WaitForSeconds(0.08f);
			yield return null;
		}
	}

	private void RefreshPrompt()
	{
		string animName = ((ControlHandler.mgr.GetCtrlType() == 1) ? "gamepadA" : ((ControlHandler.mgr.GetCtrlType() != 2) ? ("key" + SaveManager.mgr.GetActionKey()) : "gamepadCROSS"));
		prompts[0].TriggerAnim(animName);
		switch (SaveManager.GetLang())
		{
		case 0:
			prompts[0].SetLocalPosition(-3.895f, 1.318f);
			break;
		case 1:
			prompts[0].SetLocalPosition(-1.4f, 1.353f);
			break;
		case 2:
			prompts[0].SetLocalPosition(-1.4f, 1.353f);
			break;
		case 3:
			prompts[0].SetLocalPosition(-0.52f, 1.353f);
			break;
		case 4:
			prompts[0].SetLocalPosition(-1.68f, 1.353f);
			break;
		case 5:
			prompts[0].SetLocalPosition(-3.93f, 1.318f);
			break;
		case 6:
			prompts[0].SetLocalPosition(-2.93f, 1.318f);
			break;
		case 7:
			prompts[0].SetLocalPosition(-3.67f, 1.318f);
			break;
		case 8:
			prompts[0].SetLocalPosition(-3.48f, 1.318f);
			break;
		case 9:
			prompts[0].SetLocalPosition(-3.3f, 1.318f);
			break;
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
