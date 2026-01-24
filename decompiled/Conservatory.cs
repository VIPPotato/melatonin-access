using System.Collections;
using UnityEngine;

public class Conservatory : Wrapper
{
	public static Conservatory env;

	[Header("Children")]
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	public WaterCan WaterCan;

	public Meadow Meadow;

	public Garden Garden;

	public Field Field;

	public Cloud[] Clouds;

	[Header("Fragments")]
	public layer[] layers;

	[Header("Props")]
	public Fragment wateringCanHoverer;

	public Fragment wateringCan;

	private bool isActivated;

	private bool isWatering;

	private float initCanX;

	private float nextCanX;

	private float camOffset;

	private float canY;

	private float camY;

	private Coroutine movingToNextSprout;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		layer[] array = layers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		SetupFragments();
		initCanX = WaterCan.GetX();
		RenderChildren(toggle: false, 2);
	}

	public void Show()
	{
		RenderChildren(toggle: true, 2);
		isActivated = true;
		nextCanX = initCanX;
		camOffset = Mathf.Abs(nextCanX);
		canY = WaterCan.GetY();
		camY = Interface.env.Cam.GetY();
		Meadow.Show();
		Garden.Show();
		Field.Show();
		Interface.env.Cam.SetPosition(0f, 0f);
		WaterCan.SetX(initCanX);
		Cloud[] clouds = Clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].Show();
		}
		DreamWorld.env.SetFeedbacks(Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		isWatering = false;
		Meadow.Hide();
		Garden.Hide();
		Field.Hide();
		Cloud[] clouds = Clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].Hide();
		}
		Interface.env.Cam.SetPosition(0f, 0f);
		Interface.env.Cam.CancelMoving();
		CancelCoroutine(movingToNextSprout);
		RenderChildren(toggle: false, 2);
	}

	private void SlideEased()
	{
		Garden.ReorganizeSprouts();
		Interface.env.Cam.CancelMoving();
		Interface.env.Cam.SetX(camOffset + nextCanX);
		WaterCan.CancelMoving();
		WaterCan.SetX(nextCanX);
		nextCanX += 5f;
		Interface.env.Cam.MoveToTarget(new Vector3(camOffset + nextCanX, camY, 0f), 1.1f * GetSpeed());
		WaterCan.MoveToTarget(new Vector3(nextCanX, canY, 0f), 1.33f * GetSpeed());
		if (isWatering)
		{
			isWatering = false;
			WaterCan.Idle();
			speakers[1].CancelSound(1);
		}
	}

	public void MoveToNextSproutDelayed(float delta)
	{
		CancelCoroutine(movingToNextSprout);
		movingToNextSprout = StartCoroutine(MovingToNextSproutDelayed(delta));
	}

	private IEnumerator MovingToNextSproutDelayed(float delta)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		SlideEased();
	}

	public void ThirstNextSproutDelayed(float timeStarted, bool isBubbled)
	{
		CancelCoroutine(movingToNextSprout);
		movingToNextSprout = StartCoroutine(ThristingToNextSproutDelayed(timeStarted, isBubbled));
	}

	private IEnumerator ThristingToNextSproutDelayed(float timeStarted, bool isBubbled)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		SlideEased();
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isBubbled)
		{
			Garden.GetActiveCrop().PlantBubble.ToggleIsInteractive(toggle: true);
		}
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 0);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Garden.GetActiveCrop().Thirst();
		if (isBubbled)
		{
			Garden.GetActiveCrop().PlantBubble.Activate(0);
		}
	}

	public void DieNextSproutDelayed(float timeStarted, bool isBubbled)
	{
		CancelCoroutine(movingToNextSprout);
		movingToNextSprout = StartCoroutine(DieingNextSproutDelayed(timeStarted, isBubbled));
	}

	private IEnumerator DieingNextSproutDelayed(float timeStarted, bool isBubbled)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		SlideEased();
		Garden.GetActiveCrop().Die(1);
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Garden.GetActiveCrop().Die(2);
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / 2f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isBubbled)
		{
			Garden.GetActiveCrop().PlantBubble.ToggleIsInteractive(toggle: true);
		}
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 3);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Garden.GetActiveCrop().Die(3);
		if (isBubbled)
		{
			Garden.GetActiveCrop().PlantBubble.Activate(1);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isBubbled)
		{
			Garden.GetActiveCrop().PlantBubble.WaterLevel.LinearRise(checkpoint);
		}
	}

	public void SprayWater()
	{
		WaterCan.Spray();
		speakers[1].TriggerSound(0);
	}

	public void PourWater()
	{
		isWatering = true;
		WaterCan.Pour();
		speakers[1].TriggerSound(1);
	}

	public void IdleWater()
	{
		isWatering = false;
		WaterCan.Idle();
		speakers[1].CancelSound(1);
	}

	public void FinishWater()
	{
		speakers[1].TriggerSound(2);
	}

	public void Exit()
	{
		layer[] array = layers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disable();
		}
		Meadow.Exit();
		Field.Exit();
	}

	public void SetCamY(float value)
	{
		camY = value;
	}

	public void SetCanY(float value)
	{
		canY = value;
	}

	public float GetCanY()
	{
		return canY;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
