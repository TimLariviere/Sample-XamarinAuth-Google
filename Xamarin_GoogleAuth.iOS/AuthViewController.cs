using System;
using UIKit;
using Xamarin.Auth;

namespace Xamarin_GoogleAuth.iOS
{
    public partial class AuthViewController : UIViewController
    {
        public static WebAuthenticator Auth = null;

        public AuthViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            SetupGoogle();

            GoogleLoginButton.TouchUpInside += OnGoogleLoginButtonClicked;
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            var viewController = Auth.GetUI();
            PresentViewController(viewController, true, null);
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
            DismissViewController(true, null);

            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            GoogleLoginButton.SetTitle($"Connected with {email}", UIControlState.Normal);
        }

        private void OnGoogleAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var alertController = new UIAlertController
            {
                Title = e.Message,
                Message = e.Exception.ToString()
            };
            PresentViewController(alertController, true, null);
        }
    }
}