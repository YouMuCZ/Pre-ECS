using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace GamePlay
{
    public class PhysicsUtils
    {
        /// <summary>
        /// 沿指定射线投射碰撞检测，并返回所有结果
        /// </summary>
        public unsafe static NativeList<ColliderCastHit> ColliderCastHitAll(PhysicsCollider collider, float3 start, float3 end, ref CollisionWorld collisionWorld,Entity ignore,Allocator allocator = Allocator.TempJob)
        {
            ColliderCastInput input = new ColliderCastInput()
            {
                Collider = collider.ColliderPtr,
                Start = start,
                End = end,
            };

            NativeList<ColliderCastHit> allHits = new NativeList<ColliderCastHit>(allocator);

            if (collisionWorld.CastCollider(input, ref allHits))
            {
                TrimByEntity(ref allHits, ignore);
            }

            return allHits;
        }

        public unsafe static NativeList<RaycastHit> CastRayAll(float3 start, float3 end, ref CollisionWorld collisionWorld, Entity ignore, CollisionFilter? filter = null, Allocator allocator = Allocator.TempJob)
        {
            RaycastInput input = new RaycastInput()
            {
                Start  = start,
                End    = end,
                Filter = (filter.HasValue ? filter.Value : PhysicsCollisionFilters.AllWithAll)
            };

            NativeList<RaycastHit> allHits = new NativeList<RaycastHit>(allocator);

            if (collisionWorld.CastRay(input, ref allHits))
            {
                TrimByEntity(ref allHits, ignore);
            }

            return allHits;
        }

        /// <summary>
        /// Performs a raycast along the specified ray.<para/>
        /// Will return true if there was a collision and populate the provided <see cref="RaycastHit"/>.
        /// </summary>
        /// <param name="nearestHit"> 最近的碰撞体 </param>
        /// <param name="start"> 射线起点 </param>
        /// <param name="end"> 射线终点 </param>
        /// <param name="ignore"> 不想检测的Entity </param>
        /// <param name="filter"> 射线过滤器 </param>
        /// <param name="allocator"> 指定NativeArray的分配类型 </param>
        /// <returns></returns>
        public unsafe static bool CastRay(out RaycastHit nearestHit, float3 start, float3 end, ref CollisionWorld collisionWorld, Entity ignore, CollisionFilter? filter = null, Allocator allocator = Allocator.Temp)
        {
            NativeList<RaycastHit> allHits = CastRayAll(start, end, ref collisionWorld, ignore, filter, allocator);

            bool gotHit = GetSmallestFractional(ref allHits, out nearestHit);
            allHits.Dispose();

            return gotHit;
        }

        /// <summary>
        /// The specified entity is removed from the provided list if it is present.
        /// </summary>
        /// <param name="castResults"> 所有检测到的Entity </param>
        /// <param name="ignore"> 不想检测的Entity </param>
        public static void TrimByEntity<T>(ref NativeList<T> castResults, Entity ignore) where T : struct, IQueryResult
        {
            if (ignore == Entity.Null) return;

            for (int i = (castResults.Length - 1); i >= 0; --i)
            {
                if (ignore == castResults[i].Entity)
                {
                    castResults.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Retrieves the smallest <see cref="IQueryResult.Fraction"/> result in the provided list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="castResults"></param>
        /// <param name="nearest"></param>
        public static bool GetSmallestFractional<T>(ref NativeList<T> castResults, out T nearest) where T : struct, IQueryResult
        {
            nearest = default(T);

            if (castResults.Length == 0) return false;

            float smallest = float.MaxValue;

            for (int i = 0; i < castResults.Length; ++i)
            {
                if (castResults[i].Fraction < smallest)
                {
                    nearest  = castResults[i];
                    smallest = castResults[i].Fraction;
                }
            }

            return true;
        }
    }
}

