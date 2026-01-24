using System.Collections;
using UnityEngine;

public class SpaceOppWrapper : Wrapper
{
	private string monsterType;

	private float shotDuration;

	private float attackDuration;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		shotDuration = sprites[0].GetAnimDuration("zombieShot");
		attackDuration = gears[0].GetAnimDuration("fadeOut");
		RenderChildren(toggle: false, 1);
	}

	public void Activate(int monsterNum)
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("fadeIn", MechSpace.env.GetSpeed());
		if (monsterNum == 0)
		{
			int num = Random.Range(0, 2);
			monsterNum = ((!Dream.dir.CheckIsFullBeat()) ? ((num == 0) ? 3 : 4) : ((num == 0) ? 1 : 2));
		}
		switch (monsterNum)
		{
		case 1:
			monsterType = "mech";
			break;
		case 2:
			monsterType = "ufo";
			break;
		case 3:
			monsterType = "zombie";
			break;
		default:
			monsterType = "robot";
			break;
		}
		sprites[0].TriggerAnim(monsterType);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: false);
	}

	public void Die()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Dying());
	}

	private IEnumerator Dying()
	{
		gears[0].ToggleAnimator(toggle: false);
		sprites[0].SetSpriteAlpha(1f);
		sprites[0].TriggerAnim(monsterType + "Shot");
		yield return new WaitForSeconds(shotDuration);
		RenderChildren(toggle: false);
	}

	public void AttackDelayed(float timeStarted)
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(AttackingDelayed(timeStarted));
	}

	private IEnumerator AttackingDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		gears[0].TriggerAnim("fadeOut");
		yield return new WaitForSeconds(attackDuration);
		RenderChildren(toggle: false);
	}

	public void AttractFeedback()
	{
		if (DreamWorld.env.GetActiveFeedback() != null)
		{
			DreamWorld.env.GetActiveFeedback().Hide();
			DreamWorld.env.GetActiveFeedback().SetPosition(GetX(), GetY());
		}
	}
}
