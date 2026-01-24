using System.Collections;
using UnityEngine;

public class RealWorld : Wrapper
{
	public static RealWorld env;

	[Header("Children")]
	public Fader Fader;

	public TitleCard TitleCard;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
	}

	public void TransitionToDream()
	{
		StartCoroutine(TransitioningToDream());
	}

	private IEnumerator TransitioningToDream()
	{
		gears[0].TriggerAnim("chapterOut");
		yield return new WaitForSeconds(0.617f);
		Fader.SetColor(1);
		Fader.SetSpeed(10f);
		Fader.Activate();
	}

	public void TransitionFromDream()
	{
		gears[0].TriggerAnim("chapterIn");
		Fader.Show();
		Fader.SetColor(1);
		Fader.SetSpeed(10f);
		Fader.Deactivate();
	}

	public void PlayTransitionSound()
	{
		speakers[0].TriggerSound(0);
		speakers[0].SetParent(null);
		speakers[0].DestroyAfterSound(0);
		Object.DontDestroyOnLoad(speakers[0]);
	}
}
