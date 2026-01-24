using System.Collections;
using UnityEngine;

public class LvlEditor_followers : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_followers";
		base.Start();
		InfluencerLand.env.Show(1, "k", isRemix: false);
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		InfluencerLand.env.Bobble();
		InfluencerLand.env.BobbleDelayed(0f);
		InfluencerLand.env.BobbleNotifications();
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		StartCoroutine(Looping());
	}
}
