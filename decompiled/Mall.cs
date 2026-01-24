using UnityEngine;

public class Mall : Wrapper
{
	public static Mall env;

	[Header("Children")]
	public Counter Counter;

	public StoreDisplay StoreDisplay;

	public Sweat Sweat;

	public Feedback[] Feedbacks;

	private bool isActivated;

	private const float animTempo = 90f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 1);
	}

	public void Show(bool isBobble = true)
	{
		isActivated = true;
		RenderChildren(toggle: true, 1);
		Interface.env.Cam.SetPosition(0f, 0f);
		Counter.Show();
		StoreDisplay.Show();
		DreamWorld.env.SetFeedbacks(Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		StoreDisplay.Hide();
		RenderChildren(toggle: false, 1);
	}

	public void PlayFeedback()
	{
		speakers[0].TriggerSound(0);
	}

	public void PlayGoodFeedback()
	{
		speakers[0].TriggerSound(1);
	}

	public void PlayBadFeedback()
	{
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 90f;
	}
}
