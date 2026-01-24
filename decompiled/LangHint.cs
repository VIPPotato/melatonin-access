using System.Collections;
using UnityEngine;

public class LangHint : Wrapper
{
	[Header("Fragments")]
	public Fragment activator;

	public Fragment prompt;

	public textboxFragment label;

	private bool isActivated;

	private Coroutine deactivating;

	protected override void Awake()
	{
		activator.Awake();
		prompt.Awake();
		label.Initiate();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		isActivated = true;
		activator.TriggerAnim("in", 1f, 1f);
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadY");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadTRIANGLE");
		}
		else
		{
			prompt.TriggerAnim("keyTAB");
		}
		RefreshLabel();
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		isActivated = true;
		activator.TriggerAnim("in");
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadY");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadTRIANGLE");
		}
		else
		{
			prompt.TriggerAnim("keyTAB");
		}
		RefreshLabel();
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		activator.TriggerAnim("inReversed");
		yield return new WaitForSeconds(0.5f);
		RenderChildren(toggle: false);
	}

	private void RefreshLabel()
	{
		label.SetState(0);
		label.SetFontSize(4f);
		switch (SaveManager.GetLang())
		{
		case 0:
			label.SetLetterSpacing(0f);
			break;
		case 1:
			label.SetLetterSpacing(-3f);
			break;
		case 2:
			label.SetLetterSpacing(-3f);
			break;
		case 3:
			label.SetFontSize(4.2f);
			label.SetLetterSpacing(3f);
			break;
		case 4:
			label.SetFontSize(4.2f);
			label.SetLetterSpacing(3f);
			break;
		case 5:
			label.SetFontSize(3.4f);
			label.SetLetterSpacing(0f);
			break;
		case 6:
			label.SetLetterSpacing(-1f);
			break;
		case 7:
			label.SetLetterSpacing(0f);
			break;
		case 8:
			label.SetLetterSpacing(0f);
			break;
		case 9:
			label.SetFontSize(3.6f);
			label.SetLetterSpacing(-2f);
			break;
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
