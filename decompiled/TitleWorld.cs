using UnityEngine;

public class TitleWorld : Wrapper
{
	public static TitleWorld env;

	[Header("Children")]
	public AchievementsMenu AchievementsMenu;

	public LangMenu LangMenu;

	public LangHint LangHint;

	public Instruction Instruction;

	public Fader Fader;

	public CompanySplash CompanySplash;

	public DreamOcean DreamOcean;

	[Header("Fragments")]
	public Fragment speaker;

	protected override void Awake()
	{
		env = this;
		speaker.Awake();
	}

	public void ActivateInterfaces()
	{
		LangHint.Activate();
		Instruction.Activate();
	}

	public void DeactivateInterfaces()
	{
		LangHint.Deactivate();
		Instruction.Deactivate();
	}

	public void PauseMusic()
	{
		speaker.PauseSound(0);
	}

	public void PlayMusic()
	{
		speaker.TriggerSound(0);
	}
}
