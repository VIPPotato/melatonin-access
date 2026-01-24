public class Pool : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("spiraling");
	}

	public void Enable()
	{
		sprites[0].SetCurrentAnimSpeed(1.5f);
	}
}
