using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : Wrapper
{
	[Header("Children")]
	public MenuTitle MenuTitle;

	public List<Option> Options = new List<Option>();

	public Wave[] Waves;

	private bool isActivated;

	private bool isEnabled;

	private int activeOption;

	private int activeWaves;

	private Coroutine activating;

	private List<Option> alignableOptions = new List<Option>();

	private List<Option> functioningOptions = new List<Option>();

	protected override void Awake()
	{
		SetupFragments();
	}

	public void Activate(int direction)
	{
		CancelCoroutine(activating);
		activating = StartCoroutine(Activating(direction));
	}

	private IEnumerator Activating(int direction)
	{
		isActivated = true;
		activeWaves = 0;
		foreach (Option option in Options)
		{
			if (option.GetFunctionType() > 0)
			{
				alignableOptions.Add(option);
			}
		}
		float num = ((alignableOptions.Count <= 5) ? 1.3f : ((alignableOptions.Count != 6) ? 1.19f : 1.225f));
		float num2 = MenuTitle.GetLocalY() - num;
		for (int i = 0; i < alignableOptions.Count; i++)
		{
			alignableOptions[i].SetLocalY(num2 - num * (float)i);
		}
		float num3 = 7.71f;
		float num4 = Mathf.Abs(MenuTitle.GetLocalY()) + Mathf.Abs(alignableOptions[alignableOptions.Count - 1].GetLocalY());
		float num5 = 13.87f - num4;
		SetLocalY(num3 - num5 / 1.72f);
		foreach (Option option2 in Options)
		{
			if (option2.GetFunctionType() == 2)
			{
				functioningOptions.Add(option2);
			}
		}
		yield return new WaitForSecondsRealtime(0.1f);
		MenuTitle.Activate(direction);
		float startX = ((direction != 0) ? 1 : (-1));
		float distanceX = ((direction == 0) ? 1 : (-1));
		for (int j = 0; j < functioningOptions.Count; j++)
		{
			functioningOptions[j].Activate();
			functioningOptions[j].SetLocalX(startX);
			functioningOptions[j].MoveDistanceRealtime(new Vector3(distanceX, 0f, 0f), 3f, isEasingIn: false);
			yield return new WaitForSecondsRealtime(0.05f);
		}
		functioningOptions[activeOption].Enable();
		ActivateWaves();
		yield return null;
		isEnabled = true;
	}

	public void Deactivate()
	{
		CancelCoroutine(activating);
		isActivated = false;
		isEnabled = false;
		foreach (Option functioningOption in functioningOptions)
		{
			functioningOption.Deactivate();
		}
		DeactivateWaves();
		alignableOptions.Clear();
		functioningOptions.Clear();
		MenuTitle.Deactivate();
	}

	private void Update()
	{
		if (isEnabled)
		{
			if (ControlHandler.mgr.CheckIsDownPressed())
			{
				Next();
			}
			else if (ControlHandler.mgr.CheckIsUpPressed())
			{
				Prev();
			}
			else if (ControlHandler.mgr.CheckIsActionPressed())
			{
				functioningOptions[activeOption].Select();
			}
		}
	}

	public void Enable()
	{
		if (!isEnabled)
		{
			isEnabled = true;
			ActivateWaves();
			functioningOptions[activeOption].Enable();
		}
	}

	public void Disable()
	{
		if (isEnabled)
		{
			isEnabled = false;
			DeactivateWaves();
			if (functioningOptions[activeOption].CheckIsEnabled())
			{
				functioningOptions[activeOption].Disable();
			}
		}
	}

	private void Next()
	{
		DeactivateWaves();
		functioningOptions[activeOption].Disable();
		activeOption++;
		if (activeOption == functioningOptions.Count)
		{
			activeOption = 0;
			Interface.env.Submenu.PlaySfxAlt(0);
		}
		else
		{
			Interface.env.Submenu.PlaySfx(0);
		}
		functioningOptions[activeOption].Enable();
		ActivateWaves();
	}

	private void Prev()
	{
		DeactivateWaves();
		functioningOptions[activeOption].Disable();
		activeOption--;
		if (activeOption < 0)
		{
			activeOption = functioningOptions.Count - 1;
			Interface.env.Submenu.PlaySfxAlt(0);
		}
		else
		{
			Interface.env.Submenu.PlaySfx(0);
		}
		functioningOptions[activeOption].Enable();
		ActivateWaves();
	}

	private void ActivateWaves()
	{
		Waves[activeWaves].Activate();
		Waves[activeWaves].SetLocalY(functioningOptions[activeOption].GetLocalY() - 0.3836f);
		Waves[activeWaves + 2].Activate();
		Waves[activeWaves + 2].SetLocalY(functioningOptions[activeOption].GetLocalY() - 0.4106f);
	}

	private void DeactivateWaves()
	{
		Waves[activeWaves].Deactivate();
		Waves[activeWaves + 2].Deactivate();
		activeWaves++;
		if (activeWaves > 1)
		{
			activeWaves = 0;
		}
	}

	public void ResetActiveOption()
	{
		activeOption = 0;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
