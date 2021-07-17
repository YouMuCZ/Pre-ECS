using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    // Tag used to quickly identify our Player Entity
    struct PlayerTag : IComponentData {}


    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float Speed = 5.0f;
        
        /// <summary>
        /// A function which converts our Player authoring GameObject to a more optimized Entity representation
        // Call methods on 'dstManager' to create runtime components on 'entity' here. Remember that:
        //
        // * You can add more than one component to the entity. It's also OK to not add any at all.
        //
        // * If you want to create more than one entity from the data in this class, use the 'conversionSystem'
        //   to do it, instead of adding entities through 'dstManager' directly.
        //
        // For example,
        //   dstManager.AddComponentData(entity, new Unity.Transforms.Scale { Value = scale });
        /// </summary>
        /// <param name="entity">A reference to the entity this GameObject will become</param>
        /// <param name="dstManager">The EntityManager is used to make changes to Entity data.</param>
        /// <param name="conversionSystem">Used for more advanced conversion features. Not used here.</param>
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // Here we add all of the components needed by the player
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(PlayerTag),
                typeof(InputData),
                typeof(MovementComponent)
            ));

            // Set the movement speed value from the authoring component
            dstManager.SetComponentData(entity, new MovementComponent {MetersPerSecond = Speed});
        }
    }
}

