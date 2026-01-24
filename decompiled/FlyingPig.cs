using UnityEngine;

public class FlyingPig : Wrapper
{
	[Header("Children")]
	public Wings Wings;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		Wings.Show();
	}

	public void TravelRight()
	{
		SetLocalDistance(1.2f, 0f);
		if (GetLocalX() > 14.5f)
		{
			SetLocalX(-14.5f);
		}
		gears[0].TriggerAnim("travelRight", TropicalBank.env.GetSpeed());
		Wings.Flap();
	}

	public void TravelLeft()
	{
		SetLocalDistance(-1.5f, 0f);
		if (GetLocalX() < -14.5f)
		{
			SetLocalX(14.5f);
		}
		gears[0].TriggerAnim("travelLeft", TropicalBank.env.GetSpeed());
		Wings.Flap(0.5f);
	}

	public void SetRight()
	{
		gears[0].TriggerAnim("travelRight", 1f, 1f);
	}

	public void SetLeft()
	{
		gears[0].TriggerAnim("travelLeft", 1f, 1f);
	}
}
