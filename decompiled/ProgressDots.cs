using System.Collections.Generic;
using UnityEngine;

public class ProgressDots : Wrapper
{
	[Header("Fragments")]
	public spriteFragment[] dots;

	private List<spriteFragment> dots_available = new List<spriteFragment>();

	[Header("Props")]
	public int count;

	private bool isDistributed;

	private int activeDot;

	protected override void Awake()
	{
		spriteFragment[] array = dots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		RenderChildren(toggle: false);
	}

	public void Show(int newCount)
	{
		count = newCount;
		RenderChildren(toggle: true);
		if (!isDistributed)
		{
			isDistributed = true;
			for (int i = 0; i < dots.Length; i++)
			{
				if (i < count)
				{
					dots_available.Add(dots[i]);
					dots[i].ToggleSpriteRenderer(toggle: true);
				}
				else
				{
					dots[i].ToggleSpriteRenderer(toggle: false);
				}
			}
			for (int j = 0; j < dots_available.Count; j++)
			{
				dots_available[j].SetLocalX((float)j * 0.25f - (float)(dots_available.Count - 1) * 0.25f / 2f);
			}
		}
		dots_available[activeDot].SetSpriteAlpha(1f);
	}

	public void SetActiveDot(int newActiveDot)
	{
		activeDot = newActiveDot;
		for (int i = 0; i < dots_available.Count; i++)
		{
			dots_available[i].SetSpriteAlpha(0.25f);
		}
		dots_available[activeDot].SetSpriteAlpha(1f);
	}
}
