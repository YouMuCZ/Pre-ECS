using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// 根据玩家输入转动相机视角
    /// </summary>
    [UpdateBefore(typeof(MovementSystem)), UpdateAfter(typeof(MInputSystem))]
    public class CameraRotationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var look = new Vector2(0.0f, 0.0f);
            var detalTime = Time.DeltaTime;

            Entities.WithAll<PlayerTag>().ForEach((in InputData inputData) => {
                look = inputData.Look;
            }).Run();

            Entities.WithoutBurst().WithAll<PlayerCameraRootTag>().ForEach((Transform transform, ref CinemachineComponent cinemachineData) => {
                // if there is an input and camera position is not fixed
                if (look.sqrMagnitude >= cinemachineData.Threshold && !cinemachineData.LockCameraPosition)
                {
                    cinemachineData.CinemachineTargetYaw += look.x * detalTime;
                    cinemachineData.CinemachineTargetPitch += look.y * detalTime;
                }

                // clamp our rotations so our values are limited 360 degrees
                cinemachineData.CinemachineTargetYaw = ClampAngle(cinemachineData.CinemachineTargetYaw, float.MinValue, float.MaxValue);
                cinemachineData.CinemachineTargetPitch = ClampAngle(cinemachineData.CinemachineTargetPitch, cinemachineData.BottomClamp, cinemachineData.TopClamp);

                // Cinemachine will follow this target
                transform.rotation = quaternion.Euler(math.radians(cinemachineData.CinemachineTargetPitch + cinemachineData.CameraAngleOverride), math.radians(cinemachineData.CinemachineTargetYaw) , 0.0f);
            }).Run();
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
            return math.clamp(lfAngle, lfMin, lfMax);
		}
    }
}

