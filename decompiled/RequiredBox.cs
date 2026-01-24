using System.Collections;
using UnityEngine;

public class RequiredBox : Wrapper
{
	[Header("Fragments")]
	public Fragment fader;

	public textboxFragment message;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		fader.Awake();
		message.Initiate();
		timeTilOut = fader.GetAnimDuration("fadeOut");
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetParent(Interface.env.Cam.GetOuterTransform());
		SetLocalPosition(0f, 6.45f);
		SetLocalZ(20f);
		fader.TriggerAnim("fadeIn");
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		fader.TriggerAnim("fadeOut");
		yield return new WaitForSeconds(timeTilOut);
		SetParentAndReposition(Map.env.transform);
		RenderChildren(toggle: false);
	}
}
