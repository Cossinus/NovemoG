using System;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public GameObject target;

        public float camSpeed;

        private Vector3 _targetPos;

        private void Update()
        {
            var targetPosition = target.transform.position;
            var transformPosition = transform.position;
            _targetPos = new Vector3(targetPosition.x, targetPosition.y, transformPosition.z);
            transformPosition = Vector3.Lerp(transformPosition, _targetPos, camSpeed * Time.deltaTime);
            transform.position = transformPosition;
        }
    }
}
