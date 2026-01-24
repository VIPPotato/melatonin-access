using System.Collections;
using UnityEngine;

public class Circle : Wrapper
{
	[Header("Children")]
	public Ring[] Rings;

	public Radial[] Radials;

	[Header("Fragments")]
	public textboxFragment num;

	private bool isSpawnBlind;

	private bool isSpawnHalfDistance;

	private bool isOffCamera;

	private bool isActivated;

	private bool isBobbling;

	private int spawnEaseType;

	private int ringNum;

	private int radialNum;

	private int progressQuarter;

	private float timeTilActivate;

	private Coroutine activating;

	private Coroutine deactivating;

	private Coroutine flashingNumber;

	protected override void Awake()
	{
		SetupFragments();
		num.Initiate();
		timeTilActivate = gears[0].GetAnimDuration("in") / 2f;
		SetProgress(0f);
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		if (!isActivated)
		{
			CancelCoroutine(activating);
			CancelCoroutine(deactivating);
			activating = StartCoroutine(Activating());
		}
	}

	private IEnumerator Activating()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		if (!isOffCamera)
		{
			SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		}
		gears[0].TriggerAnim("in");
		gears[1].TriggerAnim("hidden");
		gears[2].TriggerAnim("in");
		num.ToggleMeshRenderer(toggle: false);
		yield return new WaitForSeconds(timeTilActivate);
		isBobbling = true;
	}

	public void Deactivate()
	{
		if (isActivated)
		{
			CancelCoroutine(activating);
			CancelCoroutine(deactivating);
			deactivating = StartCoroutine(Deactivating());
		}
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		isBobbling = false;
		gears[0].TriggerAnim("out");
		gears[1].TriggerAnim("hidden");
		gears[2].TriggerAnim("out");
		yield return new WaitForSeconds(1f);
		if (!isOffCamera)
		{
			SetParentAndReposition(Interface.env.GetTransform());
		}
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		if (!isOffCamera)
		{
			SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		}
		gears[0].TriggerAnim("in", 1f, 1f);
		gears[1].TriggerAnim("hidden");
		gears[2].TriggerAnim("in", 1f, 1f);
		num.ToggleMeshRenderer(toggle: false);
		isBobbling = true;
	}

	public void Hide()
	{
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		isActivated = false;
		isBobbling = false;
		Ring[] rings = Rings;
		for (int i = 0; i < rings.Length; i++)
		{
			rings[i].Hide();
		}
		Radial[] radials = Radials;
		for (int i = 0; i < radials.Length; i++)
		{
			radials[i].Hide();
		}
		RenderChildren(toggle: false);
	}

	public void Bobble()
	{
		if (isBobbling)
		{
			gears[1].TriggerAnim("bobble");
		}
	}

	public void FlashNumber(float timeStarted, int number, float duration)
	{
		CancelCoroutine(flashingNumber);
		flashingNumber = StartCoroutine(FlashingNumber(timeStarted, number, duration));
	}

	private IEnumerator FlashingNumber(float timeStarted, int number, float duration)
	{
		num.ToggleMeshRenderer(toggle: true);
		num.SetText(number.ToString() ?? "");
		num.SetFontAlpha(1f);
		while (MusicBox.env.GetSongTime() - timeStarted < duration)
		{
			num.SetFontAlpha(Mathf.Lerp(1f, 0f, (MusicBox.env.GetSongTime() - timeStarted) / duration));
			yield return null;
		}
		num.ToggleMeshRenderer(toggle: false);
	}

	public void SetProgress(float songProgress)
	{
		gears[3].SetLocalEulerAngles(0f, 0f, -360f * songProgress);
		if (songProgress == 0f)
		{
			progressQuarter = 0;
			sprites[1].ToggleSpriteRenderer(toggle: false);
			sprites[2].ToggleSpriteRenderer(toggle: false);
			sprites[3].ToggleSpriteRenderer(toggle: false);
			sprites[4].ToggleSpriteRenderer(toggle: false);
			sprites[1].SetSpriteMaskInteraction(0);
			sprites[2].SetSpriteMaskInteraction(0);
			sprites[3].SetSpriteMaskInteraction(0);
			sprites[4].SetSpriteMaskInteraction(0);
		}
		else if (songProgress > 0f && songProgress <= 0.25f && progressQuarter != 1)
		{
			progressQuarter = 1;
			sprites[1].ToggleSpriteRenderer(toggle: true);
			sprites[2].ToggleSpriteRenderer(toggle: false);
			sprites[3].ToggleSpriteRenderer(toggle: false);
			sprites[4].ToggleSpriteRenderer(toggle: false);
			sprites[1].SetSpriteMaskInteraction(2);
			sprites[2].SetSpriteMaskInteraction(0);
			sprites[3].SetSpriteMaskInteraction(0);
			sprites[4].SetSpriteMaskInteraction(0);
		}
		else if (songProgress > 0.25f && songProgress <= 0.5f && progressQuarter != 2)
		{
			progressQuarter = 2;
			sprites[1].ToggleSpriteRenderer(toggle: true);
			sprites[2].ToggleSpriteRenderer(toggle: true);
			sprites[3].ToggleSpriteRenderer(toggle: false);
			sprites[4].ToggleSpriteRenderer(toggle: false);
			sprites[1].SetSpriteMaskInteraction(0);
			sprites[2].SetSpriteMaskInteraction(2);
			sprites[3].SetSpriteMaskInteraction(0);
			sprites[4].SetSpriteMaskInteraction(0);
		}
		else if (songProgress > 0.5f && songProgress <= 0.75f && progressQuarter != 3)
		{
			progressQuarter = 3;
			sprites[1].ToggleSpriteRenderer(toggle: true);
			sprites[2].ToggleSpriteRenderer(toggle: true);
			sprites[3].ToggleSpriteRenderer(toggle: true);
			sprites[4].ToggleSpriteRenderer(toggle: false);
			sprites[1].SetSpriteMaskInteraction(0);
			sprites[2].SetSpriteMaskInteraction(0);
			sprites[3].SetSpriteMaskInteraction(2);
			sprites[4].SetSpriteMaskInteraction(0);
		}
		else if (songProgress > 0.75f && progressQuarter != 4)
		{
			progressQuarter = 4;
			sprites[1].ToggleSpriteRenderer(toggle: true);
			sprites[2].ToggleSpriteRenderer(toggle: true);
			sprites[3].ToggleSpriteRenderer(toggle: true);
			sprites[4].ToggleSpriteRenderer(toggle: true);
			sprites[1].SetSpriteMaskInteraction(0);
			sprites[2].SetSpriteMaskInteraction(0);
			sprites[3].SetSpriteMaskInteraction(0);
			sprites[4].SetSpriteMaskInteraction(2);
		}
	}

	public void Hit(float accuracy)
	{
		if (accuracy != 1f)
		{
			if (accuracy != 0.332f)
			{
				if (accuracy == 0.333f)
				{
					sprites[0].TriggerAnim("pingRed");
				}
			}
			else
			{
				sprites[0].TriggerAnim("pingYellow");
			}
		}
		else
		{
			sprites[0].TriggerAnim("pingBlue");
		}
	}

	public void Strike()
	{
		sprites[0].TriggerAnim("pingRed");
	}

	public void SetSpawnEaseType(int newSpawnEaseType)
	{
		spawnEaseType = newSpawnEaseType;
	}

	public void ToggleisSpawnBlind(bool toggle)
	{
		isSpawnBlind = toggle;
	}

	public void ToggleisSpawnHalfDistance(bool toggle)
	{
		isSpawnHalfDistance = toggle;
	}

	public void ToggleIsOffCamera(bool toggle)
	{
		isOffCamera = toggle;
	}

	public void ResetRingNum()
	{
		ringNum = 0;
	}

	private void IncreaseRingNum()
	{
		ringNum = ((ringNum + 1 < Rings.Length) ? (ringNum + 1) : 0);
	}

	private void IncreaseRadialNum()
	{
		radialNum = ((radialNum + 1 < Radials.Length) ? (radialNum + 1) : 0);
	}

	public Ring GetRing()
	{
		IncreaseRingNum();
		Rings[ringNum].ToggleIsBlind(isSpawnBlind);
		Rings[ringNum].ToggleIsHalfDistance(isSpawnHalfDistance);
		Rings[ringNum].SetEaseType(spawnEaseType);
		return Rings[ringNum];
	}

	public Radial GetRadial()
	{
		IncreaseRadialNum();
		Radials[radialNum].ToggleIsBlind(isSpawnBlind);
		Radials[radialNum].ToggleIsHalfDistance(isSpawnHalfDistance);
		return Radials[radialNum];
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
