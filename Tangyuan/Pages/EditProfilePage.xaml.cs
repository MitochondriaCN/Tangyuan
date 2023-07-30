using Tangyuan.Data;
using ImageCropper.Maui;

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

    private void avtAvatar_Tapped(object sender, TappedEventArgs e)
    {
        new ImageCropper.Maui.ImageCropper()
        {
            PageTitle = "�ü�ͷ��",
            CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Oval,
            AspectRatioX = 1,
            AspectRatioY = 1,
            TakePhotoTitle = "����",
            PhotoLibraryTitle = "���",
            SelectSourceTitle = "ѡ��ͼƬ��Դ",
            CancelButtonTitle = "ȡ��",
            CropButtonTitle = "�ü�",
            Success = imagefile =>
            {
                avtAvatar.ImageSource = imagefile;
            }
        }.Show(this);
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        adiSaveStatus.IsRunning = true;
        btnSave.IsEnabled = false;
        btnSave.Text = "���ڱ���";

        string avatar = await Task.Run(() => SmMsHelper.UploadImage(new FileInfo((avtAvatar.ImageSource as FileImageSource).File)));
        UserInfo newui = new(ui.UserID, ui.Password, entNickname.Text, edtSignature.Text, ui.PhoneNumber, ui.SchoolID, avatar, ui.GradeID, ui.UserRole);
        await Task.Run(() => SQLDataHelper.UpdateUser(newui));

        Shell.Current.GoToAsync("..?id=" + ui.UserID);
    }
}