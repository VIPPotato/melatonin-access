using UnityEngine;

public class Meadow : Wrapper
{
	[Header("Fragments")]
	public layer layer;

	protected override void Awake()
	{
		layer.Awake();
		SetupFragments();
	}

	public void Show()
	{
	}

	public void Hide()
	{
	}

	public void Exit()
	{
		layer.Disable();
	}
}
