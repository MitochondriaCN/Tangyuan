using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class NameTitleView : ContentView
{
	internal NameTitleView(UserInfo ui, bool isSchoolDisplay = true, bool isGradeDisplay = true, bool isRoleDisplay = true,
		params UserInfo.Role[] invisibleRoles)
	{
		InitializeComponent();

		UpdateUIAsync(ui, isSchoolDisplay, isGradeDisplay, isRoleDisplay, invisibleRoles);
	}

	private async void UpdateUIAsync(UserInfo ui, bool isSchoolDisplay = true, bool isGradeDisplay = true, bool isRoleDisplay = true,
		params UserInfo.Role[] invisibleRoles)
	{
		SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));
		lblNickname.Text = ui.Nickname;
		bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
		lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
		bodSchoolContainer.BackgroundColor = si.ThemeColor;
		lblSchool.Text = si.SchoolName;
		lblRole.Text = ui.GetUserRoleFriendlyName();
		bodRoleContainer.BackgroundColor = ui.GetThemeColorOfUserRole();
		bodGradeContainer.IsVisible = isGradeDisplay;
		bodRoleContainer.IsVisible = isRoleDisplay;
		bodSchoolContainer.IsVisible = isSchoolDisplay;

		foreach (var v in invisibleRoles)
		{
			if (ui.UserRole == v)
			{
				bodRoleContainer.IsVisible = false;
			}
		}
	}
}