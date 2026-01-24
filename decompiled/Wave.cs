using System.Collections;
using UnityEngine;

public class Wave : Wrapper
{
	[Header("Props")]
	public bool isRealTime;

	private bool isActivated;

	private float timeTilDeactivate;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		if (isRealTime)
		{
			gears[0].SetAnimatorUnscaledTime();
			gears[1].SetAnimatorUnscaledTime();
		}
		timeTilDeactivate = gears[0].GetAnimDuration("out");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		if (!isActivated)
		{
			isActivated = true;
			CancelCoroutine(deactivating);
			RenderChildren(toggle: true);
			gears[0].TriggerAnim("in");
			gears[1].TriggerAnim("scrolling");
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
		if (isRealTime)
		{
			yield return new WaitForSecondsRealtime(timeTilDeactivate);
		}
		else
		{
			yield return new WaitForSeconds(timeTilDeactivate);
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		gears[0].TriggerAnim("shown");
		gears[1].TriggerAnim("scrolling");
	}

	public void Hide()
	{
		isActivated = false;
		CancelCoroutine(deactivating);
		gears[0].TriggerAnim("awaiting");
		gears[1].TriggerAnim("awaiting");
		RenderChildren(toggle: false);
	}

	public void SetColor(Color color)
	{
		sprites[0].SetSpriteColor(color);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
