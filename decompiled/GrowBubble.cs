public class GrowBubble : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		RenderChildren(toggle: true);
		if (SceneMonitor.mgr.GetActiveSceneName() == "Chapter_2")
		{
			sprites[0].TriggerAnim("growCh2");
		}
		else if (SceneMonitor.mgr.GetActiveSceneName() == "Chapter_5")
		{
			sprites[0].TriggerAnim("growCh5");
		}
		else
		{
			sprites[0].TriggerAnim("grow");
		}
	}
}
