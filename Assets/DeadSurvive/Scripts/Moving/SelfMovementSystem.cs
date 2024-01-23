using DeadSurvive.Common;
using DeadSurvive.Condition;
using DeadSurvive.Level;
using DeadSurvive.Moving.Data;
using DeadSurvive.Unit;
using DeadSurvive.Unit.Enum;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace DeadSurvive.Moving
{
    public class SelfMovementSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<UnitComponent> _unitPool = default;
        private readonly EcsPoolInject<UnityObject<Transform>> _transformPool = default;
        private readonly EcsPoolInject<MoveDestinationComponent> _movePool = default;
        private readonly EcsPoolInject<SelfMoveComponent> _selfMovePool = default;
        private readonly EcsPoolInject<LandComponent> _landPool = default;

        private EcsFilterInject<Inc<UnitComponent, SelfMoveComponent>> _filterUnit;
        private EcsFilterInject<Inc<LandComponent>> _filterLand;

        public void Run(IEcsSystems systems)
        {
            foreach (var landEntity in _filterLand.Value)
            {
                ref var landComponent = ref _landPool.Value.Get(landEntity);
                MoreRandom(landComponent);
            }
        }

        private void MoreRandom(LandComponent landComponent)
        {
            foreach (var unitEntity in _filterUnit.Value)
            {
                ref var unitComponent = ref _unitPool.Value.Get(unitEntity);
                ref var selfMoveComponent = ref _selfMovePool.Value.Get(unitEntity);
                ref var transformComponent = ref _transformPool.Value.Get(unitEntity);

                if (unitComponent.UnitState != UnitState.Stay)
                {
                    continue;
                }

                selfMoveComponent.Delay -= Time.deltaTime;

                if (selfMoveComponent.Delay > 0)
                {
                    continue;
                }

                selfMoveComponent.RefreshDelay();

                var bounds = landComponent.LandView.AreaZone.bounds;
                var xPosition = Random.Range(bounds.min.x, bounds.max.x);
                var yPosition = Random.Range(bounds.min.y, bounds.max.y);
                var randomPoint = new Vector2(xPosition, yPosition);
                var direction =
                    Vector2.MoveTowards(transformComponent.Value.position, randomPoint, Random.Range(1f, 3f));

                var moveCondition = new MoveToPositionCondition(transformComponent.Value, direction, 0.1f);
                var vectorPosition = new VectorPositionHolder(direction);

                ref var moveComponent = ref _movePool.Value.GetOrAdd(unitEntity);
                moveComponent.Configure(vectorPosition, moveCondition);
            }
        }
    }
}