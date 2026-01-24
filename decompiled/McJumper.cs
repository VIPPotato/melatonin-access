using System.Collections;
using UnityEngine;

public class McJumper : Wrapper
{
	[Header("Props")]
	public Sweat Sweat;

	[Header("Props")]
	public int[] twinkleBar1;

	public int[] twinkleBar2;

	private float initX;

	private float initY;

	private float initTrackerX;

	private float initTrackerY;

	private float timer;

	private float prepDuration;

	private bool isEscaped;

	private bool isAwaiting = true;

	private bool isCamTracked;

	private int jumpNum = 1;

	private int moveNum;

	private int positionNum = 4;

	private Coroutine moving;

	protected override void Awake()
	{
		SetupFragments();
		initX = GetLocalX();
		initY = GetLocalY();
		initTrackerY = gears[1].GetLocalY();
		prepDuration = sprites[0].GetAnimDuration("prep");
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		isCamTracked = false;
		isEscaped = false;
		isAwaiting = true;
		timer = 0f;
		moveNum = 0;
		positionNum = 4;
		SetLocalPosition(initX, initY);
		gears[0].ToggleAnimator(toggle: false);
		gears[0].SetLocalPosition(0f, 0f);
		sprites[0].TriggerAnim("beat", 1f, 1f);
	}

	public void Hide()
	{
		isCamTracked = false;
		gears[0].ToggleAnimator(toggle: false);
		gears[0].SetLocalPosition(0f, 0f);
		SetLocalPosition(initX, initY);
		Interface.env.Cam.CancelTracking();
		CancelCoroutine(moving);
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 0.016f)
		{
			if (isCamTracked)
			{
				Interface.env.Cam.SetTarget(gears[1].GetPosition());
			}
			timer = 0f;
		}
	}

	public void Jump()
	{
		if (jumpNum == 1)
		{
			sprites[0].TriggerAnim("jump", InfluencerLand.env.GetSpeed() / (float)moveNum);
		}
		else
		{
			sprites[0].TriggerAnim("jump", InfluencerLand.env.GetSpeed() * 2f);
		}
		sprites[1].TriggerAnim("dust");
	}

	public void Ping(int bar, int beat)
	{
		if (bar % 2 == 1)
		{
			int index = twinkleBar1[beat - 1];
			speakers[0].TriggerSound(index);
		}
		else
		{
			int index2 = twinkleBar2[beat - 1];
			speakers[0].TriggerSound(index2);
		}
	}

	public void Stumble()
	{
		sprites[0].TriggerAnim("stumble", InfluencerLand.env.GetSpeed());
	}

	public void MoveDelayed(float timeStarted, int newMoveNum, bool isShort = false)
	{
		moving = StartCoroutine(MovingDelayed(timeStarted, newMoveNum, isShort));
	}

	private IEnumerator MovingDelayed(float timeStarted, int newMoveNum, bool isShort)
	{
		float oldMoveNum = moveNum;
		moveNum = newMoveNum;
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isAwaiting)
		{
			isAwaiting = false;
			sprites[0].TriggerAnim("jump", InfluencerLand.env.GetSpeed() / (float)moveNum);
			Interface.env.Cam.TrackTarget(gears[1].GetPosition(), 2.5f * InfluencerLand.env.GetSpeed(), isEasingIn: false);
		}
		else
		{
			isCamTracked = false;
			gears[0].ToggleAnimator(toggle: false);
			gears[0].SetLocalPosition(0f, 0f);
			SetDistance(InfluencerLand.env.GetSpaceBetweenX() * oldMoveNum, InfluencerLand.env.GetSpaceBetweenY() * oldMoveNum);
		}
		gears[0].ToggleAnimator(toggle: true);
		if (isShort)
		{
			gears[0].TriggerAnim("move1half", InfluencerLand.env.GetSpeed());
		}
		else
		{
			gears[0].TriggerAnim("move" + moveNum, InfluencerLand.env.GetSpeed());
		}
		if (moveNum > 1)
		{
			speakers[1].TriggerSound(moveNum - 2);
		}
		yield return null;
		isCamTracked = true;
		int multiplier = ((!isShort) ? 1 : 2);
		float num = MusicBox.env.GetSecsPerBeat() / (float)multiplier * (float)moveNum - prepDuration / (float)multiplier / InfluencerLand.env.GetSpeed();
		checkpoint += num;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		sprites[0].TriggerAnim("prep", (float)multiplier * InfluencerLand.env.GetSpeed());
	}

	public void Escape()
	{
		SetDistance(InfluencerLand.env.GetSpaceBetweenX() * (float)moveNum, InfluencerLand.env.GetSpaceBetweenY() * (float)moveNum);
		isEscaped = true;
		isCamTracked = false;
		gears[0].ToggleAnimator(toggle: true);
		gears[0].TriggerAnim("escape", InfluencerLand.env.GetSpeed());
		Interface.env.Cam.StopTracking();
	}

	public void Bobble()
	{
		sprites[0].TriggerAnim("beat", InfluencerLand.env.GetSpeed());
	}

	public void CancelMovement()
	{
		CancelCoroutine(moving);
	}

	public void SetJumpNum(int newJumpNum)
	{
		jumpNum = newJumpNum;
	}

	public void IncreasePositionNum(int amount)
	{
		positionNum += amount;
		positionNum = ((positionNum > 8) ? (positionNum - 9) : positionNum);
	}

	public bool CheckIsEscaped()
	{
		return isEscaped;
	}

	public bool CheckIsAwaiting()
	{
		return isAwaiting;
	}

	public int GetPositionNum()
	{
		return positionNum;
	}

	public int GetNextPositionNum()
	{
		if (positionNum + 1 <= 8)
		{
			return positionNum + 1;
		}
		return 0;
	}

	public int GetNextNextPositionNum()
	{
		if (GetNextPositionNum() + 1 <= 8)
		{
			return GetNextPositionNum() + 1;
		}
		return 0;
	}

	public int GetNextNextNextPositionNum()
	{
		if (GetNextNextPositionNum() + 1 <= 8)
		{
			return GetNextNextPositionNum() + 1;
		}
		return 0;
	}

	public int GetNextNextNextNextPositionNum()
	{
		if (GetNextNextNextPositionNum() + 1 <= 8)
		{
			return GetNextNextNextPositionNum() + 1;
		}
		return 0;
	}
}
