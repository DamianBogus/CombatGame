using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private Mesh PlayerMesh;
    [SerializeField] private Material PlayerMaterial;

    private void Start() 
    {
        EntityManager entityManager = World.Active.EntityManager;
        EntityArchetype playerArchetype =entityManager.CreateArchetype(
            
            typeof(MoveSpeed),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        Entity playerEntity = entityManager.CreateEntity(playerArchetype);
        entityManager.SetSharedComponentData(playerEntity,
        new RenderMesh
        {
            mesh = PlayerMesh,
            material = PlayerMaterial
        }
        );
        entityManager.SetComponentData(playerEntity, new MoveSpeed { xSpeed = 0.5f }
        );
        
    }
}
