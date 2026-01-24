using System.Collections;
using UnityEngine;

public class LvlEditor_nature : LvlEditor
{
	private int beat;

	protected override void Start()
	{
		dreamName = "Dream_nature";
		base.Start();
		Conservatory.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		if (beat == 1)
		{
			Conservatory.env.WaterCan.Hover();
			Conservatory.env.MoveToNextSproutDelayed(0f);
		}
		else if (beat == 3)
		{
			Conservatory.env.MoveToNextSproutDelayed(0f);
		}
		StartCoroutine(Looping());
	}
}
