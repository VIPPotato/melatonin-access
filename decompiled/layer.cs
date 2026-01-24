using UnityEngine;

public class layer : Fragment
{
	[Header("Props")]
	public Transform cornerTile;

	public bool isTilingX;

	public bool isTilingY;

	public bool isParallaxingX;

	public bool isParallaxingY;

	public float parallaxDivider;

	private bool isDisabed;

	private bool isInited;

	private float initX;

	private float initY;

	private float initLocalX;

	private float initLocalY;

	private float retileDistanceX;

	private float retileDistanceY;

	private float retileAmountX;

	private float retileAmountY;

	private float parallaxAmountX;

	private float parallaxAmountY;

	private void OnEnable()
	{
		if (isInited)
		{
			Update();
		}
	}

	public override void Awake()
	{
		base.Awake();
		initX = GetX();
		initY = GetY();
		initLocalX = GetLocalX();
		initLocalY = GetLocalY();
		if (isTilingX)
		{
			retileDistanceX = Mathf.Abs(cornerTile.localPosition.x / 2f);
		}
		if (isTilingY)
		{
			retileDistanceY = Mathf.Abs(cornerTile.localPosition.y / 2f);
		}
	}

	private void Start()
	{
		isInited = true;
	}

	public void Update()
	{
		if (isDisabed)
		{
			return;
		}
		if (isTilingX)
		{
			if (Interface.env.Cam.GetX() < GetX() - retileDistanceX)
			{
				retileAmountX -= retileDistanceX * 2f;
			}
			else if (Interface.env.Cam.GetX() > GetX() + retileDistanceX)
			{
				retileAmountX += retileDistanceX * 2f;
			}
		}
		if (isTilingY)
		{
			if (Interface.env.Cam.GetY() < GetY() - retileDistanceY)
			{
				retileAmountY -= retileDistanceY * 2f;
			}
			else if (Interface.env.Cam.GetY() > GetY() + retileDistanceY)
			{
				retileAmountY += retileDistanceY * 2f;
			}
		}
		if (isParallaxingX)
		{
			parallaxAmountX = Interface.env.Cam.GetShaker().GetX() / parallaxDivider;
		}
		if (isParallaxingY)
		{
			parallaxAmountY = Interface.env.Cam.GetShaker().GetY() / parallaxDivider;
		}
		if (Interface.env.Cam.GetShaker().GetX() != 0f || Interface.env.Cam.GetShaker().GetY() != 0f)
		{
			SetPosition(parallaxAmountX + retileAmountX + initX, parallaxAmountY + retileAmountY + initY);
		}
		else
		{
			SetLocalPosition(initLocalX, initLocalY);
		}
	}

	public void Disable()
	{
		isDisabed = true;
	}
}
