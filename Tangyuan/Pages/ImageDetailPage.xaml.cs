namespace Tangyuan.Pages;

public partial class ImageDetailPage : ContentPage,IQueryAttributable
{
	public ImageDetailPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        imgImage.Source = ImageSource.FromUri(new Uri(query["uri"].ToString()));
    }
}