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
    public IObservable<Unit> OnApplyButtonAsObservable()
    {
        return _applyButton.OnClickAsObservable();
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
    }
}
