using System.Collections;
using UnityEngine;

public class LvlEditor_meditation : LvlEditor
{
	private int beat = 1;

	private int bar = 1;

	protected override void Start()
	{
		dreamName = "Dream_meditation";
		base.Start();
		Matrix.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		Matrix.env.Tick();
		Matrix.env.Bobble(1f);
		if (beat == 1)
		{
			Matrix.env.LilBlocks.Wave();
			if (!Matrix.env.CheckIsZoomedFarOut())
			{
				if (bar % 2 == 1)
				{
					Matrix.env.ParallaxOut(newIsZoomedFarOut: false);
				}
				else
				{
					Matrix.env.ParallaxIn(0f, newIsZoomedFarOut: false);
				}
			}
		}
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
			bar++;
			if (bar > 8)
			{
				bar = 1;
			}
		}
		StartCoroutine(Looping());
	}
}
