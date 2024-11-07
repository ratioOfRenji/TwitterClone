using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class SignInEmailView : MonoBehaviour
{
	[SerializeField] private TMP_InputField _emailInputField;
	[SerializeField] private TMP_InputField _passwordInputField;

	

	public string Email()
	{
		return _emailInputField.text;
	}

	public string Password()
	{
		return _passwordInputField.text;
	}
}
