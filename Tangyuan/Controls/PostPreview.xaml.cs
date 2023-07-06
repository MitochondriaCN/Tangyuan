namespace Tangyuan.Controls;

public partial class PostPreview : ContentView
{
	public PostPreview(string desc, string author, string title = null, string imageUrl = null)
	{
		InitializeComponent();

		lblDesc.Text = desc;
		lblTitle.Text = title;
		imgImage.Source = imageUrl;
		lblInfo.Text = author;
	}
}