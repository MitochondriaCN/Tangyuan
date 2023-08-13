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
		if (query.Count != 0)
		{
			UICompleter(uint.Parse(query["id"].ToString()));
			query.Clear();
		}
	}

	private async Task UICompleter(uint postID)
	{
		try
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
			grdAuthorBar.Add(new NameTitleView(ui, true, true, true, UserInfo.Role.OrdinaryStudent), 1);
			lblAuthorSignature.Text = ui.Signature;

			//排版合集信息
			if (postInfo.IsInCollection)
			{
				CollectionInfo ci = await Task.Run(() => SQLDataHelper.GetCollectionByID(postInfo.CollectionID));
				grdCollectionContainer.IsVisible = true;
				lblCollectionName.Text = ci.CollectionName;
				lblCollectionInfo.Text = "合集 · " + ci.Description;
			}

			//排版正文
			TangyuanArranging(postInfo.Content);

			//排版评论
			TangyuanCommentsArranging(await Task.Run(() => SQLDataHelper.GetFirstLevelCommentsByPostID(postID)));

			//增加阅读数
			Task.Run(() => SQLDataHelper.AddPostViewByID(postID));
		}
		catch
		{
			object gray400;
			Resources.TryGetValue("Gray400", out gray400);
			this.Content = new Label()
			{
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = (Color)gray400,
				Text = "加载失败，请重试。"
			};
		}
	}

	/// <summary>
	/// 糖原排版引擎。
	/// </summary>
	/// <param name="tangyuanpost">帖子内容，自动排版至用户界面</param>
	private void TangyuanArranging(XDocument tangyuanPost)
	{
		//先清空
		images.Clear();
		stlTextLayouter.Children.Clear();

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
					Margin = new Thickness(10, 0, 10, 15),
					LineHeight=1.5
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
		//先清空
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
			if (!string.IsNullOrEmpty(edtComment.Text))
			{
				try
				{
					await ((ImageButton)sender).RotateTo(360);
					((ImageButton)sender).Rotation = 0;
					string content = edtComment.Text;
					await Task.Run(() => SQLDataHelper.NewComment(LoginStatusManager.LoggedInUserID, postID, content));
                    edtComment.Text = "";
                    try
                    {
                        TangyuanCommentsArranging(await Task.Run(() => SQLDataHelper.GetFirstLevelCommentsByPostID(postID)));
                    }
                    catch
                    {
                        stlCommentsLayouter.Children.Clear();
                        object gray400;
                        Resources.TryGetValue("Gray400", out gray400);
                        stlCommentsLayouter.Add(new Label()
                        {
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = (Color)gray400,
                            Text = "评论加载失败，请重试。"
                        });
                    }
                }
				catch
				{
					await DisplayAlert("异常", "评论失败", "确定");
				}
				
            }
		}
	}

	private async void DeletePost_Clicked(object sender, EventArgs e)
	{
		try
		{
			await Shell.Current.GoToAsync("..");
			await Task.Run(() => SQLDataHelper.DeletePostByID(postID));
		}
		catch
		{
			await DisplayAlert("异常", "删帖失败", "确定");
		}
	}

    private async void LikeButton_Clicked(object sender, EventArgs e)
    {
		if (LoginStatusManager.IsLoggedIn)
		{
			try
			{
				(sender as ImageButton).Source = ImageSource.FromFile("icon_loved.png");
				lblLikes.Text = (postInfo.Likes + 1).ToString();
				object primaryColor;
				App.Current.Resources.TryGetValue("Primary", out primaryColor);
				lblLikes.TextColorTo((Color)primaryColor, 32, 300);
                await Task.Run(() => SQLDataHelper.AddPostLikeByID(postID));
            }
			catch
			{
                (sender as ImageButton).Source = ImageSource.FromFile("icon_love.png");
                lblLikes.Text = postInfo.Likes.ToString();
                object prestigeColor;
                App.Current.Resources.TryGetValue("Prestige", out prestigeColor);
                lblLikes.TextColorTo((Color)prestigeColor, 32, 300);
                await DisplayAlert("异常", "点赞失败", "确定");
				return;
			}
		}
    }

    private void grdAuthorBar_Tapped(object sender, TappedEventArgs e)
    {
		Shell.Current.GoToAsync("/user?id=" + postInfo.AuthorID);
    }

    private void Image_Tapped(object sender, TappedEventArgs e)
    {
		Shell.Current.GoToAsync("/image?uri=" + ((sender as Image).Source as UriImageSource).Uri.ToString());
    }
}