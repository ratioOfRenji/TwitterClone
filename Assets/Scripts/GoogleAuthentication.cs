using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Google;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GoogleAuthentication : MonoBehaviour
{
	public string imageURL;
	public TMP_Text userNameTxt, userEmailTxt, idTokenText;
	public Image profilePic;
	public GameObject loginPanel, profilePanel;
	private GoogleSignInConfiguration configuration;
	public string webClientId = "499839630768-g1jj981p26cpi1fcaugigrmriv264t0o.apps.googleusercontent.com";
	private string apiBaseUrl = "https://webapiwithconfirm-production.up.railway.app/api/Account"; // Replace with your API base URL

	void Awake()
	{
		configuration = new GoogleSignInConfiguration
		{
			WebClientId = webClientId,
			RequestIdToken = true,
			UseGameSignIn = false,
			RequestEmail = true
		};
	}

	public void OnSignIn()
	{
		GoogleSignIn.Configuration = configuration;
		GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.Default);
	}

	internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
	{
		if (task.IsFaulted)
		{
			Debug.LogError("Error: " + task.Exception?.Message);
		}
		else if (task.IsCanceled)
		{
			Debug.LogError("Sign-in was canceled.");
		}
		else
		{
			StartCoroutine(UpdateUIAndSendApiRequest(task.Result));
		}
	}

	IEnumerator UpdateUIAndSendApiRequest(GoogleSignInUser user)
	{
		Debug.Log("Welcome: " + user.DisplayName + "!");
		userNameTxt.text = user.DisplayName;
		userEmailTxt.text = user.Email;
		idTokenText.text = user.IdToken;
		imageURL = user.ImageUrl.ToString();

		UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageURL);
		yield return imageRequest.SendWebRequest();

		if (imageRequest.result == UnityWebRequest.Result.Success)
		{
			Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(imageRequest);
			Rect rect = new Rect(0, 0, downloadedTexture.width, downloadedTexture.height);
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			profilePic.sprite = Sprite.Create(downloadedTexture, rect, pivot);

			loginPanel.SetActive(false);
			profilePanel.SetActive(true);

			// Send API request to register or log in
			Debug.Log(user.IdToken);
			StartCoroutine(SendRegisterOrLoginRequest(user.IdToken));
		}
		else
		{
			Debug.LogError("Failed to load profile image.");
		}
	}

	IEnumerator SendRegisterOrLoginRequest(string idToken)
	{
		UnityWebRequest request = new UnityWebRequest("https://webapiwithconfirm-production.up.railway.app/api/Account/signin/google", "POST");

		// Directly send the idToken as raw bytes without JSON serialization
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(idToken);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "text/plain"); // Using text/plain to send the raw token directly

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("User successfully logged in or registered.");
			// Handle response (e.g., store token if received)
		}
		else
		{
			Debug.LogError("Error logging in: " + request.error);
			Debug.LogError("Response: " + request.downloadHandler.text);
		}
	}

	public void OnSignOut()
	{
		userNameTxt.text = "";
		userEmailTxt.text = "";
		imageURL = "";
		loginPanel.SetActive(true);
		profilePanel.SetActive(false);
		Debug.Log("Signing out...");
		GoogleSignIn.DefaultInstance.SignOut();
	}
}