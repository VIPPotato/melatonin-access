using UnityEngine;

public class TotalBox : Wrapper
{
	[Header("Fragments")]
	public Fragment fader;

	public Fragment[] amounts;

	protected override void Awake()
	{
		fader.Awake();
		Fragment[] array = amounts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		RenderChildren(toggle: true);
		SetParent(Interface.env.Cam.GetOuterTransform());
		SetLocalPosition(12.817f, 7.211f);
		SetLocalZ(20f);
		fader.TriggerAnim("fadeIn");
		amounts[0].SetText(SaveManager.mgr.GetChapterEarnedStars(Chapter.GetActiveChapterNum()).ToString() ?? "");
		amounts[1].SetText(SaveManager.mgr.GetChapterEarnedRings(Chapter.GetActiveChapterNum()).ToString() ?? "");
		amounts[2].SetText(SaveManager.mgr.GetChapterEarnedPerfects(Chapter.GetActiveChapterNum()).ToString() ?? "");
	}

	public void Deactivate()
	{
		fader.TriggerAnim("fadeOut");
	}
}
