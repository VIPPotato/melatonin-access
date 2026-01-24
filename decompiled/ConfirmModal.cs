using System.Collections;
using UnityEngine;

public class ConfirmModal : Wrapper
{
	[Header("Fragments")]
	public Fragment activator;

	public Fragment[] prompts;

	public textboxFragment[] texts;

	private bool isActivated;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		activator.Awake();
		Fragment[] array = prompts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		textboxFragment[] array2 = texts;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Initiate();
		}
		timeTilOut = activator.GetAnimDuration("out");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		isActivated = true;
		RenderChildren(toggle: true);
		activator.TriggerAnim("in");
		Interface.env.Disable();
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompts[0].TriggerAnim("gamepadA");
			prompts[1].TriggerAnim("gamepadB");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompts[0].TriggerAnim("gamepadCROSS");
			prompts[1].TriggerAnim("gamepadCIRCLE");
		}
		else
		{
			prompts[0].TriggerAnim("key" + SaveManager.mgr.GetActionKey());
			prompts[1].TriggerAnim("keyESC");
		}
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		activator.TriggerAnim("out");
		yield return null;
		Interface.env.Enable();
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
