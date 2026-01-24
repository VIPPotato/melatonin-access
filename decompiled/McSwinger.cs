public class McSwinger : Wrapper
{
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("idled");
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Bobble(float duration)
	{
		if (sprites[0].CheckIsAnimPlaying("idled") || sprites[0].CheckIsAnimPlaying("beat"))
		{
			sprites[0].TriggerAnim("beat", Matrix.env.GetSpeed() / duration);
		}
	}

	public void Idle()
	{
		sprites[0].TriggerAnim("idled");
	}

	public void Prep()
	{
		sprites[0].TriggerAnim("prep");
	}

	public void WindUp()
	{
		sprites[0].TriggerAnim("windUp", Matrix.env.GetSpeed());
	}

	public void Swing()
	{
		sprites[0].TriggerAnim("swing");
		sprites[1].TriggerAnim("flare");
		sprites[2].TriggerAnim("fly");
	}
}
