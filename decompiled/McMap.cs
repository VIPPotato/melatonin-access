using System.Collections;
using UnityEngine;

public class McMap : Wrapper
{
	[Header("Children")]
	public ModeMenu ModeMenu;

	[Header("Fragments")]
	public Fragment elevator;

	public Fragment body;

	public Fragment[] dusts;

	[Header("Props")]
	public float speedX;

	public float speedY;

	private int dustNum;

	private int facingDirection;

	private bool isAsleep;

	private bool isWakeMirrored;

	private bool isEnabled;

	private bool isDustable;

	private bool isDusting;

	private bool isRunning;

	private float distanceX;

	private float distanceY;

	private float initLocalZ;

	private float speedMultiplier = 1f;

	private float timeTilWake;

	private Coroutine dusting;

	protected override void Awake()
	{
		SetupFragments();
		elevator.Awake();
		body.Awake();
		initLocalZ = GetLocalZ();
		timeTilWake = body.GetAnimDuration("wake");
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		if (Chapter.GetActiveChapterNum() == 5)
		{
			isDustable = false;
		}
		else
		{
			isDustable = true;
		}
		if (isAsleep)
		{
			body.TriggerAnim("asleep");
		}
		else
		{
			body.TriggerAnim("downAngledIdled");
		}
		if (isWakeMirrored)
		{
			body.ToggleSpriteFlip(toggle: true);
		}
	}

	private void Update()
	{
		if (!isEnabled || !(Time.timeScale > 0f))
		{
			return;
		}
		if (ControlHandler.mgr.GetLeftStickX() > 0.1f || ControlHandler.mgr.GetLeftStickX() < -0.1f)
		{
			distanceX = ControlHandler.mgr.GetLeftStickX();
		}
		else if (ControlHandler.mgr.CheckIsRightPressing())
		{
			distanceX = 1f;
		}
		else if (ControlHandler.mgr.CheckIsLeftPressing())
		{
			distanceX = -1f;
		}
		else
		{
			distanceX = 0f;
		}
		if (ControlHandler.mgr.GetLeftStickY() > 0.1f || ControlHandler.mgr.GetLeftStickY() < -0.1f)
		{
			distanceY = ControlHandler.mgr.GetLeftStickY();
		}
		else if (ControlHandler.mgr.CheckIsUpPressing())
		{
			distanceY = 1f;
		}
		else if (ControlHandler.mgr.CheckIsDownPressing())
		{
			distanceY = -1f;
		}
		else
		{
			distanceY = 0f;
		}
		if ((distanceX == 1f || distanceX == -1f) && (distanceY == 1f || distanceY == -1f))
		{
			distanceX /= 1.33f;
			distanceY /= 1.33f;
		}
		if (distanceY != 0f)
		{
			if (base.transform.localPosition.y > 1f)
			{
				Interface.env.Cam.MoveToTarget(new Vector3(Interface.env.transform.position.x, base.transform.position.y + 1.5f, Interface.env.transform.position.z), 2f, isEasingIn: false);
			}
			else
			{
				Interface.env.Cam.MoveToTarget(new Vector3(Interface.env.transform.position.x, base.transform.position.y - 5f, Interface.env.transform.position.z), 2f, isEasingIn: false);
			}
		}
		Run();
		if (isDustable && (distanceX != 0f || distanceY != 0f))
		{
			Dust();
		}
	}

