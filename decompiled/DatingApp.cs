public class DatingApp : Wrapper
{
	public DateCard[] DateCards;

	private int bioNum = 2;

	private int spawnNum;

	private bool isTopCardHidden;

	private bool isSliding;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		RenderChildren(toggle: true);
		isSliding = true;
		gears[0].TriggerAnim("in");
		gears[1].TriggerAnim("awaiting");
		sprites[1].TriggerAnim("awaiting");
		sprites[2].TriggerAnim("awaiting");
		sprites[3].ToggleSpriteRenderer(toggle: true);
		DateCards[0].Hide();
		DateCards[1].Hide();
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		isSliding = true;
		gears[0].TriggerAnim("cropped");
		gears[1].TriggerAnim("awaiting");
		sprites[1].TriggerAnim("awaiting");
		sprites[2].TriggerAnim("awaiting");
		sprites[3].ToggleSpriteRenderer(toggle: false);
		DateCards[0].Hide();
		DateCards[1].Hide();
	}

	public void Spin()
	{
		sprites[3].ToggleSpriteRenderer(toggle: true);
	}

	public void Slide()
	{
		if (isSliding)
		{
			gears[1].TriggerAnim("queueSlide");
			ToggleIsTopCardHidden(toggle: false);
		}
	}

	public void SlideWithDateCard()
	{
		if (isSliding)
		{
			gears[1].TriggerAnim("queueSlide");
			ToggleIsTopCardHidden(toggle: true);
		}
	}

	public void RotateInReject()
	{
		sprites[1].TriggerAnim("rotateIn", LoveLand.env.GetSpeed());
	}

	public void RotateInAccept()
	{
		sprites[2].TriggerAnim("rotateIn", LoveLand.env.GetSpeed());
	}

	public void RotateOut()
	{
		if (sprites[1].CheckIsAnimPlaying("rotateIn"))
		{
			sprites[1].TriggerAnim("rotateOut");
		}
		else if (sprites[2].CheckIsAnimPlaying("rotateIn"))
		{
			sprites[2].TriggerAnim("rotateOut");
		}
	}

	public void SetToCentered()
	{
		gears[0].TriggerAnim("centered");
	}

	public void SetToCropped()
	{
		gears[0].TriggerAnim("cropped");
	}

	public void Center()
	{
		gears[0].TriggerAnim("center", LoveLand.env.GetSpeed());
	}

	public void Crop()
	{
		gears[0].TriggerAnim("crop", LoveLand.env.GetSpeed());
	}

	public void ToggleIsSliding(bool toggle)
	{
		isSliding = toggle;
		if (!isSliding)
		{
			gears[1].TriggerAnim("awaiting");
		}
	}

	public void ToggleIsTopCardHidden(bool toggle)
	{
		isTopCardHidden = toggle;
		sprites[0].ToggleSpriteRenderer(!isTopCardHidden);
		sprites[3].ToggleSpriteRenderer(!isTopCardHidden);
	}

	public void IncreaseDateCardNumber()
	{
		spawnNum = ((spawnNum + 1 < DateCards.Length) ? (spawnNum + 1) : 0);
	}

	public void IncreaseBioNum()
	{
		bioNum++;
		if (bioNum == 5)
		{
			bioNum = 2;
		}
	}

	public DateCard GetDateCard()
	{
		return DateCards[spawnNum];
	}

	public int GetBioNum()
	{
		return bioNum;
	}
}
