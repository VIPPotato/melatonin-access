using System.Collections;
using UnityEngine;

public class LvlEditor_career : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_career";
		base.Start();
		OfficeSpace.env.Show();
		OfficeSpace.env.Loop(1f, isReverse: false);
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		OfficeSpace.env.BobbleDelayed(0f, Random.Range(1, 5));
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		StartCoroutine(Looping());
	}
}
