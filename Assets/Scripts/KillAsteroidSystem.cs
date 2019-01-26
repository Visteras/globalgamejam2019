using Leopotam.Ecs;
using Leopotam.Ecs.Reactive;

namespace Client
{
    [EcsInject]
    internal class KillAsteroidSystem : EcsReactiveSystem<KillAsteroids>
    {
        private EcsWorld _world;
        private GameState _gameState;
        protected override EcsReactiveType GetReactiveType ()
        {
            return EcsReactiveType.OnAdded;
        }

        protected override void RunReactive ()
        {
            for (int i = 0; i < ReactedEntitiesCount; i++)
            {
                var entity = ReactedEntities[i];
                var kill = _world.GetComponent<KillAsteroids> (entity);
                foreach (var id in kill.value)
                {
                    if (_gameState.Asteroids.TryGetValue (id, out var asteroidEntity))
                    {
                        _world.EnsureComponent<Destroy> (asteroidEntity, out _);
                        _gameState.Asteroids.Remove (id);
                    }
                }
            }
        }
    }
}
