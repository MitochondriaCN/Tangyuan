using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class UserHomePage : ContentPage,IQueryAttributable
{
	UserInfo ui;

	/// <summary>
	/// 用于CollectionView数据绑定
	/// </summary>
	public class PostItem
	{
		public uint PostID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime PostDate { get; set; }
	}

	public UserHomePage()
	{
		InitializeComponent();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (query.Count != 0)
		{
			UICompleter(uint.Parse(query["id"].ToString()));
			query.Clear();
		}
		//微软的这个设计简直是傻逼中的傻逼，当这个页面被导航的时候，query被填充参数，然后这个方法执行完，
		//这个参数居然他妈的一直保留，直到这个页面被弹出，也就是说，如果你在这个页面上导航到下一个页面，
		//再从下一个页面回来的时候，这个B参数还是这个页面被创建时候的参数，为了解决这个问题，我不得不在
		//这个方法执行完成前把query清理了。
	}

	private async void UICompleter(uint id)
	{
		try
		{
			ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(id));
            SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));

			//转圈
			adiInitializing.IsRunning = true;

			//头像和头衔
			lblUsername.Text = ui.Nickname;
			lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
			lblSchool.Text = si.SchoolName;
			lblRole.Text = ui.GetUserRoleFriendlyName();
			lblAuthorSignature.Text = ui.Signature;
			bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
			bodRoleContainer.BackgroundColor = ui.GetThemeColorOfUserRole();
			bodSchoolContainer.BackgroundColor = si.ThemeColor;
			imgAvatar.ImageSource = ImageSource.FromUri(new Uri(ui.Avatar));

			//关注和编辑资料按钮
			if (ui.UserID == LoginStatusManager.LoggedInUserID)
			{
				btnEditProfile.IsVisible = true;
			}
			else
			{
				btnFollow.IsVisible = true;
			}

			//关注、粉丝和帖子数
			lblFans.Text = (await Task.Run(() => SQLDataHelper.GetFansByUserID(ui.UserID))).Length.ToString();
			lblFollowings.Text = (await Task.Run(() => SQLDataHelper.GetFollowingsByUserID(ui.UserID))).Length.ToString();
			lblPostNumber.Text = (await Task.Run(() => SQLDataHelper.GetCountOfPostOfUser(ui.UserID))).ToString();

			//帖子列表
			await UpdatePostCollectionAsync();

			//管理员功能
			if (LoginStatusManager.IsLoggedIn)
			{
				UserInfo loginui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID));
				if (loginui.UserRole == UserInfo.Role.Webmaster /*|| loginui.UserRole == UserInfo.Role.CoFounder*/)
				{
					btnEditProfile.IsVisible = true;
				}
			}

			//停止转圈
			adiInitializing.IsRunning = false;
			adiInitializing.IsVisible = false;
		}
		catch (Exception ex)
		{
            object gray400;
            Resources.TryGetValue("Gray400", out gray400);
			this.Content = new Label()
			{
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = (Color)gray400,
				Text = "加载失败，请重试。\n" + ex.Message
            };
        }
    }

    private void btnEditProfile_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("/editprofile?id=" + ui.UserID);
    }

    private void clvPosts_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
		UpdatePostCollectionAsync();
    }

	private async Task UpdatePostCollectionAsync()
	{
		adiPostLoadingStatus.IsRunning = true;

		List<PostInfo> postlist = new();
		List<PostItem> postitems = new();

		if (clvPosts.ItemsSource == null)
		{
			postlist = await Task.Run(() => SQLDataHelper.GetPostsByUserID(ui.UserID, DateTime.Now, 4));
		}
		else
		{
			//若已加载的帖子数等于该用户所有帖子数
			if ((clvPosts.ItemsSource as List<PostItem>).Count == int.Parse(lblPostNumber.Text))
			{
				adiPostLoadingStatus.IsRunning = false;
				return;
			}

			postlist = await Task.Run(() => SQLDataHelper.GetPostsByUserID(
				ui.UserID,
				(clvPosts.ItemsSource as List<PostItem>).Last().PostDate,
				4));
			postitems.AddRange(clvPosts.ItemsSource as List<PostItem>);
		}
		foreach (var v in postlist)
		{
			postitems.Add(new PostItem()
			{
				PostID = v.PostID,
				Title = v.Content.Root.Descendants("Title").ToList()[0].Value,
				Description = v.Content.Root.Descendants("P").ToList()[0].Value,
				PostDate = v.PostDate
			});
		}

		clvPosts.ItemsSource = postitems;

		adiPostLoadingStatus.IsRunning = false;
	}
    private void clvPosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (e.CurrentSelection.FirstOrDefault() != null)
		{
			PostItem selection = e.CurrentSelection.FirstOrDefault() as PostItem;
			clvPosts.SelectedItem = null;
			Shell.Current.GoToAsync("/post?id=" + selection.PostID);
		}
    }
}