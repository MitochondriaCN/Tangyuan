using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class NameTitleView : ContentView
{
    internal NameTitleView(UserInfo ui)
	{
		InitializeComponent();

        UpdateUIAsync(ui);
	}

	private async void UpdateUIAsync(UserInfo ui, bool isSchoolDisplay = true, bool isGradeDisplay = true, bool isRoleDisplay = true)
	{
		SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));
		lblNickname.Text = ui.Nickname;
		bodGradeContainer.BackgroundColor = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).ThemeColor;
		lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
		bodSchoolContainer.BackgroundColor = si.ThemeColor;
		lblSchool.Text = si.SchoolName;
		switch (ui.UserRole)
		{
			case UserInfo.Role.Webmaster:
				bodRoleContainer.BackgroundColor = Color.Parse("#e74c3c");
				lblRole.Text = "站长";
				break;
			case UserInfo.Role.CoFounder:
                bodRoleContainer.BackgroundColor = Color.Parse("#f1c40f");
                lblRole.Text = "联合创始人";
				break;
            case UserInfo.Role.Observer:
                bodRoleContainer.BackgroundColor = Color.Parse("#30336b");
                lblRole.Text = "观察员";
                break;
            case UserInfo.Role.SchoolLeader:
                bodRoleContainer.BackgroundColor = Color.Parse("#27ae60");
                lblRole.Text = "校园领袖";
                break;
            case UserInfo.Role.GradeLeader:
                bodRoleContainer.BackgroundColor = Color.Parse("#3498db");
                lblRole.Text = "年级领袖";
                break;
            case UserInfo.Role.ClassLeader:
                bodRoleContainer.BackgroundColor = Color.Parse("#9b59b6");
                lblRole.Text = "班级领袖";
                break;
            case UserInfo.Role.Teacher:
                bodRoleContainer.BackgroundColor = Color.Parse("#c0392b");
                lblRole.Text = "老师";
                break;
            default:
                bodRoleContainer.IsVisible = false;
                break;
        }
        bodGradeContainer.IsVisible = isGradeDisplay;
        bodRoleContainer.IsVisible = isRoleDisplay;
        bodSchoolContainer.IsVisible = isSchoolDisplay;
    }
}