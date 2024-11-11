using Codice.CM.Common;
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
	private UserDataStorage _userDataStorage;

	private string _currentEmail;
	public SignInWithEmail(TokensStorage tokensStorage, UserDataStorage userDataStorage)
	{
		_tokenStorage = tokensStorage;
		_userDataStorage = userDataStorage;
	}

	public async UniTask<bool> Login(string email, string password)
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/login";
		var requestBody = JsonConvert.SerializeObject(new { email, password });
		_currentEmail = email;
		return await SendPostRequest(url, requestBody);
	}
	private async Task<bool> SendPostRequest(string url, string jsonBody)
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
				_userDataStorage.SaveData(_currentEmail);
				SaveTokensFromResponse(request.downloadHandler.text);

				return true;
			}
			{
				// Attempt to get the full error message from the response body
				string serverResponse = request.downloadHandler.text;
				Debug.LogError($"Request failed: {request.error}. Server Response: {serverResponse}");
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
