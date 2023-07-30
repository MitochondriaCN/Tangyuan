using Tangyuan.Data;
using Tangyuan.Pages;

namespace Tangyuan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("post", typeof(PostPage));
		Routing.RegisterRoute("login", typeof(LogInSignUpPage));
		Routing.RegisterRoute("newpost", typeof(NewPostPage));
		Routing.RegisterRoute("user", typeof(UserHomePage));
		Routing.RegisterRoute("editprofile", typeof(EditProfilePage));
    }
}
