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
	public async UniTask<(List<Blog> Blogs, PaginationInfo Pagination)> GetUserBlogsAsync(int page, int pageSize)
	{
		var url = $"{_baseApiUrl}/api/Blogs/my-blogs?page={page}&pageSize={pageSize}";

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
				// Parse and return blogs along with pagination info
				var responseJson = request.downloadHandler.text;

				// Assuming response is a JSON object with "data" and "pagination" fields
				var responseObj = JsonConvert.DeserializeObject<PaginatedResponse<Blog>>(responseJson);
				return (responseObj.Data, responseObj.Pagination);
			}
			else
			{
				// Handle 401 Unauthorized specifically for token refresh
				if (request.responseCode == 401)
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						return await GetUserBlogsAsync(page, pageSize); // Retry after refreshing token
					}
				}
				Debug.LogError($"Failed to fetch blogs: {request.error}");
				return (null, null);
			}
		}
	}
	public async UniTask<(List<Blog> Blogs, PaginationInfo Pagination)> GetAllBlogsAsync(int page, int pageSize)
	{
		var url = $"{_baseApiUrl}/api/Blogs/all-blogs?page={page}&pageSize={pageSize}";

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
				// Parse and return blogs along with pagination info
				var responseJson = request.downloadHandler.text;

				// Assuming response is a JSON object with "data" and "pagination" fields
				var responseObj = JsonConvert.DeserializeObject<PaginatedResponse<Blog>>(responseJson);
				return (responseObj.Data, responseObj.Pagination);
			}
			else
			{
				// Handle 401 Unauthorized specifically for token refresh
				if (request.responseCode == 401)
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						return await GetAllBlogsAsync(page, pageSize); // Retry after refreshing token
					}
				}
				Debug.LogError($"Failed to fetch blogs: {request.error}");
				return (null, null);
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

	public async UniTask<UserProfile> GetUserInfoByUserIdAsync(string userId)
	{
		var url = $"{_baseApiUrl}/api/user-info/get-user-info/{userId}";

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			// Set the Authorization header
			var token = _tokenStorage.UserTokens.AccessToken;
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			// Send the request and wait for it to complete
			var asyncOp = request.SendWebRequest();
			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success && request.downloadHandler != null)
			{
				// Parse and return the user profile from the response
				var responseJson = request.downloadHandler.text;
				var userProfile = JsonConvert.DeserializeObject<UserProfile>(responseJson);

				return userProfile;
			}
			else
			{
				// Handle the error case
				if (request.responseCode == 401)
				{
					Debug.LogError("Unauthorized: Token may be expired.");
					// Handle token refresh if necessary here
				}
				else
				{
					Debug.LogError($"Failed to fetch user info: {request.error}");
				}
				return null;
			}
		}
	}
	public async UniTask<bool> DeleteBlogAsync(int blogId)
	{
		var url = $"{_baseApiUrl}/api/Blogs/{blogId}";

		using (UnityWebRequest request = UnityWebRequest.Delete(url))
		{
			// Set the Authorization header
			var token = _tokenStorage.UserTokens.AccessToken;
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			// Send the request and wait for completion
			var asyncOp = request.SendWebRequest();
			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("Blog deleted successfully.");
				return true;
			}
			else
			{
				// Handle 401 Unauthorized by attempting a token refresh
				if (request.responseCode == 401)
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						return await DeleteBlogAsync(blogId); // Retry after refreshing token
					}
				}

				Debug.LogError($"Failed to delete blog: {request.error}");
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
