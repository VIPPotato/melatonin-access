using System.Collections;
using UnityEngine;

public class Sweat : Wrapper
{
	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false, 1);
	}

	public void CrossIn()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(CrossingIn());
	}

	private IEnumerator CrossingIn()
	{
		RenderChildren(toggle: true, 1);
		gears[0].TriggerAnim("drop");
		speakers[0].TriggerSound(0);
		yield return new WaitForSeconds(0.5f);
		RenderChildren(toggle: false, 1);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false, 1);
	}
}
