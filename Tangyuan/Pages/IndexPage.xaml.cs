using CommunityToolkit.Maui.Alerts;
using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

        RefreshPostsAsync();
        LoginAsync();
        HotUpdateAsync();
    }

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		if (LoginStatusManager.IsLoggedIn)
		{
            Shell.Current.GoToAsync("/user?id=" + LoginStatusManager.LoggedInUserID);
        }
		else
		{
			Shell.Current.GoToAsync("/login");
		}
	}

	private void btnNewPost_Clicked(object sender, EventArgs e)
	{
        if (LoginStatusManager.IsLoggedIn)
        {
            Shell.Current.GoToAsync("/newpost");
        }
        else
        {
            Shell.Current.GoToAsync("/login");
        }
	}

	private void btnRefresh_Clicked(object sender, EventArgs e)
	{
        btnRefresh.IsVisible = false;
        aciRefreshStatus.IsRunning = true;
        RefreshPostsAsync();
    }

    /// <summary>
    /// 刷新推荐帖。
    /// </summary>
	private async void RefreshPostsAsync()
	{
        try
        {
            List<PostInfo> posts = await Task.Run(() => SQLDataHelper.GetRecentPosts(5));
            List<PostInfo> final = new List<PostInfo>();
            foreach (var v in posts)
            {
                bool isExistedInLeft = false;
                foreach (var va in stlPostsLeft.Children)
                {
                    if (v.PostID == (va as PostPreview).Post.PostID)
                    {
                        isExistedInLeft = true;
                    }
                }
                bool isExistedInRight = false;
                foreach (var vb in stlPostsRight.Children)
                {
                    if (v.PostID == (vb as PostPreview).Post.PostID)
                    {
                        isExistedInRight = true;
                    }
                }
                if (!(isExistedInLeft || isExistedInRight))
                {
                    final.Add(v);
                }
            }

            if (final.Count > 0)
            {
                //如果只有一个帖，就哪边少放哪边
                if (final.Count == 1)
                {
                    if (stlPostsLeft.Children.Count > stlPostsRight.Children.Count)
                    {
                        stlPostsRight.Children.Insert(0, new PostPreview(final[0]));
                    }
                    else
                    {
                        stlPostsLeft.Children.Insert(0, new PostPreview(final[0]));
                    }
                }
                //如果大于一，就轮流放
                else
                {
                    uint signal = 1;
                    foreach (var v in final)
                    {
                        if (signal % 2 == 0)
                        {
                            stlPostsLeft.Children.Insert(0, new PostPreview(v));
                        }
                        else
                        {
                            stlPostsRight.Children.Insert(0, new PostPreview(v));
                        }
                        signal++;
                    }
                }
            }
            btnRefresh.Source = ImageSource.FromFile("icon_refresh.png");
        }
        catch
        {
            btnRefresh.Source = ImageSource.FromFile("icon_error.png");
        }
        finally
        {
            btnRefresh.IsVisible = true;
            aciRefreshStatus.IsRunning = false;
        }
    }

    private async void LoginAsync()
    {
        try
        {
            if (LoginStatusManager.TryLogIn())
            {
                UserInfo ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID));
                imbUserAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
                lblUserNickname.Text = ui.Nickname;
            }
            else
            {
                lblUserNickname.Text = "未登录";
            }
        }
        catch
        {
            lblUserNickname.Text = "未登录";
        }
    }

    /// <summary>
    /// 热更新
    /// </summary>
    private async void HotUpdateAsync()
    {
        try
        {
            bool isnew = await VersionControlHelper.IsANewerVersionReleasedAsync();
            await DisplayAlert("糖原内测", "当前版本：" + AppInfo.Current.VersionString + "\nBuild号：" + AppInfo.Current.BuildString +
                "\n是否有新版本：" + isnew, "确定");
            if (isnew)
            {
#if ANDROID
            Toast.Make("开始下载新版本……").Show();
            string path = await VersionControlHelper.DownloadNewestVersionAsync();
            if (await DisplayAlert("更新", "新版本已下载完毕，是否现在更新？若不选择现在更新，则该对话框每次启动糖原时显示一次。", "更新", "不更新"))
            {
                Launcher.Default.OpenAsync(new OpenFileRequest("安装", new ReadOnlyFile(path)));
            }
#endif
            }
        }
        catch
        {
            Toast.Make("热更新失败").Show();
        }
    }
}