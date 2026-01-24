using System.Collections;
using UnityEngine;

public class WaterLevel : Wrapper
{
	[Header("Fragments")]
	public Fragment line;

	public Fragment water;

	private int positionNum;

	private const float startLocalY = -0.265f;

	private const float maxY = 1.211f;

	private float offset;

	private Coroutine linearRising;

	protected override void Awake()
	{
		line.Awake();
		water.Awake();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		SetLocalY(-0.265f);
		RenderChildren(toggle: true);
		line.ToggleSpriteRenderer(toggle: false);
		water.ToggleSpriteRenderer(toggle: false);
	}

	public void Hide()
	{
		positionNum = 0;
		offset = 0f;
		CancelCoroutine(linearRising);
		RenderChildren(toggle: false);
	}

	public void RiseSegmented(float accuracy)
	{
		line.ToggleSpriteRenderer(toggle: true);
		water.ToggleSpriteRenderer(toggle: true);
		positionNum++;
		if (accuracy == 1f)
		{
			water.TriggerAnim("perfect");
		}
		else if (accuracy == 0.332f)
		{
			offset -= 0.2f;
			water.TriggerAnim("early");
		}
		else
		{
			offset -= 0.2f;
			water.TriggerAnim("late");
		}
		MoveToLocalTarget(new Vector3(0f, (float)positionNum * 0.40366665f + offset, 0f), 5f, isEasingIn: false);
	}

	public void LinearRise(float timeStarted)
	{
		CancelCoroutine(linearRising);
		linearRising = StartCoroutine(LinearRising(timeStarted));
	}

	private IEnumerator LinearRising(float timeStarted)
	{
		Vector3 startLocalPosition = new Vector3(0f, -0.265f, GetLocalZ());
		Vector3 endLocalPosition = new Vector3(0f, 1.211f, GetLocalZ());
		float duration = MusicBox.env.GetSecsPerBeat();
		while (MusicBox.env.GetSongTime() - timeStarted < duration)
		{
			base.transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (MusicBox.env.GetSongTime() - timeStarted) / duration);
			yield return null;
		}
	}

	public void RevealRise()
	{
		line.ToggleSpriteRenderer(toggle: true);
		water.ToggleSpriteRenderer(toggle: true);
	}

	public void PauseLinearRise(float accuracy)
	{
		CancelCoroutine(linearRising);
		if (accuracy == 1f)
		{
			water.TriggerAnim("perfect");
		}
		else if (accuracy == 0.332f)
		{
			offset -= 0.2f;
			water.TriggerAnim("early");
		}
		else
		{
			offset -= 0.2f;
			water.TriggerAnim("late");
		}
	}

	public void MaxLinearRise()
	{
		water.TriggerAnim("flash");
		CancelCoroutine(linearRising);
		SetLocalY(1.211f);
	}
}
