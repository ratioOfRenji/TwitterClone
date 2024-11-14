using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ChangePasswordPresenter: IInitializable, IDisposable
{
	private readonly ChangePasswordView _view;
	private readonly ChangePassword _model;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public ChangePasswordPresenter(ChangePasswordView view, ChangePassword model)
	{
		_view = view;
		_model = model;
	}

	
	public void Initialize()
	{
		_view.OnApplyAsObservable().Subscribe(_ => OnApplyButtonClicked()).AddTo(_disposables);
	}
	private async void OnApplyButtonClicked()
	{
		_view.ShowErrorText(false);
		if(_view.NewPasswordText().Length < 8)
		{
			_view.ShowErrorText(true, "new password is to short");
			return;
		}
		bool success = await _model.ChangePasswordAsync(_view.CurrentPasswordText(), _view.NewPasswordText());
		if(success)
		{
			_view.ShowSuccessText(true);
		}
		else
		{
			_view.ShowErrorText(true);
		}
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

}
