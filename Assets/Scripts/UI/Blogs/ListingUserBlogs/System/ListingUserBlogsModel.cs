using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ListingUserBlogsModel
{
	private readonly BlogView.Factory _blogsFactory;
	private readonly BlogClient _blogClient;
	private List<Blog> _cachedBlogs = new List<Blog>();
	public PaginationInfo _cachedUserPagination { get; private set; }
	public ListingUserBlogsModel(BlogView.Factory blogsFactory, BlogClient blogClient )
	{
		_blogsFactory = blogsFactory;
		_blogClient = blogClient;
		_cachedUserPagination = new PaginationInfo();
		_cachedUserPagination.CurrentPage = 1;
	}

	public async UniTask LoadAndSpawnUserBlogs(GameObject parent, int page, int pageSize)
	{
		var (blogs, pagination) = await LoadUserBlogs(page, pageSize);
		_cachedUserPagination = pagination;
		SpawnBlogPosts(blogs, parent);
	}

	public async UniTask LoadAndSpawnNextBatch(GameObject parent)
	{
		int pageSize = _cachedUserPagination.PageSize;
		if (pageSize <= 0) pageSize = 10;
		var (blogs, pagination) = await LoadUserBlogs(_cachedUserPagination.CurrentPage+1, pageSize);
		SpawnBlogPosts(blogs, parent);
	}
	public async void ReloadPosts(GameObject parent)
	{
		var (blogs, pagination) = await LoadUserBlogs(1, 5);
		ClearDisplayedPosts(parent);
		_cachedUserPagination = pagination;
		SpawnBlogPosts(blogs, parent);
	}
	public async UniTask<(List<Blog> Blogs, PaginationInfo Pagination)> LoadUserBlogs(int page, int pageSize)
	{
		var(blogs, pagination) = await _blogClient.GetUserBlogsAsync(page, pageSize);
		_cachedUserPagination = pagination;
		return (blogs, pagination);
	}

	public void SpawnBlogPosts(List<Blog> blogs, GameObject parent)
	{
		foreach (Blog blog in blogs)
		{
			BlogView prefab = _blogsFactory.Create();
			prefab.transform.parent = parent.transform;
			prefab.transform.localScale = Vector3.one;
			prefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			prefab.GetComponent<RectTransform>().rotation = Quaternion.identity;
			prefab.SetupBlogText(blog);
			prefab.AssignBlogInfo(blog);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
	}

	public void ClearDisplayedPosts(GameObject parent)
	{
		for (int i = parent.transform.childCount - 1; i >= 0; i--)
		{
			GameObject.Destroy(parent.transform.GetChild(i).gameObject);
		}
		_cachedUserPagination = new();
		_cachedUserPagination.CurrentPage = 1;
	}
}
