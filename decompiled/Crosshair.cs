using System.Collections;
using UnityEngine;

public class Crosshair : Wrapper
{
	private Coroutine aiming;

	protected override void Awake()
	{
		SetupFragments();
	}

	public void Activate()
	{
		gears[0].TriggerAnim("awaiting");
		gears[1].TriggerAnim("activate");
		gears[2].TriggerAnim("awaiting");
	}

	public void Hide()
	{
		gears[1].TriggerAnim("hidden");
	}

	public void Spin()
	{
		if (sprites[0].CheckIsAnimPlaying("idling"))
		{
			sprites[0].TriggerAnim("spin");
		}
	}

	public void AimDelayed(float delta)
	{
		CancelCoroutine(aiming);
		aiming = StartCoroutine(AimingDelayed(delta));
	}

	private IEnumerator AimingDelayed(float delta)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		gears[0].TriggerAnim("aim", MechSpace.env.GetSpeed());
	}

	public void Idle()
	{
		CancelCoroutine(aiming);
		gears[0].TriggerAnim("awaiting");
	}

	public void Shoot()
	{
		gears[2].TriggerAnim("recoil");
		sprites[0].TriggerAnim("shoot");
		sprites[1].TriggerAnim("shoot");
	}

	public void Alert()
	{
		CancelCoroutine(aiming);
		gears[0].TriggerAnim("awaiting");
		gears[1].TriggerAnim("deactivate");
	}
}
