public class Floor : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
	}

	public void Show()
	{
		gears[0].TriggerAnim("shown");
	}
}
