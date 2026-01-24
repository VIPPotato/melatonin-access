using UnityEngine;

public class SpaceMeter : Wrapper
{
	private int amount;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		amount = 0;
		gears[0].TriggerAnim("0");
		gears[1].TriggerAnim("0");
		sprites[0].ToggleSpriteRenderer(toggle: true);
		sprites[1].ToggleSpriteRenderer(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Increase(float speed, int total)
	{
		amount++;
		if (total == 2)
		{
			if (amount == 1)
			{
				gears[0].TriggerAnim(amount + "of" + total, AngrySkies.env.GetSpeed() * speed);
			}
			else if (amount == 2)
			{
				if (speed == 1f)
				{
					gears[1].TriggerAnim(amount + "of" + total + "Singled", AngrySkies.env.GetSpeed() * speed);
				}
				else
				{
					gears[1].TriggerAnim(amount + "of" + total, AngrySkies.env.GetSpeed() * speed);
				}
			}
		}
		ResetColor();
	}

	public void Decrease(float speed, int total)
	{
		switch (total)
		{
		case 3:
			if (amount <= 2)
			{
				gears[0].TriggerAnim(amount + "of" + total + "Reversed", AngrySkies.env.GetSpeed() * speed);
			}
			if (amount >= 2)
			{
				gears[1].TriggerAnim(amount + "of" + total + "Reversed", AngrySkies.env.GetSpeed() * speed);
			}
			break;
		case 2:
			if (amount == 1)
			{
				gears[0].TriggerAnim(amount + "of" + total + "Reversed", AngrySkies.env.GetSpeed() * speed);
			}
			else if (amount == 2)
			{
				gears[1].TriggerAnim(amount + "of" + total + "Reversed", AngrySkies.env.GetSpeed() * speed);
			}
			break;
		}
		amount--;
	}

	public void Reset()
	{
		amount = 0;
		gears[0].TriggerAnim("0");
		gears[1].TriggerAnim("0");
		sprites[0].ToggleSpriteRenderer(toggle: true);
		sprites[1].ToggleSpriteRenderer(toggle: true);
	}

	public void ToggleIsVisible(bool toggle)
	{
		sprites[0].ToggleSpriteRenderer(toggle);
		sprites[1].ToggleSpriteRenderer(toggle);
	}

	public void SetAmount(int value)
	{
		amount = value;
	}

	private void ResetColor()
	{
		sprites[0].SetSpriteColor(new Color(1f, 1f, 1f));
		sprites[1].SetSpriteColor(new Color(1f, 1f, 1f));
	}

	public void SetColor(float accuracy)
	{
		if (accuracy == 1f)
		{
			sprites[0].SetSpriteColor(new Color(0.6745f, 1f, 0.9882f));
			sprites[1].SetSpriteColor(new Color(0.6745f, 1f, 0.9882f));
		}
		else if (accuracy == 0.332f)
		{
			sprites[0].SetSpriteColor(new Color(1f, 0.9529f, 0.8627f));
			sprites[1].SetSpriteColor(new Color(1f, 0.9529f, 0.8627f));
		}
		else
		{
			sprites[0].SetSpriteColor(new Color(1f, 0.6941f, 0.9255f));
			sprites[1].SetSpriteColor(new Color(1f, 0.6941f, 0.9255f));
		}
	}
}
