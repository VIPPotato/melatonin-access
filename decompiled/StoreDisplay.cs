using System.Collections;
using UnityEngine;

public class StoreDisplay : Wrapper
{
	[Header("Children")]
	public LuxuryItem[] LuxuryItems;

	public Underlight[] Underlights;

	private int luxuryItemNum = 4;

	private bool isDropping1;

	private bool isDropping2;

	private bool isSecondDisplayActive;

	private bool isSlid;

	private Coroutine dropping;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isDropping1 = false;
		isDropping2 = false;
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim("shownAtTop");
		sprites[1].TriggerAnim("hidden");
		sprites[2].TriggerAnim("shown");
		Underlight[] underlights = Underlights;
		for (int i = 0; i < underlights.Length; i++)
		{
			underlights[i].Show();
		}
		LuxuryItem[] luxuryItems = LuxuryItems;
		for (int i = 0; i < luxuryItems.Length; i++)
		{
			luxuryItems[i].Hide();
		}
	}

	public void Hide()
	{
		CancelCoroutine(dropping);
		RenderChildren(toggle: false);
	}

	public void Slide(float timeStarted)
	{
		isSlid = true;
		gears[0].TriggerAnimDelayedTimeStarted(timeStarted, "slide", Mall.env.GetSpeed());
		sprites[1].TriggerAnimDelayedTimeStarted(timeStarted, "showAtBottom", Mall.env.GetSpeed());
		sprites[2].TriggerAnimDelayedTimeStarted(timeStarted, "hide", Mall.env.GetSpeed());
	}

	public void Drop(float timeStarted)
	{
		CancelCoroutine(dropping);
		dropping = StartCoroutine(Dropping(timeStarted));
	}

	private IEnumerator Dropping(float timeStarted)
	{
		isDropping1 = true;
		isDropping2 = false;
		isSecondDisplayActive = false;
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 3.9f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isDropping1 = false;
		if (isSlid)
		{
			isDropping2 = true;
			checkpoint += MusicBox.env.GetSecsPerBeat() * 4f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			isDropping2 = false;
			checkpoint += MusicBox.env.GetSecsPerBeat() * 4f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			isSecondDisplayActive = true;
		}
	}

	public void Clear()
	{
		LuxuryItem[] luxuryItems = LuxuryItems;
		for (int i = 0; i < luxuryItems.Length; i++)
		{
			luxuryItems[i].Deactivate();
		}
	}

	public void PresentSoundDelayed(float timeStarted)
	{
		if (SaveManager.mgr.GetScreenshake() > 0)
		{
			gears[1].TriggerAnim("breeze");
		}
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
	}

	public void IncreaseLuxuryItemNum()
	{
		luxuryItemNum++;
		if (luxuryItemNum > 5)
		{
			luxuryItemNum = 1;
		}
	}

	public int GetLuxuryItemNum()
	{
		return luxuryItemNum;
	}

	public bool CheckIsDropping()
	{
		if (isDropping1 || isDropping2)
		{
			return true;
		}
		return false;
	}

	public bool CheckIsDropping2()
	{
		return isDropping2;
	}

	public bool CheckIsSlid()
	{
		return isSlid;
	}

	public LuxuryItem GetActiveItem()
	{
		if (Dream.dir.CheckIsFullBeat())
		{
			if (CheckIsSlid() && isSecondDisplayActive)
			{
				return LuxuryItems[(Dream.dir.GetBeat() - 1) * 2 + 8];
			}
			return LuxuryItems[(Dream.dir.GetBeat() - 1) * 2];
		}
		if (CheckIsSlid() && isSecondDisplayActive)
		{
			return LuxuryItems[(Dream.dir.GetBeat() - 1) * 2 + 1 + 8];
		}
		return LuxuryItems[(Dream.dir.GetBeat() - 1) * 2 + 1];
	}
}
