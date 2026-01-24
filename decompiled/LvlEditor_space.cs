using System.Collections;
using UnityEngine;

public class LvlEditor_space : LvlEditor
{
	private int beat;

	private int bar;

	protected override void Start()
	{
		dreamName = "Dream_space";
		base.Start();
		AngrySkies.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		AngrySkies.env.McMarchers.MarchDelayed(0f);
		if (beat == 1 && AngrySkies.env.NasaTv.CheckIsActivated())
		{
			AngrySkies.env.NasaTv.Hover();
			AngrySkies.env.NasaTv.Tick(0f);
		}
		else if (beat == 2)
		{
			AngrySkies.env.SparkleStarDelayed(0f);
		}
		else if (beat == 3 && AngrySkies.env.NasaTv.CheckIsActivated())
		{
			AngrySkies.env.NasaTv.Tick(0f);
		}
		else if (beat == 4)
		{
			AngrySkies.env.SparkleStarDelayed(0f);
		}
		if (bar % 2 == 0 && beat == 3)
		{
			AngrySkies.env.ShootStarDelayed(0f);
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
