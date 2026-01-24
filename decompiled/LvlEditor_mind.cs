using System.Collections;
using UnityEngine;

public class LvlEditor_mind : LvlEditor
{
	private int beat = 1;

	protected override void Start()
	{
		dreamName = "Dream_mind";
		base.Start();
		HypnoLair.env.Show(isRemix: false);
		HypnoLair.env.PocketWatches[0].Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		HypnoLair.env.Bobble(0f, beat);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		beat++;
		if (beat > 4)
		{
			beat = 1;
		}
		StartCoroutine(Looping());
	}
}
