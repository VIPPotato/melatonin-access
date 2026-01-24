using System.Collections;
using UnityEngine;

public class McSpender : Wrapper
{
	private Coroutine bobbling;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false, 1);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("idling");
	}

	public void Hide()
	{
		CancelCoroutine(bobbling);
		RenderChildren(toggle: false);
	}

	public void BobbleDelayed(float delta)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta));
	}

	private IEnumerator BobblingDelayed(float delta)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 1f / 30f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		sprites[0].TriggerAnim("tap");
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		sprites[0].TriggerAnim("untap");
	}

	public void Purchase()
	{
		CancelCoroutine(bobbling);
		sprites[0].TriggerAnim("purchase");
	}
}
