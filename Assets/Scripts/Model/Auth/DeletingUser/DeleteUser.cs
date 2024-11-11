using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DeleteUser
{
	public string ErrorMessage { get; private set; }
	private readonly TokensStorage _userDataStorage;

	public DeleteUser(TokensStorage userDataStorage)
	{
		_userDataStorage = userDataStorage;
	}

	// Method to delete the authenticated user
	public async UniTask<bool> DeleteUserAsync()
	{
		var url = $"{Constants.BaseApiUrl}/api/Account/delete-user";
		return await SendDeleteRequest(url);
	}

	// Helper method to send DELETE requests
	private async UniTask<bool> SendDeleteRequest(string url)
	{
		using (UnityWebRequest request = UnityWebRequest.Delete(url))
		{
			// Set the Authorization header with the access token
			var token = _userDataStorage.UserTokens.AccessToken;
			if (!string.IsNullOrEmpty(token))
			{
				request.SetRequestHeader("Authorization", $"Bearer {token}");
			}
			else
			{
				ErrorMessage = "Authorization token is missing.";
				Debug.LogError(ErrorMessage);
				return false;
			}

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

