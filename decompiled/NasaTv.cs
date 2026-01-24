using UnityEngine;

public class NasaTv : Wrapper
{
	[Header("Children")]
	public SpaceMeter[] SpaceMeters;

	[Header("Fragments")]
	public Fragment hoverer;

	public Fragment positioner;

	public Fragment ticker;

	public Fragment feedbacker;

	public spriteFragment feedback;

	public spriteFragment overlay;

	public Fragment flash;

	private bool isActivated;

	protected override void Awake()
	{
		hoverer.Awake();
		positioner.Awake();
		ticker.Awake();
		feedbacker.Awake();
		feedback.Initiate();
		overlay.Initiate();
		flash.Awake();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		SpaceMeters[0].Show();
		SpaceMeters[0].ToggleIsVisible(toggle: false);
		SpaceMeters[1].Show();
		ticker.TriggerAnimWait(0.11667f, "tick", AngrySkies.env.GetSpeed() / 2f);
		Hover();
		positioner.TriggerAnim("idled");
		feedbacker.TriggerAnim("hidden");
	}

	public void Hide()
	{
		isActivated = false;
		SpaceMeters[0].Hide();
		SpaceMeters[1].Hide();
		RenderChildren(toggle: false);
	}

	public void Tick(float delta)
	{
		ticker.TriggerAnimDelayedDelta(delta, "tick", AngrySkies.env.GetSpeed() / 2f);
	}

	public void Hover()
	{
		hoverer.TriggerAnim("hover", AngrySkies.env.GetSpeed() / 4f);
	}

	public void MoveRight(float duration)
	{
		positioner.TriggerAnim("moveRight", AngrySkies.env.GetSpeed() / duration);
	}

	public void MoveLeft(float duration)
	{
		positioner.TriggerAnim("moveLeft", AngrySkies.env.GetSpeed() / duration);
	}

	public void TriggerFeedback(float accuracy)
	{
		feedbacker.TriggerAnim("in");
		if (accuracy == 1f)
		{
			feedback.SetState(SaveManager.GetLang());
			overlay.SetState(0);
		}
		else if (accuracy == 0.332f)
		{
			feedback.SetState(SaveManager.GetLang() + 10);
			overlay.SetState(1);
		}
		else
		{
			feedback.SetState(SaveManager.GetLang() + 20);
			overlay.SetState(2);
		}
	}

	public void Flash()
	{
		flash.TriggerAnim("flash");
	}

	public void FlashDelayed(float timeStarted)
	{
		flash.TriggerAnimDelayedTimeStarted(timeStarted, "flash");
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
