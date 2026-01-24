using System.Collections;
using UnityEngine;

public class Letterbox : Wrapper
{
	[Header("Children")]
	public Fragment gear;

	private bool isActivated;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		gear.Awake();
		timeTilOut = gear.GetAnimDuration("slideOut");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		isActivated = true;
		gear.TriggerAnim("slideIn");
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		isActivated = true;
		gear.TriggerAnim("slidIn");
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		gear.TriggerAnim("slideOut");
		yield return new WaitForSeconds(timeTilOut);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}

	public void DeactivateDelayed()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(DeactivatingDelayed());
	}

	private IEnumerator DeactivatingDelayed()
	{
		isActivated = false;
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		gear.TriggerAnim("slideOut");
		yield return new WaitForSeconds(timeTilOut);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
