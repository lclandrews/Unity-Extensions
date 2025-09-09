using System;

namespace UnityEngine.Extension
{
    public interface IReadOnlyScalarFlag
    {
        public bool Value { get; }
        public event ScalarFlagUpdate OnUpdate;
    }
    
    public delegate void ScalarFlagUpdate(bool value);
    
    public interface IScalarFlag : IReadOnlyScalarFlag
    {
        public new bool Value { get; set; }
        public void Reset(bool defaultValue = false);
    }

    public sealed class ScalarFlag : IScalarFlag
    {
        [Flags]
        public enum ValueBehaviour
        {
            Default = 0,
            RestrictFalse = 1,
            RestrictTrue = 2,
            Binary = RestrictFalse | RestrictTrue
        }

        private const int _TrueValue = 1;
        private const int _FalseValue = 0;

        private int _requestedValue;
        private int _overrideValue;

        public event ScalarFlagUpdate OnUpdate
        {
            add => _onUpdate += value;
            remove => _onUpdate -= value;
        }
        private ScalarFlagUpdate _onUpdate;
        private readonly ValueBehaviour _behaviour = ValueBehaviour.Default;

        public bool Value
        {
            get => _requestedValue > _FalseValue &&  _overrideValue > _FalseValue;
            set
            {
                bool previousValue = Value;
                if (value)
                {
                    if (_behaviour.HasFlag(ValueBehaviour.RestrictTrue))
                    {
                        _requestedValue = Mathf.Min(_requestedValue + 1, _TrueValue);
                    }
                    else
                    {
                        _requestedValue++;
                    }
                }
                else
                {
                    if (_behaviour.HasFlag(ValueBehaviour.RestrictFalse))
                    {
                        _requestedValue = Mathf.Max(_requestedValue - 1, _FalseValue);
                    }
                    else
                    {
                        _requestedValue--;
                    }
                }

                bool newValue = Value;
                if (previousValue != newValue)
                {
                    _onUpdate?.Invoke(newValue);
                }
            }
        }
        
        private ScalarFlag() { }

        public ScalarFlag(bool value, ScalarFlagUpdate onUpdate = null)
        {
            _behaviour = ValueBehaviour.Default;
            _requestedValue = value ? _TrueValue : _FalseValue;
            _overrideValue = _TrueValue;
            _onUpdate = onUpdate;
        }

        public ScalarFlag(bool value, ValueBehaviour behaviour, ScalarFlagUpdate onUpdate = null)
        {
            _behaviour = behaviour;
            _requestedValue = value ? _TrueValue : _FalseValue;
            _overrideValue = _TrueValue;
            _onUpdate = onUpdate;
        }

        public void SetOverrideValue(bool value)
        {
            bool previousValue = Value;
            if (value)
            {
                if (_behaviour.HasFlag(ValueBehaviour.RestrictTrue))
                {
                    _overrideValue = Mathf.Min(_overrideValue + 1, _TrueValue);
                }
                else
                {
                    _overrideValue++;
                }
            }
            else
            {
                if (_behaviour.HasFlag(ValueBehaviour.RestrictFalse))
                {
                    _overrideValue = Mathf.Max(_overrideValue - 1, _FalseValue);
                }
                else
                {
                    _overrideValue--;
                }
            }

            bool newValue = Value;
            if (previousValue != newValue)
            {
                _onUpdate?.Invoke(newValue);
            }
        }
        
        public void Reset(bool defaultValue = false)
        {
            bool previousValue = Value;
            _requestedValue = defaultValue ? _TrueValue : _FalseValue;
            if (previousValue != Value)
            {
                _onUpdate?.Invoke(previousValue);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}