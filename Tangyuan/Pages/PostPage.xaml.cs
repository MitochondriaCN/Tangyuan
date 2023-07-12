using System.Xml.Linq;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class PostPage : ContentPage,IQueryAttributable
{
	/// <summary>
	/// ͼƬ��������CarouselView�����ݰ󶨡�
	/// </summary>
	internal struct ImageInfo
	{
		internal string ImageUrl { get; private set; }
		internal ImageInfo(string imageUrl) { ImageUrl = imageUrl; }
	}

	private uint postID;
	List<ImageInfo> images=new List<ImageInfo>();
	public PostPage()
	{
		InitializeComponent();
		
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		postID = uint.Parse(query["id"].ToString());
		PostInfo pi = SQLDataHelper.GetPostByID(postID);
		lblTitle.Text = pi.Content.Root.Descendants("Title").ToList()[0].Value.ToString();
		lblDate.Text = pi.PostDate.Date.ToString();
		TangyuanArranging(pi.Content);
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
			//�Ű�ͼƬչʾ��
			foreach (var v in tangyuanPost.Root.Descendants("ImageGallery").ToList()[0].Descendants("Image"))
			{
				images.Add(new ImageInfo(v.Value.ToString()));
			}

			//�Ű�����
			foreach (var v in tangyuanPost.Root.Descendants("P"))
			{
				stlTextLayouter.Children.Add(new Label()
				{
					LineBreakMode = LineBreakMode.WordWrap,
					Text = v.Value.ToString(),
					Margin = new Thickness(0, 0, 0, 10)
				});
			}
		}
	}
}