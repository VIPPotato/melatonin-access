using System.Collections;
using UnityEngine;

public class Wrapper : Custom
{
	public Fragment[] gears;

	public Fragment[] sprites;

	public Fragment[] speakers;

	public Fragment[] textboxes;

	private bool isTracking;

	private Vector3 target;

	private Vector3 localTarget;

	private Coroutine moving;

	private Coroutine tracking;

	protected virtual void Awake()
	{
	}

	protected void SetupFragments()
	{
		for (int i = 0; i < gears.Length; i++)
		{
			if (gears[i] != null)
			{
				gears[i].Awake();
			}
		}
		for (int j = 0; j < sprites.Length; j++)
		{
			if (sprites[j] != null)
			{
				sprites[j].Awake();
			}
		}
		for (int k = 0; k < speakers.Length; k++)
		{
			if (speakers[k] != null)
			{
				speakers[k].Awake();
			}
		}
		for (int l = 0; l < textboxes.Length; l++)
		{
			if (textboxes[l] != null)
			{
				textboxes[l].Awake();
			}
		}
	}

	public void RenderChildren(bool toggle, int startIndex = 0)
	{
		for (int i = startIndex; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(toggle);
		}
	}

	public void MoveToTarget(Vector3 newTarget, float maxSpeed = 4f, bool isEasingIn = true)
	{
		CancelMoving();
		CancelTracking();
		moving = StartCoroutine(MovingToTarget(newTarget, maxSpeed, isEasingIn));
	}

