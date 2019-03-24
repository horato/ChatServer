using System;

namespace Chat.Server.Domain.Entities.Base
{
    public abstract class Entity
    {
        private int? _hashCode;

        public virtual Guid Id { get; protected set; }
        public virtual bool IsTransient => Id == Guid.Empty;

        #region Equals shenanigans
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsTransient || other.IsTransient || !Equals(Id, other.Id))
                return false;

            var otherType = other.GetUnproxiedType();
            var thisType = GetUnproxiedType();
            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        public override int GetHashCode()
        {
            if (_hashCode.HasValue)
                return _hashCode.Value;

            if (IsTransient)
                return base.GetHashCode();
            else
                _hashCode = Id.GetHashCode();

            return _hashCode.Value;
        }

        public static bool operator ==(Entity x, Entity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }

        #endregion
    }
}
