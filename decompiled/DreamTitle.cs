using UnityEngine;

public class DreamTitle : Wrapper
{
	[Header("Fragments")]
	public Fragment shifter;

	public spriteFragment lettering;

	public spriteFragment label;

	public Fragment ribbon;

	[Header("Props")]
	public bool isAltLabel;

	public bool isLevitating;

	private float initLocalY;

	protected override void Awake()
	{
		shifter.Awake();
		lettering.Initiate();
		label.Initiate();
		ribbon.Awake();
		initLocalY = GetLocalY();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		float num = GetY() - Interface.env.Cam.GetY();
		initLocalY += num / 25f;
		if (isAltLabel && SaveManager.GetLang() == 6)
		{
			label.SetState(10);
		}
		else
		{
			label.SetState(SaveManager.GetLang());
		}
		lettering.SetState(SaveManager.GetLang());
		if (isLevitating)
		{
			shifter.TriggerAnim("levitating", Random.Range(0.8f, 0.9f), Random.Range(0.1f, 1f));
		}
		else if (SaveManager.GetLang() == 3 || SaveManager.GetLang() == 4)
		{
			shifter.TriggerAnim("resetAlt");
		}
	}

	private void Update()
	{
		SetLocalY(initLocalY - Interface.env.Cam.GetY() / 25f);
	}

	public void React()
	{
		if (!isLevitating)
		{
			if (SaveManager.GetLang() == 3 || SaveManager.GetLang() == 4)
			{
				shifter.TriggerAnim("reactAlt");
			}
			else
			{
				shifter.TriggerAnim("react");
			}
		}
	}

	public void Reset()
	{
		if (!isLevitating)
		{
			if (SaveManager.GetLang() == 3 || SaveManager.GetLang() == 4)
			{
				shifter.TriggerAnim("resetAlt");
			}
			else
			{
				shifter.TriggerAnim("reset");
			}
		}
	}

	public void Morph()
	{
		if (label.CheckIsSpriteRendered())
		{
			label.FadeToColor(new Color(1f, 1f, 1f, 1f), 1f);
		}
		if (ribbon.CheckIsSpriteRendered())
		{
			ribbon.TriggerAnim("morph");
		}
	}

	public void Unmorph()
	{
		if (label.CheckIsSpriteRendered())
		{
			label.FadeToColor(new Color(0.82f, 0.54f, 0.84f), 1f);
		}
		if (ribbon.CheckIsSpriteRendered())
		{
			ribbon.TriggerAnim("unmorph");
		}
	}
}