	private void Run()
	{
		if (distanceX == 0f && distanceY == 0f)
		{
			if (isRunning)
			{
				isRunning = false;
				if (facingDirection == 0)
				{
					body.TriggerAnim("upIdled");
				}
				else if (facingDirection == 1)
				{
					body.TriggerAnim("upAngledIdled");
				}
				else if (facingDirection == 2)
				{
					body.TriggerAnim("sideIdled");
				}
				else if (facingDirection == 3)
				{
					body.TriggerAnim("downAngledIdled");
				}
				else if (facingDirection == 4)
				{
					body.TriggerAnim("downIdled");
				}
				else if (facingDirection == 5)
				{
					body.TriggerAnim("downAngledIdled");
				}
				else if (facingDirection == 6)
				{
					body.TriggerAnim("sideIdled");
				}
				else if (facingDirection == 7)
				{
					body.TriggerAnim("upAngledIdled");
				}
			}
		}
		else if (distanceX == 0f && distanceY > 0f)
		{
			if (facingDirection != 0 || !isRunning)
			{
				isRunning = true;
				facingDirection = 0;
				body.TriggerAnim("upRunning");
			}
		}
		else if (distanceX > 0f && distanceY > 0f)
		{
			if (facingDirection != 1 || !isRunning)
			{
				isRunning = true;
				facingDirection = 1;
				body.TriggerAnim("upAngledRunning");
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		else if (distanceX > 0f && distanceY == 0f)
		{
			if (facingDirection != 2 || !isRunning)
			{
				isRunning = true;
				facingDirection = 2;
				body.TriggerAnim("sideRunning");
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		else if (distanceX > 0f && distanceY < 0f)
		{
			if (facingDirection != 3 || !isRunning)
			{
				isRunning = true;
				facingDirection = 3;
				body.TriggerAnim("downAngledRunning");
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		else if (distanceX == 0f && distanceY < 0f)
		{
			if (facingDirection != 4 || !isRunning)
			{
				isRunning = true;
				facingDirection = 4;
				body.TriggerAnim("downRunning");
			}
		}
		else if (distanceX < 0f && distanceY < 0f)
		{
			if (facingDirection != 5 || !isRunning)
			{
				isRunning = true;
				facingDirection = 5;
				body.TriggerAnim("downAngledRunning");
				body.ToggleSpriteFlip(toggle: false);
			}
		}
		else if (distanceX < 0f && distanceY == 0f)
		{
			if (facingDirection != 6 || !isRunning)
			{
				isRunning = true;
				facingDirection = 6;
				body.TriggerAnim("sideRunning");
				body.ToggleSpriteFlip(toggle: false);
			}
		}
		else if (distanceX < 0f && distanceY > 0f && (facingDirection != 7 || !isRunning))
		{
			isRunning = true;
			facingDirection = 7;
			body.TriggerAnim("upAngledRunning");
			body.ToggleSpriteFlip(toggle: false);
		}
	}

	private void Dust()
	{
		if (!isDusting)
		{
			CancelCoroutine(dusting);
			dusting = StartCoroutine(Dusting());
		}
	}

	private IEnumerator Dusting()
	{
		isDusting = true;
		dusts[dustNum].SetParent(Map.env.Neighbourhood.gears[0].transform);
		dusts[dustNum].TriggerAnim("dust" + dustNum);
		if (facingDirection == 0)
		{
			dusts[dustNum].SetLocalZ(initLocalZ - 1f);
			if (dustNum == 0)
			{
				dusts[dustNum].SetPosition(GetX() + 0.2f, GetY() + 0.2f);
				dusts[dustNum].ToggleSpriteFlip(toggle: true);
			}
			else
			{
				dusts[dustNum].SetPosition(GetX() - 0.2f, GetY() + 0.2f);
				dusts[dustNum].ToggleSpriteFlip(toggle: false);
			}
		}
		else if (facingDirection == 1)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ - 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: true);
		}
		else if (facingDirection == 2)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ + 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: true);
		}
		else if (facingDirection == 3)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ + 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: true);
		}
		else if (facingDirection == 4)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ + 1f);
			if (dustNum == 0)
			{
				dusts[dustNum].SetPosition(GetX() + 0.125f, GetY());
				dusts[dustNum].ToggleSpriteFlip(toggle: true);
			}
			else
			{
				dusts[dustNum].SetPosition(GetX() - 0.125f, GetY());
				dusts[dustNum].ToggleSpriteFlip(toggle: false);
			}
		}
		else if (facingDirection == 5)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ + 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: false);
		}
		else if (facingDirection == 6)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ + 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: false);
		}
		else if (facingDirection == 7)
		{
			dusts[dustNum].SetPosition(GetX(), GetY());
			dusts[dustNum].SetLocalZ(initLocalZ - 1f);
			dusts[dustNum].ToggleSpriteFlip(toggle: false);
		}
		dustNum = ((dustNum == 0) ? 1 : 0);
		yield return new WaitForSeconds(0.25f);
		isDusting = false;
	}

	public virtual void FixedUpdate()
	{
		if (isEnabled)
		{
			TriggerMovement();
		}
	}

	private void TriggerMovement()
	{
		float x = distanceX * (speedX * speedMultiplier);
		float y = distanceY * (speedY * speedMultiplier);
		base.transform.Translate(new Vector3(x, y, 0f) * Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D()
	{
		speakers[0].TriggerSoundStack(1);
	}

	public void Introduce()
	{
		StartCoroutine(Introducing());
	}

	private IEnumerator Introducing()
	{
		Interface.env.Cam.MoveToTarget(new Vector3(Interface.env.transform.position.x, base.transform.position.y + 1.5f, 0f), 1f);
		if (isAsleep)
		{
			yield return new WaitForSeconds(1f);
			body.TriggerAnim("wake");
			yield return new WaitForSeconds(timeTilWake);
			isAsleep = false;
		}
		else
		{
			yield return new WaitForSeconds(2f);
		}
		isEnabled = true;
		speakers[0].TriggerSound(0);
		Interface.env.Letterbox.Deactivate();
		Map.env.TotalBox.Activate();
	}

	public void ReadyUp()
	{
		speedMultiplier = 0.9f;
		elevator.TriggerAnim("press");
	}

	public void Unready()
	{
		speedMultiplier = 1f;
		elevator.TriggerAnim("awaiting");
	}

	public void Descend()
	{
		elevator.TriggerAnim("lower");
		body.CancelSound(0);
		body.SetSpriteMaskInteraction(1);
		Map.env.TotalBox.Deactivate();
	}

	public void Disable()
	{
		isEnabled = false;
		distanceX = 0f;
		distanceY = 0f;
		Run();
		TriggerMovement();
	}

	public void Enable()
	{
		isEnabled = true;
	}

	public void ToggleIsAsleep(bool toggle)
	{
		isAsleep = toggle;
	}

	public void ToggleIsStartMirrored(bool toggle)
	{
		isWakeMirrored = toggle;
	}

	public float GetColliderPoint()
	{
		return GetY() + GetComponent<CapsuleCollider2D>().offset.y;
	}
}
