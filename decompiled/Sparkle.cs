using System.Collections;
using UnityEngine;

public class Sparkle : Wrapper
{
	private float timeTilOut;

	private Coroutine crossingIn;

	protected override void Awake()
	{
		SetupFragments();
		timeTilOut = sprites[0].GetAnimDuration("shoot1");
		RenderChildren(toggle: false);
	}

	public void CrossIn(int shootNum)
	{
		CancelCoroutine(crossingIn);
		crossingIn = StartCoroutine(CrossingIn(shootNum));
	}

	private IEnumerator CrossingIn(int shootNum)
	{
		RenderChildren(toggle: true);
		if (shootNum == 0)
		{
			sprites[0].TriggerAnim("shoot" + Random.Range(1, 4));
		}
		else
		{
			sprites[0].TriggerAnim("shoot" + shootNum);
		}
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void CrossInDelayed(float delta, int shootNum)
	{
		CancelCoroutine(crossingIn);
		crossingIn = StartCoroutine(CrossingInDelayed(delta, shootNum));
	}

	private IEnumerator CrossingInDelayed(float delta, int shootNum)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		RenderChildren(toggle: true);
		if (shootNum == 0)
		{
			sprites[0].TriggerAnim("shoot" + Random.Range(1, 4));
		}
		else
		{
			sprites[0].TriggerAnim("shoot" + shootNum);
		}
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}
}
