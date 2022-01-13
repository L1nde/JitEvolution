namespace JitEvolution.BusinessObjects.Identity
{
    public class CurrentUser
    {
        public Guid Id { get; set; }

        public IDisposable Overrider(Guid id) =>
            new UserOverrider(this, id);

        public class UserOverrider : IDisposable
        {
            private CurrentUser _currentUser;
            private Guid originalUserId;

            public UserOverrider(CurrentUser currentUser, Guid userId)
            {
                _currentUser = currentUser;
                originalUserId = currentUser.Id;
                currentUser.Id = userId;
            }

            public void Dispose()
            {
                _currentUser.Id = originalUserId;
            }
        }
    }
}