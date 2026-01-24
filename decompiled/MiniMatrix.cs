using UnityEngine;

public class MiniMatrix : Wrapper
{
	[Header("Children")]
	public McSwinger McSwinger;

	public TimeWrapper[] TimeWrappers;

	public Portal[] Portals;

	private int second;

	private bool isActivated;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		McSwinger.Show();
	}

	public void Hide()
	{
		isActivated = false;
		McSwinger.Hide();
		Portal[] portals = Portals;
		for (int i = 0; i < portals.Length; i++)
		{
			portals[i].Hide();
		}
		TimeWrapper[] timeWrappers = TimeWrappers;
		for (int i = 0; i < timeWrappers.Length; i++)
		{
			timeWrappers[i].Hide();
		}
		RenderChildren(toggle: false);
	}

	public void Reset()
	{
		Portal[] portals = Portals;
		for (int i = 0; i < portals.Length; i++)
		{
			portals[i].Hide();
		}
		TimeWrappers[0].Hide();
		TimeWrappers[1].Hide();
	}

	public void Tick()
	{
		sprites[0].TriggerAnim(second.ToString() ?? "");
		second++;
		if (second > 13)
		{
			second = 0;
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
