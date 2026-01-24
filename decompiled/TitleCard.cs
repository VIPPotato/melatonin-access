using System.Collections;
using UnityEngine;

public class TitleCard : Wrapper
{
	[Header("Fragments")]
	public textboxFragment title;

	public Fragment fill;

	[Header("Props")]
	private Coroutine deactivating;

	protected override void Awake()
	{
		title.Initiate();
		fill.Awake();
		RenderChildren(toggle: false);
	}

	public void Show(int stateNum)
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		title.SetState(stateNum);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		title.FadeOutText(1f, 1.5f);
		fill.FadeOutSprite(1f, 1.5f);
		yield return new WaitForSeconds(1.5f);
		RenderChildren(toggle: false);
	}
}
