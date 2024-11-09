using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;
public class CreateNewPostPresenter : IInitializable, IDisposable
{
	private readonly CreateNewPostView _view;
	private readonly CreateNewPostModel _model;

	private Subject<Unit> onBlogPostedSubject = new Subject<Unit>();
	public IObservable<Unit> OnBlogPostedAsObservable()
	{
		return onBlogPostedSubject.AsObservable();
	}
	public CreateNewPostPresenter(CreateNewPostView view, CreateNewPostModel model)
	{
		_view = view;
		_model = model;
	}

	public void Initialize()
	{
		_view.OnApplyButtonAsObservable().Subscribe(_ => { PostBlogAndNotify(); });
	}

	private async void PostBlogAndNotify()
	{
		_view.ShowPanel(false);
		bool success =await _model.PostNewBlog(_view.PostText());
		if (success) {
		onBlogPostedSubject?.OnNext(Unit.Default);
		}
	}
	public void Dispose()
	{

	}
}
