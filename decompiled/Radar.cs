using UnityEngine;

public class Radar : Wrapper
{
	[Header("Fragments")]
	public Fragment[] pulses;

	public Fragment fill;

	private int pulseNum;

	protected override void Awake()
	{
		Fragment[] array = pulses;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		fill.Awake();
	}

	public void Show()
	{
		fill.TriggerAnim("white");
	}

	public void Hide()
	{
	}

	public void Pulse()
	{
		pulses[pulseNum].TriggerAnim("in", NeoCity.env.GetSpeed() / 4f);
		pulseNum++;
		if (pulseNum >= pulses.Length)
		{
			pulseNum = 0;
		}
	}

	public void FlashAccuracy(float accuracy)
	{
		if (accuracy == 1f)
		{
			fill.TriggerAnim("perfect");
		}
		else if (accuracy == 0.332f)
		{
			fill.TriggerAnim("early");
		}
		else
		{
			fill.TriggerAnim("late");
		}
	}
}
