using System.Collections;
using UnityEngine;

public class Notification : Wrapper
{
	private bool isActivated;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		timeTilOut = gears[0].GetAnimDuration("out");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		if (!isActivated)
		{
			CancelCoroutine(deactivating);
			isActivated = true;
			RenderChildren(toggle: true);
			gears[0].TriggerAnim("in");
			sprites[0].TriggerAnim(Random.Range(1, 6).ToString() ?? "");
			SetPosition(Interface.env.Cam.GetX() + Random.Range(-9.5f, -1.25f), Interface.env.Cam.GetY() + Random.Range(3f, 7f));
		}
	}

	public void Deactivate()
	{
		if (isActivated)
		{
			CancelCoroutine(deactivating);
			deactivating = StartCoroutine(Deactivating());
		}
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		gears[0].TriggerAnim("out");
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		if (!isActivated)
		{
			CancelCoroutine(deactivating);
			isActivated = true;
			RenderChildren(toggle: true);
			gears[0].TriggerAnim("shown");
			sprites[0].TriggerAnim(Random.Range(1, 6).ToString() ?? "");
		}
	}

	public void Hide()
	{
		if (isActivated)
		{
			CancelCoroutine(deactivating);
			isActivated = false;
			RenderChildren(toggle: false);
		}
	}

	public void Bobble()
	{
		gears[0].TriggerAnim("bobble");
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
