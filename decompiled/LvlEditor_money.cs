using System.Collections;
using UnityEngine;

public class LvlEditor_money : LvlEditor
{
	private int beat = 1;

	protected override void Start()
	{
		dreamName = "Dream_money";
		base.Start();
		TropicalBank.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		TropicalBank.env.BobbleDelayed(0f, beat, isCameraMoving: true);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		StartCoroutine(Looping());
	}
}
