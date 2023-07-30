using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class UserHomePage : ContentPage,IQueryAttributable
{
	UserInfo ui;

	public UserHomePage()
	{
		InitializeComponent();
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(uint.Parse(query["id"].ToString())));
		UICompleter();
	}

	private async void UICompleter()
	{
		//ͷ���ͷ��
		SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));
        lblUsername.Text = ui.Nickname;
        lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
        lblSchool.Text = si.SchoolName;
        lblRole.Text = ui.GetUserRoleFriendlyName();
        lblAuthorSignature.Text = ui.Signature;
        bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
        bodRoleContainer.BackgroundColor = ui.GetThemeColorOfUserRole();
        bodSchoolContainer.BackgroundColor = si.ThemeColor;
        imgAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));

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

		//����Ա����
		if (LoginStatusManager.IsLoggedIn)
		{
			UserInfo loginui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID));
			if (loginui.UserRole == UserInfo.Role.Webmaster /*|| loginui.UserRole == UserInfo.Role.CoFounder*/)
			{
				btnEditProfile.IsVisible = true;
			}
		}
    }

    private void btnEditProfile_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("/editprofile?id=" + ui.UserID);
    }
}