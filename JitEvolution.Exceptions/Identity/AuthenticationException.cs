namespace JitEvolution.Exceptions.Identity
{
    public class AuthenticationException : BaseException
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override int HttpStatusCode => 401;
    }
}
