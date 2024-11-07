using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;
public class ConfirmEmailPresenter : IInitializable, IDisposable
{
	ConfirmEmail _model;
	ConfirmEmailView _view;
	UserDataStorage _userDataStorage;
	BlogsScreensSwicher _blogsScreensSwicher;
	AuthScreenSwicher _authScreenSwicher;
	SignInWithEmail _signInModel;
	RegisterWithEmail _registerModel;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public ConfirmEmailPresenter(ConfirmEmail model,
		ConfirmEmailView view,
		UserDataStorage userDataStorage,
		BlogsScreensSwicher blogsScreensSwicher,
		AuthScreenSwicher authScreenSwicher,
		SignInWithEmail signInModel,
		RegisterWithEmail registerModel)
	{
		_model = model;
		_view = view;
		_userDataStorage = userDataStorage;
		_blogsScreensSwicher = blogsScreensSwicher;
		_authScreenSwicher = authScreenSwicher;
		_signInModel = signInModel;
		_registerModel = registerModel;
	}
	public void Initialize()
	{
		_view.OnConfirmButtonAsObservable().Subscribe(_ => { OnConfirmButtonClick().Forget(); }).AddTo(_disposables);
	}

	private async UniTask OnConfirmButtonClick()
	{
		_view.DisplayErrorText(false);
		bool success = await _model.ConfirmEmailMethod(_userDataStorage._userData.Email, _view.CodeText());
		if (success) 
		{
			bool loginSuccess = await _signInModel.Login(_userDataStorage._userData.Email, _registerModel.CurrentPassword);
			if (loginSuccess)
			{
				_blogsScreensSwicher.ShowBlogsScreen();
			}
			else
			{
				_authScreenSwicher.ShowSignInScreen();
			}
		}
		else
		{
			_view.DisplayErrorText(true, "error");
		}
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

}
