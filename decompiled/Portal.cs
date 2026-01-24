using System.Collections;
using UnityEngine;

public class Portal : Wrapper
{
	[Header("Fragments")]
	public Fragment sprite;

	private bool isWarm;

	private float timeTilClose;

	private Coroutine deactivating;

	protected override void Awake()
	{
		sprite.Awake();
		timeTilClose = sprite.GetAnimDuration("close");
		RenderChildren(toggle: false);
	}

	public void Activate(int positionNum, bool newIsWarm)
	{
		isWarm = newIsWarm;
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		switch (positionNum)
		{
		case 0:
			sprite.ToggleSpriteFlip(toggle: true);
			SetLocalPosition(4f, -0.8f);
			SetLocalEulerAngles(0f, 0f, 25f);
			break;
		case 1:
			sprite.ToggleSpriteFlip(toggle: false);
			SetLocalPosition(11f, -0.65f);
			SetLocalEulerAngles(0f, 0f, -14f);
			break;
		case 2:
			sprite.ToggleSpriteFlip(toggle: true);
			SetLocalPosition(-4.494f, -0.3f);
			SetLocalEulerAngles(0f, 0f, 14f);
			break;
		case 3:
			sprite.ToggleSpriteFlip(toggle: false);
			SetLocalPosition(9.4f, -0.3f);
			SetLocalEulerAngles(0f, 0f, -14f);
			break;
		case 4:
			sprite.ToggleSpriteFlip(toggle: true);
			SetLocalPosition(-4.7f, -0.4f);
			SetLocalEulerAngles(0f, 0f, 0f);
			break;
		case 5:
			sprite.ToggleSpriteFlip(toggle: false);
			SetLocalPosition(11.6f, -0.4f);
			SetLocalEulerAngles(0f, 0f, 0f);
			break;
		case 6:
			sprite.ToggleSpriteFlip(toggle: false);
			SetLocalPosition(1.57f, 2.23f);
			SetLocalEulerAngles(0f, 0f, -25f);
			break;
		case 7:
			sprite.ToggleSpriteFlip(toggle: false);
			SetLocalPosition(9.93f, 2.2f);
			SetLocalEulerAngles(0f, 0f, -25f);
			break;
		}
		if (isWarm)
		{
			sprite.TriggerAnim("openWarm");
		}
		else
		{
			sprite.TriggerAnim("open");
		}
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		if (isWarm)
		{
			sprite.TriggerAnim("closeWarm");
		}
		else
		{
			sprite.TriggerAnim("close");
		}
		yield return new WaitForSeconds(timeTilClose);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void ToggleIsWarm(bool newIsWarm)
	{
		if (isWarm != newIsWarm)
		{
			isWarm = !isWarm;
			if (isWarm)
			{
				sprite.TriggerAnim("loopingWarm");
			}
			else
			{
				sprite.TriggerAnim("looping");
			}
		}
	}
}
