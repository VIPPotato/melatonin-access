using UnityEngine;

public class WaterCan : Wrapper
{
	[Header("Props")]
	public Fragment hoverer;

	public Fragment can;

	protected override void Awake()
	{
		SetupFragments();
		hoverer.Awake();
		can.Awake();
	}

	public void Show()
	{
		can.TriggerAnim("idling");
	}

	public void Hide()
	{
	}

	public void Hover()
	{
		hoverer.TriggerAnim("hover", Conservatory.env.GetSpeed() / 4f);
	}

	public void Spray()
	{
		can.TriggerAnim("spray");
	}

	public void Pour()
	{
		can.TriggerAnim("pour");
	}

	public void Idle()
	{
		can.TriggerAnim("idle");
	}
}
