

namespace Pulse
{
	public static class ServiceRegistrar
	{
		public static void Startup()
		{
			// Register ViewModels
			ServiceContainer.Register<AuthenticationViewModel>();
			ServiceContainer.Register<FriendsViewModel>();
			ServiceContainer.Register<EventViewModel>();
			ServiceContainer.Register<ProfileViewModel>();
			ServiceContainer.Register<PulseViewModel>();
		}
	}
}

