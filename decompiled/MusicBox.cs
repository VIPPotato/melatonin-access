using System.Collections;
using UnityEngine;

public class MusicBox : Wrapper
{
	public static MusicBox env;

	[Header("Props")]
	public float[] trackTempos0;

	public float[] trackTempos1;

	public float[] trackTempos2;

	private bool isPaused = true;

	private int activeTrack;

	private int speakerNum;

	private float durationPaused;

	private float timePaused;

	private float silenceDuration;

	private float scheduledTime;

	private static float skippedTime;

	private static AudioClip customSongClip;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
	}

	public void PlaySong()
	{
		if (isPaused)
		{
			isPaused = false;
			durationPaused = Technician.mgr.GetDspTime();
			silenceDuration += 0.0785f * GetSecsPerBeat();
			scheduledTime = scheduledTime + (float)AudioSettings.dspTime + 0.11667f;
			speakers[speakerNum].SetSoundTime(activeTrack, silenceDuration + skippedTime);
			speakers[speakerNum].TriggerSoundScheduled(scheduledTime, activeTrack);
		}
	}

	public void PauseSong()
	{
		if (!isPaused)
		{
			isPaused = true;
			timePaused = Technician.mgr.GetDspTime();
			speakers[speakerNum].PauseSound(activeTrack);
		}
	}

	public void UnpauseSong()
	{
		if (isPaused)
		{
			isPaused = false;
			durationPaused = durationPaused + Technician.mgr.GetDspTime() - timePaused;
			speakers[speakerNum].TriggerSound(activeTrack);
		}
	}

	public void RewindSong(float amount)
	{
		float soundTime = speakers[speakerNum].GetSoundTime(activeTrack);
		speakers[speakerNum].SetSoundTime(activeTrack, soundTime - amount);
		durationPaused += amount;
	}

	public void ResetSong()
	{
		silenceDuration = 0f;
		scheduledTime = 0f;
		speakers[speakerNum].CancelSound(activeTrack);
	}

	public void PingMetronomeDelayed(float timeStarted, int beat)
	{
		int index = ((beat != 1) ? 1 : 0);
		speakers[3].TriggerSoundDelayedTimeStarted(timeStarted, index);
	}

	public void ChangeTrackLive(float timeStarted, int newTrack, int beatTotal)
	{
		StartCoroutine(ChangingTrackLive(timeStarted, newTrack, beatTotal));
	}

	private IEnumerator ChangingTrackLive(float timeStarted, int newTrack, int beatTotal)
	{
		if (newTrack != activeTrack)
		{
			int oldTrack = activeTrack;
			activeTrack = newTrack;
			silenceDuration = 0.0785f * GetSecsPerBeat();
			float num = (float)(beatTotal - 1) * GetSecsPerBeat();
			speakers[speakerNum].SetSoundTime(activeTrack, num + silenceDuration);
			speakers[speakerNum].TriggerSoundDelayedTimeStarted(timeStarted, activeTrack);
			float checkpoint = timeStarted + 0.11667f;
			yield return new WaitUntil(() => GetSongTime() > checkpoint);
			speakers[speakerNum].CancelSound(oldTrack);
			float num2 = GetTempo(newTrack) / GetTempo(oldTrack);
			if (num2 > 1f)
			{
				Interface.env.WipeScreen.CrossInWithSpeedUp();
			}
			else if (num2 < 1f)
			{
				Interface.env.WipeScreen.CrossInWithSlowDown();
			}
		}
	}

	public void SwapToCustomizedMusic()
	{
		speakers[speakerNum].SetSoundAudioClip(0, customSongClip);
		trackTempos1[0] = SaveManager.mgr.GetEditorData().customSongTempo;
		if (SaveManager.mgr.GetEditorData().customSongSyncOffset > 0)
		{
			scheduledTime = 0.001f * (float)SaveManager.mgr.GetEditorData().customSongSyncOffset;
		}
		else if (SaveManager.mgr.GetEditorData().customSongSyncOffset < 0)
		{
			silenceDuration = 0.001f * (float)Mathf.Abs(SaveManager.mgr.GetEditorData().customSongSyncOffset);
		}
	}

	public void SetTrack(int newTrack)
	{
		if (newTrack != activeTrack)
		{
			float soundTime = speakers[speakerNum].GetSoundTime(activeTrack);
			activeTrack = newTrack;
			speakers[speakerNum].SetSoundTime(activeTrack, soundTime);
		}
	}

	public void SetSongSpeed(float speed)
	{
		speakers[speakerNum].SetSoundPitch(activeTrack, speed);
	}

	public void SetSpeakerNum(int newSpeakerNum)
	{
		speakerNum = newSpeakerNum;
	}

	public void ToggleSongMute(bool toggle)
	{
		speakers[speakerNum].ToggleSoundMute(activeTrack, toggle);
	}

	public static void ResetCustomSongClip()
	{
		customSongClip = null;
	}

	public static void SetCustomSongClip(AudioClip newClip)
	{
		customSongClip = newClip;
	}

	public static void SetSkippedTime(float newSkippedTime)
	{
		skippedTime = newSkippedTime;
	}

	private float GetTempo(int trackNum)
	{
		if (speakerNum == 0)
		{
			return trackTempos0[trackNum];
		}
		if (speakerNum == 1)
		{
			return trackTempos1[trackNum];
		}
		if (speakerNum == 2)
		{
			return trackTempos2[trackNum];
		}
		return 0f;
	}

	public float GetSongTime()
	{
		return Technician.mgr.GetDspTime() - durationPaused;
	}

	public float GetActiveTempo()
	{
		return GetTempo(activeTrack);
	}

	public float GetSecsPerBeat()
	{
		return 60f / GetTempo(activeTrack);
	}

	public static bool CheckIfCustomSongClipExists()
	{
		if (customSongClip != null)
		{
			return true;
		}
		return false;
	}
}
