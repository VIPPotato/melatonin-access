using System.Collections;
using UnityEngine;

public class LavaPool : Wrapper
{
	[Header("Props")]
	public Transform cornerTile;

	private bool isLunging;

	private float groupInitX;

	private float groupInitLocalX;

	private float initLocalY;

	private float retileDistanceX;

	private float retileAmountX;

	private float activeY;

	private float nextY;

	private int activeBubbles;

	private Coroutine linearRising;

	private Coroutine lunging;

	protected override void Awake()
	{
		SetupFragments();
		groupInitX = GetX();
		groupInitLocalX = GetLocalX();
		retileDistanceX = Mathf.Abs(cornerTile.localPosition.x / 2f);
		initLocalY = GetLocalY();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		activeY = initLocalY;
		nextY = initLocalY;
		SetLocalY(initLocalY);
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (Interface.env.Cam.GetX() < gears[0].GetX() - retileDistanceX)
		{
			retileAmountX -= retileDistanceX * 2f;
		}
		else if (Interface.env.Cam.GetX() > gears[0].GetX() + retileDistanceX)
		{
			retileAmountX += retileDistanceX * 2f;
		}
		if (Interface.env.Cam.GetShaker().GetX() != 0f || Interface.env.Cam.GetShaker().GetY() != 0f)
		{
			gears[0].SetX(retileAmountX + groupInitX);
		}
		else
		{
			gears[0].SetLocalX(groupInitX);
		}
	}

	public void LinearRise(float delta, float distance)
	{
		CancelCoroutine(linearRising);
		SetLocalY(nextY);
		linearRising = StartCoroutine(LinearRising(delta, distance));
	}

	private IEnumerator LinearRising(float delta, float distance)
	{
		activeY = GetLocalY();
		nextY = GetLocalY() + distance;
		float timeStarted = MusicBox.env.GetSongTime() - delta;
		float duration = MusicBox.env.GetSecsPerBeat();
		while (GetLocalY() < nextY)
		{
			base.transform.localPosition = Vector3.Lerp(new Vector3(0f, activeY, GetLocalZ()), new Vector3(0f, nextY, GetLocalZ()), (MusicBox.env.GetSongTime() - timeStarted) / duration);
			yield return null;
		}
	}

	public void DipDelayed(float timeStarted)
	{
		if (!isLunging)
		{
			gears[1].TriggerAnimDelayedTimeStarted(timeStarted, "dip", Underworld.env.GetSpeed());
		}
	}

	public void DipLowDelayed(float timeStarted)
	{
		if (!isLunging)
		{
			gears[1].TriggerAnimDelayedTimeStarted(timeStarted, "dipLow", Underworld.env.GetSpeed());
		}
	}

	public void LungeDelayed(float timeStarted)
	{
		CancelCoroutine(lunging);
		lunging = StartCoroutine(LungingDelayed(timeStarted));
	}

	private IEnumerator LungingDelayed(float timeStarted)
	{
		isLunging = true;
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		gears[1].TriggerAnim("lunge", Underworld.env.GetSpeed());
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isLunging = false;
	}

	public void PopBubble()
	{
		activeBubbles++;
		if (activeBubbles > 1)
		{
			activeBubbles = 0;
		}
		float num = Random.Range(0.8f, 1.2f);
		float speed = Random.Range(0.8f, 1.8f);
		float localX = Random.Range(-11f, 11f);
		bool toggle = Random.Range(0, 2) == 0;
		if (activeBubbles == 1)
		{
			sprites[0].SetLocalScale(num, num);
			sprites[1].SetLocalScale(num, num);
			sprites[2].SetLocalScale(num, num);
			sprites[3].SetLocalScale(num, num);
			sprites[0].SetLocalX(localX);
			sprites[1].SetLocalX(localX);
			sprites[2].SetLocalX(localX);
			sprites[3].SetLocalX(localX);
			sprites[0].ToggleSpriteFlip(toggle);
			sprites[1].ToggleSpriteFlip(toggle);
			sprites[2].ToggleSpriteFlip(toggle);
			sprites[3].ToggleSpriteFlip(toggle);
			sprites[0].TriggerAnim("pop", speed);
			sprites[1].TriggerAnim("pop", speed);
			sprites[2].TriggerAnim("pop", speed);
			sprites[3].TriggerAnim("pop", speed);
		}
		else
		{
			sprites[4].SetLocalScale(num, num);
			sprites[5].SetLocalScale(num, num);
			sprites[6].SetLocalScale(num, num);
			sprites[6].SetLocalScale(num, num);
			sprites[4].SetLocalX(localX);
			sprites[5].SetLocalX(localX);
			sprites[6].SetLocalX(localX);
			sprites[7].SetLocalX(localX);
			sprites[4].ToggleSpriteFlip(toggle);
			sprites[5].ToggleSpriteFlip(toggle);
			sprites[6].ToggleSpriteFlip(toggle);
			sprites[7].ToggleSpriteFlip(toggle);
			sprites[4].TriggerAnim("pop", speed);
			sprites[5].TriggerAnim("pop", speed);
			sprites[6].TriggerAnim("pop", speed);
			sprites[7].TriggerAnim("pop", speed);
		}
	}

	public void SetToLowered(float distance)
	{
		activeY = GetLocalY() - distance * 4f;
		nextY = GetLocalY() - distance * 4f;
		SetLocalY(GetLocalY() - distance * 4f);
	}
}
