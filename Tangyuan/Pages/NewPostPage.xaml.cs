using Microsoft.Maui.Media;
using System.Xml.Linq;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class NewPostPage : ContentPage
{
	public NewPostPage()
	{
		InitializeComponent();
	}

	private void edtContent_TextChanged(object sender, TextChangedEventArgs e)
	{
		lblRemainingTextLength.Text = "剩余 " + (edtContent.MaxLength - edtContent.Text.Length).ToString();
	}

	private async void btnSend_Clicked(object sender, EventArgs e)
	{
		btnSend.IsEnabled = false;
		btnSend.Text = "正在发送";
		XDocument doc = await TangyuanEncoding();
		if (doc != null)
		{
			if (LoginStatusManager.IsLoggedIn)
			{
				SQLDataHelper.NewPost(LoginStatusManager.LoggedInUserID, doc);
				await Shell.Current.GoToAsync("..");
			}
		}
		else
		{
			await DisplayAlert("错误", "请键入标题和内容。", "好的");
			btnSend.IsEnabled = true;
			btnSend.Text = "发帖";
		}
	}

	/// <summary>
	/// 糖原编码引擎。
	/// </summary>
	/// <returns></returns>
	private async Task<XDocument> TangyuanEncoding()
	{
		if (entTitle.Text != null && edtContent.Text != null)
		{
			XDocument xd = new XDocument();

			//根节点
			xd.Add(new XElement("TangyuanPost"));
			xd.Root.SetAttributeValue("Type", "Post");

			//ImageGallery
			if (hstImageBar.Children.Count > 0)
			{
				XElement gallery = new XElement("ImageGallery");
				foreach (var v in hstImageBar.Children)
				{
					gallery.Add(new XElement("Image",
						await Task.Run(() => SmMsHelper.UploadImageAsync(new((((v as Image).Source) as FileImageSource).File)))));//本语句相当壮观
				}
				xd.Root.Add(gallery);
			}

			//Title
			xd.Root.Add(new XElement("Title", entTitle.Text));

			//P
			foreach (var v in edtContent.Text.Trim().Split('\n'))
			{
				xd.Root.Add(new XElement("P", v));
			}

			return xd;
		}
		else
		{
			return null;
		}
	}

	private async void btnPhoto_Clicked(object sender, EventArgs e)
	{
		FileResult photo = await MediaPicker.Default.PickPhotoAsync();
		if (photo != null)
		{
			hstImageBar.Add(new Image
			{
				Source = photo.FullPath,
				Aspect = Aspect.AspectFill,
				HeightRequest = 80,
				WidthRequest = 80,
				Margin = new Thickness(0, 0, 5, 0)
			});
		}
	}
}