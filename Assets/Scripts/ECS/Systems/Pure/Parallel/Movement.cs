using Game.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Systems
{
    public class Movement : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = World.Time.DeltaTime;

            Entities.ForEach((
                ref Translation translation,
                in Mover mover,
                in Velocity velocity) =>
                {
                    translation.Value += mover.Direction * velocity.Value * deltaTime;
                }).ScheduleParallel();
        }
    }
} 