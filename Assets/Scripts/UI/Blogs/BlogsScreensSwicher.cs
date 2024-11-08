using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class BlogsScreensSwicher : MonoBehaviour
{
	[SerializeField] private GameObject _blogsScreen;
	[SerializeField] private GameObject _createBlogScreen;
	[SerializeField] private GoogleAuthentication _googleAuthentication;
	private CompositeDisposable _disposables = new CompositeDisposable();
	private void Awake()
	{
		_googleAuthentication.OnAuthObservable().Subscribe(success => {if(success){ ShowBlogsScreen(); } }).AddTo(_disposables);
	}
	public void ShowBlogsScreen()
	{
		_createBlogScreen.SetActive(false);
		_blogsScreen.SetActive(true);
	}

	public void ShowCreateBlogScreen()
	{
		_createBlogScreen.SetActive(true);

	}

	private void OnDestroy()
	{
		_disposables.Dispose();
	}
}
