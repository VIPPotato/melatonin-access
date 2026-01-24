using UnityEngine;

public class Wings : Wrapper
{
	private bool isActivated;

	private const float animTempo = 100f;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		isActivated = false;
		RenderChildren(toggle: false);
	}

	public void Flap(float speedMultiplier = 1f)
	{
		float speed = GetSpeed();
		sprites[0].TriggerAnim("flap", speed * speedMultiplier);
		sprites[1].TriggerAnim("flap", speed * speedMultiplier);
	}

	public void FlapSoundDelayed(float delta)
	{
		speakers[0].TriggerSoundDelayedDelta(delta, Random.Range(0, 4));
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 100f;
	}
}
