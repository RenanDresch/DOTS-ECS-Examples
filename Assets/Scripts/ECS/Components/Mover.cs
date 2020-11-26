using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct Mover : IComponentData
    {
        public float3 Direction;
    }
}