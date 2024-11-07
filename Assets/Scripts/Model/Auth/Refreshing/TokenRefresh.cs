using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
public class TokenRefresh
{
	private readonly string _apiUrl = $"{Constants.BaseApiUrl}/api/Account/refresh-token";

	

	public async UniTask<TokensDataInstance> RefreshTokenAsync(TokensDataInstance tokens)
	{
		if (string.IsNullOrEmpty(tokens.RefreshToken))
		{
			Debug.LogError("Refresh token is missing.");
			return null;
		}

		var requestBody = JsonConvert.SerializeObject(tokens.RefreshToken);
		return await SendPostRequest(_apiUrl, requestBody);
	}

	private async UniTask<TokensDataInstance> SendPostRequest(string url, string jsonBody)
	{
		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			var asyncOp = request.SendWebRequest();

			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success)
			{
				return ParseTokens(request.downloadHandler.text);
			}
			else
			{
				Debug.LogError($"Token refresh request failed: {request.error}");
				return null;
			}
		}
	}

	private TokensDataInstance ParseTokens(string responseJson)
	{
		try
		{
			// Deserialize the response JSON into a TokensDataInstance
			var newTokens = JsonConvert.DeserializeObject<TokensDataInstance>(responseJson);
			if (newTokens != null)
			{
				
				return newTokens;
			}
			else
			{
				Debug.LogError("Failed to parse new tokens.");
				return null;
			}
		}
		catch (JsonException ex)
		{
			Debug.LogError($"Failed to parse tokens: {ex.Message}");
			return null;
		}
	}
}
