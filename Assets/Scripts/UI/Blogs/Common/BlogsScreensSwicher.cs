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
	[SerializeField] private GameObject _headerFooterScreen;
	[SerializeField] private GameObject _editProfileScreen;
	[SerializeField] private GameObject _profileScreen;
	private CompositeDisposable _disposables = new CompositeDisposable();
	private void Awake()
	{
		_googleAuthentication.OnAuthObservable().Subscribe(success => {if(success){ ShowBlogsScreen(); } }).AddTo(_disposables);
	}
	public void ShowBlogsScreen()
	{
		_createBlogScreen.SetActive(false);
		_editProfileScreen.SetActive(false);
		_profileScreen.SetActive(false);
		_blogsScreen.SetActive(true);
		_headerFooterScreen.SetActive(true);
	}

	public void ShowEditProfileScreen()
	{
		_editProfileScreen.SetActive(true);
	}
	public void ShowProfileScreen()
	{
		_editProfileScreen.SetActive(false);
		_headerFooterScreen.SetActive(true);
		_profileScreen.SetActive(true);
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
