using System.Collections;
using UnityEngine;

public class LadderColumn : Wrapper
{
	private float timeTilDeactivated;

	private Coroutine deactivating;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		SetupFragments();
		timeTilDeactivated = gears[0].GetAnimDuration("slideOutToLeft");
		RenderChildren(toggle: false);
	}

	public void Activate(int animType)
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		switch (animType)
		{
		case 1:
			gears[0].TriggerAnim("slideInFromLeft", GetSpeed());
			break;
		case 2:
			gears[0].TriggerAnim("slideInFromRight", GetSpeed());
			break;
		}
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		gears[0].TriggerAnim("slidIn");
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Deactivate(string direction)
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating(direction));
	}

	private IEnumerator Deactivating(string direction)
	{
		CancelCoroutine(deactivating);
		gears[0].TriggerAnim("slideOutTo" + direction, GetSpeed());
		yield return new WaitForSeconds(timeTilDeactivated / GetSpeed());
		RenderChildren(toggle: false);
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}
}
