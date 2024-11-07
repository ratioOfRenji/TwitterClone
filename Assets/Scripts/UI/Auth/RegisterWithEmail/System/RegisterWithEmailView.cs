using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;
using System;
public class RegisterWithEmailView : MonoBehaviour
{
	[SerializeField] private TMP_InputField _emailInputField;
	[SerializeField] private TMP_InputField _passwordInputField;
	[SerializeField] private TMP_InputField _repeatPasswordInputField;

	[SerializeField] private TMP_Text _warrningText;

	[SerializeField] private Button _registerButton;
	public IObservable<Unit> OnRegisterButtonAsObservable()
	{
		return _registerButton.OnClickAsObservable();
	}
	public string Email()
	{
		return _emailInputField.text;
	}

	public string Password()
	{
		return _passwordInputField.text;
	}

	public string RepeatPassword()
	{
		return _repeatPasswordInputField.text;
	}

	public void DisplayWarning(bool active, string message ="")
	{
		_warrningText.text = message;
		_warrningText.gameObject.SetActive(active);
	}
}
