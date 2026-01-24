using System.Collections;
using UnityEngine;

public class Shuttle : Wrapper
{
	[Header("Fragment")]
	public Fragment shuttle;

	public Fragment smoke;

	public Fragment farShuttle;

	public Fragment flame;

	private float prepDuration;

	private bool isLifted;

	private Coroutine lifting;

	protected override void Awake()
	{
		shuttle.Awake();
		smoke.Awake();
		farShuttle.Awake();
		flame.Awake();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		isLifted = false;
		shuttle.TriggerAnim("idled");
		shuttle.SetSpriteMaskInteraction(1);
		smoke.TriggerAnim("hidden");
		farShuttle.TriggerAnim("hidden");
	}

	public void Hide()
	{
		CancelCoroutine(lifting);
		RenderChildren(toggle: false);
	}

	public void Prep()
	{
		shuttle.TriggerAnim("prepping");
		shuttle.SetSpriteMaskInteraction(1);
		smoke.TriggerAnim("prep", AngrySkies.env.GetSpeed() / prepDuration);
	}

	public void Unprep()
	{
		shuttle.TriggerAnim("idled");
		shuttle.SetSpriteMaskInteraction(1);
		smoke.TriggerAnim("unprep", AngrySkies.env.GetSpeed());
	}

	public void Lift(float accuracy)
	{
		CancelCoroutine(lifting);
		lifting = StartCoroutine(Lifting(accuracy));
	}

	private IEnumerator Lifting(float accuracy)
	{
		isLifted = true;
		flame.TriggerAnim("launch");
		shuttle.TriggerAnim("lift");
		shuttle.SetSpriteMaskInteraction(0);
		smoke.TriggerAnim("hidden");
		float timeStarted = Technician.mgr.GetDspTime();
		float timeDelayed = MusicBox.env.GetSecsPerBeat() - 0.3f;
		if (accuracy == 1f)
		{
			flame.SetSpriteColor(new Color(0.549f, 0.9647f, 1f));
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > timeDelayed);
			farShuttle.TriggerAnim("blast");
		}
		else if (accuracy == 0.332f)
		{
			flame.SetSpriteColor(new Color(1f, 0.9804f, 0.8235f));
		}
		else
		{
			flame.SetSpriteColor(new Color(1f, 0.6745f, 0.8352f));
		}
	}

	public void Reset(int numBeats)
	{
		isLifted = false;
		shuttle.TriggerAnim("reset", AngrySkies.env.GetSpeed() / (float)numBeats);
		shuttle.SetSpriteMaskInteraction(1);
	}

	public void SetPrepDuration(float value)
	{
		prepDuration = value;
	}

	public bool CheckIsLifted()
	{
		return isLifted;
	}
}
