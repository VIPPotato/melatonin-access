using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class textboxFragment : Custom
{
	[Serializable]
	public class state
	{
		[TextArea]
		public string[] translations;
	}

	public bool isSkewable;

	public state[] states;

	public List<string> stateNames = new List<string>();

	private bool isRealTimeFader;

	private MeshRenderer meshRenderer;

	private TextMeshPro textMeshPro;

	private Coroutine fadingText;

	public void Initiate()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		textMeshPro = GetComponent<TextMeshPro>();
		if (states.Length != 0)
		{
			SetState(0);
		}
		if (isSkewable)
		{
			meshRenderer.material = UnityEngine.Object.Instantiate(meshRenderer.material);
		}
	}

	public void FadeInText(float newAlpha, float duration)
	{
		CancelCoroutine(fadingText);
		fadingText = StartCoroutine(FadingInText(newAlpha, duration));
	}

	private IEnumerator FadingInText(float newAlpha, float duration)
	{
		meshRenderer.enabled = true;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = textMeshPro.color;
			color.a = Mathf.Lerp(0f, newAlpha, elapsed / duration);
			textMeshPro.color = color;
			yield return null;
		}
	}

	public void FadeOutText(float startAlpha, float duration)
	{
		CancelCoroutine(fadingText);
		fadingText = StartCoroutine(FadingOutText(startAlpha, duration));
	}

	private IEnumerator FadingOutText(float startAlpha, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed = (isRealTimeFader ? (elapsed + Time.unscaledDeltaTime) : (elapsed + Time.deltaTime));
			Color color = textMeshPro.color;
			color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
			textMeshPro.color = color;
			yield return null;
		}
		meshRenderer.enabled = false;
	}

	public void ToggleMeshRenderer(bool toggle)
	{
		CancelCoroutine(fadingText);
		meshRenderer.enabled = toggle;
	}

	public void ToggleIsRealTimeFader(bool toggle)
	{
		isRealTimeFader = toggle;
	}

	public void SetState(int num)
	{
		textMeshPro.text = states[num].translations[SaveManager.GetLang()];
	}

	public void SetStateByName(string name)
	{
		if (stateNames.Contains(name))
		{
			textMeshPro.text = states[stateNames.IndexOf(name)].translations[SaveManager.GetLang()];
		}
		else
		{
			textMeshPro.text = "";
		}
	}

	public void SetText(string newText)
	{
		textMeshPro.text = newText;
	}

	public void SetFontColor(Color newColor)
	{
		textMeshPro.color = newColor;
	}

	public void SetFontAlpha(float newAlpha)
	{
		meshRenderer.enabled = true;
		CancelCoroutine(fadingText);
		Color color = textMeshPro.color;
		color.a = newAlpha;
		textMeshPro.color = color;
	}

	public void SetFontSize(float newFontSize)
	{
		textMeshPro.fontSize = newFontSize;
	}

	public void SetFontBold(bool isBold)
	{
		if (isBold)
		{
			textMeshPro.fontStyle = FontStyles.Bold;
		}
		else
		{
			textMeshPro.fontStyle = FontStyles.Normal;
		}
	}

	public void SetLetterSpacing(float newLetterSpacing)
	{
		textMeshPro.characterSpacing = newLetterSpacing;
	}

	public void SetLineSpacing(float newLineSpacing)
	{
		textMeshPro.lineSpacing = newLineSpacing;
	}

	public void SetHorizontalAlignment(int alignNum)
	{
		switch (alignNum)
		{
		case 0:
			textMeshPro.alignment = TextAlignmentOptions.Left;
			break;
		case 1:
			textMeshPro.alignment = TextAlignmentOptions.Center;
			break;
		default:
			textMeshPro.alignment = TextAlignmentOptions.Right;
			break;
		}
	}

	public void SetVerticalAlignment(int alignNum)
	{
		switch (alignNum)
		{
		case 0:
			textMeshPro.verticalAlignment = VerticalAlignmentOptions.Top;
			break;
		case 1:
			textMeshPro.verticalAlignment = VerticalAlignmentOptions.Middle;
			break;
		default:
			textMeshPro.verticalAlignment = VerticalAlignmentOptions.Bottom;
			break;
		}
	}

	public bool CheckIsMeshRendered()
	{
		return meshRenderer.enabled;
	}

	public string GetText()
	{
		return textMeshPro.text;
	}

	public int GetCharacterCount()
	{
		return textMeshPro.text.Length;
	}

	public float GetFontAlpha()
	{
		return textMeshPro.color.a;
	}

	public int GetState()
	{
		for (int i = 0; i < states.Length; i++)
		{
			if (textMeshPro.text == states[i].translations[SaveManager.GetLang()])
			{
				return i;
			}
		}
		MonoBehaviour.print("error: no states");
		return 0;
	}

	public string GetStateName()
	{
		for (int i = 0; i < states.Length; i++)
		{
			if (textMeshPro.text == states[i].translations[SaveManager.GetLang()])
			{
				return stateNames[i];
			}
		}
		MonoBehaviour.print("error: no states or no matching name");
		return "";
	}
}
