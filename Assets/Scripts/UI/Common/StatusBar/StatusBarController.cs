using UnityEngine;

public static class StatusBarController
{
	// Method to set status bar color and icon color from Unity
	public static void SetStatusBarAppearance(Color backgroundColor, bool useDarkIcons)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        // Convert Unity Color to ARGB int format
        int androidColor = (int)(backgroundColor.a * 255) << 24 |
                           (int)(backgroundColor.r * 255) << 16 |
                           (int)(backgroundColor.g * 255) << 8 |
                           (int)(backgroundColor.b * 255);

        // Call the method on CustomUnityActivity in Android
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                currentActivity.Call("setStatusBarAppearance", androidColor, useDarkIcons);
            }
        }
#endif
	}
}
