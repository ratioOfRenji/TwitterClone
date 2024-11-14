using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class CreateNewPostView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _postText;

    [SerializeField] private Button _applyButton;
    [SerializeField] private GameObject _createBlogPanel;

    [SerializeField] private Button _showButton;

    [SerializeField] private Button _hideButton;
    public IObservable<Unit> OnApplyButtonAsObservable()
    {
        return _applyButton.OnClickAsObservable();
    }

    public IObservable<Unit> OnShowAsObservable()
    {
        return _showButton.OnClickAsObservable();
    }

    public IObservable<Unit> OnHideAsObservable()
    {
        return _hideButton.OnClickAsObservable();
    }

    public string PostText()
    {
        return _postText.text;
    }
    public void ClearInputField()
    {
        _postText.text = string.Empty;
    }
    public void ShowPanel(bool active)
    {
        _createBlogPanel.SetActive(active);
  //      if (active)
  //      {
		//	Debug.Log("Setting status bar to color 3B3B3F with light icons");
		//	StatusBarController.SetStatusBarAppearance(new Color32(0x3B, 0x3B, 0x3F, 0xFF), true);
		//}
  //      else
  //      {
		//	Debug.Log("Setting status bar to gray color  with light icons");
		//	StatusBarController.SetStatusBarAppearance(Color.gray, true);
  //      }
    }
}
