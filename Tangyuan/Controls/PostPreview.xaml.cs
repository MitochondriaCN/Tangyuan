using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class PostPreview : ContentView
{
	private PostInfo postInfo;

	public PostPreview(PostInfo post)
	{
        InitializeComponent();

		imgImage.Source = (post.Content.Root.Descendants("Image").ToList().Count != 0) ?
			(ImageSource.FromUri(new Uri(post.Content.Root.Descendants("Image").ToList()[0].Value))) :
			null;
		lblTitle.Text = post.Content.Root.Descendants("Title").ToList()[0].Value;
		lblDesc.Text = post.Content.Root.Descendants("P").ToList()[0].Value;
		lblInfo.Text = SQLDataHelper.GetUserNicknameByID(post.AuthorID);

		postInfo=post;
    }

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("/post?id=" + postInfo.PostID);
	}
}