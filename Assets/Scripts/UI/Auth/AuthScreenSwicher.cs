using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthScreenSwicher : MonoBehaviour
{

	[SerializeField] private GameObject _registerScreen;
	[SerializeField] private GameObject _signInScreen;
	[SerializeField] private GameObject _confirmEmailScreen;
	[SerializeField] private GameObject _loadingScreen;

	
	public void ShowRegisterScreen()
	{
		_registerScreen.SetActive(true);
		_signInScreen.SetActive(false);
		_confirmEmailScreen.SetActive(false);
	}

	public void ShowSignInScreen()
	{
		_registerScreen.gameObject.SetActive(false);
		_signInScreen.gameObject.SetActive(true);
		_confirmEmailScreen.SetActive(false);
	}

	public void ShowConfirmEmailScreen()
	{
		_registerScreen.gameObject.SetActive(false);
		_signInScreen.SetActive(false);
		_confirmEmailScreen.SetActive(true);
	}

	public void ShowLoadingScreen(bool active)
	{
		_loadingScreen .SetActive(active);
	}
}
