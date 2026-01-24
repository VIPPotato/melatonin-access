using System.Collections;
using UnityEngine;

public class LvlEditor_future : LvlEditor
{
	private int beat = 1;

	private int bar = 1;

	protected override void Start()
	{
		dreamName = "Dream_future";
		base.Start();
		NeoCity.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		NeoCity.env.Bobble(0f, bar, beat);
		if (beat == 1 && NeoCity.env.Targets.GetActiveLocalZ() >= 179)
		{
			NeoCity.env.Targets.ResetActiveLocalZ();
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