	private IEnumerator MovingToTarget(Vector3 newTarget, float maxSpeed, bool isEasingIn)
	{
		SetTarget(newTarget);
		Vector3 difference = target - base.transform.position;
		float activeSpeed = 0.15f;
		float timer = 0f;
		if (!isEasingIn)
		{
			activeSpeed = maxSpeed;
		}
		while (Mathf.Abs(difference.x) > 0.015f || Mathf.Abs(difference.y) > 0.015f)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.015f)
				{
					activeSpeed *= 2.15f;
					timer -= 0.015f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.position = Vector3.Lerp(base.transform.position, target, activeSpeed * (Time.deltaTime * 2f));
			difference = target - base.transform.position;
			yield return null;
		}
		base.transform.position = target;
	}

	public void MoveToLocalTarget(Vector3 newLocalTarget, float maxSpeed = 4f, bool isEasingIn = true)
	{
		CancelMoving();
		CancelTracking();
		moving = StartCoroutine(MovingToLocalTarget(newLocalTarget, maxSpeed, isEasingIn));
	}

	private IEnumerator MovingToLocalTarget(Vector3 newLocalTarget, float maxSpeed, bool isEasingIn)
	{
		SetLocalTarget(newLocalTarget);
		Vector3 difference = localTarget - base.transform.localPosition;
		float activeSpeed = 0.1f;
		float timer = 0f;
		if (!isEasingIn)
		{
			activeSpeed = maxSpeed;
		}
		while (Mathf.Abs(difference.x) > 0.015f || Mathf.Abs(difference.y) > 0.015f)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.015f)
				{
					activeSpeed *= 2.15f;
					timer -= 0.015f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, localTarget, activeSpeed * (Time.deltaTime * 2f));
			difference = localTarget - base.transform.localPosition;
			yield return null;
		}
		base.transform.localPosition = localTarget;
	}

	public void MoveDistance(Vector3 distance, float maxSpeed = 4f, bool isEasingIn = true)
	{
		CancelMoving();
		CancelTracking();
		moving = StartCoroutine(MovingDistance(distance, maxSpeed, isEasingIn));
	}

	private IEnumerator MovingDistance(Vector3 distance, float maxSpeed, bool isEasingIn)
	{
		Vector3 newPosition = base.transform.localPosition + distance;
		newPosition.z = base.transform.localPosition.z;
		Vector3 difference = newPosition - base.transform.localPosition;
		float activeSpeed = 0.1f;
		float timer = 0f;
		if (!isEasingIn)
		{
			activeSpeed = maxSpeed;
		}
		while (Mathf.Abs(difference.x) > 0.015f || Mathf.Abs(difference.y) > 0.015f)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.015f)
				{
					activeSpeed *= 2.15f;
					timer -= 0.015f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, newPosition, activeSpeed * (Time.deltaTime * 2f));
			difference = newPosition - base.transform.localPosition;
			yield return null;
		}
		base.transform.localPosition = newPosition;
	}

	public void MoveDistanceRealtime(Vector3 distance, float maxSpeed = 4f, bool isEasingIn = true)
	{
		CancelMoving();
		CancelTracking();
		moving = StartCoroutine(MovingDistanceRealtime(distance, maxSpeed, isEasingIn));
	}

	private IEnumerator MovingDistanceRealtime(Vector3 distance, float maxSpeed, bool isEasingIn)
	{
		Vector3 newPosition = base.transform.localPosition + distance;
		newPosition.z = base.transform.localPosition.z;
		Vector3 difference = newPosition - base.transform.localPosition;
		float activeSpeed = 0.1f;
		float timer = 0f;
		if (!isEasingIn)
		{
			activeSpeed = maxSpeed;
		}
		while (Mathf.Abs(difference.x) > 0.006f || Mathf.Abs(difference.y) > 0.006f)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.015f)
				{
					activeSpeed *= 2.15f;
					timer -= 0.015f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, newPosition, activeSpeed * (Time.unscaledDeltaTime * 2f));
			difference = newPosition - base.transform.localPosition;
			yield return null;
		}
		base.transform.localPosition = newPosition;
	}

	public void CancelMoving()
	{
		CancelCoroutine(moving);
	}

	public void TrackTarget(Vector3 newTarget, float maxSpeed = 4f, bool isEasingIn = true)
	{
		CancelMoving();
		CancelTracking();
		tracking = StartCoroutine(TrackingTarget(newTarget, maxSpeed, isEasingIn));
	}

	private IEnumerator TrackingTarget(Vector3 newTarget, float maxSpeed, bool isEasingIn)
	{
		isTracking = true;
		SetTarget(newTarget);
		float activeSpeed = 0.1f;
		float timer = 0f;
		if (!isEasingIn)
		{
			activeSpeed = maxSpeed;
		}
		while (isTracking)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.015f)
				{
					activeSpeed *= 2.15f;
					timer -= 0.015f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.position = Vector3.Lerp(base.transform.position, target, activeSpeed * (Time.deltaTime * 2f));
			yield return null;
		}
		Vector3 difference = target - base.transform.position;
		while (Mathf.Abs(difference.x) > 0.015f || Mathf.Abs(difference.y) > 0.015f)
		{
			if (activeSpeed != maxSpeed)
			{
				timer += Time.deltaTime;
				if (timer > 0.25f)
				{
					activeSpeed *= 1.5f;
				}
				if (activeSpeed > maxSpeed)
				{
					activeSpeed = maxSpeed;
				}
			}
			base.transform.position = Vector3.Lerp(base.transform.position, target, activeSpeed * (Time.deltaTime * 2f));
			difference = target - base.transform.position;
			yield return null;
		}
		base.transform.position = target;
	}

	public void StopTracking()
	{
		isTracking = false;
	}

	public void CancelTracking()
	{
		isTracking = false;
		CancelCoroutine(tracking);
	}

	public void SetTarget(Vector3 newTarget)
	{
		newTarget.z = base.transform.position.z;
		target = newTarget;
	}

	public void SetLocalTarget(Vector3 newLocalTarget)
	{
		newLocalTarget.z = base.transform.localPosition.z;
		localTarget = newLocalTarget;
	}

	public override void SetPosition(float newX, float newY)
	{
		CancelMoving();
		base.SetPosition(newX, newY);
	}

	public override void SetX(float newX)
	{
		CancelMoving();
		base.SetX(newX);
	}

	public override void SetY(float newY)
	{
		CancelMoving();
		base.SetY(newY);
	}

	public override void SetLocalPosition(float newX, float newY)
	{
		CancelMoving();
		base.SetLocalPosition(newX, newY);
	}

	public override void SetLocalX(float newX)
	{
		CancelMoving();
		base.SetLocalX(newX);
	}

	public override void SetLocalY(float newY)
	{
		CancelMoving();
		base.SetLocalY(newY);
	}

	public override void SetDistance(float distanceX, float distanceY)
	{
		CancelMoving();
		base.SetDistance(distanceX, distanceY);
	}
}
