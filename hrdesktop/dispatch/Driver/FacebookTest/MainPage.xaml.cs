using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Navigation;
using Facebook;
using Microsoft.Phone.Controls;

namespace FacebookTest {
    public partial class MainPage : PhoneApplicationPage {
        // コンストラクター
        public MainPage() {
            InitializeComponent();
        }

        // アプリケーション キー
        private readonly string ApplicationID = "取得したアプリキー";
        private readonly string ApplicationSecret = "取得したシークレットキー";
        // アクセストークンを保持する
        string accessToken = "";

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            var oauthClient = new FacebookOAuthClient { AppId = ApplicationID };
            var prams = new Dictionary<string, object>();
            prams["response_type"] = "code";
            prams["scope"] = "user_about_me,user_photos,offline_access";

            // 認証用のページURLを取得
            var url = oauthClient.GetLoginUrl(prams);

            // 認証ページへ遷移
            webBrowser.Navigate(url);
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e) {
            Debug.WriteLine("webBrowser_Navigated: {0}", e.Uri);

            FacebookOAuthResult oauthResult;
            if (!FacebookOAuthResult.TryParse(e.Uri, out oauthResult) || !oauthResult.IsSuccess) {
                return;
            }

            var oauthClient = new FacebookOAuthClient { AppId = ApplicationID };
            oauthClient.AppSecret = ApplicationSecret;
            var code = oauthResult.Code;

            // アクセストークンを要求(非同期実行)
            oauthClient.ExchangeCodeForAccessTokenCompleted += oauthClient_ExchangeCodeForAccessTokenCompleted;
            oauthClient.ExchangeCodeForAccessTokenAsync(code);
        }

        // アクセストークンを取得
        void oauthClient_ExchangeCodeForAccessTokenCompleted(object sender, FacebookApiEventArgs e) {
            var result = e.GetResultData() as IDictionary<string, object>;
            if (result == null) return;

            accessToken = (string)result["access_token"];

            Dispatcher.BeginInvoke(() => {
                textAccessToken.Text = accessToken;
            });
        }
    }
}