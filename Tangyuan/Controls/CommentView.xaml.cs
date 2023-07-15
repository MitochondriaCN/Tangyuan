using Tangyuan.Data;

namespace Tangyuan.Controls;

public partial class CommentView : ContentView
{
	internal CommentView(CommentInfo comment)
	{
		InitializeComponent();

		UserInfo user = SQLDataHelper.GetUserInfoByID(comment.UserID);
		lblUserName.Text = user.Nickname;
		imbAvatar.Source = ImageSource.FromUri(new Uri(user.Avatar));
		lblDate.Text = comment.Date.ToString();
		lblLikes.Text = comment.Likes.ToString();
		lblGrade.Text = "¸ßÈý";
		lblContent.Text = comment.Content;
	}
}