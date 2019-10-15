using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

namespace StarRagBrawl {
    public class GameManager : MonoBehaviour
    {

        public int starCount = 10;
        public float mass;
        public UnityEngine.Mesh _mesh;
        public Material _material;
        EntityManager manager;
        private Entity starEntity;
        private EntityArchetype starArchetype;
        
        void Start()
        {
            manager = World.Active.EntityManager;
            //_material = Resources.Load<Material>("Materials/YellowStar");
            starArchetype = manager.CreateArchetype(
                typeof(StarEntity), 
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld)
            );
            AddStars(starCount);
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                AddStars(starCount);
            }
        }

        // Method which creates Star Entities and adds them to ZA WARUDO!
        void AddStars(int starAmount)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(starAmount, Allocator.TempJob);
            starEntity = manager.CreateEntity(starArchetype);

            manager.Instantiate(starEntity, entities);

            for (int i=0; i<starAmount; i++)
            {
                float3 pos = new float3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f));
                float3 spd = new float3(0,0,0);
                
                StarEntity sE = new StarEntity
                {
                    position = new Position { Value = pos },
                    acceleration = new Acceleration { Value = new float3(0, 0, 0) },
                    speed = new Speed { Value = spd },
                    //mass = new Mass { Value = UnityEngine.Random.Range(1000, 3000) }
                    mass = new Mass { Value = mass }
                };
                
                manager.SetComponentData(entities[i], sE);
                manager.SetComponentData(entities[i], new Translation { Value = pos });
                manager.SetSharedComponentData(entities[i], new RenderMesh { mesh = _mesh, material = _material });


            }

            entities.Dispose();
        }
    }
}
