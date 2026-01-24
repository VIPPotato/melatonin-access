using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderGroup : Wrapper
{
	public List<LadderColumn> LadderColumns = new List<LadderColumn>();

	private float initY;

	private LadderColumn LadderColumn_active;

	protected override void Awake()
	{
		SetupFragments();
		initY = GetY();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		foreach (LadderColumn ladderColumn in LadderColumns)
		{
			ladderColumn.Hide();
		}
		LadderColumn_active = LadderColumns[0];
		LadderColumn_active.Show();
		LadderColumn_active.SetPosition(0f, initY);
	}

	public void Hide()
	{
		foreach (LadderColumn ladderColumn in LadderColumns)
		{
			ladderColumn.Hide();
		}
		RenderChildren(toggle: false);
	}

	public void HintLeftDelayed(float timeStarted)
	{
		StartCoroutine(HintingLeftDelayed(timeStarted));
	}

	private IEnumerator HintingLeftDelayed(float timeStarted)
	{
		LadderColumn LadderColumn_prev = LadderColumn_active;
		LadderColumns.RemoveAt(0);
		LadderColumns.Add(LadderColumn_prev);
		LadderColumn_active = LadderColumns[0];
		LadderColumn_active.SetPosition(LadderColumn_prev.GetX() - 5.5f, LadderColumn_prev.GetY());
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		LadderColumn_active.Activate(1);
		checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		LadderColumn_prev.Deactivate("Right");
	}

	public void HintRightDelayed(float timeStarted)
	{
		StartCoroutine(HintingRightDelayed(timeStarted));
	}

	private IEnumerator HintingRightDelayed(float timeStarted)
	{
		LadderColumn LadderColumn_prev = LadderColumn_active;
		LadderColumns.RemoveAt(0);
		LadderColumns.Add(LadderColumn_prev);
		LadderColumn_active = LadderColumns[0];
		LadderColumn_active.SetPosition(LadderColumn_prev.GetX() + 5.5f, LadderColumn_prev.GetY());
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		LadderColumn_active.Activate(2);
		checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		LadderColumn_prev.Deactivate("Left");
	}

	public LadderColumn GetLadderColumnActive()
	{
		return LadderColumn_active;
	}
}
