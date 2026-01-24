using System.Collections;
using UnityEngine;

public class ModeMenu : Wrapper
{
	[Header("Children")]
	public Wave[] Waves;

	[Header("Fragments")]
	public Fragment activator;

	public Fragment bubble;

	public Fragment prompt;

	public Fragment starDisplay;

	public Fragment ringDisplay;

	public Fragment[] padlocks;

	public Fragment wavesGroup;

	public textboxFragment[] modeLabels;

	private int activeItemNum;

	private int starScore;

	private int ringScore;

	private bool isRemix;

	private bool isActivated;

	private bool isTransitioned;

	private string dreamName;

	private float initWavesGroupLocalY;

	private Coroutine deactivating;

	protected override void Awake()
	{
		activator.Awake();
		bubble.Awake();
		prompt.Awake();
		starDisplay.Awake();
		ringDisplay.Awake();
		Fragment[] array = padlocks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		textboxFragment[] array2 = modeLabels;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Initiate();
		}
		wavesGroup.Awake();
		initWavesGroupLocalY = wavesGroup.GetLocalY();
		RenderChildren(toggle: false);
	}

	public void Activate(bool newIsRemix)
	{
		isActivated = true;
		isTransitioned = false;
		isRemix = newIsRemix;
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		activator.TriggerAnim("activate");
		if (isRemix)
		{
			bubble.TriggerAnim("shrankForShort");
		}
		else
		{
			bubble.TriggerAnim("shrankForTall");
		}
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadA");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadCROSS");
		}
		else
		{
			prompt.TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		}
		textboxFragment[] array = modeLabels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFontAlpha(0f);
		}
		if (SaveManager.GetLang() == 5)
		{
			modeLabels[3].SetFontSize(2.7f);
		}
	}

	public void Deactivate(int type)
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating(type));
	}

	private IEnumerator Deactivating(int type)
	{
		isActivated = false;
		if (type == 1)
		{
			activator.TriggerAnim("floatAway");
			for (int i = 0; i < 4; i++)
			{
				if (i == activeItemNum)
				{
					modeLabels[i].FadeOutText(1f, 0.2f);
				}
				else
				{
					modeLabels[i].FadeOutText(0.67f, 0.2f);
				}
			}
			if (isRemix)
			{
				bubble.TriggerAnim("shrinkFromShort");
				modeLabels[0].SetFontAlpha(0f);
				modeLabels[3].SetFontAlpha(0f);
			}
			else
			{
				bubble.TriggerAnim("shrinkFromTall");
			}
			Waves[0].Deactivate();
			Waves[1].Deactivate();
			yield return new WaitForSeconds(activator.GetAnimDuration("floatAway"));
		}
		else
		{
			activator.TriggerAnim("deactivate");
			yield return new WaitForSeconds(activator.GetAnimDuration("deactivate"));
		}
		RenderChildren(toggle: false);
	}

	public void Transition(string newDreamName, int newStarScore, int newRingScore)
	{
		StartCoroutine(Transitioning(newDreamName, newStarScore, newRingScore));
	}

	private IEnumerator Transitioning(string newDreamName, int newStarScore, int newRingScore)
	{
		dreamName = newDreamName;
		starScore = newStarScore;
		ringScore = newRingScore;
		activator.TriggerAnim("transition");
		if (isRemix)
		{
			bubble.TriggerAnim("growToShort");
		}
		else
		{
			bubble.TriggerAnim("growToTall");
		}
		if (isRemix)
		{
			activeItemNum = 1;
			modeLabels[0].SetFontAlpha(0f);
		}
		else
		{
			activeItemNum = 0;
			modeLabels[0].FadeInText(1f, 0.25f);
		}
		if (starScore > 0)
		{
			starDisplay.ToggleSpriteRenderer(toggle: true);
			starDisplay.TriggerAnim(starScore.ToString() ?? "");
			padlocks[0].ToggleSpriteRenderer(toggle: false);
			modeLabels[1].FadeInText(0.67f, 0.25f);
		}
		else if (isRemix)
		{
			starDisplay.ToggleSpriteRenderer(toggle: true);
			starDisplay.TriggerAnim(starScore.ToString() ?? "");
			padlocks[0].ToggleSpriteRenderer(toggle: false);
			modeLabels[1].FadeInText(1f, 0.25f);
		}
		else
		{
			starDisplay.ToggleSpriteRenderer(toggle: false);
			padlocks[0].ToggleSpriteRenderer(toggle: true);
			modeLabels[1].FadeInText(0.67f, 0.25f);
		}
		modeLabels[2].FadeInText(0.67f, 0.25f);
		if (starScore >= 2)
		{
			padlocks[1].ToggleSpriteRenderer(toggle: false);
			ringDisplay.ToggleSpriteRenderer(toggle: true);
			ringDisplay.TriggerAnim(ringScore.ToString() ?? "");
		}
		else
		{
			padlocks[1].ToggleSpriteRenderer(toggle: true);
			ringDisplay.ToggleSpriteRenderer(toggle: false);
		}
		if (starScore >= 2 && Builder.mgr.CheckIsFullGame())
		{
			modeLabels[3].FadeInText(0.67f, 0.25f);
			padlocks[2].ToggleSpriteRenderer(toggle: false);
		}
		else
		{
			modeLabels[3].FadeInText(0.67f, 0.25f);
			padlocks[2].ToggleSpriteRenderer(toggle: true);
		}
		wavesGroup.SetLocalY(initWavesGroupLocalY - 0.72f * (float)activeItemNum);
		Waves[0].Activate();
		Waves[1].Activate();
		Interface.env.Disable();
		yield return null;
		isTransitioned = true;
	}

	public void Cancel()
	{
		StartCoroutine(Canceling());
	}

	private IEnumerator Canceling()
	{
		isTransitioned = false;
		activator.TriggerAnim("cancel");
		if (isRemix)
		{
			bubble.TriggerAnim("shrinkFromShort");
		}
		else
		{
			bubble.TriggerAnim("shrinkFromTall");
		}
		Waves[0].Deactivate();
		Waves[1].Deactivate();
		for (int i = 0; i < 4; i++)
		{
			if (i == activeItemNum)
			{
				modeLabels[i].FadeOutText(1f, 0.2f);
			}
			else
			{
				modeLabels[i].FadeOutText(0.67f, 0.2f);
			}
		}
		if (isRemix)
		{
			modeLabels[0].SetFontAlpha(0f);
		}
		yield return null;
		Interface.env.Enable();
	}

	public void NavigateDown()
	{
		activeItemNum++;
		if (activeItemNum > 3)
		{
			if (isRemix)
			{
				activeItemNum = 1;
			}
			else
			{
				activeItemNum = 0;
			}
			Map.env.PlayToggleSoundAlt();
		}
		else
		{
			Map.env.PlayToggleSound();
		}
		wavesGroup.SetLocalY(initWavesGroupLocalY - 0.72f * (float)activeItemNum);
		for (int i = 0; i < 4; i++)
		{
			if (i == activeItemNum)
			{
				modeLabels[i].SetFontAlpha(1f);
			}
			else
			{
				modeLabels[i].SetFontAlpha(0.67f);
			}
		}
		if (isRemix)
		{
			modeLabels[0].SetFontAlpha(0f);
		}
	}

	public void NavigateUp()
	{
		activeItemNum--;
		if (activeItemNum < 1 && isRemix)
		{
			activeItemNum = 3;
			Map.env.PlayToggleSoundAlt();
		}
		else if (activeItemNum < 0)
		{
			activeItemNum = 3;
			Map.env.PlayToggleSoundAlt();
		}
		else
		{
			Map.env.PlayToggleSound();
		}
		wavesGroup.SetLocalY(initWavesGroupLocalY - 0.72f * (float)activeItemNum);
		for (int i = 0; i < 4; i++)
		{
			if (i == activeItemNum)
			{
				modeLabels[i].SetFontAlpha(1f);
			}
			else
			{
				modeLabels[i].SetFontAlpha(0.67f);
			}
		}
		if (isRemix)
		{
			modeLabels[0].SetFontAlpha(0f);
		}
	}

	public int GetActiveItemNum()
	{
		return activeItemNum;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsTranstioned()
	{
		return isTransitioned;
	}
}
