using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

namespace GamePlay
{
    public class CameraController : MonoBehaviour
    {
        private Entity MEntity;
        private EntityManager MEntityManager;
        private Vector3 Offset;
        private Vector3 Position;
        private float3 TempPosition;
        private float Smoothing;


        // Start is called before the first frame update
        void Start()
        {
            MEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // 查询含有PlayerTag的Entity
            var QueryDesc = new EntityQueryDesc
            {
                None = new ComponentType[] {},
                All = new ComponentType[] { ComponentType.ReadOnly<PlayerTag>() }
            };

            EntityQuery player = MEntityManager.CreateEntityQuery(QueryDesc);
            if (player.CalculateChunkCount() > 0)
            {
                NativeArray<Entity> temp = new NativeArray<Entity>(1, Allocator.Temp);
                temp = player.ToEntityArray(Allocator.Temp);
                MEntity = temp[0];
                temp.Dispose();
            }
            player.Dispose();

            TempPosition = MEntityManager.GetComponentData<Translation>(MEntity).Value;
            Offset = transform.position - (Vector3)TempPosition;

            Smoothing = 5.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (MEntity != Entity.Null)
            {
                if (MEntityManager.HasComponent<PlayerTag>(MEntity))
                {
                    TempPosition = MEntityManager.GetComponentData<Translation>(MEntity).Value;
                }
            }

            transform.position = Vector3.Lerp(transform.position, (Vector3)TempPosition + Offset, Smoothing * Time.deltaTime);
        }
    }
}

