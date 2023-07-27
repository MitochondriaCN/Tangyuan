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
		if ((!string.IsNullOrEmpty(entPhoneNumber.Text)) && entPhoneNumber.Text.Length == 11)
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
				btnNext.Text = "ע��";
				if (!(string.IsNullOrEmpty(entUsername.Text)) && (!string.IsNullOrEmpty(entNewUserPasswd.Text)) && lstSchoolSelector.SelectedItem != null && pckGradePicker.SelectedItem != null)
				{
					if (SQLDataHelper.TrySignUp(entPhoneNumber.Text,
						entUsername.Text,
						entNewUserPasswd.Text, (lstSchoolSelector.SelectedItem as SchoolInfo).SchoolID,
						(pckGradePicker.SelectedItem as SchoolInfo.GradeDefinition).GradeID))
					{
						await DisplayAlert("��ע��", "ע��ɹ����������¼��", "ȷ��");
						await Shell.Current.GoToAsync("..");
						await Shell.Current.GoToAsync("/login");
					}
				}
			}
		}
		if ((!string.IsNullOrEmpty(entPasswd.Text)) && entPasswd.Text.Length > 6)
		{
			if (LoginStatusManager.TryLogIn(entPhoneNumber.Text, entPasswd.Text))
			{
				await DisplayAlert("�ѵ�¼", "��¼�ɹ���������ԭ��Ч��", "ȷ��");
				await Shell.Current.GoToAsync("..");
				App.Current.Quit();
			}
		}
		
	}

	private async void SearchSchoolAsync()
	{
		List<SchoolInfo> schools = await Task.Run(() => SQLDataHelper.GetAllSchoolInfos());
		lstSchoolSelector.ItemsSource = schools;
	}

	private void entPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
	{
		entPasswd.IsVisible = false;
		btnNext.Text = "����";
		entPasswd.Text = "";
		vstSignUpFormLayouter.IsVisible = false;
		entUsername.Text = null;
		entNewUserPasswd.Text = null;
		scbSchoolSelector.Text = "";
		lstSchoolSelector.ItemsSource = null;
		pckGradePicker.ItemsSource = null;
	}

	private void scbSchoolSelector_TextChanged(object sender, TextChangedEventArgs e)
	{
		lstSchoolSelector.IsVisible = true;
		pckGradePicker.ItemsSource = null;
		SearchSchoolAsync();
	}

	private void lstSchoolSelector_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		pckGradePicker.ItemsSource = (lstSchoolSelector.SelectedItem as SchoolInfo).GradeDefinitions;
		pckGradePicker.SelectedIndex = 0;
	}
}