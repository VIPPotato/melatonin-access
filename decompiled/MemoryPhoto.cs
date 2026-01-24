using System.Collections;
using UnityEngine;

public class MemoryPhoto : Wrapper
{
	private bool isGood;

	private int size;

	private bool isBurning;

	private float initClipY;

	private int startBeat;

	private int endBeat;

	private int cueNum;

	private Coroutine showing;

	protected override void Awake()
	{
		SetupFragments();
		initClipY = sprites[3].GetLocalY();
		RenderChildren(toggle: false);
	}

	public void Show(float timeStarted, bool isBurnDelayed)
	{
		showing = StartCoroutine(Showing(timeStarted, isBurnDelayed));
	}

	private IEnumerator Showing(float timeStarted, bool isBurnDelayed)
	{
		RenderChildren(toggle: true);
		isBurning = false;
		gears[0].TriggerAnim("convey", Darkroom.env.GetSpeed());
		sprites[0].TriggerAnim("awaiting");
		sprites[1].TriggerAnim("goodIdled" + size);
		sprites[2].TriggerAnim("hidden");
		float z = Random.Range(-1.5f, 1.5f);
		sprites[0].SetLocalEulerAngles(0f, 0f, z);
		sprites[1].SetLocalEulerAngles(0f, 0f, z);
		sprites[2].SetLocalEulerAngles(0f, 0f, z);
		if (Random.Range(0, 2) == 0)
		{
			sprites[3].ToggleSpriteFlip(toggle: true);
		}
		else
		{
			sprites[3].ToggleSpriteFlip(toggle: false);
		}
		sprites[3].SetLocalEulerAngles(0f, 0f, Random.Range(-4f, 4f));
		sprites[3].SetLocalY(initClipY + Random.Range(-0.15f, 0.15f));
		if (!Dream.dir)
		{
			yield break;
		}
		if (size == 1)
		{
			sprites[3].SetLocalX(0.75f);
		}
		else if (size == 3)
		{
			sprites[3].SetLocalX(-1.75f);
		}
		else
		{
			sprites[3].SetLocalX(0f);
		}
		if (isGood)
		{
			float beat = MusicBox.env.GetSecsPerBeat();
			float burnDelay = (isBurnDelayed ? (beat * 0.5f) : 0f);
			float offset = ((!SaveManager.mgr.CheckIsBiggerHitWindows()) ? 0.04667f : 0.06267f);
			float checkpoint = timeStarted + beat * 4f + burnDelay + offset;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			Darkroom.env.PhotoPulley.AddBurnableMemoryPhoto(this);
			if (size == 0)
			{
				checkpoint = timeStarted + beat * 5f - offset;
				yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			}
			else if (size == 1)
			{
				checkpoint = timeStarted + beat * 4.5f + burnDelay - offset;
				yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			}
			else if (size == 3)
			{
				checkpoint = timeStarted + beat * 6f - offset;
				yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			}
			Darkroom.env.PhotoPulley.RemoveBurnableMemoryPhoto(this);
			StopBurn();
			yield break;
		}
		float checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 3f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint2);
		if (size == 1)
		{
			Darkroom.env.PlayPrepCueDelayed(checkpoint2, 3);
		}
		else if (size == 3)
		{
			Darkroom.env.PlayPrepCueDelayed(checkpoint2, 2);
		}
		else
		{
			Darkroom.env.PlayPrepCueDelayed(checkpoint2, 1);
		}
		checkpoint2 += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint2);
		sprites[0].TriggerAnim("flash" + size, Darkroom.env.GetSpeed());
		sprites[1].TriggerAnim("badIdled" + size);
		if (size == 1)
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 4f;
		}
		else if (size == 3)
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 4f;
		}
		else
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 4f;
		}
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint2);
		Darkroom.env.PhotoPulley.AddBurnableMemoryPhoto(this);
		Darkroom.env.PhotoPulley.SetDissolvableMemoryPhoto(this);
		if (size == 1)
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 4.5f + 0.11667f;
		}
		else if (size == 3)
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 6f + 0.11667f;
		}
		else
		{
			checkpoint2 = timeStarted + MusicBox.env.GetSecsPerBeat() * 5f + 0.11667f;
		}
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint2);
		Darkroom.env.PhotoPulley.RemoveBurnableMemoryPhoto(this);
		if (Darkroom.env.PhotoPulley.GetDissolvableMemoryPhoto() == this)
		{
			Darkroom.env.PhotoPulley.ClearDissolvableMemoryPhoto();
		}
		StopBurn();
	}

	public void Hide()
	{
		CancelCoroutine(showing);
		RenderChildren(toggle: false);
	}

	public void StartBurn()
	{
		if (!isBurning)
		{
			isBurning = true;
			if (!sprites[0].CheckIsAnimPlaying("burn" + size))
			{
				sprites[0].TriggerAnim("burn" + size, Darkroom.env.GetSpeed());
			}
			if (!sprites[2].CheckIsAnimPlaying("burn" + size))
			{
				sprites[2].TriggerAnim("burn" + size, Darkroom.env.GetSpeed());
			}
			sprites[0].SetCurrentAnimSpeed(Darkroom.env.GetSpeed());
			sprites[2].SetCurrentAnimSpeed(Darkroom.env.GetSpeed());
		}
	}

	public void StopBurn()
	{
		if (isBurning)
		{
			isBurning = false;
			sprites[0].SetCurrentAnimSpeed(0f);
			sprites[2].SetCurrentAnimSpeed(0f);
		}
	}

	public void Dissolve()
	{
		isBurning = false;
		sprites[0].TriggerAnim("awaiting");
		sprites[1].TriggerAnim("dissolve" + size);
		sprites[2].TriggerAnim("blaze" + size);
	}

	public void Setup(bool newIsGood, int newSize)
	{
		isGood = newIsGood;
		size = newSize;
	}

	public bool CheckIsConveying()
	{
		return gears[0].CheckIsAnimPlaying("convey");
	}
}
