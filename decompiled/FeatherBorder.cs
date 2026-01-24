using System.Collections;
using UnityEngine;

public class FeatherBorder : Wrapper
{
	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetOuterTransform());
		sprites[0].TriggerAnim("fadeIn");
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		sprites[0].TriggerAnim("fadeOut");
		yield return new WaitForSeconds(1f);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetOuterTransform());
		sprites[0].TriggerAnim("fadedIn");
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}
}
