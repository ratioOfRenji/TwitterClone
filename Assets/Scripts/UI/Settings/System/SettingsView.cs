using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class SettingsView : MonoBehaviour
{
	[SerializeField] private Button _signOutButton;
	[SerializeField] private Button _deleteUserButton;

	public IObservable<Unit> OnSignOutAsObservable()
	{
		return _signOutButton.OnClickAsObservable();
	}

	public IObservable<Unit> OnDeleteUserAsObservable()
	{
		return _deleteUserButton.OnClickAsObservable();
	}
}
