using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;

namespace GamePlay
{
    // 接收按键输入，移动角色系统
    [UpdateAfter(typeof(MInputSystem)), UpdateAfter(typeof(GroundCheckSystem))]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var targetRotation = new float();
            var _rotation = new float();
            var _rotationVelocity = new float();
            var RotationSmoothTime = 0.12f;
            var cameraEulerAnglesY = Camera.main.transform.eulerAngles.y;
            
            Entities.WithName("MoveCharacter").
            ForEach((ref Translation translation, ref Rotation rotation, ref CinemachineComponent cinemachine, in InputData input, in MovementComponent movement, in GravityComponent gravity) => {
                var move = new float3(input.Move.x, 0.0f, input.Move.y) * movement.MoveSpeed * deltaTime;
                var down = gravity.VerticalVelocity * deltaTime;

                // 转向
                // if (math.length(input.Move) > 0.1f)
                // {
                //     rotation.Value = quaternion.LookRotation(math.normalize(move), math.up());
                // }
                // normalise input direction
                var inputDirection = math.normalize(move);

                if (math.length(input.Move) > 0.1f)
                {
                    targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraEulerAnglesY;

                    // var eulerAngle = quaternion.EulerXYZ(new float3(rotation.Value.value.x, rotation.Value.value.y, rotation.Value.value.z)).value.y;
                    // _rotation = Mathf.SmoothDampAngle(eulerAngle, targetRotation, ref _rotationVelocity, RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    rotation.Value = quaternion.Euler(0.0f, math.radians(targetRotation), 0.0f);

                    // var targetDirection = quaternion.Euler(0.0f, math.radians(targetRotation), 0.0f);
                    Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

                    // 移动
                    translation.Value += math.normalize(targetDirection) * movement.MoveSpeed * deltaTime  + down;
                }

                // 重力
                translation.Value += down;
                
            }).Run();
        }
    }
}

