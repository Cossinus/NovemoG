using UnityEngine;

namespace Novemo.Player
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 3f;
        public Transform interactionTransform;

        public virtual void Interact()
        {
            // This is meant to be overwritten
        }

        private void OnDrawGizmosSelected()
        {
            if (interactionTransform == null) {
                interactionTransform = transform;
            }
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }
    }
}
