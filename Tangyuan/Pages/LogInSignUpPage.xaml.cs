using Tangyuan.Data;

namespace Tangyuan.Pages;

public partial class LogInSignUpPage : ContentPage
{
	public LogInSignUpPage()
	{
		InitializeComponent();
	}

	private async void btnNext_Clicked(object sender, EventArgs e)
	{
		if (entPhoneNumber.Text.Length == 11)
		{
			entPasswd.IsVisible = true;
			btnNext.Text = "登录";
		}
		if (entPasswd.Text.Length > 6)
		{
			if (LoginStatusManager.TryLogIn(entPhoneNumber.Text, entPasswd.Text))
			{
				await DisplayAlert("已登录", "登录成功，重启糖原生效。", "确定");
				await Shell.Current.GoToAsync("..");
				App.Current.Quit();
			}
		}
		
	}

	private void entPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
	{
		entPasswd.IsVisible = false;
		btnNext.Text = "继续";
		entPasswd.Text = "";
	}
}