using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilites
{
    internal class Boundaries : MonoBehaviour
    {
        private Vector2 _screenBounds;
        private float _objectWidth;
        private float _objectHeight;
        private Camera _camera;
        Vector2 _viewPosition;

        // Start is called before the first frame update
        void Start()
        {
            _objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
            _objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
            _camera = Camera.main;
            _viewPosition = transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _screenBounds = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            _viewPosition.x = Mathf.Clamp(_viewPosition.x, (-_screenBounds.x / 2), _screenBounds.x);
            _viewPosition.y = Mathf.Clamp(_viewPosition.y, -_screenBounds.y + (_objectHeight * 2), _screenBounds.y - (_objectHeight * 2));
            transform.position = _viewPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_screenBounds.x / 6, _screenBounds.y));
        }
    }
}
