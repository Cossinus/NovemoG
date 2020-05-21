using System;
using UnityEngine;

namespace Novemo.Interactable
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
