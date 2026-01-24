using System.Collections;
using UnityEngine;

public class LvlEditor_dating : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_dating";
		base.Start();
		LoveLand.env.Show(1);
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		LoveLand.env.Phone.DatingApp.Slide();
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		StartCoroutine(Looping());
	}
}
