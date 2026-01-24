using UnityEngine;

public class Spinner : Wrapper
{
	[Header("Fragments")]
	public Fragment gear;

	protected override void Awake()
	{
		gear.Awake();
		RenderChildren(toggle: false);
	}

	public void Activate(bool isOffsetted = false)
	{
		RenderChildren(toggle: true);
		if (isOffsetted)
		{
			SetLocalX(9.627f);
		}
		else
		{
			SetLocalX(10.627f);
		}
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		gear.TriggerAnim("activate");
	}

	public void Deactivate()
	{
		gear.TriggerAnim("deactivate");
	}
}
