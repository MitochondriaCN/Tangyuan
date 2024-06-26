using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class CommentView : ContentView
{
	private CommentInfo comment { get; set; }
	internal CommentView(CommentInfo comment)
	{
		InitializeComponent();

		this.comment = comment;
		UpdateUIAsync();
	}

	private async Task UpdateUIAsync()
	{
		lblDate.Text = comment.Date.ToString();
		lblLikes.Text = comment.Likes.ToString();
		lblContent.Text = comment.Content;
		UserInfo user = await Task.Run(() => SQLDataHelper.GetUserInfoByID(comment.UserID));
		grdNameDateLayouter.Add(new NameTitleView(user, true, true, true, UserInfo.Role.OrdinaryStudent));
		imbAvatar.Source = ImageSource.FromUri(new Uri(user.Avatar));
	}
}