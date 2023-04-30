using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    internal class CameraMovement : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private float _followSpeed = 2;
        [SerializeField] private float _yOffset = 1;
        private Transform _playerTransform;
        private readonly string PLAYER_PARAM = "Player";

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag(PLAYER_PARAM).transform;
        }

        internal void SetNewPlayer()
        {
            _playerTransform = GameObject.FindGameObjectWithTag(PLAYER_PARAM).transform;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 newPosition = new(_playerTransform.position.x, _playerTransform.position.y + _yOffset, -10);
            transform.position = Vector3.Slerp(transform.position, newPosition, _followSpeed * Time.deltaTime);
        }
    }
}
