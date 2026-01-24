using UnityEngine;

public class ScoreMeter : Wrapper
{
	private bool isActivated;

	private bool isOnFire;

	private int combo;

	private float maxMeterHeight;

	private float maxMarkerPosition;

	private float markerNudge;

	private float timeTilOut;

	private string mod;

	private Color colorFire = new Color(62f / 85f, 1f, 0.96862745f);

	private Color color3star = new Color(73f / 85f, 0.85490197f, 1f);

	private Color color2star = new Color(0.9372549f, 44f / 51f, 1f);

	private Color color1star0star = new Color(1f, 73f / 85f, 81f / 85f);

	protected override void Awake()
	{
		SetupFragments();
		maxMeterHeight = sprites[0].GetLocalHeight();
		maxMarkerPosition = gears[0].GetLocalY() - sprites[0].GetLocalY();
		markerNudge = maxMarkerPosition - gears[0].GetLocalY();
		RenderChildren(toggle: false);
	}

	public void Show(int gameMode)
	{
		isActivated = true;
		if (gameMode == 2 || gameMode == 4)
		{
			mod = "alt";
		}
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		TriggerScoreUpdate();
	}

	public void TriggerScoreUpdate()
	{
		float num = Dream.dir.GetScore();
		if (isOnFire)
		{
			sprites[0].SetSpriteColor(colorFire);
			sprites[2].SetSpriteColor(colorFire);
			sprites[3].SetSpriteColor(colorFire);
		}
		else
		{
			Color spriteColor = ((num == 3f) ? color3star : ((num == 2f) ? color2star : color1star0star));
			sprites[0].SetSpriteColor(spriteColor);
			sprites[2].SetSpriteColor(spriteColor);
			sprites[3].SetSpriteColor(spriteColor);
		}
		sprites[1].TriggerAnim(num + mod);
		sprites[0].SetLocalScale(sprites[0].GetLocalWidth(), maxMeterHeight * Dream.dir.GetPercentScore());
		gears[0].SetLocalPosition(gears[0].GetLocalX(), maxMarkerPosition * Dream.dir.GetPercentScore() - markerNudge);
		sprites[2].SetLocalPosition(sprites[2].GetLocalX(), gears[0].GetLocalY());
	}

	public void FireItUp()
	{
		isOnFire = true;
		speakers[0].TriggerSound(0);
		speakers[0].TriggerSound(1);
		sprites[4].TriggerAnim("idling");
		sprites[5].TriggerAnim("idling");
	}

	public void Extinguish()
	{
		isOnFire = false;
		sprites[4].TriggerAnim("hidden");
		sprites[5].TriggerAnim("hidden");
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsOnFire()
	{
		return isOnFire;
	}
}
