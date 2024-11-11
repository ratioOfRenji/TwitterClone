using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class KeyboardPanelAdjuster : MonoBehaviour
{
	[SerializeField] private RectTransform authPanel;  // The panel to move
	[SerializeField] private float offset = 300f;      // Offset to move the panel by when keyboard appears
	[SerializeField] private float hideDelay = 0.3f;   // Delay before moving the panel down
	private Vector3 _originalPosition;
	private bool _isKeyboardVisible;
	private CancellationTokenSource _cts;

	private void Start()
	{
		// Save the original position of the panel
		_originalPosition = authPanel.localPosition;
	}

	private void Update()
	{
		// Check if the keyboard is visible
		bool isKeyboardNowVisible = TouchScreenKeyboard.visible;

		if (isKeyboardNowVisible && !_isKeyboardVisible)
		{
			// Keyboard became visible - cancel any hide task and move the panel up
			_cts?.Cancel();
			MovePanelUp();
		}
		else if (!isKeyboardNowVisible && _isKeyboardVisible)
		{
			// Keyboard became hidden - start a delayed task to move the panel down
			StartDelayedMovePanelDown();
		}

		_isKeyboardVisible = isKeyboardNowVisible;
	}

	private void MovePanelUp()
	{
		authPanel.localPosition = _originalPosition + new Vector3(0, offset, 0);
	}

	private async void StartDelayedMovePanelDown()
	{
		// Cancel any ongoing token and create a new one for the current task
		_cts?.Cancel();
		_cts = new CancellationTokenSource();

		try
		{
			// Wait for the specified delay before moving down, unless canceled
			await UniTask.Delay((int)(hideDelay * 1000), cancellationToken: _cts.Token);
			authPanel.localPosition = _originalPosition;
		}
		catch (OperationCanceledException)
		{
			// Handle the task being canceled if the keyboard reopens
		}
	}

	private void OnDestroy()
	{
		// Cancel any active cancellation tokens when the object is destroyed
		_cts?.Cancel();
	}
}
