using UnityEngine;

public class Shore : Wrapper
{
	private bool isCollided;

	protected override void Awake()
	{
		SetupFragments();
	}

	private void OnTriggerEnter2D()
	{
		isCollided = true;
	}

	private void OnTriggerExit2D()
	{
		isCollided = false;
	}

	public void TriggerSplashSound()
	{
		speakers[0].SetSoundPitch(0, Random.Range(0.75f, 1f));
		speakers[0].SetSoundVolume(0, Random.Range(0.6f, 0.9f));
		speakers[0].TriggerSoundStack(0);
	}

	public bool CheckIsCollided()
	{
		return isCollided;
	}
}
