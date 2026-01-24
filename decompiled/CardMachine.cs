public class CardMachine : Wrapper
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
		sprites[1].TriggerAnim("idled");
		sprites[2].TriggerAnim("shown");
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Bobble()
	{
		sprites[2].TriggerAnim("bobble");
	}

	public void ReactGood()
	{
		sprites[0].TriggerAnim("hit");
		sprites[1].TriggerAnim("receipt", Mall.env.GetSpeed());
	}

	public void ReactBad()
	{
		sprites[0].TriggerAnim("miss");
	}

	public void IncreaseMeter(float timeStarted, int beats)
	{
		sprites[3].TriggerAnimDelayedTimeStarted(timeStarted, "increase", Mall.env.GetSpeed() / (float)beats);
	}

	public void ResetMeter()
	{
		sprites[3].TriggerAnim("zero");
	}
}
