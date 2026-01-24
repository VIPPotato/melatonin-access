using System.Collections;
using UnityEngine;

public class FallingMoney : Wrapper
{
	private float timeTilOut;

	private Coroutine crossing;

	protected override void Awake()
	{
		SetupFragments();
		timeTilOut = gears[0].GetAnimDuration("dropLeft");
		RenderChildren(toggle: false);
	}

	public void CrossIn(string type)
	{
		CancelCoroutine(crossing);
		crossing = StartCoroutine(Crossing(type));
	}

	private IEnumerator Crossing(string type)
	{
		RenderChildren(toggle: true);
		gears[0].TriggerAnim(type, TropicalBank.env.GetSpeed());
		yield return new WaitForSeconds(timeTilOut / TropicalBank.env.GetSpeed());
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(crossing);
		RenderChildren(toggle: false);
	}
}
