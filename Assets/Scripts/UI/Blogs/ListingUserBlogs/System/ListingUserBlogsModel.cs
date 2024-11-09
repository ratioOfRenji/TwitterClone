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
	public ListingUserBlogsModel(DiContainer container,BlogClient blogClient , BlogView prefab)
	{
		_container = container;
		_blogClient = blogClient;
		_prefab = prefab;
	}

	public async UniTask LoadAndSpawnUserBlogs(GameObject parent)
	{
		await UniTask.Delay(5000);
		List<Blog> blogs = await LoadUserBlogs();
		SpawnBlogPosts(blogs, parent);
	}

	private UniTask<List<Blog>> LoadUserBlogs()
	{
		return _blogClient.GetAllUserBlogsAsync();
	}

	private void SpawnBlogPosts(List<Blog> blogs, GameObject parent)
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
	}
}
