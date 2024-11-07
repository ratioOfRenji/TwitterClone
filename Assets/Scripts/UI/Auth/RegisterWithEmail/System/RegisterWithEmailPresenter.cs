using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;

public class RegisterWithEmailPresenter : IInitializable, IDisposable
{
	private readonly RegisterWithEmail _model;
	private readonly RegisterWithEmailView _view;
	private readonly AuthScreenSwicher _screenSwicher;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public RegisterWithEmailPresenter(RegisterWithEmail model, RegisterWithEmailView view, AuthScreenSwicher screenSwicher)
	{
		_model = model;
		_view = view;
		_screenSwicher = screenSwicher;
	}
	public void Initialize()
	{
		_view.OnRegisterButtonAsObservable().Subscribe(_ => { OnRegisterButtonClick(); }).AddTo(_disposables);
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

	private async UniTask OnRegisterButtonClick()
	{
		_view.DisplayWarning(false);
		bool passwordMatch = _model.CheckPasswordMatching(_view.Password(), _view.RepeatPassword());
		if (passwordMatch)
		{
			bool success= await _model.Register(_view.Email(), _view.Password());
			if (success)
			{
				_screenSwicher.ShowConfirmEmailScreen();
			}
		}
		else
		{
			_view.DisplayWarning(true, "Passwords doesnt match");
		}

	}
	
}
