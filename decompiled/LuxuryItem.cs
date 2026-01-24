using System.Collections;
using UnityEngine;

public class LuxuryItem : Wrapper
{
	private float fadeOutduration;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		fadeOutduration = gears[0].GetAnimDuration("fadeOut");
		RenderChildren(toggle: false);
	}

	public void Activate(int itemNum = 0)
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		gears[0].TriggerAnim("fadeIn", Mall.env.GetSpeed());
		if (itemNum == 0)
		{
			itemNum = Mall.env.StoreDisplay.GetLuxuryItemNum();
			if (!Dream.dir.CheckIsFullBeat() && itemNum == 5)
			{
				itemNum = 1;
			}
		}
		sprites[0].TriggerAnim(itemNum.ToString() ?? "");
		sprites[1].TriggerAnim(itemNum.ToString() ?? "");
		sprites[2].TriggerAnim("face" + Random.Range(1, 4));
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
		if (gears[0].CheckIsAnimPlaying("fadeIn"))
		{
			gears[0].TriggerAnim("fadeOut");
		}
		if (gears[1].CheckIsAnimPlaying("fadeIn"))
		{
			gears[1].TriggerAnim("fadeOut");
		}
		yield return new WaitForSeconds(fadeOutduration);
		RenderChildren(toggle: false);
	}

	public void BagUp()
	{
		gears[0].TriggerAnim("fadeOut", Mall.env.GetSpeed());
		gears[1].TriggerAnim("fadeIn", Mall.env.GetSpeed());
	}

	public void AttractFeedback()
	{
		if (DreamWorld.env.GetActiveFeedback() != null)
		{
			DreamWorld.env.GetActiveFeedback().Hide();
			DreamWorld.env.GetActiveFeedback().SetPosition(GetX(), GetY() + 1.33f);
		}
		Mall.env.Sweat.Hide();
		Mall.env.Sweat.SetPosition(GetX() - 0.5f, GetY() - 0.5f);
	}
}
