using Tangyuan.Data;
using ImageCropper.Maui;
using CommunityToolkit.Maui.Alerts;

namespace Tangyuan.Pages;

public partial class EditProfilePage : ContentPage,IQueryAttributable
{
    UserInfo ui;

    private bool isAvatarChanged;

	public EditProfilePage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        UICompleter(uint.Parse(query["id"].ToString()));
    }

    private async void UICompleter(uint id)
    {
        try
        {
            ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(id));
            SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(ui.SchoolID));
            avtAvatar.ImageSource = ImageSource.FromUri(new Uri(ui.Avatar));
            entNickname.Text = ui.Nickname;
            edtSignature.Text = ui.Signature;
            lblSchool.Text = si.SchoolName;
            lblGrade.Text = si.GradeDefinitions.Find(x => x.GradeID == ui.GradeID).GradeName;
        }
        catch(Exception ex)
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

    private void avtAvatar_Tapped(object sender, TappedEventArgs e)
    {
        new ImageCropper.Maui.ImageCropper()
        {
            PageTitle = "裁剪头像",
            CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Oval,
            AspectRatioX = 1,
            AspectRatioY = 1,
            TakePhotoTitle = "拍照",
            PhotoLibraryTitle = "相册",
            SelectSourceTitle = "选择图片来源",
            CancelButtonTitle = "取消",
            CropButtonTitle = "裁剪",
            Success = imagefile =>
            {
                avtAvatar.ImageSource = imagefile;
                isAvatarChanged = true;
            }
        }.Show(this);
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        adiSaveStatus.IsRunning = true;
        btnSave.IsEnabled = false;
        btnSave.Text = "正在保存";

        try
        {
            string avatar = (avtAvatar.ImageSource as UriImageSource) == null ?
                await Task.Run(() => SmMsHelper.UploadImage(new FileInfo((avtAvatar.ImageSource as FileImageSource).File))) :
                (avtAvatar.ImageSource as UriImageSource).Uri.ToString();
            UserInfo newui = new(ui.UserID, ui.Password, entNickname.Text, edtSignature.Text, ui.PhoneNumber, ui.SchoolID, avatar, ui.GradeID, ui.UserRole);
            await Task.Run(() => SQLDataHelper.UpdateUser(newui));

            Shell.Current.GoToAsync("..?id=" + ui.UserID);
        }
        catch
        {
            Toast.Make("保存失败，请重试").Show();
            adiSaveStatus.IsRunning = false;
            btnSave.IsEnabled = true;
            btnSave.Text = "保存";
        }
    }
}