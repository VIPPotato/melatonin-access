using System.Collections;
using UnityEngine;

public class McClimber : Wrapper
{
	private bool isClimbing;

	private bool isDangerous;

	private bool isCamTracked;

	private float initLocalX;

	private float initLocalY;

	private float distanceX;

	private float distanceY;

	private float prepTransition;

	private float jumpDuration;

	private Coroutine prepping;

	private const float animTempo = 122f;

	protected override void Awake()
	{
		SetupFragments();
		initLocalX = GetLocalX();
		initLocalY = GetLocalY();
		prepTransition = sprites[0].GetAnimDuration("prep1");
		jumpDuration = sprites[0].GetAnimDuration("jumpLeft");
		RenderChildren(toggle: false);
	}

	public void Show(bool isCuedUp)
	{
		StartCoroutine(Showing(isCuedUp));
	}

	private IEnumerator Showing(bool isCuedUp)
	{
		RenderChildren(toggle: true);
		SetLocalPosition(initLocalX, initLocalY);
		if (isCuedUp)
		{
			isClimbing = true;
			sprites[0].TriggerAnim("ready");
			yield return null;
			Interface.env.Cam.TrackTarget(sprites[0].transform.position - new Vector3(0f, initLocalY, 0f), 2.75f * GetSpeed(), isEasingIn: false);
			isCamTracked = true;
		}
		else
		{
			sprites[0].TriggerAnim("gripped");
		}
	}

	public void Hide()
	{
		isClimbing = false;
		isCamTracked = false;
		isDangerous = false;
		distanceX = 0f;
		distanceY = 0f;
		CancelCoroutine(prepping);
		Interface.env.Cam.CancelTracking();
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (isCamTracked)
		{
			if (isDangerous)
			{
				Interface.env.Cam.SetTarget(sprites[0].transform.position - new Vector3(0f, initLocalY + 1.75f, 0f));
			}
			else
			{
				Interface.env.Cam.SetTarget(sprites[0].transform.position - new Vector3(0f, initLocalY, 0f));
			}
		}
	}

	public void Climb(float newDistanceY)
	{
		StartCoroutine(Climbing(newDistanceY));
	}

	private IEnumerator Climbing(float newDistanceY)
	{
		isCamTracked = false;
		SetDistance(distanceX, distanceY);
		distanceX = 0f;
		distanceY = newDistanceY;
		if (isClimbing)
		{
			if (sprites[0].CheckIsAnimPlaying("climb") || sprites[0].CheckIsAnimPlaying("reset"))
			{
				sprites[0].ToggleSpriteFlip(!sprites[0].CheckIsSpriteFlipped());
				sprites[0].TriggerAnim("climb");
			}
			else
			{
				sprites[0].ToggleSpriteFlip(toggle: false);
				sprites[0].TriggerAnim("reset");
			}
		}
		else
		{
			isClimbing = true;
			Interface.env.Cam.TrackTarget(sprites[0].transform.position - new Vector3(0f, initLocalY, 0f), 2.75f * GetSpeed(), isEasingIn: false);
			sprites[0].TriggerAnim("climb");
		}
		RepeatLadder();
		yield return null;
		isCamTracked = true;
	}

	public void Prep(int direction)
	{
		CancelCoroutine(prepping);
		prepping = StartCoroutine(Prepping(direction));
	}

	private IEnumerator Prepping(int direction)
	{
		float timeStarted = Technician.mgr.GetDspTime();
		float timeDelayed = MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > timeDelayed);
		yield return null;
		SetDistance(distanceX, distanceY);
		distanceX = 0f;
		distanceY = 0f;
		if (sprites[0].CheckIsAnimPlaying("climb") || sprites[0].CheckIsAnimPlaying("reset"))
		{
			sprites[0].ToggleSpriteFlip(!sprites[0].CheckIsSpriteFlipped());
			sprites[0].TriggerAnim("prep1", GetSpeed());
		}
		else
		{
			sprites[0].TriggerAnim("ready");
		}
		timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > prepTransition / GetSpeed());
		switch (direction)
		{
		case 1:
			sprites[0].ToggleSpriteFlip(toggle: false);
			sprites[0].TriggerAnim("prep2", GetSpeed());
			break;
		case 2:
			sprites[0].ToggleSpriteFlip(toggle: true);
			sprites[0].TriggerAnim("prep2", GetSpeed());
			break;
		case 3:
			sprites[0].ToggleSpriteFlip(toggle: false);
			sprites[0].TriggerAnim("prep3", GetSpeed());
			break;
		}
	}

	public void Move(string animName, float newDistanceX, float newDistanceY)
	{
		SetDistance(distanceX, distanceY);
		distanceX = newDistanceX;
		distanceY = newDistanceY;
		RepeatLadder();
		sprites[0].TriggerAnim(animName, GetSpeed());
		switch (animName)
		{
		case "jumpLeft":
			sprites[0].ToggleSpriteFlip(toggle: false);
			break;
		case "jumpRight":
			sprites[0].ToggleSpriteFlip(toggle: true);
			break;
		case "burnLeft":
		case "burnRight":
		case "burnUp":
			CancelCoroutine(prepping);
			break;
		}
	}

	private void RepeatLadder()
	{
		if (GetY() > Underworld.env.LadderGroup.GetLadderColumnActive().GetY() + 13.63f)
		{
			Underworld.env.LadderGroup.GetLadderColumnActive().SetY(Underworld.env.LadderGroup.GetLadderColumnActive().GetY() + 13.63f);
		}
	}

	public void ToggleIsDangeorus(bool toggle)
	{
		isDangerous = toggle;
	}

	private float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 122f;
	}

	public Vector3 GetHeadPosition()
	{
		return sprites[0].transform.position - new Vector3(0f, 1.4f, 0f);
	}

	public Vector3 GetButtPosition()
	{
		return sprites[0].transform.position - new Vector3(Random.Range(-0.5f, 0.5f), 3.57f, 0f);
	}
}
