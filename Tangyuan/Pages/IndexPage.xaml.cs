using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

        #region 获取最近帖
        List<PostInfo> posts = SQLDataHelper.GetRecentPosts(5);
		uint signal = 1;
		foreach (var v in posts)
		{
			if (signal % 2 == 0)
			{
				stlPostsLeft.Children.Add(new PostPreview(v));
            }
			else
			{
				stlPostsRight.Children.Add(new PostPreview(v));
            }
			signal++;
		}
		#endregion

		#region 登录
		if (LoginStatusManager.TryLogIn())
		{
			UserInfo ui = SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID);
			imbUserAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
			lblUserNickname.Text = ui.Nickname;
		}
		else
		{
			lblUserNickname.Text = "未登录";
		}
        #endregion
    }

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		if (LoginStatusManager.IsLoggedIn)
		{
            Shell.Current.GoToAsync("/login");
        }
		else
		{
			Shell.Current.GoToAsync("/login");
		}
	}

	private void btnNewPost_Clicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("/newpost");
	}
}