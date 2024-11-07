using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class StartupUi : IInitializable, IDisposable
{
	private CompositeDisposable _disposables = new CompositeDisposable();

	private readonly StartUp _startUp;
	private readonly AuthScreenSwicher _authScreenSwicher;
	private readonly BlogsScreensSwicher _blogScreensSwicher;
    public StartupUi(StartUp startUp, AuthScreenSwicher authScreenSwicher, BlogsScreensSwicher blogsScreensSwicher)
	{
		_startUp = startUp;
		_authScreenSwicher = authScreenSwicher;
		_blogScreensSwicher = blogsScreensSwicher;
	}
	public void Initialize()
	{
		_startUp.OnTokensLoadedAsObservable().Subscribe(success => { OnTokensLaded(success); }).AddTo(_disposables);
	}

	private void OnTokensLaded(bool success)
	{
		_authScreenSwicher.ShowLoadingScreen(false);
		if (success)
		{
			_blogScreensSwicher.ShowBlogsScreen();
		}
		else
		{
			_authScreenSwicher.ShowRegisterScreen();
		}
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}

}
