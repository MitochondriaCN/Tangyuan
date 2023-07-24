using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class PostPage : ContentPage,IQueryAttributable
{
	/// <summary>
	/// 图片对象，用于CarouselView的数据绑定。
	/// </summary>
	public class ImageInfo
	{
		public string ImageUrl { get; set; }
		public ImageInfo(string imageUrl) { ImageUrl = imageUrl; }
	}

	private uint postID;
	private PostInfo postInfo;

	List<ImageInfo> images=new List<ImageInfo>();
	public PostPage()
	{
		InitializeComponent();
		
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		UICompleter(uint.Parse(query["id"].ToString()));
	}

	private async void UICompleter(uint postID)
	{
		this.postID = postID;
		postInfo = await Task.Run(() => SQLDataHelper.GetPostByID(postID));
		SchoolInfo si = await Task.Run(() => SQLDataHelper.GetSchoolInfoByID(postInfo.SchoolID));


        crvImages.IndicatorView = idvCurrentImage;
		
		//排版基本信息
		lblTitle.Text = postInfo.Content.Root.Descendants("Title").ToList()[0].Value.ToString();
		lblDate.Text = postInfo.PostDate.ToString();
		lblViews.Text = postInfo.Views.ToString();
		lblLikes.Text = postInfo.Likes.ToString();
		bodSchoolContainer.BackgroundColor = Color.Parse(si.ThemeColor.ToHex());
		lblSchool.Text = si.SchoolName;

		//设置作者和管理员功能
		//管理员功能暂缺
		if (LoginStatusManager.IsLoggedIn)
		{
			if (LoginStatusManager.LoggedInUserID == postInfo.AuthorID)
			{
				btnDeletePost.IsVisible = true;
			}
		}

		//排版作者信息
		UserInfo ui = await Task.Run(() => SQLDataHelper.GetUserInfoByID(postInfo.AuthorID));
		imgAvatar.Source = ImageSource.FromUri(new Uri(ui.Avatar));
		grdAuthorBar.Add(new NameTitleView(ui), 1);
		lblAuthorSignature.Text = ui.Signature;

        //排版正文
        TangyuanArranging(postInfo.Content);

		//排版评论
		TangyuanCommentsArranging(await Task.Run(() => SQLDataHelper.GetFirstLevelCommentsByPostID(postID)));
	}

	/// <summary>
	/// 糖原排版引擎。
	/// </summary>
	/// <param name="tangyuanpost">帖子内容，自动排版至用户界面</param>
	private void TangyuanArranging(XDocument tangyuanPost)
	{
		//帖子类型
		if (tangyuanPost.Root.Attribute("Type").Value == "Post")
		{
			if (tangyuanPost.Root.Descendants("ImageGallery").ToList().Count != 0)
			{
				//排版图片展示框
				foreach (var v in tangyuanPost.Root.Descendants("ImageGallery").ToList()[0].Descendants("Image"))
				{
					images.Add(new ImageInfo(v.Value.ToString()));
				}
				crvImages.ItemsSource = images;
			}

			//排版正文
			foreach (var v in tangyuanPost.Root.Descendants("P"))
			{
				stlTextLayouter.Children.Add(new Label()
				{
					LineBreakMode = LineBreakMode.WordWrap,
					Text = v.Value.ToString(),
					Margin = new Thickness(10, 0, 10, 10),
					LineHeight=1.4
				});
			}
		}
	}

	/// <summary>
	/// 糖原评论排版引擎。
	/// </summary>
	/// <param name="comments"></param>
	private void TangyuanCommentsArranging(List<CommentInfo> comments)
	{
		stlCommentsLayouter.Children.Clear();

		lblCommentsNumber.Text = "评论 · " + comments.Count.ToString();
		if (comments.Count > 0)
		{
			foreach (var v in comments)
			{
				stlCommentsLayouter.Children.Add(new CommentView(v));
			}
		}
	}

	private async void ImageButton_Clicked(object sender, EventArgs e)
	{
		if (LoginStatusManager.IsLoggedIn)
		{
			if (edtComment.Text != null && edtComment.Text.Trim() != "")
			{
				await ((ImageButton)sender).RotateTo(360);
				((ImageButton)sender).Rotation = 0;
				string content = edtComment.Text;
				edtComment.Text = "";
				await Task.Run(() => SQLDataHelper.NewComment(LoginStatusManager.LoggedInUserID, postID, content));
                TangyuanCommentsArranging(await Task.Run(() => SQLDataHelper.GetFirstLevelCommentsByPostID(postID)));
            }
		}
	}

	private async void DeletePost_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
		await Task.Run(() => SQLDataHelper.DeletePostByID(postID));
	}
}