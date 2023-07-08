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
				stlPostsLeft.Children.Add(new PostPreview(v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "P").Value,
					v.AuthorID.ToString(),
					v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "Title").Value,
					v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "Image").Value));
			}
			else
			{
                stlPostsRight.Children.Add(new PostPreview(v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "P").Value,
                    v.AuthorID.ToString(),
                    v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "Title").Value,
                    v.Content.Root.Descendants().FirstOrDefault(a => a.Name.LocalName == "Image").Value));
            }
			signal++;
		}
    }
}