using Game.Components;
using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Core;
using Game.Systems;

public class PureBootstrapper : MonoBehaviour
{
    [SerializeField]
    [Range(1, 100000)]
    private int entitiesAmmount = 1000;

    [SerializeField]
    private Mesh mesh = default;
    [SerializeField]
    private Material material = default;

    [SerializeField]
    private float minY = -10;
    [SerializeField]
    private float maxY = 10;
    [SerializeField]
    private float minVelocity = 1;
    [SerializeField]
    private float maxVelocity = 3;

    private void Awake()
    {
        DefaultWorldInitialization.Initialize("World", Application.isEditor);

        var world = World.DefaultGameObjectInjectionWorld;
        var simulationGroup = world.GetOrCreateSystem<SimulationSystemGroup>();

        simulationGroup.AddSystemToUpdateList(world.GetOrCreateSystem<UpdateWorldTimeSystem>());
        simulationGroup.AddSystemToUpdateList(world.GetOrCreateSystem<Movement>());
        simulationGroup.AddSystemToUpdateList(world.GetOrCreateSystem<Decease>());
        simulationGroup.AddSystemToUpdateList(world.GetOrCreateSystem<Respawn>());

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        CreateAndSetPositionerSingleton(entityManager);
        CreateEntities(entityManager);
    }

    private void CreateAndSetPositionerSingleton(EntityManager entityManager)
    {
        var singleton = entityManager.CreateEntity();
        entityManager.AddComponent<Positioner>(singleton);

        var positionerSingletonData = new Positioner()
        {
            MinY = minY,
            MaxY = maxY
        };

        var respawnSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<Respawn>();
        respawnSystem.SetSingleton(positionerSingletonData);
    }

    private void CreateEntities(EntityManager entityManager)
    {
        var archetype = entityManager.CreateArchetype(
            typeof(Lifetime),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(RenderBounds),
            typeof(WorldRenderBounds),
            typeof(ChunkWorldRenderBounds),
            typeof(PerInstanceCullingTag));

        var renderMesh = new RenderMesh()
        {
            mesh = mesh,
            material = material
        };

        var mover = new Mover()
        {
            Direction = new Unity.Mathematics.float3(1, 0, 0)
        };

        for (int i = 0; i < entitiesAmmount; i++)
        {
            var e = entityManager.CreateEntity(archetype);

            entityManager.AddSharedComponentData(e, renderMesh);
            entityManager.AddComponentData(e, mover);

            var randomVelocity = new Velocity()
            {
                Value = Random.Range(minVelocity, maxVelocity)
            };

            entityManager.AddComponentData(e, randomVelocity);
        }
    }
}
