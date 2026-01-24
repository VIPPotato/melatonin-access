using System.Collections;
using UnityEngine;

public class Instruction : Wrapper
{
	[Header("Fragments")]
	public Fragment gear;

	public Fragment prompt;

	public textboxFragment label;

	private bool isActivated;

	private float timeTilFadeOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		gear.Awake();
		prompt.Awake();
		label.Initiate();
		timeTilFadeOut = gear.GetAnimDuration("lowerAndFadeOut");
		RenderChildren(toggle: false, 1);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		isActivated = true;
		RenderChildren(toggle: true, 1);
		gear.TriggerAnim("riseAndFadeIn");
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadCROSS");
		}
		else
		{
			prompt.TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		}
		RefreshLabel();
	}

	public void Deactivate()
	{
		if (isActivated)
		{
			CancelCoroutine(deactivating);
			deactivating = StartCoroutine(Deactivating());
		}
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		gear.TriggerAnim("lowerAndFadeOut");
		yield return new WaitForSeconds(timeTilFadeOut);
		RenderChildren(toggle: false, 1);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 1);
		isActivated = true;
		gear.TriggerAnim("shown");
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadCROSS");
		}
		else
		{
			prompt.TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		}
		RefreshLabel();
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false, 1);
		isActivated = false;
	}

	private void RefreshLabel()
	{
		label.SetState(0);
		prompt.SetLocalScale(1f, 1f);
		switch (SaveManager.GetLang())
		{
		case 0:
			label.SetFontSize(5f);
			label.SetLetterSpacing(0f);
			prompt.SetLocalX(-0.182f);
			break;
		case 1:
			label.SetFontSize(5f);
			label.SetLetterSpacing(6f);
			prompt.SetLocalX(-0.25f);
			break;
		case 2:
			label.SetFontSize(5f);
			label.SetLetterSpacing(12f);
			prompt.SetLocalX(0f);
			break;
		case 3:
			label.SetFontSize(5f);
			label.SetLetterSpacing(-4.5f);
			prompt.SetLocalX(-1.96f);
			break;
		case 4:
			label.SetFontSize(5f);
			label.SetLetterSpacing(-1.5f);
			prompt.SetLocalX(-1.85f);
			break;
		case 5:
			label.SetFontSize(4.8f);
			label.SetLetterSpacing(-1f);
			prompt.SetLocalX(-1.93f);
			break;
		case 6:
			label.SetFontSize(3.2f);
			label.SetLetterSpacing(-3f);
			prompt.SetLocalX(-0.317f);
			prompt.SetLocalScale(0.9f, 0.9f);
			break;
		case 7:
			label.SetFontSize(3.8f);
			label.SetLetterSpacing(-3f);
			prompt.SetLocalX(-2.01f);
			break;
		case 8:
			label.SetFontSize(3.6f);
			label.SetLetterSpacing(-2f);
			prompt.SetLocalX(-0.5f);
			break;
		case 9:
			label.SetFontSize(4f);
			label.SetLetterSpacing(-2f);
			prompt.SetLocalX(-0.08f);
			break;
		}
	}
}
