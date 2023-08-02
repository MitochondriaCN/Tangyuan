using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class UserHomePage : ContentPage,IQueryAttributable
{
	UserInfo ui;

	/// <summary>
	/// ����CollectionView���ݰ�
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
		//΢��������Ƽ�ֱ��ɵ���е�ɵ�ƣ������ҳ�汻������ʱ��query����������Ȼ���������ִ���꣬
		//���������Ȼ�����һֱ������ֱ�����ҳ�汻������Ҳ����˵������������ҳ���ϵ�������һ��ҳ�棬
		//�ٴ���һ��ҳ�������ʱ�����B�����������ҳ�汻����ʱ��Ĳ�����Ϊ�˽��������⣬�Ҳ��ò���
		//�������ִ�����ǰ��query�����ˡ�
	}

	private async void UICompleter(uint id)
	{
		try
		{
			ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(id));
            SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));

			//תȦ
			adiInitializing.IsRunning = true;

			//ͷ���ͷ��
			lblUsername.Text = ui.Nickname;
			lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
			lblSchool.Text = si.SchoolName;
			lblRole.Text = ui.GetUserRoleFriendlyName();
			lblAuthorSignature.Text = ui.Signature;
			bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
			bodRoleContainer.BackgroundColor = ui.GetThemeColorOfUserRole();
			bodSchoolContainer.BackgroundColor = si.ThemeColor;
			imgAvatar.ImageSource = ImageSource.FromUri(new Uri(ui.Avatar));

			//��ע�ͱ༭���ϰ�ť
			if (ui.UserID == LoginStatusManager.LoggedInUserID)
			{
				btnEditProfile.IsVisible = true;
			}
			else
			{
				btnFollow.IsVisible = true;
			}

			//��ע����˿��������
			lblFans.Text = (await Task.Run(() => SQLDataHelper.GetFansByUserID(ui.UserID))).Length.ToString();
			lblFollowings.Text = (await Task.Run(() => SQLDataHelper.GetFollowingsByUserID(ui.UserID))).Length.ToString();
			lblPostNumber.Text = (await Task.Run(() => SQLDataHelper.GetCountOfPostOfUser(ui.UserID))).ToString();

			//�����б�
			await UpdatePostCollectionAsync();

			//����Ա����
			if (LoginStatusManager.IsLoggedIn)
			{
				UserInfo loginui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID));
				if (loginui.UserRole == UserInfo.Role.Webmaster /*|| loginui.UserRole == UserInfo.Role.CoFounder*/)
				{
					btnEditProfile.IsVisible = true;
				}
			}

			//ֹͣתȦ
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
				Text = "����ʧ�ܣ������ԡ�\n" + ex.Message
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
			//���Ѽ��ص����������ڸ��û�����������
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