using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;
using System;
using UniRx;

public class SignInEmailView : MonoBehaviour
{
	[SerializeField] private TMP_InputField _emailInputField;
	[SerializeField] private TMP_InputField _passwordInputField;
	[SerializeField] private TMP_Text _errorText;
	[SerializeField] private Button _signInButton;
	public IObservable<Unit> OnSignInButtonAsObservable()
	{
		return _signInButton.OnClickAsObservable();
	}

	public string Email()
	{
		return _emailInputField.text;
	}

	public string Password()
	{
		return _passwordInputField.text;
	}

	public void DispayErrorText(bool active, string message = "unable to sign in")
	{
		_errorText.text = message;
		_errorText.gameObject.SetActive(active);
	}
}
