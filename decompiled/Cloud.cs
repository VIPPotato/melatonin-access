using UnityEngine;

public class Cloud : Wrapper
{
	[Header("Props")]
	public bool isOneShot;

	public bool isDriftRandomized = true;

	public float driftMultiplier = 1f;

	public float parallaxMultiplier = 1f;

	private bool isDrifting;

	private bool isParallaxing;

	private float driftSpeed;

	private float parallaxSpeed;

	private float initialY;

	private float timer;

	protected override void Awake()
	{
		SetupFragments();
		initialY = GetY();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		timer = 0f;
		RenderChildren(toggle: true);
		ConfigureDrift();
		ConfigureParallax();
	}

	public void Hide()
	{
		isDrifting = false;
		isParallaxing = false;
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 0.033f)
		{
			if (isDrifting)
			{
				Drift();
			}
			if (isParallaxing)
			{
				Parallax();
			}
			timer = 0f;
		}
	}

	private void Drift()
	{
		SetPosition(GetX() + timer * driftSpeed, GetY());
		if (driftSpeed > 0f)
		{
			if (CheckIsOutOfRangeRight())
			{
				if (isOneShot)
				{
					isDrifting = false;
				}
				else
				{
					SetPosition(-21.5f, GetY());
				}
			}
		}
		else if (driftSpeed < 0f && CheckIsOutOfRangeLeft())
		{
			if (isOneShot)
			{
				isDrifting = false;
			}
			else
			{
				SetPosition(12f, GetY());
			}
		}
	}

	private void Parallax()
	{
		SetPosition(GetX(), initialY - Interface.env.Cam.GetY() / parallaxSpeed);
	}

	private void ConfigureDrift()
	{
		driftSpeed = (isDriftRandomized ? (Random.Range(0.25f, 0.75f) * driftMultiplier) : (0.5f * driftMultiplier));
		isDrifting = driftSpeed != 0f;
	}

	private void ConfigureParallax()
	{
		parallaxSpeed = ((parallaxMultiplier == 0f) ? 0f : (4f / parallaxMultiplier));
		isParallaxing = parallaxSpeed != 0f;
	}

	public void SetDriftMultiplier(float newDriftMultiplier)
	{
		driftMultiplier = newDriftMultiplier;
		ConfigureDrift();
	}

	public float GetDriftMultiplier()
	{
		return driftMultiplier;
	}

	private bool CheckIsOutOfRangeRight()
	{
		if (!(base.transform.position.x >= 12.8f))
		{
			return false;
		}
		return true;
	}

	private bool CheckIsOutOfRangeLeft()
	{
		if (!(base.transform.position.x <= -22.5f))
		{
			return false;
		}
		return true;
	}
}
