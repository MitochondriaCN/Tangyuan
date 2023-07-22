using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class PostPage : ContentPage,IQueryAttributable
{
	/// <summary>
	/// ͼƬ��������CarouselView�����ݰ󶨡�
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

		crvImages.IndicatorView = idvCurrentImage;
		
		//�Ű������Ϣ
		lblTitle.Text = postInfo.Content.Root.Descendants("Title").ToList()[0].Value.ToString();
		lblDate.Text = postInfo.PostDate.ToString();
		lblViews.Text = "�Ķ� " + postInfo.Views.ToString();

        //�Ű�����
        TangyuanArranging(postInfo.Content);

		//�Ű�����
		TangyuanCommentsArranging(await Task.Run(() => SQLDataHelper.GetFirstLevelCommentsByPostID(postID)));
	}

	/// <summary>
	/// ��ԭ�Ű����档
	/// </summary>
	/// <param name="tangyuanpost">�������ݣ��Զ��Ű����û�����</param>
	private void TangyuanArranging(XDocument tangyuanPost)
	{
		//��������
		if (tangyuanPost.Root.Attribute("Type").Value == "Post")
		{
			if (tangyuanPost.Root.Descendants("ImageGallery").ToList().Count != 0)
			{
				//�Ű�ͼƬչʾ��
				foreach (var v in tangyuanPost.Root.Descendants("ImageGallery").ToList()[0].Descendants("Image"))
				{
					images.Add(new ImageInfo(v.Value.ToString()));
				}
				crvImages.ItemsSource = images;
				if (images.Count == 1)
				{
					crvImages.IsSwipeEnabled = false;
				}
			}

			//�Ű�����
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
	/// ��ԭ�����Ű����档
	/// </summary>
	/// <param name="comments"></param>
	private void TangyuanCommentsArranging(List<CommentInfo> comments)
	{
		lblCommentsNumber.Text = "���� �� " + comments.Count.ToString();
		if (comments.Count > 0)
		{
			foreach (var v in comments)
			{
				stlCommentsLayouter.Children.Add(new CommentView(v));
			}
		}
	}
}