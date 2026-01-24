using UnityEngine;

public class ScrollingBar : Wrapper
{
	[Header("Fragments")]
	public spriteFragment[] bars;

	private bool isActivated;

	protected override void Awake()
	{
		spriteFragment[] array = bars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
	}

	public void Activate()
	{
		isActivated = true;
		spriteFragment[] array = bars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FadeInSprite(1f, 0.33f);
		}
	}

	public void Deactivate()
	{
		isActivated = false;
		spriteFragment[] array = bars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FadeOutSprite(1f, 0.33f);
		}
	}

	public void Hide()
	{
		isActivated = false;
		spriteFragment[] array = bars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ToggleSpriteRenderer(toggle: false);
		}
	}

	public void Show()
	{
		isActivated = true;
		spriteFragment[] array = bars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ToggleSpriteRenderer(toggle: true);
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
