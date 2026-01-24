using UnityEngine;

public class LivingRoom : Wrapper
{
	public static LivingRoom env;

	[Header("Fragments")]
	public Fragment parallaxer;

	public Fragment mc;

	public Fragment glow;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		parallaxer.Awake();
		mc.Awake();
		glow.Awake();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
	}

	public void PlayAmbience(int soundNum, float startPos)
	{
		speakers[soundNum].TriggerSound(0);
		speakers[soundNum].SetSoundTime(0, startPos);
		speakers[3].TriggerSound(0);
	}

	public void FadeOutAmbience(int soundNum)
	{
		speakers[soundNum].FadeOutSound(0, 0.2f);
		speakers[3].FadeOutSound(0, 0.2f);
	}

	public void FadeInAmbience(int soundNum)
	{
		if (soundNum == 0)
		{
			speakers[0].FadeInSound(0, 0.2f, 0.33f);
		}
		else
		{
			speakers[1].FadeInSound(0, 0.2f, 0.67f);
		}
		speakers[3].FadeInSound(0, 0.2f, 0.25f);
	}

	public void StopAmbience()
	{
		speakers[0].CancelSound(0);
		speakers[1].CancelSound(0);
		speakers[3].CancelSound(0);
		speakers[3].CancelSound(1);
	}

	public void Static()
	{
		glow.TriggerAnim("staticGlowing");
		speakers[3].TriggerSound(1);
	}

	public void PlaySoundEffect(int soundNum)
	{
		speakers[2].TriggerSound(soundNum);
	}

	public void StopSoundEffect(int soundNum)
	{
		speakers[2].CancelSound(soundNum);
	}

	public void SetCharacter(string animName)
	{
		mc.TriggerAnim(animName);
	}

	public void SetCharacterTrigger(string triggerName)
	{
		mc.PingAnimTrigger(triggerName);
	}

	public void SetComposition(string animName)
	{
		parallaxer.TriggerAnim(animName);
	}

	public void PauseCharacter()
	{
		mc.SetCurrentAnimSpeed(0f);
	}

	public float GetCharacterAnimDuration(string animName)
	{
		return mc.GetAnimDuration(animName);
	}
}
