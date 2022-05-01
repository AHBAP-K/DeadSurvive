using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DeadSurvive.Health;
using DeadSurvive.Moving;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;

namespace DeadSurvive.Attack
{
    //TODO: Refactor
    public struct AttackComponent
    {
        public float AttackDamage { get; private set; }
        
        private CancellationTokenSource _cancellationToken;

        
        public void Configure(float attackDamage)
        {
            AttackDamage = attackDamage;
        }

        public void AttackUnit(EcsWorld ecsWorld, ref UnitComponent currentUnit, ref UnitComponent targetUnit)
        {
            CancelAttack();
            _cancellationToken = new CancellationTokenSource();
            
            var positionPool = ecsWorld.GetPool<TargetPositionComponent>();
            var healthPool = ecsWorld.GetPool<HealthComponent>();

            if (positionPool.Has(currentUnit.UnitEntity))
            {
                positionPool.Del(currentUnit.UnitEntity);
            }
            
            var healthComponent = healthPool.Get(targetUnit.UnitEntity);
            ref var targetPositionComponent = ref positionPool.Add(currentUnit.UnitEntity);
            
            var transformTarget = new TransformPositionHolder(targetUnit.UnitTransform);

            var attack = new Attack();
            
            targetPositionComponent.Configure(transformTarget);
            CancellationTokenSource cancellationToken = _cancellationToken;
            AttackComponent tmpThis = this;
            currentUnit.UnitState = UnitState.Attack;

            targetPositionComponent.ReachedTarget += () => { attack.AttackUnit(tmpThis.AttackDamage, healthComponent, cancellationToken.Token); };

        }

        public void CancelAttack()
        {
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
        }
    }

    public class Attack
    {
        public async void AttackUnit(float attackDamage, HealthComponent healthComponent, CancellationToken token)
        {
            while (healthComponent.Health > 0f)
            {
                healthComponent.ChangeHealth(-attackDamage);
                
                await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: token).SuppressCancellationThrow();
                
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}