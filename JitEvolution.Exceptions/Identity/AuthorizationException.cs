namespace JitEvolution.Exceptions.Identity
{
    public class AuthorizationException : BaseException
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) : base(message)
        {
        }

        public AuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override int HttpStatusCode => 403;
    }
}
