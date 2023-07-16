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
			btnNext.Text = "��¼";
		}
		if (entPasswd.Text.Length > 6)
		{
			if (LoginStatusManager.TryLogIn(entPhoneNumber.Text, entPasswd.Text))
			{
				await DisplayAlert("�ѵ�¼", "��¼�ɹ���������ԭ��Ч��", "ȷ��");
				await Shell.Current.GoToAsync("..");
				App.Current.Quit();
			}
		}
		
	}

	private void entPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
	{
		entPasswd.IsVisible = false;
		btnNext.Text = "����";
		entPasswd.Text = "";
	}
}