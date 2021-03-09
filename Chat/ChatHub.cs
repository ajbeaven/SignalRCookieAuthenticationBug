using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Chat
{
	public class ChatHub : Hub
	{
		public Task CheckUser()
		{
			string message;
			if (Context.User.Identity.IsAuthenticated)
				message = $"SignalR sees '{Context.User.Identity.Name}' as logged in";
			else
				message = "SignalR sees no user logged in";

			return Clients.All.SendAsync("ReceiveMessage", message);
		}
	}
}