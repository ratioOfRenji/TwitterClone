using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class ListingUserBlogsPresenter : IInitializable, IDisposable
{
	private readonly ListingUserBlogsView _view;
	private readonly ListingUserBlogsModel _model;
	private readonly CreateNewPostPresenter _createNewPostPresenter;

	private CompositeDisposable _disposables = new CompositeDisposable();
	public ListingUserBlogsPresenter(ListingUserBlogsView view, ListingUserBlogsModel model, CreateNewPostPresenter createNewPostPresenter)
	{
		_view = view;
		_model = model;
		_createNewPostPresenter = createNewPostPresenter;
	}
	public void Initialize()
	{
		_model.LoadAndSpawnUserBlogs(_view.BlogsParent);
		_createNewPostPresenter.OnBlogPostedAsObservable().Subscribe(_ => { _model.ClearDisplayedPosts(_view.BlogsParent); _model.LoadAndSpawnUserBlogs(_view.BlogsParent); }).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}
}
