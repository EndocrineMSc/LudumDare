using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    internal class ParallaxBackground : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Vector2 _parallaxEffectMultiplier;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;

        #endregion

        #region Functions

        // Start is called before the first frame update
        void Start()
        {
            _cameraTransform = Camera.main.transform;
            _lastCameraPosition = _cameraTransform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
            _lastCameraPosition = _cameraTransform.position;
        }

        #endregion
    }
}
