using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ListingFeedModel
{
	private readonly BlogView.Factory _blogsFactory;
	private readonly BlogClient _blogClient;
	private List<Blog> _cachedBlogs = new List<Blog>();
	public PaginationInfo _cachedUserPagination { get; private set; }
	public ListingFeedModel(BlogView.Factory BlogsFactory, BlogClient blogClient)
	{
		_blogsFactory = BlogsFactory;
		_blogClient = blogClient;
		_cachedUserPagination = new PaginationInfo();
		_cachedUserPagination.CurrentPage = 1;
	}

	public async UniTask LoadAndSpawnFeedBlogs(GameObject parent, int page, int pageSize)
	{
		var (blogs, pagination) = await LoadFeedBlogs(page, pageSize);
		_cachedUserPagination = pagination;
		SpawnBlogPosts(blogs, parent);
	}

	public async UniTask LoadAndSpawnNextBatch(GameObject parent)
	{
		int pageSize = _cachedUserPagination.PageSize;
		if (pageSize <= 0) pageSize = 10;
		var (blogs, pagination) = await LoadFeedBlogs(_cachedUserPagination.CurrentPage + 1, pageSize);
		SpawnBlogPosts(blogs, parent);
	}
	public async void ReloadPosts(GameObject parent)
	{
		var (blogs, pagination) = await LoadFeedBlogs(1, 5);
		ClearDisplayedPosts(parent);
		_cachedUserPagination = pagination;
		SpawnBlogPosts(blogs, parent);
	}
	public async UniTask<(List<Blog> Blogs, PaginationInfo Pagination)> LoadFeedBlogs(int page, int pageSize)
	{
		var (blogs, pagination) = await _blogClient.GetAllBlogsAsync(page, pageSize);
		_cachedUserPagination = pagination;
		return (blogs, pagination);
	}

	public void SpawnBlogPosts(List<Blog> blogs, GameObject parent)
	{
		foreach (Blog blog in blogs)
		{
			BlogView prefab = _blogsFactory.Create();
			prefab.transform.parent = parent.transform;
			prefab.SetupBlogText(blog);
			prefab.AssignBlogInfo(blog);
			prefab.ShowDeleteBlogButton(false);
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
