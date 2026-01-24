using UnityEngine;

public class McLighter : Wrapper
{
	[Header("Children")]
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	private float initLocalY;

	protected override void Awake()
	{
		initLocalY = GetLocalY();
	}

	public void Activate()
	{
		SetLocalPosition(GetLocalX(), initLocalY);
		gears[0].TriggerAnim("in");
		sprites[0].TriggerAnim("idling");
		sprites[1].TriggerAnim("hidden");
		sprites[2].SetSpriteAlpha(0f);
		sprites[3].SetSpriteAlpha(0f);
	}

	public void Deactivate()
	{
		gears[0].TriggerAnim("out");
	}

	public void Light()
	{
		gears[0].TriggerAnim("lower");
		sprites[0].TriggerAnim("light");
		sprites[1].TriggerAnim("light");
		sprites[2].FadeInSprite(0.25f, 0.075f);
		sprites[3].FadeInSprite(1f, 0.075f);
		sprites[4].SetLocalEulerAngles(0f, 0f, Random.Range(0, 360));
		sprites[4].TriggerAnim("spark");
		speakers[0].TriggerSound(0);
		speakers[0].TriggerSound(1);
		Interface.env.Cam.Breeze();
	}

	public void Release()
	{
		gears[0].TriggerAnim("rise");
		sprites[0].TriggerAnim("close");
		sprites[1].TriggerAnim("hidden");
		sprites[2].SetSpriteAlpha(0f);
		sprites[3].SetSpriteAlpha(0f);
		speakers[0].CancelSound(1);
		speakers[0].TriggerSound(2);
		Interface.env.Cam.Breeze();
	}
}
