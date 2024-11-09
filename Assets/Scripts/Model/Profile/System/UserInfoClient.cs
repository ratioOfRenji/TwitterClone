using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserInfoClient
{
	private TokensStorage _tokenStorage;
	private TokenRefresh _tokenRefresh;

	public UserInfoClient(TokensStorage tokenStorage, TokenRefresh tokenRefresh)
	{
		_tokenStorage = tokenStorage;
		_tokenRefresh = tokenRefresh;
	}

	// Change Displayed Name
	public async UniTask<bool> ChangeDisplayedNameAsync(string newDisplayedName)
	{
		var url = $"{Constants.BaseApiUrl}/api/user-info/change-displayed-name";
		var requestBody = JsonConvert.SerializeObject(new { DisplayedName = newDisplayedName });

		return await SendPostRequest(url, requestBody);
	}

	// Change Profile Icon
	public async UniTask<bool> ChangeProfileIconAsync(EIconType newIcon)
	{
		var url = $"{Constants.BaseApiUrl}/api/user-info/change-profile-icon";
		var requestBody = JsonConvert.SerializeObject(new { Icon = newIcon });

		return await SendPostRequest(url, requestBody);
	}

	// Get User Profile
	public async UniTask<UserProfile> GetUserProfileAsync()
	{
		var url = $"{Constants.BaseApiUrl}/api/user-info/get-current-user-info";
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
				// Successfully received response
				var responseJson = request.downloadHandler.text;
				return JsonConvert.DeserializeObject<UserProfile>(responseJson);
			}
			else
			{
				// Handle 401 Unauthorized specifically for token refresh
				if (request.responseCode == 401) // Unauthorized, usually token expired
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						// Retry the original request after refreshing the token
						return await GetUserProfileAsync();
					}
				}

				// Log detailed error information
				Debug.LogError($"Request failed with code {request.responseCode}: {request.error}");
				if (request.downloadHandler != null)
				{
					Debug.LogError($"Response: {request.downloadHandler.text}");
				}
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

			// Get the JWT token and add it to the header
			var token = _tokenStorage.UserTokens.AccessToken; // Get token from token storage
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			var asyncOp = request.SendWebRequest();

			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success)
			{
				return true;
			}
			else
			{
				// If the token has expired (401), try refreshing once and then stop if still failing
				if (request.responseCode == 401) // Unauthorized, usually token expired
				{
					bool tokenRefreshed = await RefreshTokenAsync();
					if (tokenRefreshed)
					{
						// Retry the original request after refreshing the token
						return await SendPostRequest(url, jsonBody);
					}
				}

				Debug.LogError($"Request failed: {request.error}");
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
