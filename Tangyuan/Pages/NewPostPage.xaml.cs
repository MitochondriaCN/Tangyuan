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
		lblRemainingTextLength.Text = "ʣ�� " + (edtContent.MaxLength - edtContent.Text.Length).ToString();
	}

	private async void btnSend_Clicked(object sender, EventArgs e)
	{
		btnSend.IsEnabled = false;
		Microsoft.Maui.Graphics.Color srcColor = btnSend.BackgroundColor;
		btnSend.BackgroundColor = Colors.Gray;
		btnSend.Text = "���ڷ���";
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
				await DisplayAlert("�쳣", ex.Message, "ȷ��");
                btnSend.IsEnabled = true;
                btnSend.Text = "����";
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
			await DisplayAlert("����", "������������ݡ�", "�õ�");
			btnSend.IsEnabled = true;
			btnSend.Text = "����";
			btnSend.BackgroundColor = srcColor;
			aidSendStatus.IsRunning = false;
		}
	}

	/// <summary>
	/// ��ԭ�������档
	/// </summary>
	/// <returns></returns>
	private async Task<XDocument> TangyuanEncoding()
	{
		XDocument xd = new XDocument();

		//���ڵ�
		xd.Add(new XElement("TangyuanPost"));
		xd.Root.SetAttributeValue("Type", "Post");

		//ImageGallery
		if (hstImageBar.Children.Count > 0)
		{
			//UI���뱾���ó����ڴ˷�������ĿǰҲû�취
			string ori = btnSend.Text;
			int i = 1;

			XElement gallery = new XElement("ImageGallery");
			foreach (var v in hstImageBar.Children)
			{
				//UI����
				btnSend.Text = "�ϴ�ͼƬ " + i + "/" + hstImageBar.Children.Count;
				i++;

				gallery.Add(new XElement("Image",
					await Task.Run(() => SmMsHelper.UploadImage(new(((v as Image).Source as FileImageSource).File)))));//������൱׳��
			}
			xd.Root.Add(gallery);

			//UI����
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