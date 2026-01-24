public class Arcade : Wrapper
{
	public Fragment lightGear;

	public Fragment screen1;

	public Fragment screen2;

	private int screen1num;

	private int screen2num;

	protected override void Awake()
	{
		lightGear.Awake();
		screen1.Awake();
		screen2.Awake();
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

	public void RefreshScreensDelayed(float delta)
	{
		screen1num++;
		if (screen1num > 13)
		{
			screen1num = 0;
		}
		screen1.TriggerAnimDelayedDelta(delta, screen1num.ToString() ?? "");
		screen2num++;
		if (screen2num > 1)
		{
			screen2num = 0;
		}
		screen2.TriggerAnimDelayedDelta(delta, screen2num.ToString() ?? "");
	}

	public void Darken()
	{
		lightGear.TriggerAnim("darken", Espot.env.GetSpeed() / 4f);
	}

	public void Light()
	{
		lightGear.TriggerAnim("light", Espot.env.GetSpeed() / 4f);
	}
}
