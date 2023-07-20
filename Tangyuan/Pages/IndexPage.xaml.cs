using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();
        //ɵ�ƶ���Bug������Ķ࣬RefreshView��Ϊҳ���Ԫ�������費���ҳ���ȫ�߶ȣ�ֻ������������֮����
        //����֮��ֻ�ü�һ��Grid��Ϊ��Ԫ�أ�Ȼ����RefreshViewȥƥ�����ĸ߶ȣ�΢���������ɵ��
        rfvMainScroll.HeightRequest = this.Content.Height;

        #region ��ȡ�����
        RefreshPosts();
		#endregion

		#region ��¼
		if (LoginStatusManager.TryLogIn())
		{
			UserInfo ui = SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID);
			imbUserAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
			lblUserNickname.Text = ui.Nickname;
		}
		else
		{
			lblUserNickname.Text = "δ��¼";
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

	private void RefreshView_Refreshing(object sender, EventArgs e)
	{
        RefreshPosts();
        rfvMainScroll.IsRefreshing = false;
    }

    /// <summary>
    /// ˢ���Ƽ�����
    /// </summary>
	private void RefreshPosts()
	{
        List<PostInfo> posts = SQLDataHelper.GetRecentPosts(5);
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
            //���ֻ��һ���������ı��ٷ��ı�
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
            //�������һ����������
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
    }
}