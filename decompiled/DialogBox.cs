using System.Collections;
using UnityEngine;

public class DialogBox : Wrapper
{
	public Fragment speaker;

	public textboxFragment dialog;

	public textboxFragment leftGraphicText;

	public textboxFragment rightGraphicText;

	public Fragment paragraph;

	public textboxFragment label;

	public Fragment slider;

	public Fragment subslider;

	public Fragment graphic;

	public Fragment arrowGear;

	public Fragment prompt;

	private bool isActivated;

	private bool isSubInfoOpen;

	private bool isHighlight1 = true;

	private float timeTilOut;

	private Coroutine activating;

	private Coroutine deactivating;

	protected override void Awake()
	{
		speaker.Awake();
		dialog.Initiate();
		leftGraphicText.Initiate();
		rightGraphicText.Initiate();
		paragraph.Awake();
		label.Initiate();
		slider.Awake();
		subslider.Awake();
		graphic.Awake();
		arrowGear.Awake();
		prompt.Awake();
		timeTilOut = slider.GetAnimDuration("slideOut");
		RenderChildren(toggle: false, 1);
	}

	public void Show()
	{
		isActivated = true;
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 1);
		slider.TriggerAnim("idled");
		paragraph.TriggerAnim("idled");
		graphic.TriggerAnim("hidden");
	}

	public void Activate(bool isSoundTriggered)
	{
		isActivated = true;
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 1);
		slider.TriggerAnim("slideIn");
		paragraph.TriggerAnim("in");
		graphic.TriggerAnim("hidden");
		if (isSoundTriggered)
		{
			speaker.TriggerSound(0);
		}
	}

	public void ActivateDelayed(float delta, bool isSoundTriggered)
	{
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		activating = StartCoroutine(ActivatingDelayed(delta, isSoundTriggered));
	}

	private IEnumerator ActivatingDelayed(float delta, bool isSoundTriggered)
	{
		if (isSoundTriggered)
		{
			speaker.TriggerSoundDelayedDelta(delta, 0);
		}
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		isActivated = true;
		RenderChildren(toggle: true, 1);
		slider.TriggerAnim("slideIn");
		paragraph.TriggerAnim("in");
		graphic.TriggerAnim("hidden");
	}

	public void Deactivate(bool isSoundTriggered)
	{
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating(isSoundTriggered));
	}

	private IEnumerator Deactivating(bool isSoundTriggered)
	{
		isActivated = false;
		isSubInfoOpen = false;
		isHighlight1 = true;
		slider.TriggerAnim("slideOut");
		arrowGear.TriggerAnim("react");
		if (!graphic.CheckIsAnimPlaying("hidden"))
		{
			graphic.TriggerAnim("out");
		}
		else
		{
			paragraph.TriggerAnim("out");
		}
		if (isSoundTriggered)
		{
			speaker.TriggerSound(0);
		}
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false, 1);
	}

	public void DeactivateDelayed(float delta, bool isSoundTriggered)
	{
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(DeactivatingDelayed(delta, isSoundTriggered));
	}

	private IEnumerator DeactivatingDelayed(float delta, bool isSoundTriggered)
	{
		if (isSoundTriggered)
		{
			speaker.TriggerSoundDelayedDelta(delta, 0);
		}
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		isActivated = false;
		isSubInfoOpen = false;
		isHighlight1 = true;
		slider.TriggerAnim("slideOut");
		arrowGear.TriggerAnim("react");
		if (!graphic.CheckIsAnimPlaying("hidden"))
		{
			graphic.TriggerAnim("out");
		}
		paragraph.TriggerAnim("out");
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false, 1);
	}

	public void Hide()
	{
		isActivated = false;
		isHighlight1 = true;
		CancelCoroutine(activating);
		CancelCoroutine(deactivating);
		isSubInfoOpen = false;
		RenderChildren(toggle: false, 1);
	}

	public void ChangeToGraphic(int graphicNum, int newStateLeft, int newStateRight)
	{
		slider.TriggerAnim("press");
		paragraph.TriggerAnim("refresh");
		dialog.ToggleMeshRenderer(toggle: false);
		leftGraphicText.ToggleMeshRenderer(toggle: true);
		rightGraphicText.ToggleMeshRenderer(toggle: true);
		leftGraphicText.SetState(newStateLeft);
		rightGraphicText.SetState(newStateRight);
		graphic.TriggerAnim(graphicNum.ToString() ?? "");
		arrowGear.TriggerAnim("react");
		speaker.TriggerSound(0);
	}

	public void ChangeDialogState(int newState, float newFontSize = 4.4f, int newVerticalAlignment = 0, bool isDelayed = false)
	{
		StartCoroutine(ChangingDialogState(newState, newFontSize, newVerticalAlignment, isDelayed));
	}

	private IEnumerator ChangingDialogState(int newState, float newFontSize, int newVerticalAlignment, bool isDelayed)
	{
		if (isDelayed)
		{
			float num = 0.11667f;
			float checkpoint = Technician.mgr.GetDspTime() + num;
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		}
		slider.TriggerAnim("press");
		paragraph.TriggerAnim("refresh");
		dialog.ToggleMeshRenderer(toggle: true);
		dialog.SetState(newState);
		dialog.SetFontSize(newFontSize);
		dialog.SetVerticalAlignment(newVerticalAlignment);
		leftGraphicText.ToggleMeshRenderer(toggle: false);
		rightGraphicText.ToggleMeshRenderer(toggle: false);
		graphic.TriggerAnim("hidden");
		arrowGear.TriggerAnim("react");
		speaker.TriggerSound(0);
	}

	public void ToggleSubInfo(bool toggle)
	{
		isSubInfoOpen = toggle;
		if (isSubInfoOpen)
		{
			subslider.TriggerAnim("in");
			if (ControlHandler.mgr.GetCtrlType() == 1)
			{
				prompt.TriggerAnim("gamepadX");
			}
			else if (ControlHandler.mgr.GetCtrlType() == 2)
			{
				prompt.TriggerAnim("gamepadSQUARE");
			}
			else
			{
				prompt.TriggerAnim("keyR");
			}
		}
		else
		{
			subslider.TriggerAnim("out");
		}
	}

	public void SetDialogState(int newState, float newFontSize = 4.4f, int newVerticalAlignment = 0)
	{
		dialog.SetState(newState);
		dialog.SetFontSize(newFontSize);
		dialog.SetVerticalAlignment(newVerticalAlignment);
	}

	public void SetText(string newText)
	{
		dialog.SetText(newText);
	}

	public string GetText()
	{
		return dialog.GetText();
	}

	public bool CheckIsSubInfoOpen()
	{
		return isSubInfoOpen;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsHighlight1()
	{
		return isHighlight1;
	}
}
