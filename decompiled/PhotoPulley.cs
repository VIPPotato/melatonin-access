using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPulley : Wrapper
{
	[Header("Children")]
	public MemoryPhoto[] MemoryPhotos;

	[Header("Fragments")]
	public Fragment dragger;

	public Fragment elevator;

	private bool isPhotoQueued;

	private bool isQueuedGood;

	private int queuedSize;

	private int timesDragged;

	private int dragType;

	private int spawnNum;

	private float photoWidth = 3.5418f;

	private List<int> doubleDrags = new List<int>();

	private List<MemoryPhoto> burnableMemoryPhotos = new List<MemoryPhoto>();

	private MemoryPhoto dissolvableMemoryPhoto;

	private Coroutine dragging;

	private Coroutine linearMoving;

	protected override void Awake()
	{
		dragger.Awake();
		elevator.Awake();
	}

	public void Show()
	{
		SetLocalX(0f);
	}

	public void Hide()
	{
		timesDragged = 0;
		spawnNum = 0;
		isPhotoQueued = false;
		burnableMemoryPhotos.Clear();
		dissolvableMemoryPhoto = null;
		doubleDrags.Clear();
		CancelCoroutine(dragging);
		CancelCoroutine(linearMoving);
		MemoryPhoto[] memoryPhotos = MemoryPhotos;
		for (int i = 0; i < memoryPhotos.Length; i++)
		{
			memoryPhotos[i].Hide();
		}
	}

	public void QueuePhoto(bool newIsQueuedGood, int newQueuedSize)
	{
		isPhotoQueued = true;
		isQueuedGood = newIsQueuedGood;
		queuedSize = newQueuedSize;
	}

	public void DragDelayed(float delta)
	{
		CancelCoroutine(dragging);
		dragging = StartCoroutine(DraggingDelayed(delta));
	}

	private IEnumerator DraggingDelayed(float delta)
	{
		float checkpoint = MusicBox.env.GetSongTime() + 0.11667f - delta;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPhotoQueued)
		{
			isPhotoQueued = false;
			MemoryPhotos[spawnNum].Setup(isQueuedGood, queuedSize);
			MemoryPhotos[spawnNum].SetLocalPosition(-14.38f - (float)timesDragged * photoWidth, 3.1f);
			MemoryPhotos[spawnNum].Show(checkpoint, isBurnDelayed: false);
			spawnNum = ((spawnNum + 1 < MemoryPhotos.Length) ? (spawnNum + 1) : 0);
			if (queuedSize == 1)
			{
				doubleDrags.Add(timesDragged + 5);
				MemoryPhotos[spawnNum].Setup(newIsGood: true, 1);
				MemoryPhotos[spawnNum].SetLocalPosition(-14.45f - photoWidth / 2f - (float)timesDragged * photoWidth, 3.1f);
				MemoryPhotos[spawnNum].Show(checkpoint, isBurnDelayed: true);
				spawnNum = ((spawnNum + 1 < MemoryPhotos.Length) ? (spawnNum + 1) : 0);
			}
		}
		timesDragged++;
		SetLocalX((float)timesDragged * photoWidth);
		if (dragType == 0)
		{
			dragger.ToggleAnimator(toggle: true);
			foreach (int doubleDrag in doubleDrags)
			{
				if (timesDragged == doubleDrag)
				{
					dragger.TriggerAnim("staggerDouble", Darkroom.env.GetSpeed());
					doubleDrags.Remove(doubleDrag);
					yield break;
				}
			}
			dragger.TriggerAnim("stagger", Darkroom.env.GetSpeed());
		}
		else if (dragType == 1)
		{
			dragger.ToggleAnimator(toggle: true);
			dragger.TriggerAnim("staggerDouble", Darkroom.env.GetSpeed());
		}
		else if (dragType == 2)
		{
			LinearMove(checkpoint);
		}
	}

	private void LinearMove(float timeStarted)
	{
		CancelCoroutine(linearMoving);
		linearMoving = StartCoroutine(LinearMoving(timeStarted));
	}

	private IEnumerator LinearMoving(float timeStarted)
	{
		Vector3 startLocalPosition = new Vector3(photoWidth * -1f, 0f, dragger.GetLocalZ());
		Vector3 endLocalPosition = new Vector3(0f, 0f, dragger.GetLocalZ());
		dragger.ToggleAnimator(toggle: false);
		dragger.transform.localPosition = startLocalPosition;
		float duration = MusicBox.env.GetSecsPerBeat();
		while (dragger.GetLocalPosition() != endLocalPosition)
		{
			dragger.transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, (MusicBox.env.GetSongTime() - timeStarted) / duration);
			yield return null;
		}
	}

	public void Out()
	{
		elevator.TriggerAnim("out", Darkroom.env.GetSpeed());
	}

	public void SetDragType(int value)
	{
		dragType = value;
	}

	public void AddBurnableMemoryPhoto(MemoryPhoto newBurnableMemoryPhoto)
	{
		burnableMemoryPhotos.Add(newBurnableMemoryPhoto);
	}

	public void RemoveBurnableMemoryPhoto(MemoryPhoto removedBurnableMemoryPhoto)
	{
		burnableMemoryPhotos.Remove(removedBurnableMemoryPhoto);
	}

	public void SetDissolvableMemoryPhoto(MemoryPhoto newDissolvableMemoryPhoto)
	{
		dissolvableMemoryPhoto = newDissolvableMemoryPhoto;
	}

	public void ClearDissolvableMemoryPhoto()
	{
		dissolvableMemoryPhoto = null;
	}

	public List<MemoryPhoto> GetBurnableMemoryPhotos()
	{
		return burnableMemoryPhotos;
	}

	public MemoryPhoto GetDissolvableMemoryPhoto()
	{
		return dissolvableMemoryPhoto;
	}
}
