using UnityEngine;

public class Waters : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		for (int i = 0; i < sprites.Length; i++)
		{
			sprites[i].TriggerAnimWait(Random.Range(0f, 2.5f), "waving");
		}
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}
}
