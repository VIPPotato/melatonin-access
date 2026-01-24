public class TimeWrapper : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
	}

	public void Activate(string type)
	{
		gears[0].TriggerAnim(type, Matrix.env.GetSpeed());
	}

	public void Hide()
	{
		gears[0].TriggerAnim("awaiting");
	}

	public void Fly()
	{
		gears[0].TriggerAnim("hit");
	}
}
