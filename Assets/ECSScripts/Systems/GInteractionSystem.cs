using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace StarRagBrawl
{
    public class GInteractionSystem : JobComponentSystem
    {
        public EntityQuery starsGroup;


        [BurstCompile]
        struct ComputeGInteractionsJob : IJobForEach<StarEntity>
        {
            [ReadOnly] [DeallocateOnJobCompletion] public  NativeArray<StarEntity> stars;
            //[WriteOnly] public NativeArray<float3> accRegistry;


            public void Execute(ref StarEntity currentStar)
            {
                //int starRef=0;
                float3 accForce = float3(0,0,0);
                for (int i=0; i<stars.Length; i++)
                {
                    //the attraction direction
                    float3 distVect = stars[i].position.Value - currentStar.position.Value;
                    bool3 samePosition = currentStar.position.Value != stars[i].position.Value;
                    //If distance is (0,0,0), we have currentStar being stars[i], so we don't compute
                    if (samePosition.x||samePosition.y||samePosition.z)
                    {
                        float3 distNorm = normalize(distVect);
                        float attraction = (6.67f * currentStar.mass.Value * stars[i].mass.Value) / (pow(distVect.x, 2) + pow(distVect.y, 2) + pow(distVect.z, 2)) / 10000;
                        accForce += attraction * distNorm;
                    }
                   // else
                        //starRef = i;
                }
                //accRegistry[starRef] = accForce;
                currentStar.acceleration.Value = accForce;
            }
        }
        /*
        [BurstCompile]
        struct AssignAcc : IJobParallelFor
        {
            [DeallocateOnJobCompletion] public NativeArray<StarEntity> stars;
            [ReadOnly] public NativeArray<float3> accRegistry;

            public void Execute(int chunkIndex)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].acceleration.Value = accRegistry[i];
                }
            }
        }
        */

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            starsGroup = GetEntityQuery(typeof(StarEntity));
            NativeArray<StarEntity> starArray = starsGroup.ToComponentDataArray<StarEntity>(Allocator.TempJob);
            //NativeArray<float3> accRegistry = new NativeArray<float3>(starArray.Length, Allocator.Temp);

            var computeJob = new ComputeGInteractionsJob()
            {
                //accRegistry = accRegistry,
                stars = starArray
            };
            var computed = computeJob.Schedule(this, inputDependencies);
            /*
            var assigningJob = new AssignAcc()
            {
                stars = starArray,
                accRegistry = accRegistry
            };*/
            computed.Complete();
            //starArray.Dispose();
            return computed;
        }


    }
}