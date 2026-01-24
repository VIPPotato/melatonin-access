public class LandLayer : Wrapper
{
	private float initLocalZ;

	protected override void Awake()
	{
		SetupFragments();
		initLocalZ = GetLocalZ();
	}

	private void Update()
	{
		if (Map.env.Neighbourhood.McMap.GetColliderPoint() < gears[0].GetY())
		{
			SetLocalZ(initLocalZ);
		}
		else
		{
			SetLocalZ(2f);
		}
	}
}
