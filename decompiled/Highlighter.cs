public class Highlighter : Wrapper
{
	private bool isActivated;

	protected override void Awake()
	{
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		isActivated = false;
		RenderChildren(toggle: false);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
