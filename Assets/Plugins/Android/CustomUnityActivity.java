package com.RatioCompany.BlogPostsWithGoogle;


import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.graphics.Color;
import androidx.core.content.ContextCompat;
import com.unity3d.player.UnityPlayerActivity;

public class CustomUnityActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Show the status bar
        Window window = this.getWindow();
        window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
        window.getDecorView().setSystemUiVisibility(View.SYSTEM_UI_FLAG_VISIBLE);

        // Set initial status bar color and icon color (example: white bg with dark icons)
        setStatusBarAppearance(0xDBDBDB, true);
    }

    // Method to set status bar color and icon color from Unity
    public void setStatusBarAppearance(int backgroundColor, boolean useDarkIcons) {
        Window window = this.getWindow();
        window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
        window.setStatusBarColor(backgroundColor);

        // Set the status bar icons color based on useDarkIcons parameter
        if (useDarkIcons) {
            window.getDecorView().setSystemUiVisibility(View.SYSTEM_UI_FLAG_LIGHT_STATUS_BAR);
        } else {
            window.getDecorView().setSystemUiVisibility(0); // Clears the flag for dark icons
        }
    }
}
