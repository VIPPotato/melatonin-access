using System.Collections;
using UnityEngine;

public class Cam : Wrapper
{
	private bool isGlowable = true;

	private float unslideLeftDuration;

	private float unslideUpDuration;

	private float shakeDuration;

	private float swayDuration;

	private float breezeDuration;

	private Coroutine sliding;

	private Coroutine shaking;

	protected override void Awake()
	{
		SetupFragments();
		unslideLeftDuration = gears[0].GetAnimDuration("resetRight");
		unslideUpDuration = gears[0].GetAnimDuration("resetDown");
		shakeDuration = gears[1].GetAnimDuration("shake1");
		swayDuration = gears[1].GetAnimDuration("sway1");
		breezeDuration = gears[1].GetAnimDuration("breeze1");
		RenderChildren(toggle: true);
	}

	private void OnDrawGizmos()
	{
		float num = Camera.main.orthographicSize * 2f;
		Gizmos.color = new Color(0.7058824f, 1f, 0.8392157f);
		Gizmos.DrawWireCube(base.transform.position, new Vector3(num * Camera.main.aspect, num, 0f));
	}

	public void Glow(float accuracy)
	{
		if (!isGlowable)
		{
			return;
		}
		if (accuracy != 1f)
		{
			if (accuracy != 0.332f)
			{
				if (accuracy == 0.333f)
				{
					sprites[0].TriggerAnim("red");
				}
			}
			else
			{
				sprites[0].TriggerAnim("yellow");
			}
		}
		else
		{
			sprites[0].TriggerAnim("blue");
		}
	}

	public void SlideCameraLeft()
	{
		CancelCoroutine(sliding);
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("slideLeft");
	}

	public void UnslideCameraLeft()
	{
		CancelCoroutine(sliding);
		sliding = StartCoroutine(UnslidingCameraLeft());
	}

	private IEnumerator UnslidingCameraLeft()
	{
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("resetRight");
		yield return new WaitForSecondsRealtime(unslideLeftDuration);
		ResetGear(0);
	}

	public void SlideCameraUp()
	{
		CancelCoroutine(sliding);
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("slideUp");
	}

	public void UnslideCameraUp()
	{
		CancelCoroutine(sliding);
		sliding = StartCoroutine(UnslidingCameraUp());
	}

	private IEnumerator UnslidingCameraUp()
	{
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("resetDown");
		yield return new WaitForSecondsRealtime(unslideUpDuration);
		ResetGear(0);
	}

	public void Shake(float multiplier = 1f, bool isDelayed = false)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(Shaking(multiplier, isDelayed));
		}
	}

	private IEnumerator Shaking(float multiplier, bool isDelayed)
	{
		if (isDelayed)
		{
			float timeStarted = Technician.mgr.GetDspTime();
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > 0.11667f);
		}
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		if (SaveManager.mgr.GetScreenshake() == 2)
		{
			gears[1].TriggerAnim("shake" + num, 1f * multiplier);
		}
		else
		{
			gears[1].TriggerAnim("sway" + num, 1.05f * multiplier);
		}
		yield return new WaitForSeconds(shakeDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void DelayShake(float timeStarted, float multiplier = 1f)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(DelayingShake(timeStarted, multiplier));
		}
	}

	private IEnumerator DelayingShake(float timeStarted, float multiplier)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		if (SaveManager.mgr.GetScreenshake() == 2)
		{
			gears[1].TriggerAnim("shake" + num, 1f * multiplier);
		}
		else
		{
			gears[1].TriggerAnim("sway" + num, 1.05f * multiplier);
		}
		yield return new WaitForSeconds(shakeDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void Sway(float multiplier = 1f)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(Swaying(multiplier));
		}
	}

	private IEnumerator Swaying(float multiplier)
	{
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		if (SaveManager.mgr.GetScreenshake() == 2)
		{
			gears[1].TriggerAnim("sway" + num, 1.05f * multiplier);
		}
		else
		{
			gears[1].TriggerAnim("breeze" + num, 1.1f * multiplier);
		}
		yield return new WaitForSeconds(swayDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void DelaySway(float timeStarted, float multiplier = 1f)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(DelayingSway(timeStarted, multiplier));
		}
	}

	private IEnumerator DelayingSway(float timeStarted, float multiplier)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		if (SaveManager.mgr.GetScreenshake() == 2)
		{
			gears[1].TriggerAnim("sway" + num, 1.05f * multiplier);
		}
		else
		{
			gears[1].TriggerAnim("breeze" + num, 1.1f * multiplier);
		}
		yield return new WaitForSeconds(swayDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void Breeze(float multiplier = 1f)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(Breezing(multiplier));
		}
	}

	private IEnumerator Breezing(float multiplier)
	{
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		gears[1].TriggerAnim("breeze" + num, 1.1f * multiplier);
		yield return new WaitForSeconds(breezeDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void DelayBreeze(float timeStarted, float multiplier = 1f)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			CancelCoroutine(shaking);
			shaking = StartCoroutine(DelayingBreeze(timeStarted, multiplier));
		}
	}

	private IEnumerator DelayingBreeze(float timeStarted, float multiplier)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(0f);
		}
		gears[1].ToggleAnimator(toggle: true);
		int num = Random.Range(1, 3);
		gears[1].TriggerAnim("breeze" + num, 1.1f * multiplier);
		yield return new WaitForSeconds(breezeDuration / multiplier);
		if (gears[2].CheckIsAnimEnabled())
		{
			gears[2].SetCurrentAnimSpeed(1f);
		}
		ResetGear(1);
	}

	public void Drift(int intensity)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			gears[2].ToggleAnimator(toggle: true);
			gears[2].TriggerAnim("drifting" + intensity);
		}
	}

	private void ResetGear(int gearNum)
	{
		gears[gearNum].ToggleAnimator(toggle: false);
		gears[gearNum].SetLocalPosition(0f, 0f);
		gears[gearNum].SetLocalEulerAngles(0f, 0f, 0f);
	}

	public void ToggleIsGlowable(bool toggle)
	{
		isGlowable = toggle;
	}

	public Fragment GetShaker()
	{
		return gears[1];
	}

	public Transform GetInnerTransform()
	{
		return gears[2].transform;
	}

	public Transform GetOuterTransform()
	{
		return base.transform;
	}

	public Transform GetSliderTransform()
	{
		return gears[0].transform;
	}
}
