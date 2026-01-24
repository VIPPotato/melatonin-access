using System.Collections;
using UnityEngine;

public class Credits : Wrapper
{
	public static Credits env;

	[Header("Children")]
	public Fader[] Faders;

	[Header("Fragments")]
	public Fragment lister;

	public spriteFragment logoGame;

	public spriteFragment logoCompany;

	public textboxFragment[] labels;

	public textboxFragment[] names;

	public Fragment speaker;

	protected override void Awake()
	{
		env = this;
		lister.Awake();
		logoGame.Initiate();
		logoCompany.Initiate();
		textboxFragment[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		array = names;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		speaker.Awake();
		RenderChildren(toggle: false, 1);
	}

	public void Show()
	{
		RenderChildren(toggle: true, 1);
		logoGame.ToggleSpriteRenderer(toggle: true);
		logoCompany.ToggleSpriteRenderer(toggle: false);
		labels[0].ToggleMeshRenderer(toggle: false);
		names[0].ToggleMeshRenderer(toggle: false);
		Faders[0].Show();
		Faders[0].Deactivate();
		Faders[1].Show();
		if (SaveManager.GetLang() == 3)
		{
			labels[0].SetLocalY(-1.15f);
			labels[1].SetLocalY(0.872f);
			labels[2].SetLocalY(-1.271f);
			labels[3].SetLocalY(-3.32f);
			labels[4].SetLocalY(-5.417f);
			labels[5].SetLocalY(-7.487f);
			labels[6].SetLocalY(-9.403f);
			labels[7].SetLocalY(-11.57f);
			labels[8].SetLocalY(-13.47f);
			names[6].SetLocalY(-8.894f);
			names[7].SetLocalY(-11.06f);
			names[8].SetLocalY(-12.91f);
		}
	}

	public void TransitionLogoGameToLogoCompany()
	{
		StartCoroutine(TransitioningLogoGameToLogoCompany());
	}

	private IEnumerator TransitioningLogoGameToLogoCompany()
	{
		Faders[0].SetSpeed(1.75f);
		Faders[0].Activate();
		yield return new WaitForSeconds(0.572f);
		logoGame.ToggleSpriteRenderer(toggle: false);
		logoCompany.ToggleSpriteRenderer(toggle: true);
		Faders[0].Deactivate();
	}

	public void TransitionLogoCompanyToCreator()
	{
		StartCoroutine(TransitioningLogoCompanyToCreator());
	}

	private IEnumerator TransitioningLogoCompanyToCreator()
	{
		Faders[0].SetSpeed(1.75f);
		Faders[0].Activate();
		yield return new WaitForSeconds(0.572f);
		logoCompany.ToggleSpriteRenderer(toggle: false);
		labels[0].ToggleMeshRenderer(toggle: true);
		names[0].ToggleMeshRenderer(toggle: true);
		Faders[0].Deactivate();
	}

	public void ScrollList()
	{
		StartCoroutine(ScrollingList());
	}

	private IEnumerator ScrollingList()
	{
		Faders[0].Activate();
		Faders[1].Deactivate();
		lister.TriggerAnim("scroll");
		yield return new WaitForSeconds(0.572f);
		labels[0].ToggleMeshRenderer(toggle: false);
		names[0].ToggleMeshRenderer(toggle: false);
		Faders[0].Deactivate();
	}

	public void PlayCreditsMusic(int songNum)
	{
		speaker.TriggerSound(songNum);
	}

	public float GetScrollDuration()
	{
		return lister.GetAnimDuration("scroll");
	}
}
