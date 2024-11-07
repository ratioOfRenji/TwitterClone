using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SignInWithEmail 
{
	private TokensStorage _tokenStorage;
	public SignInWithEmail(TokensStorage tokensStorage)
	{
		_tokenStorage = tokensStorage;
	}

	public async UniTask<bool> Login(string email, string password)
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/login";
		var requestBody = JsonConvert.SerializeObject(new { email, password });
		return await SendPostRequest(url, requestBody, saveTokens: true);
	}
	private async Task<bool> SendPostRequest(string url, string jsonBody, bool saveTokens = false)
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
				if (saveTokens)
				{
					SaveTokensFromResponse(request.downloadHandler.text);
				}
				return true;
			}
			else
			{
				Debug.LogError($"Request failed: {request.error}");
				return false;
			}
		}
	}

	private void SaveTokensFromResponse(string responseJson)
	{
		var tokenResponse = JsonConvert.DeserializeObject<TokensDataInstance>(responseJson);
		_tokenStorage.UpdateData(tokenResponse);
	}
}
