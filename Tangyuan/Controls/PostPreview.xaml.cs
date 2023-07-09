using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class PostPreview : ContentView
{
	public PostPreview(string desc, string author, string title = null, string imageUrl = null)
	{
		InitializeComponent();

		imgImage.Source = ImageSource.FromUri(new Uri(imageUrl));
		lblDesc.Text = desc;
		lblTitle.Text = title;
		lblInfo.Text = author;
	}

	public PostPreview(PostInfo post)
	{
        InitializeComponent();

		imgImage.Source=ImageSource.FromUri(new Uri(post.Content.Root.Descendants("Image").ToList()[0].Value));
		lblTitle.Text = post.Content.Root.Descendants("Title").ToList()[0].Value;
		lblDesc.Text = post.Content.Root.Descendants("P").ToList()[0].Value;
		lblInfo.Text = SQLDataHelper.GetUserNicknameByID(post.AuthorID);
    }
}