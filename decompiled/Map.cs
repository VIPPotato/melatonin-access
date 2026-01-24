using System.Collections;
using UnityEngine;

public class Map : Wrapper
{
	public static Map env;

	[Header("Children")]
	public TotalBox TotalBox;

	public RequiredBox RequiredBox;

	public Feathering Feathering;

	public Cloud[] Clouds;

	public Fence[] Fences;

	public Neighbourhood Neighbourhood;

	public Pool Pool;

	public Floor Floor;

	public GrowBubble GrowBubble;

	private bool isEnabled;

	private AudioLowPassFilter lowPassFilter;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		lowPassFilter = speakers[0].GetComponent<AudioLowPassFilter>();
	}

	public void Activate()
	{
		StartCoroutine(Activating());
	}

	private IEnumerator Activating()
	{
		GrowBubble.Activate();
		yield return new WaitForSeconds(1.875f);
		sprites[0].TriggerAnim("deactivate");
		Feathering.Show();
		Cloud[] clouds = Clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].Show();
		}
		Neighbourhood.Show();
		Floor.Show();
		if (Chapter.GetActiveChapterNum() <= 4)
		{
			Fence[] fences = Fences;
			for (int i = 0; i < fences.Length; i++)
			{
				fences[i].Show();
			}
			Pool.Show();
		}
		Neighbourhood.McMap.Show();
		if (Chapter.GetActiveChapterNum() <= 4 && Chapter.dir.CheckIsOnSavedChapter() && SaveManager.mgr.GetChapterEarnedStars(Chapter.GetActiveChapterNum()) >= 8 && !Chapter.dir.CheckIsRemixSeen())
		{
			Chapter.dir.ToggleIsRemixSeen(toggle: true);
			Interface.env.Cam.MoveToTarget(new Vector3(Interface.env.transform.position.x, Neighbourhood.GetRemixLandmark().transform.position.y, 0f), 1.5f);
			yield return new WaitForSeconds(2f);
			Neighbourhood.GetRemixLandmark().Enable(isPlaySound: true);
			Pool.Enable();
			yield return new WaitForSeconds(1.75f);
			Neighbourhood.McMap.Introduce();
		}
		else
		{
			if (Chapter.GetActiveChapterNum() <= 4 && SaveManager.mgr.GetChapterEarnedStars(Chapter.GetActiveChapterNum()) >= 8)
			{
				Neighbourhood.GetRemixLandmark().Enable(isPlaySound: false);
				Pool.Enable();
			}
			Neighbourhood.McMap.Introduce();
		}
	}

	private void Update()
	{
		if (isEnabled)
		{
			if (Interface.env.Cam.GetY() > 5f)
			{
				lowPassFilter.enabled = false;
			}
			else if (Interface.env.Cam.GetY() > 1f)
			{
				lowPassFilter.enabled = true;
				lowPassFilter.cutoffFrequency = 420f * Interface.env.Cam.GetY();
			}
			else
			{
				lowPassFilter.enabled = true;
				lowPassFilter.cutoffFrequency = 420f;
			}
		}
	}

	public void FadeOutMusic()
	{
		speakers[0].FadeOutSound(0, 0.25f);
	}

	public void PlayMusic()
	{
		if (!isEnabled)
		{
			isEnabled = true;
			speakers[0].TriggerSound(0);
		}
	}

	public void PlayToggleSound()
	{
		speakers[1].SetSoundPitch(0, 1f);
		speakers[1].TriggerSoundStack(0);
	}

	public void PlayToggleSoundAlt()
	{
		speakers[1].SetSoundPitch(0, 1.48f);
		speakers[1].TriggerSoundStack(0);
	}

	public void PlayNextSound()
	{
		speakers[1].TriggerSoundStack(1);
	}

	public void PlayCancelSound()
	{
		speakers[1].TriggerSound(2);
	}

	public void PlayNopeSound()
	{
		speakers[1].TriggerSoundStack(3);
	}
}
