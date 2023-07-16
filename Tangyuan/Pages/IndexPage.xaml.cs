using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

        #region ��ȡ�����
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

		#region ��¼
		LocalSQLDataHelper.LocalLoginInfo logininfo = LocalSQLDataHelper.GetLoginInfo();
		if (logininfo != null)
		{
			if (LoginStatusManager.TryLogIn(logininfo.LoginUserPhoneNumber, logininfo.LoginUserPassword))
			{
				UserInfo ui = SQLDataHelper.GetUserInfoByPhoneNumber(logininfo.LoginUserPhoneNumber);
				imbUserAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
				lblUserNickname.Text = ui.Nickname;
			}
			else
			{
				lblUserNickname.Text = "δ��¼";
			}
		}
		else
		{
			lblUserNickname.Text = "δ��¼";
		}
        #endregion
    }
}