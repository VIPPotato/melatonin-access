using System.Collections;
using UnityEngine;

public class LvlEditor_desires : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_desires";
		base.Start();
		Espot.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		Espot.env.UfoMachine.FlashSlotsDelayed(0f, Random.Range(1, 5));
		Espot.env.Arcade.RefreshScreensDelayed(0f);
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		StartCoroutine(Looping());
	}
}
