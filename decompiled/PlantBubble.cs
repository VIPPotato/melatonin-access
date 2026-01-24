using UnityEngine;

public class PlantBubble : Wrapper
{
	public WaterLevel WaterLevel;

	public Fragment bubbler;

	public Fragment fg;

	[Header("Props")]
	private bool isActivated;

	private bool isInteractive;

	public Sprite[] fgSprites;

	protected override void Awake()
	{
		bubbler.Awake();
		fg.Awake();
		RenderChildren(toggle: false);
	}

	public void Activate(int fgNum)
	{
		isActivated = true;
		RenderChildren(toggle: true);
		bubbler.TriggerAnim("activate");
		fg.SetSprite(fgSprites[fgNum]);
		WaterLevel.Show();
	}

	public void Hide()
	{
		isActivated = false;
		isInteractive = false;
		WaterLevel.Hide();
		RenderChildren(toggle: false);
	}

	public void ToggleIsInteractive(bool toggle)
	{
		isInteractive = toggle;
	}

	public bool CheckIsInteractive()
	{
		if (isActivated)
		{
			return isInteractive;
		}
		return false;
	}
}
