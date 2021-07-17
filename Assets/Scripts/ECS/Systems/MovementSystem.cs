using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace GamePlay
{
    // 接收按键输入，移动角色系统
    [UpdateAfter(typeof(MInputSystem))]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.
            WithName("MoveCharacter").
            ForEach((ref Translation translation, ref Rotation rotation, in InputData input, in MovementComponent movement) => {
                var move = new float3(input.Move.x, 0.0f, input.Move.y);
                var dir = move * movement.MetersPerSecond * deltaTime;

                // 转向
                // if (math.length(input.Move) > 0.1f)
                // {
                //     rotation.Value = quaternion.LookRotation(math.normalize(dir), math.up());
                // }

                // 移动
                translation.Value += dir;
            }).ScheduleParallel();
        }
    }
}

