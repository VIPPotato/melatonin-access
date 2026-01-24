using System.Collections;
using UnityEngine;

public class LvlEditor_shopping : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_shopping";
		base.Start();
		Mall.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		Mall.env.Counter.McSpender.BobbleDelayed(0f);
		Mall.env.Counter.CardMachine.Bobble();
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat() * 2f);
		StartCoroutine(Looping());
	}
}
