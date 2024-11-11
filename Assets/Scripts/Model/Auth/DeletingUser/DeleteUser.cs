using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DeleteUser
{
	public string ErrorMessage { get; private set; }
	private UserDataStorage _userDataStorage;

	public DeleteUser(UserDataStorage userDataStorage)
	{
		_userDataStorage = userDataStorage;
	}

	// Method to delete a user by userId
	public async UniTask<bool> DeleteUserm(string userId)
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/delete-user/{userId}";
		return await SendDeleteRequest(url);
	}

	// Helper method to send DELETE requests
	private async UniTask<bool> SendDeleteRequest(string url)
	{
		using (UnityWebRequest request = UnityWebRequest.Delete(url))
		{
			// Optionally set headers, such as Authorization if required
			// var token = _userDataStorage.GetAccessToken();
			// request.SetRequestHeader("Authorization", $"Bearer {token}");

			var asyncOp = request.SendWebRequest();

			while (!asyncOp.isDone)
			{
				await UniTask.Yield();
			}

			if (request.result == UnityWebRequest.Result.Success)
			{
				Debug.Log("User successfully deleted!");
				
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
