using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace GamePlay
{
    public class GravitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.WithAll<PlayerTag>().ForEach((ref Translation translation, ref GravityComponent gravity, in GroundCheckComponent ground) => 
            {
                if (ground.IsGrounded)
                {
                    gravity.FallTimeoutDelta = gravity.FallTimeout;

                    if (gravity.VerticalVelocity.y < 0.0f)
                    {
                        gravity.VerticalVelocity.y = 0.0f;
                    }
                }
                else
                {
                    if (gravity.FallTimeoutDelta >= 0.0f)
                    {
                        gravity.FallTimeoutDelta -= deltaTime;
                    }

                    // multiply by delta time twice to linearly speed up over time
                    gravity.VerticalVelocity += gravity.Gravity * deltaTime;
                }

            }).Run();
        }
    }
}

