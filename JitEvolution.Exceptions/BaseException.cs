namespace JitEvolution.Exceptions
{
    public abstract class BaseException : Exception
    {
        public virtual int HttpStatusCode { get; set; }

        protected BaseException()
        {

        }

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}