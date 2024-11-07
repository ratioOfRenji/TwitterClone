using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class SignInEmailPresenter : IInitializable, IDisposable
{
	private readonly SignInWithEmail _model;
	private readonly SignInEmailView _view;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public SignInEmailPresenter(SignInWithEmail signInWithEmail, SignInEmailView signInEmailView)
	{
		_model = signInWithEmail;
		_view = signInEmailView;
	}
	public void Initialize()
	{
		_view.OnSignInButtonAsObservable().Subscribe(_ => { _model.Login(_view.Email(), _view.Password()); }).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

}
