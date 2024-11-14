package com.RatioCompany.BlogPostsWithGoogle;

import android.os.Build;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import com.unity3d.player.UnityPlayer;

public class StatusBarController {
    public static void showStatusBar() {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Window window = UnityPlayer.currentActivity.getWindow();
                // Clear fullscreen flag to show status bar
                window.clearFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN);
                window.getDecorView().setSystemUiVisibility(View.SYSTEM_UI_FLAG_VISIBLE);
            }
        });
    }
}
