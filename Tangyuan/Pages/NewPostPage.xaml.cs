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

			xd.Add(new XElement("TangyuanPost"));
			xd.Root.SetAttributeValue("Type", "Post");
			xd.Root.Add(new XElement("Title", entTitle.Text));

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
}