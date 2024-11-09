using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class ChangeDisplayedNameView : MonoBehaviour
{
	[SerializeField] private Button _applyButton;
	public IObservable<Unit> OnApplyButtonAsObservable()
	{
		return _applyButton.OnClickAsObservable();
	}
	[SerializeField] private TMP_InputField _changeNameInputField;

	public string NewDisplayedName()
	{
		return _changeNameInputField.text;
	}

	
}
