using System.Collections;
using UnityEngine;

public class WipeScreen : Wrapper
{
	[Header("Fragments")]
	public Fragment speeder;

	public Fragment slower;

	public Fragment speederLocalized;

	public Fragment slowerLocalized;

	public textboxFragment[] localizedTextboxes;

	private int timesActivated;

	private Coroutine crossing;

	protected override void Awake()
	{
		speeder.Awake();
		slower.Awake();
		speederLocalized.Awake();
		slowerLocalized.Awake();
		textboxFragment[] array = localizedTextboxes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		RenderChildren(toggle: false);
	}

	public void CrossInWithSpeedUp()
	{
		CancelCoroutine(crossing);
		crossing = StartCoroutine(CrossingInWithSpeedUp());
	}

	private IEnumerator CrossingInWithSpeedUp()
	{
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetOuterTransform());
		if (SaveManager.GetLang() != 0)
		{
			speederLocalized.TriggerAnim("crossIn");
		}
		else
		{
			speeder.TriggerAnim("crossIn");
		}
		yield return new WaitForSeconds(2f);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}

	public void CrossInWithSlowDown()
	{
		CancelCoroutine(crossing);
		crossing = StartCoroutine(CrossingInWithSlowDown());
	}

	private IEnumerator CrossingInWithSlowDown()
	{
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetOuterTransform());
		if (SaveManager.GetLang() != 0)
		{
			slowerLocalized.TriggerAnim("crossIn");
		}
		else
		{
			slower.TriggerAnim("crossIn");
		}
		yield return new WaitForSeconds(2f);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false);
	}
}
