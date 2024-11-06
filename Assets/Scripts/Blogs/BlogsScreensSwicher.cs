using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlogsScreensSwicher : MonoBehaviour
{
	[SerializeField] private GameObject _blogsScreen;
	[SerializeField] private GameObject _createBlogScreen;

	public void ShowBlogsScreen()
	{
		_createBlogScreen.SetActive(false);
		_blogsScreen.SetActive(true);
	}

	public void ShowCreateBlogScreen()
	{
		_createBlogScreen.SetActive(true);

	}
}
