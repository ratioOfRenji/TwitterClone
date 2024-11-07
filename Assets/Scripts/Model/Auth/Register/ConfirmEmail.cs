using Codice.CM.Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ConfirmEmail
{
	public async UniTask<bool> ConfirmEmailMethod(string email, string code)
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/confirmation";
		var requestBody = JsonConvert.SerializeObject(new { email = email, code = code });
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
				Debug.Log("Email confirmed successfully!");
				return true;
			}
			else
			{
				// Attempt to get the full error message from the response body
				string serverResponse = request.downloadHandler.text;
				Debug.LogError($"Request failed: {request.error}. Server Response: {serverResponse}");
				return false;
			}
		}
	}
}
