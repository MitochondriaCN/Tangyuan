using Microsoft.Maui.Graphics;
using Microsoft.Maui.Media;
using System.Drawing;
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
		Microsoft.Maui.Graphics.Color srcColor = btnSend.BackgroundColor;
		btnSend.BackgroundColor = Colors.Gray;
		btnSend.Text = "正在发帖";
		aidSendStatus.IsRunning = true;

		if (!(string.IsNullOrEmpty(edtContent.Text) || string.IsNullOrEmpty(entTitle.Text)))
		{
			XDocument doc;
			try
			{
				doc = await TangyuanEncoding();
			}
			catch (Exception ex)
			{
				await DisplayAlert("异常", ex.Message, "确认");
                btnSend.IsEnabled = true;
                btnSend.Text = "发帖";
                btnSend.BackgroundColor = srcColor;
                aidSendStatus.IsRunning = false;
				return;
            }
			if (LoginStatusManager.IsLoggedIn)
			{
				uint postid = await Task.Run(() => SQLDataHelper.NewPost(LoginStatusManager.LoggedInUserID, doc));
				await Shell.Current.GoToAsync("..");
				Shell.Current.GoToAsync("/post?id=" + postid);
			}
		}
		else
		{
			await DisplayAlert("错误", "请键入标题和内容。", "好的");
			btnSend.IsEnabled = true;
			btnSend.Text = "发帖";
			btnSend.BackgroundColor = srcColor;
			aidSendStatus.IsRunning = false;
		}
	}

	/// <summary>
	/// 糖原编码引擎。
	/// </summary>
	/// <returns></returns>
	private async Task<XDocument> TangyuanEncoding()
	{
		XDocument xd = new XDocument();

		//根节点
		xd.Add(new XElement("TangyuanPost"));
		xd.Root.SetAttributeValue("Type", "Post");

		//ImageGallery
		if (hstImageBar.Children.Count > 0)
		{
			//UI代码本不该出现在此方法，但目前也没办法
			string ori = btnSend.Text;
			int i = 1;

			XElement gallery = new XElement("ImageGallery");
			foreach (var v in hstImageBar.Children)
			{
				//UI代码
				btnSend.Text = "上传图片 " + i + "/" + hstImageBar.Children.Count;
				i++;

				gallery.Add(new XElement("Image",
					await Task.Run(() => SmMsHelper.UploadImage(new(((v as Image).Source as FileImageSource).File)))));//本语句相当壮观
			}
			xd.Root.Add(gallery);

			//UI代码
			btnSend.Text = ori;
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