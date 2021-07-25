using Unity.Physics;


namespace GamePlay
{
    /// <summary>
    /// 物理碰撞检测层标签,通过进制法判断
    /// </summary>
    public enum PhysicsCollisionFilterLayers
    {
        Nothing = 0,
        Everything = 0xFFFF,
        Terrain = 1 << 0,
        Wall = 1 << 1,
        Character = 1 << 2,
    }

    /// <summary>
    /// 射线过滤器
    /// </summary>
    /// <param name="BelongsTo">射线身上的碰撞器属于哪个标签</param>
    /// <param name="CollidesWith">射线可以和哪些碰撞器发生碰撞检测</param>
    public static class PhysicsCollisionFilters
    {
        /// <summary>
        /// 不检测角色
        /// </summary>
        /// <returns></returns>
        public static readonly CollisionFilter GroundCheckFilter = new CollisionFilter()
        {
            BelongsTo = (int)PhysicsCollisionFilterLayers.Character,
            CollidesWith = ~(uint)(PhysicsCollisionFilterLayers.Character),
        };
    }

}
