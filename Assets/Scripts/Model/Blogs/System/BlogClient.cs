using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class BlogClient
{
	private readonly string _baseApiUrl = $"{Constants.BaseApiUrl}";
	private readonly TokensStorage _tokenStorage;
	private readonly TokenRefresh _tokenRefresh;

	public BlogClient(TokensStorage tokenStorage, TokenRefresh tokenRefresh)
	{
		_tokenStorage = tokenStorage;
		_tokenRefresh = tokenRefresh;
	}

	// Method to post a new blog
	public async UniTask<bool> PostNewBlogAsync(string title, string description)
	{
		var url = $"{_baseApiUrl}/api/Blogs";

		// Create a new Blog object with default values for BlogId, BlogAuthor, and CreatedAt
		var blogData = new Blog
		{
			BlogId = 0,
			BlogTitle = title,
			BlogDescription = description,
			BlogAuthor = "placeholder",
			CreatedAt = DateTime.Now
		};

		// Serialize the Blog object
		string jsonData = JsonConvert.SerializeObject(blogData);

		// Send the request
		return await SendPostRequest(url, jsonData);
	}

	// Method to get all blogs for the current user
	public async UniTask<List<Blog>> GetAllUserBlogsAsync()
	{
		var url = $"{_baseApiUrl}/api/Blogs/my-blogs";

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			// Set the Authorization header
			var token = _tokenStorage.UserTokens.AccessToken;
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			var asyncOp = request.SendWebRequest();
			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success && request.downloadHandler != null)
			{
				// Parse and return blogs
				var responseJson = request.downloadHandler.text;
				return JsonConvert.DeserializeObject<List<Blog>>(responseJson);
			}
			else
			{
				// Handle 401 Unauthorized specifically for token refresh
				if (request.responseCode == 401)
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						return await GetAllUserBlogsAsync(); // Retry after refreshing token
					}
				}
				Debug.LogError($"Failed to fetch blogs: {request.error}");
				return null;
			}
		}
	}

	// Helper method to send POST requests
	private async UniTask<bool> SendPostRequest(string url, string jsonBody)
	{
		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			// Set Authorization header
			var token = _tokenStorage.UserTokens.AccessToken;
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			var asyncOp = request.SendWebRequest();
			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("Request succeeded.");
				return true;
			}
			else
			{
				// Handle 401 Unauthorized
				if (request.responseCode == 401)
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						return await SendPostRequest(url, jsonBody); // Retry after refreshing token
					}
				}
				Debug.LogError($"Request failed: {request.error}");
				if (request.downloadHandler != null)
				{
					Debug.LogError($"Response: {request.downloadHandler.text}");
				}
				return false;
			}
		}
	}

	// Method to refresh the token once
	private async UniTask<bool> RefreshTokenAsync()
	{
		try
		{
			var refreshed = await _tokenRefresh.RefreshTokenAsync(_tokenStorage.UserTokens);
			_tokenStorage.UpdateData(refreshed);
			return true;
		}
		catch (System.Exception ex)
		{
			Debug.LogError($"Token refresh failed: {ex.Message}");
			return false;
		}
	}
}
