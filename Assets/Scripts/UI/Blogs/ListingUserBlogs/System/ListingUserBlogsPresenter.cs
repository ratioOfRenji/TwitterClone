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
	public ListingUserBlogsPresenter(ListingUserBlogsView view, ListingUserBlogsModel model, CreateNewPostPresenter createNewPostPresenter, StartUp startUp)
	{
		_view = view;
		_model = model;
		_createNewPostPresenter = createNewPostPresenter;
		_startUp = startUp;
	}
	public void Initialize()
	{
		_startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) _model.LoadAndSpawnUserBlogs(_view.BlogsParent); }).AddTo(_disposables);
		_createNewPostPresenter.OnBlogPostedAsObservable().Subscribe(_ => { ReloadPosts(); }).AddTo(_disposables);
	}

	private async void ReloadPosts()
	{
		List<Blog> newBlogs=  await _model.LoadUserBlogs();
		_model.ClearDisplayedPosts(_view.BlogsParent); 
		_model.SpawnBlogPosts(newBlogs, _view.BlogsParent);
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}
}
