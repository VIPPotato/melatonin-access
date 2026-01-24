using System.Collections;
using UnityEngine;

public class Food : Wrapper
{
	[Header("Fragments")]
	public Fragment thrower;

	public Fragment spawner;

	public Fragment item;

	private Coroutine deactivating;

	protected override void Awake()
	{
		thrower.Awake();
		spawner.Awake();
		item.Awake();
		RenderChildren(toggle: false);
	}

	public void CrossIn(int num, string foodType)
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(CrossingIn(num, foodType));
	}

	private IEnumerator CrossingIn(int num, string foodType)
	{
		RenderChildren(toggle: true);
		float speed = FoodySkies.env.GetSpeed();
		item.TriggerAnim(foodType);
		thrower.TriggerAnim("throw" + num, speed);
		if (num != 1)
		{
			spawner.TriggerAnim("spawn");
		}
		else
		{
			spawner.TriggerAnim("spawned");
		}
		yield return new WaitForSeconds(thrower.GetAnimDuration("throw" + num) / speed);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		thrower.SetCurrentAnimSpeed(FoodySkies.env.GetSpeed() * 0.25f);
		spawner.TriggerAnim("despawn");
		yield return new WaitForSeconds(spawner.GetAnimDuration("despawn"));
		RenderChildren(toggle: false);
	}
}
