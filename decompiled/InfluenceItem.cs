using UnityEngine;

public class InfluenceItem : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim(Random.Range(1, 3).ToString() ?? "");
	}

	public void ShiftPosition()
	{
		SetLocalX(GetLocalX() + Random.Range(3f, 11f));
		SetLocalY(GetLocalY() + (float)Random.Range(0, -8));
	}
}
