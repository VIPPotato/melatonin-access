using UnityEngine;

public class body : Fragment
{
	private int stepNum;

	private void TriggerFootstep()
	{
		if (Map.env.Neighbourhood.Shore != null && Map.env.Neighbourhood.Shore.CheckIsCollided())
		{
			Map.env.Neighbourhood.Shore.TriggerSplashSound();
			return;
		}
		stepNum++;
		if (stepNum > 1)
		{
			stepNum = 0;
		}
		SetSoundPitch(stepNum, Random.Range(0.75f, 1f));
		SetSoundVolume(stepNum, Random.Range(0.4f, 0.567f));
		TriggerSoundStack(stepNum);
	}
}
