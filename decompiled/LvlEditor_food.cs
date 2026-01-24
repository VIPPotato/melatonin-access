using System.Collections;
using UnityEngine;

public class LvlEditor_food : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_food";
		base.Start();
		FoodySkies.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		StartCoroutine(Looping());
	}
}
