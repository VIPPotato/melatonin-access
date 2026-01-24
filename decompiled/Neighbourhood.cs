using UnityEngine;

public class Neighbourhood : Wrapper
{
	[Header("Children")]
	public DreamTitle[] DreamTitles;

	public McMap McMap;

	public Landmark[] Landmarks;

	public Shore Shore;

	protected override void Awake()
	{
		SetupFragments();
	}

	public void Show()
	{
		gears[0].TriggerAnim("shown");
		DreamTitle[] dreamTitles = DreamTitles;
		for (int i = 0; i < dreamTitles.Length; i++)
		{
			dreamTitles[i].Show();
		}
		if (Chapter.dir.GetActiveDreamName() != "")
		{
			Vector3 activeLandmarkPosition = GetActiveLandmarkPosition();
			McMap.SetLocalPosition(activeLandmarkPosition.x, activeLandmarkPosition.y - 1f);
			Chapter.dir.ResetActiveDreamName();
		}
		Landmark[] landmarks = Landmarks;
		for (int i = 0; i < landmarks.Length; i++)
		{
			landmarks[i].Show();
		}
	}

	public Landmark GetRemixLandmark()
	{
		return Landmarks[Landmarks.Length - 1];
	}

	private Vector3 GetActiveLandmarkPosition()
	{
		string activeDreamName = Chapter.dir.GetActiveDreamName();
		Landmark[] landmarks = Landmarks;
		foreach (Landmark landmark in landmarks)
		{
			if (landmark.GetDreamName() == activeDreamName)
			{
				return landmark.GetLocalPosition();
			}
		}
		return new Vector3(0f, 0f, 0f);
	}
}
