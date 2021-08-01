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
            var cameraEulerAnglesY = Camera.main.transform.eulerAngles.y;
            
            Entities.ForEach((ref Translation translation, ref Rotation rotation, ref CinemachineComponent cinemachine, in InputData input, in MovementComponentData movement, in GravityComponentData gravity) => {
                // normalise input direction
                var inputDirection = input.Move.normalized;
                var speed = input.Sprint ? movement.SprintSpeed : movement.MoveSpeed;

                // 转向
                if (math.length(input.Move) > 0.1f)
                {
                    // rotate to face input direction relative to camera position
                    targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraEulerAnglesY;
                    rotation.Value = quaternion.Euler(0.0f, math.radians(targetRotation), 0.0f);
                }

                // 移动
                if (math.length(input.Move) < 0.1f) speed = 0.0f;
                Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
                translation.Value += math.normalize(targetDirection) * speed * deltaTime + gravity.VerticalVelocity * deltaTime;
            }).Run();
        }
    }
}

