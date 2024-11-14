using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		ApplySafeArea();
	}

	private void OnRectTransformDimensionsChange()
	{
		ApplySafeArea();
	}

	private void ApplySafeArea()
	{
		// Get the safe area from the device
		Rect safeArea = Screen.safeArea;

		// Convert safe area rectangle into normalized anchor coordinates (0 to 1 range)
		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;

		// Normalize the safe area
		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;

		// Apply anchor coordinates to the RectTransform
		_rectTransform.anchorMin = anchorMin;
		_rectTransform.anchorMax = anchorMax;

		// Optionally, update the position if you want to avoid stretching issues
		_rectTransform.anchoredPosition = Vector2.zero;
	}

	public float CalculateSafeAreaHeight()
	{
		// Get the safe area from the device
		Rect safeArea = Screen.safeArea;

		// Convert safe area rectangle into normalized anchor coordinates (0 to 1 range)
		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;
		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;

		// Apply anchor coordinates to the RectTransform
		_rectTransform.anchorMin = anchorMin;
		_rectTransform.anchorMax = anchorMax;

		return Screen.height - (safeArea.y + safeArea.height);
	}
}
