using System.Collections;
using UnityEngine;

public class DreamOcean : Wrapper
{
	public static DreamOcean env;

	[Header("Children")]
	public Cloud[] Clouds;

	private bool isActivated;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		LoopShootingStar();
		Clouds[0].Show();
		Clouds[1].Show();
		gears[0].TriggerAnim("parallaxing");
		sprites[2].TriggerAnim("idling", 1f, 0.5f);
		sprites[3].TriggerAnim("waving", 1f, 0.5f);
		sprites[4].TriggerAnim("idling");
		sprites[5].TriggerAnim("idlingAlt");
	}

	public void TrailerZoom()
	{
		gears[0].TriggerAnim("zoom");
		sprites[2].TriggerAnim("idling", 1.5f);
		sprites[3].TriggerAnim("waving", 1.5f);
		sprites[4].TriggerAnim("idling", 1.5f);
		sprites[5].TriggerAnim("idlingAlt", 1.5f);
	}

	private void LoopShootingStar()
	{
		StartCoroutine(LoopingShootingStar());
	}

	private IEnumerator LoopingShootingStar()
	{
		yield return new WaitForSeconds(1f);
		sprites[0].TriggerAnim("shoot");
		float seconds = Random.Range(1.75f, 2.5f);
		yield return new WaitForSeconds(seconds);
		int num = Random.Range(-10, 11);
		int num2 = Random.Range(0, 7);
		sprites[0].transform.position = new Vector3(num, num2, sprites[0].transform.position.z);
		LoopShootingStar();
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
