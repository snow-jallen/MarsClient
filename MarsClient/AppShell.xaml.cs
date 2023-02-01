namespace MarsClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("Move", typeof(MovePage));
	}
}
