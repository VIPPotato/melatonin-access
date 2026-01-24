using System.Collections;
using UnityEngine;

public class Capsule : Wrapper
{
	private float initLocalX;

	private float initLocalY;

	private float initZ;

	private float timeTilStick;

	private float timeTilDrop;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		initLocalX = GetLocalX();
		initLocalY = GetLocalY();
		initZ = GetZ();
		timeTilStick = gears[0].GetAnimDuration("stick");
		timeTilDrop = gears[0].GetAnimDuration("drop");
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetLocalZ(2f);
		SetLocalPosition(0f, -2.42f);
		SetLocalEulerAngles(0f, 0f, 0f);
		gears[0].TriggerAnim("grabbed");
		sprites[0].TriggerAnim(Random.Range(0, 7).ToString() ?? "");
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Stick()
	{
		deactivating = StartCoroutine(Sticking());
	}

	private IEnumerator Sticking()
	{
		SetLocalZ(19f);
		SetLocalEulerAngles(0f, 0f, 0f);
		gears[0].TriggerAnim("stick");
		yield return new WaitForSeconds(timeTilStick);
		RenderChildren(toggle: false);
	}

	public void Drop()
	{
		deactivating = StartCoroutine(Dropping());
	}

	private IEnumerator Dropping()
	{
		SetLocalX(-5.65f);
		SetZ(initZ);
		SetLocalEulerAngles(0f, 0f, 0f);
		gears[0].TriggerAnim("drop");
		yield return new WaitForSeconds(timeTilDrop);
		RenderChildren(toggle: false);
	}
}
