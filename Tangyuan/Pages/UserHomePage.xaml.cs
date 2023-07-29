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
    }
}