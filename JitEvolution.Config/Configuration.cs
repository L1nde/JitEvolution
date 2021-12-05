namespace JitEvolution.Config
{
    public class Configuration
    {
        public virtual Jwt Jwt { get; set; }
    }

    public class Jwt
    {
        public virtual string Secret { get; set; }
        public virtual string Issuer { get; set; }
        public virtual string Audience { get; set; }
        public virtual int ExpireHours { get; set; }
    }
}