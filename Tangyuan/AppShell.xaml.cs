using Tangyuan.Data;
using Tangyuan.Pages;

namespace Tangyuan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("index/post", typeof(PostPage));
		Routing.RegisterRoute("index/login", typeof(LogInSignUpPage));
		Routing.RegisterRoute("index/newpost", typeof(NewPostPage));
    }
}
