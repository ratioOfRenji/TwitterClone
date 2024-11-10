using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlogModel 
{
	public Blog BlogInfo { get; private set; }

	public string AuthorName { get; private set; }
	public EIconType AuthorsAvatar { get; private set; }

	private readonly BlogClient _blogClient;
	public BlogModel(BlogClient blogClient )
	{
		_blogClient = blogClient;
	}
	public void AssignBlog(Blog blog)
	{
		BlogInfo = blog;
	}

	public async UniTask<bool> GetAuthorInfo()
	{
		UserProfile profile = await _blogClient.GetUserInfoByUserIdAsync(BlogInfo.BlogAuthor);
		if (profile == null)
		{
			return false;
		}
		AuthorName = profile.DisplayedName;
		AuthorsAvatar = profile.Icon;
		return true;
	}
}
