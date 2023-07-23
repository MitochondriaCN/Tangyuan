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
				//��¼����
				entPasswd.IsVisible = true;
				btnNext.Text = "��¼";
			}
			else
			{
				//ע������
				vstSignUpFormLayouter.IsVisible = true;
			}
		}
		if (entPasswd.Text != null && entPasswd.Text.Length > 6)
		{
			if (LoginStatusManager.TryLogIn(entPhoneNumber.Text, entPasswd.Text))
			{
				await DisplayAlert("�ѵ�¼", "��¼�ɹ���������ԭ��Ч��", "ȷ��");
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
		btnNext.Text = "����";
		entPasswd.Text = "";
		vstSignUpFormLayouter.IsVisible = false;
		entUsername.Text = "";
		entNewUserPasswd.Text = "";
		scbSchoolSelector.Text = "";
		lstSchoolSelector.ItemsSource = null;
		pckGradePicker.ItemsSource = null;
	}
}