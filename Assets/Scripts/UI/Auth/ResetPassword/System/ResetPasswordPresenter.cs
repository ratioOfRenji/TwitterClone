using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class ResetPasswordPresenter : IInitializable, IDisposable
{
	private readonly ResetPasswordView _view;
	private readonly ResetPassword _model;
	private readonly AuthScreenSwicher _screenSwicher;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public ResetPasswordPresenter(ResetPasswordView view, ResetPassword model, AuthScreenSwicher screenSwicher)
	{
		_view = view;
		_model = model;
		_screenSwicher = screenSwicher;
	}
	public void Initialize()
	{
		_view.OnApplyButtonAsObservable().Subscribe(_ => CallEndpoint()).AddTo(_disposables);
		_view.OnBackButonAsObservable().Subscribe(_ => _screenSwicher.ShowSignInScreen()).AddTo(_disposables);
	}

	private async void CallEndpoint()
	{
		_view.ShowError(false);
		if (_view.EmailText().Length< 5)
		{
			_view.ShowError(true, "password is to short");
			return;
		}
		bool success = await _model.ResetPasswordAsync(_view.EmailText());
		if (success)
		{
			_screenSwicher.ShowPasswordResetPopUp(true);
		}
		else
		{
			_view.ShowError(true);
		}
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}
}
