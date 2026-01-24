using UnityEngine;

public class TutorialWorld : Wrapper
{
	public static TutorialWorld env;

	[Header("Children")]
	public Fader Fader;

	public Feedback[] Feedbacks;

	public Sweat Sweat;

	[Header("Fragments")]
	public textboxFragment rewindText;

	protected override void Awake()
	{
		env = this;
		rewindText.Initiate();
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		DreamWorld.env.SetFeedbacks(Feedbacks);
	}

	public void Scratch()
	{
		gears[0].TriggerAnim("activate");
		speakers[1].TriggerSound(0);
		Fader.Activate();
	}

	public void Rewind()
	{
		gears[0].TriggerAnim("deactivate");
		speakers[1].TriggerSound(1);
		Fader.Deactivate();
	}

	public void PlayActionSound(int soundNum)
	{
		speakers[0].TriggerSound(soundNum);
	}
}
