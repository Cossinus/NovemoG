using UnityEngine;

namespace Novemo.Interactables
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 3f;
        public Transform interactionTransform;

        private void OnValidate()
        {
            GetComponent<CircleCollider2D>().radius = radius;
        }

        public virtual void Interact() { }
        
        public virtual void Disable() { }

        private void OnDrawGizmosSelected()
        {
            if (interactionTransform == null) 
            {
                interactionTransform = transform;
            }
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }
    }
}
