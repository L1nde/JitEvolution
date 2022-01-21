using JitEvolution.BusinessObjects.Identity;
using JitEvolution.SignalR.Constants;
using Microsoft.AspNetCore.SignalR;

namespace JitEvolution.SignalR.Hubs
{
    public class JitEvolutionHub : Hub
    {
        private readonly CurrentUser _currentUser;

        public JitEvolutionHub(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            if (!await SetupAndValidateSessionAsync())
            {
                return;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            if (!await SetupAndValidateSessionAsync())
            {
                return;
            }
        }

        private async Task<bool> SetupAndValidateSessionAsync()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Caller.SendAsync(SignalRConstants.Unauthenticated);
                return false;
            }

            _currentUser.Id = Guid.Parse(Context.UserIdentifier);

            return true;
        }
    }
}