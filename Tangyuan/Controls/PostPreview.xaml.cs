using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class PostPreview : ContentView
{
	internal PostInfo Post;

	public PostPreview(PostInfo post)
	{
        InitializeComponent();

		Post=post;
		UICompleter();
    }

	private async void UICompleter()
	{
		imgImage.Source = (Post.Content.Root.Descendants("Image").ToList().Count != 0) ?
			(ImageSource.FromUri(new Uri(Post.Content.Root.Descendants("Image").ToList()[0].Value))) :
			null;
		lblTitle.Text = Post.Content.Root.Descendants("Title").ToList()[0].Value;
		lblDesc.Text = Post.Content.Root.Descendants("P").ToList()[0].Value;
		lblInfo.Text = await Task.Run(() => SQLDataHelper.GetUserNicknameByID(Post.AuthorID));
	}

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("/post?id=" + Post.PostID);
	}
}