public class ClotheslinePulley : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
	}

	public void Swap()
	{
		gears[0].TriggerAnim("swap", Darkroom.env.GetSpeed());
	}
}
