using UnityEngine;

public class Garden : Wrapper
{
	[Header("Children")]
	public Crop[] Crops;

	[Header("Props")]
	private int leftObjectNum;

	private int activeObjectNum;

	protected override void Awake()
	{
		SetupFragments();
	}

	public void Show()
	{
		leftObjectNum = 0;
		activeObjectNum = 2;
		for (int i = 0; i < Crops.Length; i++)
		{
			Crops[i].Show();
			Crops[i].SetLocalX(-14.51f + (float)(5 * i));
		}
	}

	public void Hide()
	{
		Crop[] crops = Crops;
		for (int i = 0; i < crops.Length; i++)
		{
			crops[i].Hide();
		}
	}

	public void ReorganizeSprouts()
	{
		Crops[leftObjectNum].Reset();
		leftObjectNum++;
		activeObjectNum++;
		if (leftObjectNum >= Crops.Length)
		{
			leftObjectNum = 0;
		}
		else if (activeObjectNum >= Crops.Length)
		{
			activeObjectNum = 0;
		}
	}

	public Crop GetActiveCrop()
	{
		return Crops[activeObjectNum];
	}
}
