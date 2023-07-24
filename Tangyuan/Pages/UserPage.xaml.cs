using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();

		if (LoginStatusManager.IsLoggedIn)
		{
			UserInfo ui = SQLDataHelper.GetUserInfoByID(LoginStatusManager.LoggedInUserID);
			SchoolInfo si = SQLDataHelper.GetSchoolInfoByID(ui.SchoolID);
			lblUsername.Text = ui.Nickname;
			lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
			lblSchool.Text = si.SchoolName;
			lblRole.Text = ui.GetUserRoleFriendlyName();
			lblAuthorSignature.Text = ui.Signature;
			bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
			bodRoleContainer.BackgroundColor = ui.GetThemeColorOfUserRole();
			bodSchoolContainer.BackgroundColor = si.ThemeColor;
			imgAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
        }
	}
}