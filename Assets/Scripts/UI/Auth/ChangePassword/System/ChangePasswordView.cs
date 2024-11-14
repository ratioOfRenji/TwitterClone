using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class ChangePasswordView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _currentPassword;
    [SerializeField] private TMP_InputField _newPassword;
    [SerializeField] private Button _applyButton;
    [SerializeField] private GameObject _ChangePasswordPanel;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _successText;
    public IObservable<Unit> OnApplyAsObservable()
    {
        return _applyButton.OnClickAsObservable();
    }

    public string CurrentPasswordText()
    {
        return _currentPassword.text;
    }

    public string NewPasswordText()
    {
        return _newPassword.text;
    }

    public void ShowPanel(bool show)
    {
		_ChangePasswordPanel.SetActive(show);

	}
    public void ShowErrorText(bool show, string message ="Unable to change password")
    {
        _errorText.text = message;
        _errorText.gameObject.SetActive(show);
    }

    public void ShowSuccessText(bool show)
    {
        _successText.gameObject.SetActive(show);
    }
}
