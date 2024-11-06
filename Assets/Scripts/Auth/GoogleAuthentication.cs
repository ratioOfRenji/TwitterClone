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

	[SerializeField] private BlogsScreensSwicher _blogsScreenSwicher;
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
	public void OnRegister()
	{
		GoogleSignIn.Configuration = configuration;
		GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinishedR, TaskScheduler.Default);
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
			StartCoroutine(SendLoginRequest(task.Result.IdToken));
		}
	}
	internal void OnAuthenticationFinishedR(Task<GoogleSignInUser> task)
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
			StartCoroutine(SendRegisterRequest(task.Result.IdToken));
		}
	}

	IEnumerator SendLoginRequest(string idToken)
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
			Debug.Log("User successfully logged in.");
			// Handle response (e.g., store token if received)
			_blogsScreenSwicher.ShowBlogsScreen();
		}
		else
		{
			Debug.LogError("Error logging in: " + request.error);
			Debug.LogError("Response: " + request.downloadHandler.text);
		}
	}

	IEnumerator SendRegisterRequest(string idToken)
	{
		UnityWebRequest request = new UnityWebRequest("https://webapiwithconfirm-production.up.railway.app/api/Account/register/google", "POST");

		// Directly send the idToken as raw bytes without JSON serialization
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(idToken);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "text/plain"); // Using text/plain to send the raw token directly

		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("User successfully registered.");
			// Handle response (e.g., store token if received)
			_blogsScreenSwicher.ShowBlogsScreen();
		}
		else
		{
			Debug.LogError("Error logging in: " + request.error);
			Debug.LogError("Response: " + request.downloadHandler.text);
		}
	}

	public void OnSignOut()
	{
		Debug.Log("Signing out...");
		GoogleSignIn.DefaultInstance.SignOut();
	}
}