using System.Collections;
using UnityEngine;

public class Cockpit : Wrapper
{
	[Header("Children")]
	public Radar Radar;

	[Header("Fragments")]
	public Fragment grid;

	public Fragment panelLeft;

	public Fragment panelRight;

	public Fragment buttonCenter;

	public Fragment buttonSide_l;

	public Fragment buttonSide_r;

	private Coroutine bobbling;

	protected override void Awake()
	{
		grid.Awake();
		panelLeft.Awake();
		panelRight.Awake();
		buttonCenter.Awake();
		buttonSide_l.Awake();
		buttonSide_r.Awake();
	}

	public void Show()
	{
		panelLeft.TriggerAnim("4", 1f, 1f);
		panelRight.TriggerAnim("4", 1f, 1f);
		buttonCenter.TriggerAnim("idled");
		buttonSide_l.TriggerAnim("idled");
		buttonSide_r.TriggerAnim("idled");
		Radar.Show();
	}

	public void Hide()
	{
		Radar.Hide();
	}

	public void BobbleDelayed(float delta, int beat)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta, beat));
	}

	private IEnumerator BobblingDelayed(float delta, int beat)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		grid.TriggerAnim("pulse");
		panelLeft.TriggerAnim(beat.ToString() ?? "");
		panelRight.TriggerAnim(beat.ToString() ?? "");
		Radar.Pulse();
	}

	public void PressCenter()
	{
		buttonCenter.TriggerAnim("press");
	}

	public void PressLeft()
	{
		buttonSide_l.TriggerAnim("press");
	}

	public void PressRight()
	{
		buttonSide_r.TriggerAnim("press");
	}
}
