using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class PickingProfileIconView : MonoBehaviour
{
	[SerializeField] private Button _iconButton;
	public IObservable<Unit> OnIconButtonClickedAsObservable()
	{
		return _iconButton.OnClickAsObservable();
	}

	[SerializeField] private EIconType _iconType;

	public EIconType IconType => _iconType;

	[SerializeField] private GameObject _checkMark;

	public void ShowCheckMark(bool acrive)
	{
		_checkMark.SetActive(acrive);
	}
}
