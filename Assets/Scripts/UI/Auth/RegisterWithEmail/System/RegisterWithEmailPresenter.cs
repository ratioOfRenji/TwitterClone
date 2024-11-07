using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class RegisterWithEmailPresenter : IInitializable, IDisposable
{
	private readonly RegisterWithEmail _model;
	private readonly RegisterWithEmailView _view;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public RegisterWithEmailPresenter(RegisterWithEmail model, RegisterWithEmailView view)
	{
		_model = model;
		_view = view;
	}
	public void Initialize()
	{
		_view.OnRegisterButtonAsObservable().Subscribe(_ => { OnRegisterButtonClick(); }).AddTo(_disposables);
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void OnRegisterButtonClick()
	{
		_view.DisplayWarning(false);
		bool passwordMatch = _model.CheckPasswordMatching(_view.Password(), _view.RepeatPassword());
		if (passwordMatch)
		{
			_model.Register(_view.Email(), _view.Password());
		}
		else
		{
			_view.DisplayWarning(true, "Passwords doesnt match");
		}

	}
	
}
