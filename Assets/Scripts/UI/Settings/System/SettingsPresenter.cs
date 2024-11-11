using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class SettingsPresenter : IInitializable, IDisposable
{
	private readonly SettingsView _view;
	private readonly SettingsModel _model;
	private CompositeDisposable _disposables = new CompositeDisposable();
	public SettingsPresenter(SettingsView view, SettingsModel model)
	{
		_view = view;
		_model = model;
	}
	
	public void Initialize()
	{
		_view.OnSignOutAsObservable().Subscribe(_ => _model.SignOut()).AddTo(_disposables);
		_view.OnDeleteUserAsObservable().Subscribe(_ => _model.DeleteUser()).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

}
