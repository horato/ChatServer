using System;

namespace Chat.Server.Domain.Entities
{
    public class AuditInfo
    {
        public virtual DateTime On { get; protected set; }
        public virtual string By { get; protected set; }

        protected AuditInfo()
        {
        }

        public AuditInfo(DateTime on, string by)
        {
            On = on;
            By = by;
        }

        protected bool Equals(AuditInfo other)
        {
            return On.Equals(other.On) && string.Equals(By, other.By);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(AuditInfo))
                return false;

            return Equals((AuditInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (On.GetHashCode() * 397) ^ By.GetHashCode();
            }
        }

        public static bool operator ==(AuditInfo left, AuditInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AuditInfo left, AuditInfo right)
        {
            return !Equals(left, right);
        }
    }
}
