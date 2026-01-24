using System.Collections;
using UnityEngine;

public class UfoMachine : Wrapper
{
	[Header("Children")]
	public Capsule[] Capsules;

	public Claw Claw;

	private bool isPickedUp;

	private int activeCapsuleNum;

	private Coroutine stickingCapsule;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		CancelCoroutine(stickingCapsule);
		RenderChildren(toggle: true);
		sprites[1].TriggerAnim("idled");
		Claw.Show();
		Capsules[0].Hide();
		Capsules[1].Hide();
	}

	public void Hide()
	{
		isPickedUp = false;
		CancelCoroutine(stickingCapsule);
		Capsules[0].Hide();
		Capsules[1].Hide();
		Claw.Hide();
		RenderChildren(toggle: false);
	}

	public void JoystickRight()
	{
		sprites[2].TriggerAnim("right");
	}

	public void JoystickLeft()
	{
		sprites[2].TriggerAnim("left");
	}

	public void Grab()
	{
		sprites[1].TriggerAnim("press");
		Claw.Grab();
	}

	public void Release()
	{
		sprites[1].TriggerAnim("release");
		Claw.Release();
	}

	public void PickUpCapsule()
	{
		isPickedUp = true;
		Capsules[activeCapsuleNum].SetParent(Claw.grabber.transform);
		Capsules[activeCapsuleNum].Show();
	}

	public void DropCapsule()
	{
		isPickedUp = false;
		Capsules[activeCapsuleNum].SetParent(base.transform);
		Capsules[activeCapsuleNum].Drop();
		activeCapsuleNum = ((activeCapsuleNum == 0) ? 1 : 0);
	}

	public void StickCapsule()
	{
		stickingCapsule = StartCoroutine(StickingCapsule());
	}

	private IEnumerator StickingCapsule()
	{
		isPickedUp = false;
		yield return new WaitUntil(() => Claw.GetGrabberY() == 0f);
		Capsules[activeCapsuleNum].SetParent(base.transform);
		Capsules[activeCapsuleNum].Stick();
	}

	public void FlashSlotsDelayed(float delta, int num)
	{
		sprites[3].TriggerAnimDelayedDelta(delta, num.ToString() ?? "");
	}

	public bool CheckIsPickedUp()
	{
		return isPickedUp;
	}
}
