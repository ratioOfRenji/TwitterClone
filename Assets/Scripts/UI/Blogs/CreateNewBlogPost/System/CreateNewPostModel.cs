using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewPostModel 
{
	private readonly BlogClient _blogClient;
	public CreateNewPostModel(BlogClient blogClient)
	{
		_blogClient = blogClient;
	}

	public async UniTask<bool> PostNewBlog(string blogText)
	{

		bool success = await _blogClient.PostNewBlogAsync("placeholder", blogText);
		return success;
	}
}
