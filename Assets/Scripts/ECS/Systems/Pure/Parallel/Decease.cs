using Game.Components;
using Unity.Entities;

namespace Game.Systems
{
    public class Decease : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = World.Time.DeltaTime;

            Entities.ForEach((
                ref Lifetime lifetime,
                in Velocity velocity) =>
                {
                    lifetime.Value -= deltaTime * velocity.Value;
                }).ScheduleParallel();
        }
    }
}