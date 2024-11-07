using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;
public class SignInEmailPresenter : IInitializable, IDisposable
{
	private readonly SignInWithEmail _model;
	private readonly SignInEmailView _view;
	private readonly BlogsScreensSwicher _blogScreenSwicher;
	private CompositeDisposable _disposables = new CompositeDisposable();
	public SignInEmailPresenter(SignInWithEmail signInWithEmail, SignInEmailView signInEmailView, BlogsScreensSwicher blogScreenSwicher)
	{
		_model = signInWithEmail;
		_view = signInEmailView;
		_blogScreenSwicher = blogScreenSwicher;
	}
	public void Initialize()
	{
		_view.OnSignInButtonAsObservable().Subscribe(_ => { OnSignInButtonClicked(); }).AddTo(_disposables);
	}

	private async UniTask OnSignInButtonClicked()
	{
		bool success = await _model.Login(_view.Email(), _view.Password());
		if (success)
		{
			_blogScreenSwicher.ShowBlogsScreen();
		}
		_view.DispayErrorText(!success);
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

}
