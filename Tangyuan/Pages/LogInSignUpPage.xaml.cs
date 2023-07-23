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
		if (entPhoneNumber.Text != null && entPhoneNumber.Text.Length == 11)
		{
			if (SQLDataHelper.GetUserInfoByPhoneNumber(entPhoneNumber.Text) != null)
			{
				//登录流程
				entPasswd.IsVisible = true;
				btnNext.Text = "登录";
			}
			else
			{
				//注册流程
				vstSignUpFormLayouter.IsVisible = true;
			}
		}
		if (entPasswd.Text != null && entPasswd.Text.Length > 6)
		{
			if (LoginStatusManager.TryLogIn(entPhoneNumber.Text, entPasswd.Text))
			{
				await DisplayAlert("已登录", "登录成功，重启糖原生效。", "确定");
				await Shell.Current.GoToAsync("..");
				App.Current.Quit();
			}
		}
		
	}

	private async void UpdateSchoolInfoAsync()
	{
		
	}

	private void entPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
	{
		entPasswd.IsVisible = false;
		btnNext.Text = "继续";
		entPasswd.Text = "";
		vstSignUpFormLayouter.IsVisible = false;
		entUsername.Text = "";
		entNewUserPasswd.Text = "";
		scbSchoolSelector.Text = "";
		lstSchoolSelector.ItemsSource = null;
		pckGradePicker.ItemsSource = null;
	}
}