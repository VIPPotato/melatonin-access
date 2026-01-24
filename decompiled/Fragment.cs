using System.Collections;
using UnityEngine;

public class Fragment : Custom
{
	private bool isAwakened;

	private bool isRealTimeFader;

	private SpriteRenderer spriteRenderer;

	private MeshRenderer meshRenderer;

	private TextMesh textMesh;

	private AudioSource[] sounds;

	private Animator animator;

	private Coroutine fadingSprite;

	private Coroutine fadingText;

	private Coroutine fadingSound;

	private static float audioSync;

	private static float audioOffset;

	public virtual void Awake()
	{
		if (!isAwakened)
		{
			isAwakened = true;
			spriteRenderer = GetComponent<SpriteRenderer>();
			meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				textMesh = GetComponent<TextMesh>();
			}
			sounds = GetComponents<AudioSource>();
			animator = GetComponent<Animator>();
		}
	}

	public void TriggerAnim(string animName, float speed = 1f, float startPercent = 0f)
	{
		animator.speed = speed;
		animator.Play(animName, 0, startPercent);
	}

	public void TriggerAnimWait(float waitTime, string animName, float speed = 1f, float startPercent = 0f)
	{
		StartCoroutine(TriggeringAnimWait(waitTime, animName, speed, startPercent));
	}

	private IEnumerator TriggeringAnimWait(float waitTime, string animName, float speed, float startPercent)
	{
		yield return new WaitForSeconds(waitTime);
		TriggerAnim(animName, speed, startPercent);
	}

	public void TriggerAnimDelayedTimeStarted(float timeStarted, string animName, float speed = 1f, float startPercent = 0f)
	{
		StartCoroutine(TriggeringAnimDelayedTimeStarted(timeStarted, animName, speed, startPercent));
	}

	private IEnumerator TriggeringAnimDelayedTimeStarted(float timeStarted, string animName, float speed, float startPercent)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		TriggerAnim(animName, speed, startPercent);
	}

	public void TriggerAnimDelayedDelta(float delta, string animName, float speed = 1f, float startPercent = 0f)
	{
		StartCoroutine(TriggeringAnimDelayedDelta(delta, animName, speed, startPercent));
	}

	private IEnumerator TriggeringAnimDelayedDelta(float delta, string animName, float speed, float startPercent)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		TriggerAnim(animName, speed, startPercent);
	}

	public void TriggerSound(int index)
	{
		if (!sounds[index].isPlaying)
		{
			sounds[index].Play();
		}
		else if (!sounds[index].loop)
		{
			sounds[index].Play();
		}
	}

	public void TriggerSoundDelayedTimeStarted(float timeStarted, int index)
	{
		float num = MusicBox.env.GetSongTime() - timeStarted;
		float num2 = (float)AudioSettings.dspTime - num + (0.11667f + audioOffset + audioSync) * (1f / Technician.mgr.GetWarpSpeed());
		if (!sounds[index].isPlaying)
		{
			sounds[index].PlayScheduled(num2);
		}
		else if (!sounds[index].loop)
		{
			sounds[index].PlayScheduled(num2);
		}
	}

	public void TriggerSoundDelayedDelta(float delta, int index)
	{
		float num = (float)AudioSettings.dspTime - delta + (0.11667f + audioOffset + audioSync) * (1f / Technician.mgr.GetWarpSpeed());
		if (!sounds[index].isPlaying)
		{
			sounds[index].PlayScheduled(num);
		}
		else if (!sounds[index].loop)
		{
			sounds[index].PlayScheduled(num);
		}
	}

	public void TriggerSoundScheduled(float timeScheduled, int index)
	{
		if (!sounds[index].isPlaying)
		{
			sounds[index].PlayScheduled(timeScheduled + audioOffset + audioSync);
		}
		else if (!sounds[index].loop)
		{
			sounds[index].PlayScheduled(timeScheduled + audioOffset + audioSync);
		}
	}

	public void TriggerSoundStack(int index)
	{
		sounds[index].PlayOneShot(sounds[index].clip);
	}

	public void PauseSound(int index)
	{
		sounds[index].Pause();
	}

	public void CancelSound(int index)
	{
		sounds[index].Stop();
	}

	public void CancelAllSounds()
	{
		AudioSource[] array = sounds;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
	}

	public void FadeInSprite(float newAlpha, float duration)
	{
		CancelCoroutine(fadingSprite);
		fadingSprite = StartCoroutine(FadingInSprite(newAlpha, duration));
	}

	private IEnumerator FadingInSprite(float newAlpha, float duration)
	{
		spriteRenderer.enabled = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(0f, newAlpha, elapsed / duration);
			spriteRenderer.color = color;
			yield return null;
		}
	}

	public void FadeOutSprite(float startAlpha, float duration)
	{
		CancelCoroutine(fadingSprite);
		fadingSprite = StartCoroutine(FadingOutSprite(startAlpha, duration));
	}

	private IEnumerator FadingOutSprite(float startAlpha, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
			spriteRenderer.color = color;
			yield return null;
		}
		spriteRenderer.enabled = false;
	}

	public void FadeInText(float newAlpha, float duration)
	{
		CancelCoroutine(fadingText);
		fadingText = StartCoroutine(FadingInText(newAlpha, duration));
	}

	private IEnumerator FadingInText(float newAlpha, float duration)
	{
		meshRenderer.enabled = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = meshRenderer.material.color;
			color.a = Mathf.Lerp(0f, newAlpha, elapsed / duration);
			meshRenderer.material.color = color;
			yield return null;
		}
	}

	public void FadeOutText(float startAlpha, float duration)
	{
		CancelCoroutine(fadingText);
		fadingText = StartCoroutine(FadingOutText(startAlpha, duration));
	}

	private IEnumerator FadingOutText(float startAlpha, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = meshRenderer.material.color;
			color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
			meshRenderer.material.color = color;
			yield return null;
		}
		meshRenderer.enabled = false;
	}

	public void FadeInSound(int index, float rate, float stopVol)
	{
		CancelCoroutine(fadingSound);
		fadingSound = StartCoroutine(FadingInSound(index, rate, stopVol));
	}

	private IEnumerator FadingInSound(int index, float rate, float stopVol)
	{
		sounds[index].volume = 0f;
		while (sounds[index].volume < stopVol)
		{
			sounds[index].volume = sounds[index].volume + rate * Time.deltaTime;
			yield return null;
		}
	}

	public void FadeOutSound(int index, float rate)
	{
		CancelCoroutine(fadingSound);
		fadingSound = StartCoroutine(FadingOutSound(index, rate));
	}

	private IEnumerator FadingOutSound(int index, float rate)
	{
		while (sounds[index].volume > 0f)
		{
			sounds[index].volume = sounds[index].volume - rate * Time.deltaTime;
			yield return null;
		}
	}

	public void ToggleSpriteRenderer(bool toggle)
	{
		spriteRenderer.enabled = toggle;
	}

	public void ToggleTextRenderer(bool toggle)
	{
		meshRenderer.enabled = toggle;
	}

	public void ToggleAnimator(bool toggle)
	{
		animator.enabled = toggle;
	}

	public void ToggleSoundMute(int index, bool toggle)
	{
		sounds[index].mute = toggle;
	}

	public void ToggleIsRealTimeFader(bool toggle)
	{
		isRealTimeFader = toggle;
	}

	public void ToggleSpriteFlip(bool toggle)
	{
		spriteRenderer.flipX = toggle;
	}

	public void PingAnimTrigger(string name)
	{
		animator.SetTrigger(name);
	}

	public void EnableAnimBool(string name)
	{
		animator.SetBool(name, value: true);
	}

	public void DisableAnimBool(string name)
	{
		animator.SetBool(name, value: false);
	}

	public void SetAnimBool(string name, bool toggle)
	{
		animator.SetBool(name, toggle);
	}

	public void SetAnimFloat(string name, float value)
	{
		animator.SetFloat(name, value);
	}

	public void SetSprite(Sprite newSprite)
	{
		spriteRenderer.sprite = newSprite;
	}

	public void SetSpriteColor(Color newColor)
	{
		spriteRenderer.color = newColor;
	}

	public void SetSpriteAlpha(float newAlpha)
	{
		spriteRenderer.enabled = true;
		CancelCoroutine(fadingSprite);
		Color color = spriteRenderer.color;
		color.a = newAlpha;
		spriteRenderer.color = color;
	}

	public void SetTextAlpha(float newAlpha)
	{
		meshRenderer.enabled = true;
		CancelCoroutine(fadingText);
		Color color = meshRenderer.material.color;
		color.a = newAlpha;
		meshRenderer.material.color = color;
	}

	public void SetSpriteMaskInteraction(int interactionType)
	{
		switch (interactionType)
		{
		case 0:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
			break;
		case 1:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case 2:
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		}
	}

	public void SetCurrentAnimSpeed(float value)
	{
		animator.speed = value;
	}

	public void SetSoundAudioClip(int index, AudioClip newAudioClip)
	{
		sounds[index].clip = newAudioClip;
	}

	public void SetSoundVolume(int index, float newVolume)
	{
		sounds[index].volume = newVolume;
	}

	public void SetSoundTime(int index, float newTime)
	{
		sounds[index].time = newTime;
	}

	public void SetSoundPitch(int index, float newPitch)
	{
		sounds[index].pitch = newPitch;
	}

	public void SetAllSoundPitch(float newPitch)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			SetSoundPitch(i, newPitch);
		}
	}

	public void SetSoundIgnoreListenerPause(int index, bool toggle)
	{
		sounds[index].ignoreListenerPause = toggle;
	}

	public void SetAllSoundIgnoreListenerPause(bool toggle)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			SetSoundIgnoreListenerPause(i, toggle);
		}
	}

	public void SetAnimatorUnscaledTime()
	{
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
	}

	public void SetText(string newText)
	{
		textMesh.text = newText;
	}

	public void SetTextColor(Color newColor)
	{
		textMesh.color = newColor;
	}

	public void SetSoundPanning(int index, float value)
	{
		sounds[index].panStereo = value;
	}

	public void DestroyAfterSound(int index)
	{
		StartCoroutine(DestroyingAfterSound(index));
	}

	private IEnumerator DestroyingAfterSound(int index)
	{
		yield return new WaitForSeconds(GetSoundLength(index));
		Object.Destroy(base.gameObject);
	}

	public static void SetAudioSync(float newAudioSync)
	{
		audioSync = newAudioSync;
	}

	public static void SetAudioOffset(float newAudioOffset)
	{
		audioOffset = newAudioOffset;
	}

	public bool CheckIsSpriteRendered()
	{
		return spriteRenderer.enabled;
	}

	public bool CheckIsTextRendered()
	{
		return meshRenderer.enabled;
	}

	public bool CheckIsAnimEnabled()
	{
		return animator.enabled;
	}

	public bool CheckIsSpriteFlipped()
	{
		return spriteRenderer.flipX;
	}

	public Color GetSpriteColor()
	{
		return spriteRenderer.color;
	}

	public float GetSpriteAlpha()
	{
		return spriteRenderer.color.a;
	}

	public bool CheckAnimBool(string name)
	{
		return animator.GetBool(name);
	}

	public float GetSoundVolume(int index)
	{
		return sounds[index].volume;
	}

	public float GetSoundTime(int index)
	{
		float num = sounds[index].clip.frequency;
		return (float)sounds[index].timeSamples / num;
	}

	public float GetSoundPitch(int index)
	{
		return sounds[index].pitch;
	}

	public float GetSoundLength(int index)
	{
		return sounds[index].clip.length / sounds[index].pitch;
	}

	public float GetAnimDuration(string animName)
	{
		AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip animationClip in animationClips)
		{
			if (animationClip.name == animName)
			{
				return animationClip.length;
			}
		}
		return 0f;
	}

	public Animator GetAnimator()
	{
		return animator;
	}

	public bool CheckIsSoundMute(int soundNum)
	{
		return sounds[soundNum].mute;
	}

	public bool CheckIsAnimPlaying(string animName)
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
	}

	public bool CheckIsAnimExists(string animName)
	{
		AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++)
		{
			if (animationClips[i].name == animName)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckIsSoundPlaying(int index)
	{
		return sounds[index].isPlaying;
	}

	public string GetText()
	{
		return textMesh.text;
	}
}
