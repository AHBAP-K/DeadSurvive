using System;
using UnityEngine;

namespace DeadSurvive.Health
{
    public struct HealthComponent
    {
        public float Health { get; private set; }
        
        public event Action<float> HealthChanged;

        private float _maxHealth;

        public void Configure(float maxHealth)
        {
            _maxHealth = maxHealth;

            HealthChanged = delegate { };
            
            Health = maxHealth;
        }

        public void ChangeHealth(float number)
        {
            HealthChanged?.Invoke(number);
            Health = Mathf.Clamp(Health + number, 0f, _maxHealth);
        }
    }
}