public class McMarchers : Wrapper
{
	private bool isMarching = true;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isMarching = true;
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void MarchDelayed(float delta)
	{
		if (isMarching)
		{
			gears[0].TriggerAnimDelayedDelta(delta, "march", AngrySkies.env.GetSpeed());
		}
	}

	public void ToggleIsMarching(bool toggle)
	{
		isMarching = toggle;
	}
}
