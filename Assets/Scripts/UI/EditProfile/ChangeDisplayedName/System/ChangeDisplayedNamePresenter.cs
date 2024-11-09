using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
public class ChangeDisplayedNamePresenter : IInitializable, IDisposable
{
	private readonly ChangeDisplayedNameView _view;
	private readonly ChangeDisplayedNameModel _model;

	private Subject<Unit> onNameChangedSubject= new Subject<Unit>();
	public IObservable<Unit> OnNameChangedAsObservable()
	{
		return onNameChangedSubject.AsObservable();
	}
	public ChangeDisplayedNamePresenter(ChangeDisplayedNameView view, ChangeDisplayedNameModel model)
	{
		_view = view;
		_model = model;
	}
	public void Initialize()
	{
		_view.OnApplyButtonAsObservable().Subscribe(_ => { UpdateNameAndNotify(); });
	}

	private async UniTask UpdateNameAndNotify()
	{
		bool success = await _model.TryUpdateDisplayedName(_view.NewDisplayedName());
		if (success)
		{
			onNameChangedSubject.OnNext(Unit.Default);
		}
	}
	public void Dispose()
	{

	}
}
