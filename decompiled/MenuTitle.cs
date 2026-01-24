using UnityEngine;

public class MenuTitle : Wrapper
{
	[Header("Fragments")]
	public textboxFragment title;

	private float initLocalX;

	protected override void Awake()
	{
		initLocalX = GetLocalX();
		title.Initiate();
		title.ToggleIsRealTimeFader(toggle: true);
		RenderChildren(toggle: false);
	}

	public void Activate(int direction)
	{
		RenderChildren(toggle: true);
		float localX = ((direction == 0) ? (initLocalX - 1f) : (initLocalX + 1f));
		float x = ((direction == 0) ? (initLocalX + 1f) : (initLocalX - 1f));
		SetLocalX(localX);
		MoveDistanceRealtime(new Vector3(x, 0f, 0f), 3f, isEasingIn: false);
		title.FadeInText(1f, 0.33f);
	}

	public void Deactivate()
	{
		title.FadeOutText(1f, 0.33f);
	}

	public void SetTitle(string newStateName)
	{
		title.SetStateByName(newStateName);
	}
}
