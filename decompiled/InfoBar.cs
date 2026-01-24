using UnityEngine;

public class InfoBar : Wrapper
{
	[Header("Fragments")]
	public Fragment activator;

	public Fragment promptAction;

	public spriteFragment promptRemove;

	public spriteFragment promptSwitch;

	public spriteFragment promptMore;

	public spriteFragment promptPlay;

	public textboxFragment[] labels;

	public textboxFragment[] sublabels;

	private float timer;

	protected override void Awake()
	{
		activator.Awake();
		promptAction.Awake();
		promptRemove.Initiate();
		promptSwitch.Initiate();
		promptMore.Initiate();
		promptPlay.Initiate();
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		array = sublabels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
	}

	public void Activate()
	{
		ConfigPrompts();
		activator.TriggerAnim("activate5");
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 2f)
		{
			ConfigPrompts();
			timer = 0f;
		}
	}

	private void ConfigPrompts()
	{
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			promptAction.TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			promptAction.TriggerAnim("gamepadCROSS");
		}
		else
		{
			promptAction.TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		}
		promptRemove.SetState(ControlHandler.mgr.GetCtrlType());
		promptSwitch.SetState(ControlHandler.mgr.GetCtrlType());
		promptMore.SetState(ControlHandler.mgr.GetCtrlType());
		promptPlay.SetState(ControlHandler.mgr.GetCtrlType());
	}
}
