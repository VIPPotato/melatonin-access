using System.Collections;
using UnityEngine;

public class LvlEditor_setbacks : LvlEditor
{
	private int beat;

	protected override void Start()
	{
		dreamName = "Dream_setbacks";
		base.Start();
		Underworld.env.Show(isCuedUp: false);
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		Underworld.env.Climb(0.8518f);
		if (beat == 1 || beat == 3)
		{
			Underworld.env.LavaPool.PopBubble();
		}
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		StartCoroutine(Looping());
	}
}
