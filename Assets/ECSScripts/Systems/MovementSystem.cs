using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using UnityEngine;

namespace StarRagBrawl
{

    public class MovementSystem : JobComponentSystem
    {

        [BurstCompile]
        struct MovementSystemJob : IJobForEach<StarEntity, Translation>
        {
            public float deltaTime;


            public void Execute(ref StarEntity starEntity, ref Translation translation)
            {
                starEntity.speed.Value += starEntity.acceleration.Value;
                starEntity.position.Value += starEntity.speed.Value;
                translation.Value = starEntity.position.Value;

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var moveJob = new MovementSystemJob();

            moveJob.deltaTime = Time.deltaTime;

            // Now that the job is set up, schedule it to be run. 
            return moveJob.Schedule(this, inputDependencies);
        }
    }
}