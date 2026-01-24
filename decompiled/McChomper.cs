using System.Collections;
using UnityEngine;

public class McChomper : Wrapper
{
	[Header("Children")]
	public Feedback[] Feedbacks;

	public Sweat Sweat;

	public Wings Wings;

	[Header("Fragments")]
	public Fragment hoverer;

	public Fragment body;

	public Fragment head;

	public Fragment splatterVfx;

	private Coroutine chomping;

	private Coroutine bobbling;

	protected override void Awake()
	{
		hoverer.Awake();
		body.Awake();
		head.Awake();
		splatterVfx.Awake();
	}

	public void Show()
	{
		Wings.Show();
	}

	public void Hide()
	{
		Wings.Hide();
		CancelCoroutine(bobbling);
	}

	public void BobbleDelayed(float delta)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta));
	}

	private IEnumerator BobblingDelayed(float delta)
	{
		Wings.FlapSoundDelayed(delta);
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		hoverer.TriggerAnim("hover", FoodySkies.env.GetSpeed());
		body.TriggerAnim("hover", FoodySkies.env.GetSpeed());
		Wings.Flap();
	}

	public void Strike()
	{
		head.TriggerAnim("strike");
	}

	public void Chomp()
	{
		head.TriggerAnim("chomp");
		splatterVfx.TriggerAnim("splat" + Random.Range(0, 5), 1.5f);
	}

	public void Swallow()
	{
		head.TriggerAnim("swallow");
	}
}
