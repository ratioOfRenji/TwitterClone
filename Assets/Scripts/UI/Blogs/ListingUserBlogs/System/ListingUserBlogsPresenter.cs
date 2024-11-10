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
	private readonly StartUp _startUp;
	private CompositeDisposable _disposables = new CompositeDisposable();

	private bool loadingNewBatch = false;
	public ListingUserBlogsPresenter(ListingUserBlogsView view, ListingUserBlogsModel model, CreateNewPostPresenter createNewPostPresenter, StartUp startUp)
	{
		_view = view;
		_model = model;
		_createNewPostPresenter = createNewPostPresenter;
		_startUp = startUp;
	}
	public void Initialize()
	{
		_startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) _model.LoadAndSpawnUserBlogs(_view.BlogsParent,1,5); }).AddTo(_disposables);
		_createNewPostPresenter.OnBlogPostedAsObservable().Subscribe(_ => { ReloadPosts(); }).AddTo(_disposables);
		_view.OnBottomReachedAsObservable().Subscribe(_ => { if(!loadingNewBatch&& _model._cachedUserPagination.HasMore) LoadNewBatch(); }).AddTo(_disposables);
	}

	private async void ReloadPosts()
	{
		_model.ReloadPosts(_view.BlogsParent);
	}
	private async void LoadNewBatch()
	{
		loadingNewBatch = true;
		await _model.LoadAndSpawnNextBatch(_view.BlogsParent);
		loadingNewBatch = false;
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}
}
