using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class ConfirmEmailView : MonoBehaviour
{
	[SerializeField] private TMP_InputField _codeInputField;
	[SerializeField] private Button _confirmButton;
	[SerializeField] private TMP_Text _errorText;
	public IObservable<Unit> OnConfirmButtonAsObservable()
	{
		return _confirmButton.OnClickAsObservable();
	}

	public string CodeText()
	{
		return _codeInputField.text;
	}
	public void DisplayErrorText(bool active, string message ="")
	{
		_errorText.gameObject.SetActive(active);

		_errorText.text = message;
	}
}
