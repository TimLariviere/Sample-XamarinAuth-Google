using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin.Auth;

namespace Xamarin_GoogleAuth.Droid
{
    [Activity (Label = "Xamarin_GoogleAuth", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
    {
        public static OAuth2Authenticator Auth;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            
			SetContentView (Resource.Layout.Main);

            SetupGoogle();

            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleLoginButton.Click += delegate
            {
                var intent = Auth.GetUI(this);
                StartActivity(intent);
            };
		}

        private void SetupGoogle()
        {
            Auth = new OAuth2Authenticator(Configurations.ClientId, string.Empty, 
                                           Configurations.Scope, 
                                           new Uri(Configurations.AuthorizeUrl), 
                                           new Uri(Configurations.RedirectUrl), 
                                           new Uri(Configurations.AccessTokenUrl), 
                                           null, true);

            Auth.Completed += OnGoogleAuthCompleted;
            Auth.Error += OnGoogleAuthError;
        }

        private async void OnGoogleAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            var googleButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleButton.Text = $"Connected with {email}";
        }

        private void OnGoogleAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var alertBuilder = new AlertDialog.Builder(this);
            alertBuilder.SetTitle(e.Message);
            alertBuilder.SetMessage(e.Exception.ToString());
            alertBuilder.Create().Show();
        }
    }
}
