using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ListingUserBlogsModel
{
	private readonly DiContainer _container;
	private readonly BlogView _prefab;
	private readonly BlogClient _blogClient;
	private List<Blog> _cachedBlogs = new List<Blog>();
	public PaginationInfo _cachedUserPagination { get; private set; }
	public ListingUserBlogsModel(DiContainer container,BlogClient blogClient , BlogView prefab)
	{
		_container = container;
		_blogClient = blogClient;
		_prefab = prefab;
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
			BlogView prefab =_container.InstantiatePrefab(_prefab, parent.transform).GetComponent<BlogView>();
			prefab.SetupBlogText(blog);
		}
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
