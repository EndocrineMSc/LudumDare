using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilites;

namespace Utility
{
    internal class Arrow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private bool _bagIsLost;
        private SpriteRenderer _arrow;
        [SerializeField] private float _hideDistance;
        private GameObject _player;


        private void Start()
        {
            LevelEvents.Instance.BagIsLost.AddListener(OnBagIsLost);
            LevelEvents.Instance.BagIsRetrieved.AddListener(OnBagIsRetrieved);
            _arrow = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            _arrow.enabled = false;
            _player = transform.parent.gameObject;
        }

        private void OnDisable()
        {
            LevelEvents.Instance.BagIsLost?.RemoveListener(OnBagIsLost);
            LevelEvents.Instance.BagIsRetrieved?.RemoveListener(OnBagIsRetrieved);
        }

        void LateUpdate()
        {
            if (_target != null && _bagIsLost)
            {
                var relativePosition = _target.position - transform.position;

                if (relativePosition.magnitude < _hideDistance)
                {
                    _arrow.enabled = false;
                }
                else
                {
                    _arrow.enabled = true;
                }

                var angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
                angle = _player.transform.localScale.x > 0 ? angle : angle -180;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        private void OnBagIsLost()
        {
            _bagIsLost = true;
            _target = GameObject.FindGameObjectWithTag("DeliveryBag").transform;
            _arrow.enabled = true;
        }

        private void OnBagIsRetrieved()
        {
            _bagIsLost = false;
            _arrow.enabled = false;
        }
    }
}
