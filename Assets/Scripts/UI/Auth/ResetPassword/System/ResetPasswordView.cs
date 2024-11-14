using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
public class ResetPasswordView : MonoBehaviour
{
	[SerializeField] private TMP_InputField _emailInputField;
	[SerializeField] private Button _applyButton;
	[SerializeField] private GameObject _resetPasswordPanel;
	[SerializeField] private Button _backButton;
	[SerializeField] private Button _popUpButton;
	[SerializeField] private TMP_Text _errorText;

	public IObservable<Unit> OnApplyButtonAsObservable()
	{
		return _applyButton.OnClickAsObservable();
	}
	public IObservable<Unit> OnBackButonAsObservable()
	{
		return Observable.Merge<Unit>( new [] { _backButton.OnClickAsObservable(), _popUpButton.OnClickAsObservable() });
	}
	public string EmailText()
	{
		return _emailInputField.text;
	}

	public void ShowPanel(bool show)
	{
		_resetPasswordPanel.SetActive(show);
	}

	public void ShowError(bool show, string message = "failed to reset password")
	{
		_errorText.text = message;
		_errorText.gameObject.SetActive(show);
	}
}
