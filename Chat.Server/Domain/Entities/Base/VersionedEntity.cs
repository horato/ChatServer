namespace Chat.Server.Domain.Entities.Base
{
    public abstract class VersionedEntity : Entity
    {
        public virtual int Version { get; protected set; }
    }
}
