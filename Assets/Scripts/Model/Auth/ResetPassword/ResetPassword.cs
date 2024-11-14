using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class ResetPassword
{

	public async UniTask<bool> ResetPasswordAsync(string email)
	{
		var requestData = new ResetPasswordRequest { Email = email };
		string url = $"{Constants.BaseApiUrl}/api/Account/reset-password";

		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			string json = JsonConvert.SerializeObject(requestData);
			byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			var operation = request.SendWebRequest();
			while (!operation.isDone)
				await UniTask.Yield();

			if (request.result == UnityWebRequest.Result.Success)
			{
				// Parse the response JSON
				ResetPasswordResponse response = JsonUtility.FromJson<ResetPasswordResponse>(request.downloadHandler.text);
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
