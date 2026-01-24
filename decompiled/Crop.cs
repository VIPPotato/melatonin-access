using UnityEngine;

public class Crop : Wrapper
{
	[Header("Children")]
	public PlantBubble PlantBubble;

	[Header("Props")]
	public Fragment sprout;

	public Fragment plot;

	private int type;

	private int sprayNum;

	protected override void Awake()
	{
		SetupFragments();
		sprout.Awake();
		plot.Awake();
	}

	public void Show()
	{
		type = Random.Range(0, 2);
		sprayNum = 0;
		sprout.TriggerAnim(type + "quenched");
		plot.TriggerAnim("dry");
		PlantBubble.Hide();
	}

	public void Hide()
	{
		PlantBubble.Hide();
	}

	public void Reset()
	{
		type = Random.Range(0, 2);
		sprayNum = 0;
		SetDistance(35f, 0f);
		sprout.TriggerAnim(type + "quenched");
		plot.TriggerAnim("dry");
		PlantBubble.Hide();
	}

	public void Thirst()
	{
		sprout.TriggerAnim(type + "thirst");
	}

	public void Die(int num)
	{
		sprout.TriggerAnim(type + "die" + num);
	}

	public void Spray(float accuracy)
	{
		sprayNum++;
		if (sprayNum <= 3)
		{
			sprout.TriggerAnim(type + "quench" + sprayNum);
			plot.TriggerAnim("spray" + sprayNum);
			if (PlantBubble.CheckIsInteractive())
			{
				PlantBubble.WaterLevel.RiseSegmented(accuracy);
			}
		}
	}

	public void Soak()
	{
		sprout.TriggerAnim(type + "quench", Conservatory.env.GetSpeed());
		plot.TriggerAnim("soak", Conservatory.env.GetSpeed());
		if (PlantBubble.CheckIsInteractive())
		{
			PlantBubble.WaterLevel.RevealRise();
		}
	}

	public void SoakCancel()
	{
		sprout.SetCurrentAnimSpeed(0f);
		plot.SetCurrentAnimSpeed(0f);
	}
}
