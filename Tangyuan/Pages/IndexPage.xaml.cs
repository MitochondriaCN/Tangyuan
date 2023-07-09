using System.Xml.Linq;
using Tangyuan.Controls;
using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

		List<PostInfo> posts = SQLDataHelper.GetRecentPosts(5);
		uint signal = 1;
		foreach (var v in posts)
		{
			if (signal % 2 == 0)
			{
				stlPostsLeft.Children.Add(new PostPreview(v));
            }
			else
			{
				stlPostsRight.Children.Add(new PostPreview(v));
            }
			signal++;
		}
    }
}