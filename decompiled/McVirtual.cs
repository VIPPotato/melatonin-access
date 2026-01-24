using System.Collections;
using UnityEngine;

public class McVirtual : Wrapper
{
	[Header("Children")]
	public Sweat Sweat;

	private Coroutine aiming;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Bobble()
	{
		sprites[1].TriggerAnim("beat");
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
		gears[1].TriggerAnim("aim", MechSpace.env.GetSpeed());
		gears[2].TriggerAnim("aim", MechSpace.env.GetSpeed());
	}

	public void Idle()
	{
		CancelCoroutine(aiming);
		gears[1].TriggerAnim("awaiting");
		gears[2].TriggerAnim("awaiting");
	}

	public void Shoot()
	{
		if (Dream.dir.CheckIsFullBeat())
		{
			sprites[0].TriggerAnim("shoot");
		}
		else if (!Dream.dir.CheckIsFullBeat())
		{
			sprites[2].TriggerAnim("shoot");
		}
		else
		{
			sprites[0].TriggerAnim("shoot");
			sprites[2].TriggerAnim("shoot");
		}
		gears[0].TriggerAnim("recoil");
	}
}
