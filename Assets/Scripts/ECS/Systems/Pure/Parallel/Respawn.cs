using Game.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Systems
{
    public class Respawn : SystemBase
    {
        private Random random;

        protected override void OnCreate()
        {
            random = new Random((uint)System.Environment.TickCount);
        }

        protected override void OnUpdate()
        {
            var positioningData = GetSingleton<Positioner>();
            var randomGenerator = random;

            Entities.ForEach((
                ref Translation translation,
                ref Lifetime lifetime
                ) =>
            {
                if (lifetime.Value <= 0)
                {
                    var yPosition = randomGenerator.NextFloat(positioningData.MinY, positioningData.MaxY);
                    translation.Value = new float3(0, yPosition, 0);
                    lifetime.Value = 4;
                }

            }).ScheduleParallel();
        }
    }
}