public class LilBlocks : Wrapper
{
	private int waveNum = 1;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Wave()
	{
		gears[0].TriggerAnim("wave" + waveNum, Matrix.env.GetSpeed() / 2f);
		gears[1].TriggerAnim("wave" + waveNum, Matrix.env.GetSpeed() / 2f);
		waveNum++;
		if (waveNum > 2)
		{
			waveNum = 1;
		}
	}
}
