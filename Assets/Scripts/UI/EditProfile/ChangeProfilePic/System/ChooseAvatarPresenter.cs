using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class ChooseAvatarPresenter: IInitializable, IDisposable
{
	private ChooseAvatarView _view;
	private ChooseAvatarModel _model;
	private CompositeDisposable _disposables = new CompositeDisposable();

	private Subject<Unit> profilePicUpdatedSubject = new Subject<Unit>();
	public IObservable<Unit> ProfilePicUpdatedAsObservable()
	{
		return profilePicUpdatedSubject.AsObservable();
	}
	public ChooseAvatarPresenter(ChooseAvatarView chooseAvatarView, ChooseAvatarModel chooseAvatarModel)
	{
		_view = chooseAvatarView;
		_model = chooseAvatarModel;
	}

	
	public void Initialize()
	{
		ApplySubscribtions();
	}

	private void ApplySubscribtions()
	{
		foreach (PickingProfileIconView iconView in _view.PickingProfileIconViewsList)
		{
			iconView.OnIconButtonClickedAsObservable().Subscribe(_ => RedrawCheckMarks(iconView)).AddTo(_disposables);
			iconView.OnIconButtonClickedAsObservable().Subscribe(_ => { _model.UpdateCachedImage(iconView.IconType); Debug.Log(iconView.IconType); }).AddTo(_disposables);
		}
		_view.OnApplyButtonAsObservable().Subscribe(_ => UpdateProfilePickAndNotify()).AddTo(_disposables);
		_view.OnOpenPanelButtonAsObservable().Subscribe(_ =>_view.ShowPanel(true)).AddTo(_disposables);
		_view.OnBackButtonAsObservable().Subscribe(_ => _view.ShowPanel(false)).AddTo(_disposables);
	}
	private async void UpdateProfilePickAndNotify()
	{
		bool success = await _model.UpdateProfilePick();
		if( success)
		{
			Debug.Log("updated icon successfuly!");
			profilePicUpdatedSubject.OnNext(Unit.Default);
		}
	}
	private void RedrawCheckMarks(PickingProfileIconView iconView)
	{
		foreach (PickingProfileIconView _iconView in _view.PickingProfileIconViewsList)
		{
			_iconView.ShowCheckMark(false);
		}
		iconView.ShowCheckMark(true);
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

}
