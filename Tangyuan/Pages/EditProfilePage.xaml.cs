using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class EditProfilePage : ContentPage,IQueryAttributable
{
    UserInfo ui;

	public EditProfilePage()
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
        avtAvatar.ImageSource = ImageSource.FromUri(new Uri(ui.Avatar));
        entNickname.Text = ui.Nickname;
        edtSignature.Text = ui.Signature;
        lblSchool.Text = si.SchoolName;
        lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
    }
}