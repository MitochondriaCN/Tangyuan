using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class MarketPage : ContentPage
{
	/// <summary>
	/// CollectionView绑定商品信息
	/// </summary>
	public class ProductItem
	{
		public string Description { get; set; }
		public string Price { get; set; }
		public ImageSource PreviewImage { get; set; }
	}

	public MarketPage()
	{
		InitializeComponent();

		UICompleter();
	}

	private async Task UICompleter()
	{
		UpdateRecommondedProducts();
	}

	private async Task UpdateRecommondedProducts()
	{
		adiListLoading.IsRunning = true;

		List<ProductInfo> pis = await Task.Run(() => SQLDataHelper.GetProductInfosRandomly(100));
		var pitems = new List<ProductItem>();
		foreach (var v in pis)
		{
			pitems.Add(new()
			{
				Description = v.Content.Root.Descendants("P").ToList()[0].Value,
				Price = v.Content.Root.Descendants("Price").ToList()[0].Value,
				PreviewImage = ImageSource.FromUri(new(v.Content.Root.Descendants("ImageGallery").ToList()[0].Descendants("Image").ToList()[0].Value))
			});
		}
		clvRecommendedProductList.ItemsSource = pitems;

		adiListLoading.IsRunning = false;
	}

    private void clvRecommendedProductList_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
		UpdateRecommondedProducts();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		UpdateRecommondedProducts();
    }
}