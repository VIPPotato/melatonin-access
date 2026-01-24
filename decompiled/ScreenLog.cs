public class ScreenLog : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show(string newText)
	{
		SetParent(Interface.env.Cam.GetInnerTransform());
		SetLocalPosition(0f, 0f);
		RenderChildren(toggle: true);
		textboxes[0].SetText(newText);
	}

	public void Hide()
	{
		SetParent(Interface.env.GetTransform());
		RenderChildren(toggle: false);
	}
}
