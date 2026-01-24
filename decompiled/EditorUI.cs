using UnityEngine;

public class EditorUI : Wrapper
{
	public static EditorUI env;

	[Header("Children")]
	public ConfirmModal ConfirmModal;

	public CustomizeMenu[] CustomizeMenus;

	public Fader Fader;

	public InfoBar InfoBar;

	public Daw Daw;

	public AdvancedMenu AdvancedMenu;

	protected override void Awake()
	{
		env = this;
		RenderChildren(toggle: false);
	}

	public void Activate(char initiatedDataType)
	{
		RenderChildren(toggle: true);
		SetParent(Interface.env.Cam.GetInnerTransform());
		SetLocalPosition(0f, 0f);
		InfoBar.Activate();
		Daw.Activate(isReactivated: false, initiatedDataType);
		Interface.env.DisableAberration();
	}

	public void PlayToggleSfx()
	{
		speakers[0].TriggerSound(0);
	}

	public void PlaySelectSfx()
	{
		speakers[0].TriggerSound(1);
	}

	public void PlayInSfx()
	{
		speakers[0].TriggerSound(2);
	}

	public void PlayOutSfx()
	{
		speakers[0].TriggerSound(3);
	}

	public void PlayRemoveSfx()
	{
		speakers[0].TriggerSound(4);
	}

	public void PlaySwitchSfx()
	{
		speakers[0].TriggerSound(5);
	}

	public void PlayBlockedSfx()
	{
		speakers[0].TriggerSound(6);
	}

	public CustomizeMenu GetActiveCustomizeMenu()
	{
		if (Daw.TimelineTabs.GetCharType() == 'd')
		{
			return CustomizeMenus[0];
		}
		if (Daw.TimelineTabs.GetCharType() == 'u')
		{
			return CustomizeMenus[1];
		}
		if (Daw.TimelineTabs.GetCharType() == 'e')
		{
			return CustomizeMenus[2];
		}
		return CustomizeMenus[3];
	}
}
