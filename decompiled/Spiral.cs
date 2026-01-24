using UnityEngine;

public class Spiral : Wrapper
{
	[Header("Fragments")]
	public Fragment spiral;

	private bool isRemix;

	private bool isDoubled;

	private float speed;

	protected override void Awake()
	{
		spiral.Awake();
	}

	public void Show(bool newIsRemix)
	{
		isRemix = newIsRemix;
		speed = HypnoLair.env.GetSpeed();
		if (isRemix)
		{
			spiral.TriggerAnim("spin", speed);
		}
		else
		{
			spiral.TriggerAnim("spin", speed / 4f);
		}
	}

	public void Hide()
	{
	}

	public void Spin()
	{
		if (isDoubled || isRemix)
		{
			spiral.TriggerAnim("spin", speed * 2f);
		}
		else
		{
			spiral.TriggerAnim("spin", speed);
		}
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public void ToggleIsDoubled(bool toggle)
	{
		isDoubled = toggle;
	}
}
