using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangePassword
{
	private readonly TokensStorage _tokensStorage;

	public ChangePassword(TokensStorage tokensStorage)
	{
		_tokensStorage = tokensStorage;
	}

	public async UniTask<bool> ChangePasswordAsync(string currentPassword, string newPassword)
	{
		var requestData = new ChangePasswordRequest
		{
			CurrentPassword = currentPassword,
			NewPassword = newPassword
		};

		string url = $"{Constants.BaseApiUrl}/api/Account/change-password";
		string json = JsonConvert.SerializeObject(requestData);

		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();

			// Set headers for JSON content and Authorization
			request.SetRequestHeader("Content-Type", "application/json");
			var token = _tokensStorage.UserTokens.AccessToken;
			request.SetRequestHeader("Authorization", $"Bearer {token}");

			// Send the request
			var operation = request.SendWebRequest();
			while (!operation.isDone)
				await UniTask.Yield();

			// Check response and return result
			if (request.result == UnityWebRequest.Result.Success)
			{
				return true;
			}
			else
			{
				string serverResponse = request.downloadHandler.text;
				Debug.LogError($"Request failed: {request.error}. Server Response: {serverResponse}");
				return false;
			}
		}
	}
}
