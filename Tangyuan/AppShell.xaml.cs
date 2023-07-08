using Tangyuan.Data;

namespace Tangyuan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        SQLDataHelper.SetNewConnection("81.68.124.30", 3306, "tangyuan", "tangyuan", "Fuyuxuan372819");
    }
}
