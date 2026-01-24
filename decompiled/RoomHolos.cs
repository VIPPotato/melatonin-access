using System.Collections;
using UnityEngine;

public class RoomHolos : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		gears[0].TriggerAnim("drifting", 1.05f);
		gears[1].TriggerAnim("drifting", 0.95f);
		sprites[0].TriggerAnim("idling");
		sprites[1].TriggerAnim("idling");
		sprites[2].TriggerAnim("up");
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Scroll(float delta)
	{
		StartCoroutine(Scrolling(delta));
	}

	private IEnumerator Scrolling(float delta)
	{
		sprites[0].SetCurrentAnimSpeed(5f);
		sprites[1].SetCurrentAnimSpeed(5f);
		sprites[2].PingAnimTrigger("switch");
		float checkpoint = Technician.mgr.GetDspTime() + MusicBox.env.GetSecsPerBeat() / 2f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		sprites[0].SetCurrentAnimSpeed(1f);
		sprites[1].SetCurrentAnimSpeed(1f);
	}
}
