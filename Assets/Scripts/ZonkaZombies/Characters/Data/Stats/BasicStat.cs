using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ZonkaZombies.Characters.Data.Stats
{
    [SuppressMessage("ReSharper", "DelegateSubtraction")]
    public class BasicStat
    {
        private Action<int, int, Character> _onValueChanged;

        public readonly int Maximum;
        public int Current { get; private set; }
        private Character _character;

        public BasicStat(int maximum, Character character)
        {
            Maximum = maximum;
            Current = maximum;
            _character = character;
        }

        internal void Add(int value)
        {
            Current = Mathf.Min(Current + value, Maximum);
            DispatchOnChangedEvent(value);
        }
        internal void Remove(int value)
        {
            Current = Mathf.Max(Current - value, 0);
            DispatchOnChangedEvent(value);
        }

        public void AddListener(Action<int, int, Character> callback)
        {
            _onValueChanged += callback;
        }
        public void RemoveListener(Action<int, int, Character> callback)
        {
            _onValueChanged -= callback;
        }

        private void DispatchOnChangedEvent(int damage)
        {
            if (_onValueChanged != null)
            {
                _onValueChanged(Current, damage, _character);
            }
        }
    }
}