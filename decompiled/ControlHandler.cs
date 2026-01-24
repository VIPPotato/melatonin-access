using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ControlHandler : Custom
{
	public static ControlHandler mgr;

	private static int ctrlType = 0;

	private static float vibrationHard = 1f;

	private static float vibrationLight = 0.5f;

	private bool isRebinding;

	private float leftStickX;

	private float leftStickY;

	private Coroutine displayingCursor;

	private Coroutine rumbling;

	private KeyControl actionKey;

	private KeyControl leftKey;

	private KeyControl rightKey;

	private KeyControl upKey;

	private KeyControl downKey;

	private void Awake()
	{
		mgr = this;
	}

	private void Start()
	{
		if (SceneMonitor.mgr.GetActiveSceneName() != "BootUp")
		{
			Cursor.visible = false;
			actionKey = GetKeyControlFromKey(SaveManager.mgr.GetActionKey());
			ToggleDirectionKeysAlt(SaveManager.mgr.CheckIsDirectionKeysAlt());
		}
	}

	private void OnDisable()
	{
		CancelCoroutine(displayingCursor);
		CancelCoroutine(rumbling);
	}

	private void Update()
	{
		if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero && !Cursor.visible)
		{
			DisplayCursor();
		}
		if (Keyboard.current != null && ctrlType != 0 && Keyboard.current.anyKey.wasPressedThisFrame)
		{
			ctrlType = 0;
		}
		else if (Gamepad.current != null && ctrlType == 0 && (Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame || GetLeftStickX() > 0f))
		{
			ConfigureGamepadType();
		}
		if (isRebinding)
		{
			if (Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.spaceKey);
			}
			else if (Keyboard.current.enterKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.enterKey);
			}
			else if (Keyboard.current.qKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.qKey);
			}
			else if (Keyboard.current.eKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.eKey);
			}
			else if (Keyboard.current.tKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.tKey);
			}
			else if (Keyboard.current.yKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.yKey);
			}
			else if (Keyboard.current.uKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.uKey);
			}
			else if (Keyboard.current.iKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.iKey);
			}
			else if (Keyboard.current.oKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.oKey);
			}
			else if (Keyboard.current.fKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.fKey);
			}
			else if (Keyboard.current.gKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.gKey);
			}
			else if (Keyboard.current.hKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.hKey);
			}
			else if (Keyboard.current.jKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.jKey);
			}
			else if (Keyboard.current.kKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.kKey);
			}
			else if (Keyboard.current.lKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.lKey);
			}
			else if (Keyboard.current.zKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.zKey);
			}
			else if (Keyboard.current.xKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.xKey);
			}
			else if (Keyboard.current.cKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.cKey);
			}
			else if (Keyboard.current.vKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.vKey);
			}
			else if (Keyboard.current.bKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.bKey);
			}
			else if (Keyboard.current.nKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.nKey);
			}
			else if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.leftBracketKey);
			}
			else if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.rightBracketKey);
			}
			else if (Keyboard.current.semicolonKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.semicolonKey);
			}
			else if (Keyboard.current.quoteKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.quoteKey);
			}
			else if (Keyboard.current.commaKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.commaKey);
			}
			else if (Keyboard.current.periodKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.periodKey);
			}
			else if (Keyboard.current.slashKey.wasPressedThisFrame)
			{
				Rebind(Keyboard.current.slashKey);
			}
			else if (CheckIsCancelPressed())
			{
				EndRebind();
			}
			else if (Keyboard.current.anyKey.wasPressedThisFrame)
			{
				RebindError("invalid key");
			}
		}
	}

	public void StartRebind()
	{
		StartCoroutine(StartingRebind());
	}

	private IEnumerator StartingRebind()
	{
		Interface.env.Submenu.PlaySfx(0);
		Interface.env.Disable();
		Interface.env.Submenu.RebindModal.Show();
		yield return null;
		isRebinding = true;
	}

	public void EndRebind()
	{
		StartCoroutine(EndingRebind());
	}

	private IEnumerator EndingRebind()
	{
		isRebinding = false;
		Interface.env.Submenu.PlaySfx(0);
		Interface.env.Submenu.RebindModal.Hide();
		yield return null;
		Interface.env.Enable();
	}

	public void Rebind(KeyControl newKey)
	{
		actionKey = newKey;
		string keyFromKeyControl = GetKeyFromKeyControl(newKey);
		Interface.env.Submenu.KeyboardDisplay.SetActionKey(keyFromKeyControl);
		SaveManager.mgr.SetActionKey(keyFromKeyControl);
		EndRebind();
	}

	public void RebindError(string message)
	{
		Interface.env.Cam.Glow(0.333f);
		Interface.env.Submenu.PlaySfx(2);
		MonoBehaviour.print(message);
	}

	private void DisplayCursor()
	{
		CancelCoroutine(displayingCursor);
		displayingCursor = StartCoroutine(DisplayingCursor());
	}

	private IEnumerator DisplayingCursor()
	{
		Cursor.visible = true;
		yield return new WaitForSecondsRealtime(0.5f);
		Cursor.visible = false;
	}

	public void RumbleHard()
	{
		if (ctrlType > 0 && Gamepad.current != null && !SaveManager.mgr.CheckIsVibrationDisabled())
		{
			CancelCoroutine(rumbling);
			rumbling = StartCoroutine(RumblingHard());
		}
	}

	private IEnumerator RumblingHard()
	{
		Gamepad.current.SetMotorSpeeds(vibrationHard, vibrationHard);
		yield return new WaitForSecondsRealtime(0.15f);
		InputSystem.ResetHaptics();
	}

	public void RumbleLight()
	{
		if (ctrlType > 0 && Gamepad.current != null && !SaveManager.mgr.CheckIsVibrationDisabled())
		{
			CancelCoroutine(rumbling);
			rumbling = StartCoroutine(RumblingLight());
		}
	}

	private IEnumerator RumblingLight()
	{
		Gamepad.current.SetMotorSpeeds(vibrationLight, vibrationLight);
		yield return new WaitForSecondsRealtime(0.15f);
		InputSystem.ResetHaptics();
	}

	public void ToggleDirectionKeysAlt(bool toggle)
	{
		if (!toggle)
		{
			leftKey = Keyboard.current.leftArrowKey;
			rightKey = Keyboard.current.rightArrowKey;
			upKey = Keyboard.current.upArrowKey;
			downKey = Keyboard.current.downArrowKey;
		}
		else
		{
			leftKey = Keyboard.current.aKey;
			rightKey = Keyboard.current.dKey;
			upKey = Keyboard.current.wKey;
			downKey = Keyboard.current.sKey;
		}
	}

	public void ConfigureGamepadType()
	{
		if (Builder.mgr.GetOperatingSystemNum() == 2)
		{
			ctrlType = 1;
			vibrationHard = 0.2f;
			vibrationLight = 0.1f;
		}
		else if (Gamepad.current.layout.ToLower().Contains("dualshock"))
		{
			ctrlType = 2;
			vibrationHard = 1f;
			vibrationLight = 0.5f;
		}
		else if (Gamepad.current.layout.ToLower().Contains("dualsense"))
		{
			ctrlType = 2;
			vibrationHard = 0.1f;
			vibrationLight = 0.05f;
		}
		else
		{
			ctrlType = 1;
			vibrationHard = 1f;
			vibrationLight = 0.5f;
		}
	}

	private KeyControl GetKeyControlFromKey(string key)
	{
		return key switch
		{
			"SPACE" => Keyboard.current.spaceKey, 
			"ENTER" => Keyboard.current.enterKey, 
			"Q" => Keyboard.current.qKey, 
			"E" => Keyboard.current.eKey, 
			"T" => Keyboard.current.tKey, 
			"Y" => Keyboard.current.yKey, 
			"U" => Keyboard.current.uKey, 
			"I" => Keyboard.current.iKey, 
			"O" => Keyboard.current.oKey, 
			"F" => Keyboard.current.fKey, 
			"G" => Keyboard.current.gKey, 
			"H" => Keyboard.current.hKey, 
			"J" => Keyboard.current.jKey, 
			"K" => Keyboard.current.kKey, 
			"L" => Keyboard.current.lKey, 
			"Z" => Keyboard.current.zKey, 
			"X" => Keyboard.current.xKey, 
			"C" => Keyboard.current.cKey, 
			"V" => Keyboard.current.vKey, 
			"B" => Keyboard.current.bKey, 
			"N" => Keyboard.current.nKey, 
			"M" => Keyboard.current.mKey, 
			"[" => Keyboard.current.leftBracketKey, 
			"]" => Keyboard.current.rightBracketKey, 
			";" => Keyboard.current.semicolonKey, 
			"'" => Keyboard.current.quoteKey, 
			"," => Keyboard.current.commaKey, 
			"PERIOD" => Keyboard.current.periodKey, 
			"SLASH" => Keyboard.current.slashKey, 
			_ => Keyboard.current.spaceKey, 
		};
	}

	private string GetKeyFromKeyControl(KeyControl keyControl)
	{
		if (keyControl == Keyboard.current.spaceKey)
		{
			return "SPACE";
		}
		if (keyControl == Keyboard.current.enterKey)
		{
			return "ENTER";
		}
		if (keyControl == Keyboard.current.qKey)
		{
			return "Q";
		}
		if (keyControl == Keyboard.current.eKey)
		{
			return "E";
		}
		if (keyControl == Keyboard.current.tKey)
		{
			return "T";
		}
		if (keyControl == Keyboard.current.yKey)
		{
			return "Y";
		}
		if (keyControl == Keyboard.current.uKey)
		{
			return "U";
		}
		if (keyControl == Keyboard.current.iKey)
		{
			return "I";
		}
		if (keyControl == Keyboard.current.oKey)
		{
			return "O";
		}
		if (keyControl == Keyboard.current.fKey)
		{
			return "F";
		}
		if (keyControl == Keyboard.current.gKey)
		{
			return "G";
		}
		if (keyControl == Keyboard.current.hKey)
		{
			return "H";
		}
		if (keyControl == Keyboard.current.jKey)
		{
			return "J";
		}
		if (keyControl == Keyboard.current.kKey)
		{
			return "K";
		}
		if (keyControl == Keyboard.current.lKey)
		{
			return "L";
		}
		if (keyControl == Keyboard.current.zKey)
		{
			return "Z";
		}
		if (keyControl == Keyboard.current.xKey)
		{
			return "X";
		}
		if (keyControl == Keyboard.current.cKey)
		{
			return "C";
		}
		if (keyControl == Keyboard.current.vKey)
		{
			return "V";
		}
		if (keyControl == Keyboard.current.bKey)
		{
			return "B";
		}
		if (keyControl == Keyboard.current.nKey)
		{
			return "N";
		}
		if (keyControl == Keyboard.current.mKey)
		{
			return "M";
		}
		if (keyControl == Keyboard.current.leftBracketKey)
		{
			return "[";
		}
		if (keyControl == Keyboard.current.rightBracketKey)
		{
			return "]";
		}
		if (keyControl == Keyboard.current.semicolonKey)
		{
			return ";";
		}
		if (keyControl == Keyboard.current.quoteKey)
		{
			return "'";
		}
		if (keyControl == Keyboard.current.commaKey)
		{
			return ",";
		}
		if (keyControl == Keyboard.current.periodKey)
		{
			return "PERIOD";
		}
		if (keyControl == Keyboard.current.slashKey)
		{
			return "SLASH";
		}
		return "SPACE";
	}

	public Mouse GetMouse()
	{
		return Mouse.current;
	}

	public Keyboard GetKeyboard()
	{
		return Keyboard.current;
	}

	public Gamepad GetGamepad()
	{
		return Gamepad.current;
	}

	public int GetCtrlType()
	{
		return ctrlType;
	}

	public bool CheckIsActionPressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && actionKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionReleased()
	{
		if (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && actionKey.wasReleasedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionLeftPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.leftShoulder.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && leftKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionLeftPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.leftShoulder.isPressed || Gamepad.current.leftTrigger.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && leftKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionLeftReleased()
	{
		if (Gamepad.current != null && (Gamepad.current.leftShoulder.wasReleasedThisFrame || Gamepad.current.leftTrigger.wasReleasedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && leftKey.wasReleasedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionRightPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.rightShoulder.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && rightKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionRightPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.rightShoulder.isPressed || Gamepad.current.rightTrigger.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && rightKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionRightReleased()
	{
		if (Gamepad.current != null && (Gamepad.current.rightShoulder.wasReleasedThisFrame || Gamepad.current.rightTrigger.wasReleasedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && rightKey.wasReleasedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionUpPressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && upKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsActionUpReleased()
	{
		if (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && upKey.wasReleasedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsCancelPressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsExtraPressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsSwapPressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsLeftPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.left.wasPressedThisFrame || Gamepad.current.leftStick.left.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && leftKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsLeftPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.left.isPressed || Gamepad.current.leftStick.left.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && leftKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsRightPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.right.wasPressedThisFrame || Gamepad.current.leftStick.right.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && rightKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsRightPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.right.isPressed || Gamepad.current.leftStick.right.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && rightKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsUpPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.leftStick.up.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && upKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsUpPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.up.isPressed || Gamepad.current.leftStick.up.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && upKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsDownPressed()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.down.wasPressedThisFrame || Gamepad.current.leftStick.down.wasPressedThisFrame))
		{
			return true;
		}
		if (Keyboard.current != null && downKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsDownPressing()
	{
		if (Gamepad.current != null && (Gamepad.current.dpad.down.isPressed || Gamepad.current.leftStick.down.isPressed))
		{
			return true;
		}
		if (Keyboard.current != null && downKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsStartPressed()
	{
		if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsRemovePressed()
	{
		if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsRemovePressing()
	{
		if (Gamepad.current != null && Gamepad.current.buttonEast.isPressed)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.rKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsPlayReleased()
	{
		if (Gamepad.current != null && Gamepad.current.buttonWest.wasReleasedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.pKey.wasReleasedThisFrame)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsPlayPressing()
	{
		if (Gamepad.current != null && Gamepad.current.buttonWest.isPressed)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.pKey.isPressed)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsMorePressed()
	{
		if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
		{
			return true;
		}
		if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	public float GetLeftStickX()
	{
		if (Gamepad.current != null)
		{
			return Gamepad.current.leftStick.x.ReadValue();
		}
		return 0f;
	}

	public float GetLeftStickY()
	{
		if (Gamepad.current != null)
		{
			return Gamepad.current.leftStick.y.ReadValue();
		}
		return 0f;
	}
}
