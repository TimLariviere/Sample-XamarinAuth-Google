using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin_GoogleAuth.Authentication;
using Xamarin_GoogleAuth.Services;

namespace Xamarin_GoogleAuth.Droid
{
    [Activity(Label = "Xamarin_GoogleAuth", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IGoogleAuthenticationDelegate
    {
        // Need to be static because we need to access it in GoogleAuthInterceptor for continuation
        public static GoogleAuthenticator Auth;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);

            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            // Display the activity handling the authentication
            var authenticator = Auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            // Retrieve the user's email address
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(token.TokenType, token.AccessToken);

            // Display it on the UI
            var googleButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleButton.Text = $"Connected with {email}";
        }

        public void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(this)
                           .SetTitle("Authentication canceled")
                           .SetMessage("You didn't completed the authentication process")
                           .Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                           .SetTitle(message)
                           .SetMessage(exception?.ToString())
                           .Show();
        }
    }
}
