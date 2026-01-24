using UnityEngine;

public class Crumbs : Wrapper
{
	[Header("Fragments")]
	public Fragment exploder;

	protected override void Awake()
	{
		exploder.Awake();
	}

	public void CrossIn()
	{
		exploder.TriggerAnim("explode");
	}
}
