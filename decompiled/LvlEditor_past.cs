using System.Collections;
using UnityEngine;

public class LvlEditor_past : LvlEditor
{
	protected override void Start()
	{
		dreamName = "Dream_past";
		base.Start();
		Darkroom.env.Show();
		Loop();
	}

	private void Loop()
	{
		StartCoroutine(Looping());
	}

	private IEnumerator Looping()
	{
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat());
		Darkroom.env.PhotoPulley.DragDelayed(0f);
		Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
		StartCoroutine(Looping());
	}
}
