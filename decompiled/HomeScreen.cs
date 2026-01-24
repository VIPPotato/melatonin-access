using System.Collections;
using UnityEngine;

public class HomeScreen : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("hidden");
	}

	public void Deactivate()
	{
		StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		sprites[0].TriggerAnim("out");
		sprites[1].TriggerAnim("fadeIn");
		speakers[0].TriggerSound(0);
		yield return new WaitForSeconds(1f);
		RenderChildren(toggle: false);
	}
}
