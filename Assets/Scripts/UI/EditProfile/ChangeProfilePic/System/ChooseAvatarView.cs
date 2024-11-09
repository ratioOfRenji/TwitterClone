using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class ChooseAvatarView : MonoBehaviour
{
	[SerializeField] private List<PickingProfileIconView> pickingProfileIconViewsList;
	public List<PickingProfileIconView> PickingProfileIconViewsList => pickingProfileIconViewsList;


	[SerializeField] Button _applyButton;
	public IObservable<Unit> OnApplyButtonAsObservable()
	{
		return _applyButton.OnClickAsObservable();
	}

	[SerializeField] private Button _backButton;
	public IObservable<Unit> OnBackButtonAsObservable()
	{
		return _backButton.OnClickAsObservable();
	}

	[SerializeField] private Button openPanelButton;
	public IObservable<Unit> OnOpenPanelButtonAsObservable()
	{
		return openPanelButton.OnClickAsObservable();
	}

	[SerializeField] private GameObject _chooseAvatarPanel;

	public void ShowPanel(bool active)
	{
		_chooseAvatarPanel.SetActive(active);
	}
}
