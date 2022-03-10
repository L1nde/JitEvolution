namespace JitEvolution.Config
{
    public class Configuration
    {
        public virtual Jwt Jwt { get; set; }
        public virtual GraphifyEvolution GraphifyEvolution { get; set; }
    }

    public class Jwt
    {
        public virtual string Secret { get; set; }
        public virtual string Issuer { get; set; }
        public virtual string Audience { get; set; }
        public virtual int ExpireHours { get; set; }
    }

    public class GraphifyEvolution
    {
        public virtual Neo4j Neo4J { get; set; }

        public virtual string ApiUrl { get; set; }

        public virtual string Port { get; set; }

        public virtual string DockerImageName { get; set; }

        public virtual string DockerVolumeName { get; set; }

        public virtual string SourcesDirectoryPath { get; set; }
    }

    public class Neo4j
    {
        public virtual string Uri { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }
}