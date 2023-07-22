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

	private void btnSend_Clicked(object sender, EventArgs e)
	{
		XDocument doc = TangyuanEncoding();
		if (doc != null)
		{
			if (LoginStatusManager.IsLoggedIn)
			{
				SQLDataHelper.NewPost(LoginStatusManager.LoggedInUserID, doc);
				Shell.Current.GoToAsync("..");
			}
		}
		else
		{
			DisplayAlert("错误", "请键入标题和内容。", "好的");
		}
	}

	/// <summary>
	/// 糖原编码引擎。
	/// </summary>
	/// <returns></returns>
	private XDocument TangyuanEncoding()
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
					gallery.Add(new XElement("Image", (((v as Image).Source) as FileImageSource).File));
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