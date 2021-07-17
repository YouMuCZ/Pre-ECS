using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GamePlay
{
    

    public class CinemachineSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var TargetPosition = new float3(0.0f, 0.0f, 0.0f);
            
            // 1. 获取玩家实时位置
            Entities.WithAll<PlayerTag>().ForEach((ref Translation translation) => {
                TargetPosition = translation.Value;
            }).Run();

            // 2. 设置Tip的Entity位置
            Entities.WithAll<CinemachineTag>().ForEach((ref Translation translation) => {
                translation.Value = TargetPosition;
            }).Run();

            // 3. 设置Tip的Gameobject位置
            Entities.WithoutBurst().WithAll<CinemachineTag>().ForEach((Transform transform) => {
                transform.position = TargetPosition;
            }).Run();
        }
    }
}

