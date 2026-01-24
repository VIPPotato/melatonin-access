using UnityEngine;

public class RebindModal : Wrapper
{
	[Header("Fragments")]
	public textboxFragment message;

	public textboxFragment[] labels;

	protected override void Awake()
	{
		message.Initiate();
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		message.SetState(0);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}
}
