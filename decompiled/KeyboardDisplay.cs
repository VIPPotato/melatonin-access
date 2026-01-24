using System.Collections;
using UnityEngine;

public class KeyboardDisplay : Wrapper
{
	private float initLocalX;

	private bool isActivated;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		initLocalX = GetLocalX();
		sprites[0].ToggleIsRealTimeFader(toggle: true);
		sprites[1].ToggleIsRealTimeFader(toggle: true);
		sprites[2].ToggleIsRealTimeFader(toggle: true);
		sprites[3].ToggleIsRealTimeFader(toggle: true);
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		isActivated = true;
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true);
		sprites[0].FadeInSprite(1f, 0.33f);
		sprites[1].FadeInSprite(1f, 0.33f);
		sprites[1].TriggerAnim("key" + SaveManager.mgr.GetActionKey());
		if (SaveManager.mgr.CheckIsDirectionKeysAlt())
		{
			sprites[2].ToggleSpriteRenderer(toggle: false);
			sprites[3].FadeInSprite(1f, 0.33f);
		}
		else
		{
			sprites[2].FadeInSprite(1f, 0.33f);
			sprites[3].ToggleSpriteRenderer(toggle: false);
		}
		SetLocalX(initLocalX + 1f);
		MoveDistanceRealtime(new Vector3(-1f, 0f, 0f), 3f, isEasingIn: false);
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		sprites[0].FadeOutSprite(1f, 0.33f);
		sprites[1].FadeOutSprite(1f, 0.33f);
		if (SaveManager.mgr.CheckIsDirectionKeysAlt())
		{
			sprites[3].FadeOutSprite(1f, 0.33f);
		}
		else
		{
			sprites[2].FadeOutSprite(1f, 0.33f);
		}
		yield return new WaitForSecondsRealtime(0.33f);
		RenderChildren(toggle: false);
	}

	public void SetActionKey(string key)
	{
		sprites[1].TriggerAnim("key" + key);
	}

	public void ToggleIsDirectionKeysAlt(bool toggle)
	{
		if (toggle)
		{
			sprites[2].ToggleSpriteRenderer(toggle: false);
			sprites[3].ToggleSpriteRenderer(toggle: true);
		}
		else
		{
			sprites[2].ToggleSpriteRenderer(toggle: true);
			sprites[3].ToggleSpriteRenderer(toggle: false);
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
