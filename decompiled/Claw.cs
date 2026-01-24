public class Claw : Wrapper
{
	public Fragment slider;

	public Fragment grabber;

	public Fragment head;

	public Fragment[] arms;

	private float initLocalX;

	private float distance = 4.5f;

	private float multiplier;

	protected override void Awake()
	{
		slider.Awake();
		grabber.Awake();
		head.Awake();
		Fragment[] array = arms;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		initLocalX = GetLocalX();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		slider.TriggerAnim("awaiting");
		grabber.TriggerAnim("awaiting");
		head.TriggerAnim("default");
	}

	public void Hide()
	{
		SetLocalX(initLocalX);
		RenderChildren(toggle: false);
	}

	public void Grab()
	{
		grabber.TriggerAnim("grab");
		arms[0].TriggerAnim("grab");
		arms[1].TriggerAnim("grab");
	}

	public void Release()
	{
		arms[0].TriggerAnim("release");
		arms[1].TriggerAnim("release");
	}

	public void SlideRight()
	{
		multiplier = 2f;
		SetLocalX(initLocalX);
		slider.TriggerAnim("forward", Espot.env.GetSpeed() * 2f);
	}

	public void SlideLeft()
	{
		multiplier = 2f;
		SetLocalX(initLocalX);
		slider.TriggerAnim("backward", Espot.env.GetSpeed() * 2f);
	}

	public void NudgeRight(int position)
	{
		multiplier = 1f;
		SetLocalX(initLocalX + (float)position * distance);
		slider.TriggerAnim("nudgeRight", Espot.env.GetSpeed());
	}

	public void NudgeLeft(int position)
	{
		multiplier = 1f;
		SetLocalX(initLocalX + (float)position * distance);
		slider.TriggerAnim("nudgeLeft", Espot.env.GetSpeed());
	}

	public void React(float accuracy)
	{
		if (accuracy != 1f)
		{
			if (accuracy == 0.332f)
			{
				head.TriggerAnim("yellow");
			}
			else
			{
				head.TriggerAnim("red");
			}
		}
		else
		{
			head.TriggerAnim("blue");
		}
	}

	public float GetGrabberY()
	{
		return grabber.GetLocalY();
	}

	public float GetMultiplier()
	{
		return multiplier;
	}
}
