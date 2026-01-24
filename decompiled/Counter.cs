public class Counter : Wrapper
{
	public McSpender McSpender;

	public CardMachine CardMachine;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("shown");
		sprites[1].TriggerAnim("idled");
		McSpender.Show();
		CardMachine.Show();
	}

	public void Hide()
	{
		McSpender.Hide();
		CardMachine.Hide();
		RenderChildren(toggle: false);
	}

	public void ReactGood()
	{
		sprites[1].TriggerAnim("hit");
		CardMachine.ReactGood();
	}

	public void ReactBad()
	{
		sprites[1].TriggerAnim("miss");
		CardMachine.ReactBad();
	}
}
