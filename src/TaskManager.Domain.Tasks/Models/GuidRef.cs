using System;
using System.Threading;

namespace TaskManager.Domain.Tasks.Models
{
    public abstract class GuidRef
    {
        private int _locker;
        private Guid? _ref;

        [Obsolete("Явно нигде не вызывать! Нужен для сериализатора", true)]
        protected GuidRef()
        { }

        protected GuidRef(Guid @ref)
        {
            Value = @ref;
        }
        
        public Guid Value
        {
            get
            {
                return _ref.Value;
            }

            set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentException("Guid.Empty is not allowed as object reference");
                }

                if (_locker > 0)
                {
                    throw new InvalidOperationException("Value already set");
                }

                var result = Interlocked.Increment(ref _locker);
                if (result > 1)
                {
                    throw new InvalidOperationException("Value already set");
                }

                _ref = value;
            }
        }

        private bool Equals(GuidRef other)
        {
            // ReSharper disable PossibleInvalidOperationException
            return _ref.Value == other._ref.Value;
            // ReSharper restore PossibleInvalidOperationException
        }

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((GuidRef)obj);
        }

        public sealed override int GetHashCode()
        {
            // ReSharper disable once PossibleInvalidOperationException
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _ref.Value.GetHashCode();
        }

        public static bool operator ==(GuidRef left, GuidRef right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GuidRef left, GuidRef right)
        {
            return !Equals(left, right);
        }

        private static bool Equals(GuidRef left, GuidRef right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return true;
            }

            if (!ReferenceEquals(null, left) && !ReferenceEquals(null, right))
            {
                return left.GetType() == right.GetType() &&
                       left.Equals(right);
            }

            return false;
        }

        public override string ToString()
        {
            // ReSharper disable once PossibleInvalidOperationException
            return _ref.Value.ToString();
        }
    }
}