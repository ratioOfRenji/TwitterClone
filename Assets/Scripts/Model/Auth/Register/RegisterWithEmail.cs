using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class RegisterWithEmail
{
	public string ErrorMessage { get; set; }
	public RegisterWithEmail()
	{
	}

	public bool CheckPasswordMatching(string password, string confirmedPassword)
	{
		if(string.IsNullOrEmpty(password))
		{
			return false;
		}
		if(password == confirmedPassword)
		{
			return true;
		}
		return false;

	}

	public async UniTask<bool> Register(string email, string password)
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/register";
		var requestBody = JsonConvert.SerializeObject(new { email = email, password = password });
		return await SendPostRequest(url, requestBody);
	}
	private async UniTask<bool> SendPostRequest(string url, string jsonBody)
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
				Debug.Log("User Succesfuly registered!");
				return true;
			}
			else
			{
				// Attempt to get the full error message from the response body
				string serverResponse = request.downloadHandler.text;
				Debug.LogError($"Request failed: {request.error}. Server Response: {serverResponse}");
				ErrorMessage = !string.IsNullOrEmpty(serverResponse) ? serverResponse : request.error;
				return false;
			}
		}
	}
}
