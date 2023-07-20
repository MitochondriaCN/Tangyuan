using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class PostPreview : ContentView
{
	internal PostInfo Post;

	public PostPreview(PostInfo post)
	{
        InitializeComponent();

		imgImage.Source = (post.Content.Root.Descendants("Image").ToList().Count != 0) ?
			(ImageSource.FromUri(new Uri(post.Content.Root.Descendants("Image").ToList()[0].Value))) :
			null;
		lblTitle.Text = post.Content.Root.Descendants("Title").ToList()[0].Value;
		lblDesc.Text = post.Content.Root.Descendants("P").ToList()[0].Value;
		lblInfo.Text = SQLDataHelper.GetUserNicknameByID(post.AuthorID);

		Post=post;
    }

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("/post?id=" + Post.PostID);
	}
}