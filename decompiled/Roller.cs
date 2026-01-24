using UnityEngine;

public class Roller : Wrapper
{
	[Header("Fragments")]
	public Fragment rotator;

	public Fragment square;

	private float speed;

	protected override void Awake()
	{
		rotator.Awake();
		square.Awake();
	}

	public void Show()
	{
		speed = HypnoLair.env.GetSpeed();
		rotator.TriggerAnim("rotate", speed);
		square.TriggerAnim("rotate", speed);
	}

	public void Hide()
	{
	}

	public void Rotate()
	{
		rotator.TriggerAnim("rotate", speed);
		square.TriggerAnim("rotate", speed);
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public void Freeze()
	{
		rotator.TriggerAnim("rotate", 0f);
		square.TriggerAnim("rotate", 0f);
	}
}
