using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace SpeakWithMe
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState) {
            base.OnCreate(savedInstanceState);
            //手机刘海变灰
            Window.SetStatusBarColor(Android.Graphics.Color.White);
            //手机底部变灰
            Window.SetNavigationBarColor(Android.Graphics.Color.White);
            //隐藏刘海
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
        }

        
    }
}
