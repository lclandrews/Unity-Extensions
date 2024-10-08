using System;

namespace UnityEngine.Extension
{
    [Serializable]
    public class SubclassOf<T> : ISerializationCallbackReceiver, IEquatable<SubclassOf<T>>, IEquatable<Type> where T : class
    {
        public Type ClassType
        {
            get
            {
                if (_classType == null)
                {
                    _classType = Type.GetType(_classTypeName, true);
                }
                return _classType;
            }
        }
        private Type _classType;

        public Type SubclassType
        {
            get
            {
                if (_subclassType == null || _subclassType.AssemblyQualifiedName != _subclassTypeName)
                {
                    if (!string.IsNullOrEmpty(_subclassTypeName))
                    {
                        try
                        {
                            _subclassType = Type.GetType(_subclassTypeName, true);
                        }
                        catch (TypeLoadException exception)
                        {
                            Debug.LogException(exception);
                            _subclassTypeName = null;
                        }
                    }
                    else if (_subclassType != null)
                    {
                        _subclassType = null;
                    }
                }
                return _subclassType;
            }

            set
            {
                if (value != null)
                {
                    if (!value.IsSubclassOf(ClassType))
                    {
                        throw new ArgumentException(string.Format("value is not subclass of {0}", ClassType));
                    }
                    _subclassTypeName = value.AssemblyQualifiedName;
                }
                else
                {
                    _subclassTypeName = null;
                }
                _subclassType = value;
            }
        }
        private Type _subclassType;

        // AssemblyQualifiedName
        public string ClassTypeName { get { return _classTypeName; } }
        [SerializeField, HideInInspector] private string _classTypeName = null;

        // AssemblyQualifiedName
        public string SubclassTypeName { get { return _subclassTypeName; } }
        [SerializeField, HideInInspector] private string _subclassTypeName = null;

        public SubclassOf()
        {
            _classTypeName = typeof(T).AssemblyQualifiedName;
        }

        public SubclassOf(Type subclassType)
        {
            _classTypeName = typeof(T).AssemblyQualifiedName;
            this.SubclassType = subclassType;
        }

        public static implicit operator SubclassOf<T>(Type subclassType)
        {
            return new SubclassOf<T>(subclassType);
        }

        public static implicit operator Type(SubclassOf<T> subclassOf)
        {
            return subclassOf.SubclassType;
        }

        public static bool operator ==(SubclassOf<T> lhs, SubclassOf<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SubclassOf<T> lhs, SubclassOf<T> rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(SubclassOf<T> lhs, Type rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SubclassOf<T> lhs, Type rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(Type lhs, SubclassOf<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Type lhs, SubclassOf<T> rhs)
        {
            return !(lhs == rhs);
        }

        public T CreateInstance(params object[] args)
        {
            if (SubclassType == null)
            {
                throw new InvalidOperationException("No assigned subclass type");
            }
            return (T)Activator.CreateInstance(SubclassType, args);
        }

        private void Validate()
        {
            _classTypeName = typeof(T).AssemblyQualifiedName;
            Type tempSubclassType = SubclassType;
            if (tempSubclassType != null)
            {
                if (!tempSubclassType.IsSubclassOf(ClassType))
                {
                    SubclassType = null;
                    return;
                }
            }
            SubclassType = tempSubclassType;
        }

        public bool Equals(Type other)
        {
            return SubclassType.Equals(other);
        }

        public bool Equals(SubclassOf<T> other)
        {
            return SubclassType.Equals(other.SubclassType);
        }

        public override bool Equals(object obj)
        {
            if (obj is SubclassOf<T> otherSubclassOf)
            {
                return Equals(otherSubclassOf);
            }
            else if (obj is Type otherType)
            {
                return Equals(otherType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_classTypeName, _subclassTypeName);
        }

        public void OnBeforeSerialize()
        {
            Validate();
        }

        public void OnAfterDeserialize()
        {
            Validate();
        }
    }
}