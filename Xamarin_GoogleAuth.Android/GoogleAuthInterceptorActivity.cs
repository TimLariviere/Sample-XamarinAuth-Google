using Android.App;
using Android.Content;
using Android.OS;
using System;

namespace Xamarin_GoogleAuth.Droid
{
    [Activity(Label = "GoogleAuthInterceptorActivity")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
                    {
                            Intent.CategoryDefault,
                            Intent.CategoryBrowsable
                    },
            DataSchemes = new[]
                    {
                        "com.woodenmoose.xamarin.googleauth"
                    },
            DataPaths = new[]
                    {
                        "/oauth2redirect"
                    }
        )
    ]
    public class GoogleAuthInterceptorActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());

            // load redirect_url Page
            MainActivity.Auth?.OnPageLoading(uri_netfx);

            this.Finish();

            return;
        }

    }
}