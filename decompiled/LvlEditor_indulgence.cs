using System.Collections;
using UnityEngine;

public class LvlEditor_indulgence : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_indulgence";
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
