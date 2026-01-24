using UnityEngine;

public class Custom : MonoBehaviour
{
	protected const float windowDuration = 0.23334f;

	protected const float windowDurationHalf = 0.11667f;

	protected const float windowDurationEarly = 0.067f;

	protected const float windowDurationPerfect = 0.09334f;

	protected const float windowDurationLate = 0.073f;

	protected const float windowDurationEarlyBig = 0.051f;

	protected const float windowDurationPerfectBig = 0.12534f;

	protected const float windowDurationLateBig = 0.057f;

	public void Delete()
	{
		Object.Destroy(base.gameObject);
	}

	protected void CancelCoroutine(Coroutine coroutine)
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}

	public void SetParent(Transform newParent)
	{
		base.transform.SetParent(newParent, worldPositionStays: true);
	}

	public void SetParentAndReposition(Transform newParent)
	{
		base.transform.SetParent(newParent, worldPositionStays: false);
	}

	public virtual void SetPosition(float newX, float newY)
	{
		base.transform.position = new Vector3(newX, newY, GetZ());
	}

	public virtual void SetX(float newX)
	{
		base.transform.position = new Vector3(newX, GetY(), GetZ());
	}

	public virtual void SetY(float newY)
	{
		base.transform.position = new Vector3(GetX(), newY, GetZ());
	}

	public virtual void SetZ(float newZ)
	{
		base.transform.position = new Vector3(GetX(), GetY(), newZ);
	}

	public virtual void SetLocalPosition(float newX, float newY)
	{
		base.transform.localPosition = new Vector3(newX, newY, GetLocalZ());
	}

	public virtual void SetLocalX(float newLocalX)
	{
		base.transform.localPosition = new Vector3(newLocalX, GetLocalY(), GetLocalZ());
	}

	public virtual void SetLocalY(float newLocalY)
	{
		base.transform.localPosition = new Vector3(GetLocalX(), newLocalY, GetLocalZ());
	}

	public void SetLocalZ(float newLocalZ)
	{
		base.transform.localPosition = new Vector3(GetLocalX(), GetLocalY(), newLocalZ);
	}

	public virtual void SetLocalDistance(float distanceX, float distanceY)
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x + distanceX, base.transform.localPosition.y + distanceY, base.transform.localPosition.z);
	}

	public virtual void SetDistance(float distanceX, float distanceY)
	{
		base.transform.position = new Vector3(base.transform.position.x + distanceX, base.transform.position.y + distanceY, base.transform.position.z);
	}

	public void SetLocalScale(float newWidth, float newHeight)
	{
		base.transform.localScale = new Vector3(newWidth, newHeight, 1f);
	}

	public void SetLocalEulerAngles(float x, float y, float z)
	{
		base.transform.localEulerAngles = new Vector3(x, y, z);
	}

	public Vector3 GetPosition()
	{
		return base.transform.position;
	}

	public float GetX()
	{
		return base.transform.position.x;
	}

	public float GetY()
	{
		return base.transform.position.y;
	}

	public float GetZ()
	{
		return base.transform.position.z;
	}

	public Vector3 GetLocalPosition()
	{
		return base.transform.localPosition;
	}

	public float GetLocalX()
	{
		return base.transform.localPosition.x;
	}

	public float GetLocalY()
	{
		return base.transform.localPosition.y;
	}

	public float GetLocalZ()
	{
		return base.transform.localPosition.z;
	}

	public Vector3 GetLocalScale()
	{
		return base.transform.localScale;
	}

	public float GetLocalWidth()
	{
		return base.transform.localScale.x;
	}

	public float GetLocalHeight()
	{
		return base.transform.localScale.y;
	}
}
