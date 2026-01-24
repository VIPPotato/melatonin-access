using System.Collections;
using UnityEngine;

public class LvlEditor_tech : LvlEditor
{
	private int beat;

	protected override void Start()
	{
		dreamName = "Dream_tech";
		base.Start();
		MechSpace.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		StartCoroutine(Looping());
	}
}
