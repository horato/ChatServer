using Chat.Server.Domain.Entities;
using Chat.Server.Domain.Entities.Base;

namespace Chat.Server.Domain.Maps.Base
{
    public abstract class VersionedEntityMap<T> : EntityMap<T> where T : VersionedEntity
    {
        protected VersionedEntityMap()
        {
            Version(x => x.Version);
            OptimisticLock.Version();
        }
    }
}
