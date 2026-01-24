using System.Collections;
using UnityEngine;

public class LvlEditor_pressure : LvlEditor
{
	private int beat = 1;

	protected override void Start()
	{
		dreamName = "Dream_pressure";
		base.Start();
		Gym.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		Gym.env.BobbleDelayed(0f, beat, isHitWindow: false);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		StartCoroutine(Looping());
	}
}
