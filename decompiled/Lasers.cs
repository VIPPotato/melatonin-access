using UnityEngine;

public class Lasers : Wrapper
{
	[Header("Fragments")]
	public Fragment[] lasers;

	protected override void Awake()
	{
		SetupFragments();
		Fragment[] array = lasers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
	}

	public void Show()
	{
	}

	public void Hide()
	{
		Fragment[] array = lasers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("hidden");
		}
	}

	public void ShootLeft()
	{
		lasers[0].TriggerAnim("shoot");
	}

	public void ShootCenter()
	{
		lasers[1].TriggerAnim("shoot");
	}

	public void ShootRight()
	{
		lasers[2].TriggerAnim("shoot");
	}

	public void SetLaserColor(float accuracy)
	{
		if (accuracy == 1f)
		{
			Fragment[] array = lasers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSpriteColor(new Color(0.38431373f, 1f, 0.9490196f));
			}
		}
		else if (accuracy == 0.332f)
		{
			Fragment[] array = lasers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSpriteColor(new Color(1f, 0.99607843f, 0.88235295f));
			}
		}
		else
		{
			Fragment[] array = lasers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSpriteColor(new Color(1f, 0.6784314f, 0.94509804f));
			}
		}
	}
}
