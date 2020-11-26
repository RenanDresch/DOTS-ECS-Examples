using Unity.Entities;

namespace Game.Components
{
    public struct Positioner : IComponentData
    {
        public float MaxY;
        public float MinY;
    }
}