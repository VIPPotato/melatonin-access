using UnityEngine;

public class Builder : Custom
{
	public static Builder mgr;

	[Header("Props")]
	public bool isFullGame;

	public int operatingSystemNum;

	private void Awake()
	{
		mgr = this;
	}

	public bool CheckIsFullGame()
	{
		return isFullGame;
	}

	public int GetOperatingSystemNum()
	{
		return operatingSystemNum;
	}
}
