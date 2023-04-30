using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Utilites;

namespace Characters
{
    internal class CarrierBird : MonoBehaviour
    {
        #region Fields and Properties

        private DeliveryBag _deliveryBag;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _flySpeed = 5f;
        private bool _bagIsRetrievedByPlayer;
        private bool _bagIsRetrievedByBird;
        private float _feetDockingY;

        #endregion

        #region Functions

        private void Awake()
        {
            _deliveryBag = GameObject.FindFirstObjectByType<DeliveryBag>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _feetDockingY = _spriteRenderer.bounds.size.y / 2;
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelEvents.Instance.BagIsRetrieved?.AddListener(OnBagRetrieved);
        }

        private void OnDisable()
        {
            LevelEvents.Instance.BagIsRetrieved?.RemoveListener(OnBagRetrieved);
        }

        private void Update()
        {
            if (!_bagIsRetrievedByPlayer && !_bagIsRetrievedByBird)
            {
                MoveToBag();
            }

            if (_bagIsRetrievedByBird)
            {
                _bagIsRetrievedByBird = false;
                transform.DOMoveY(200, 20f);
            }
        }

        private void MoveToBag()
        {
            Vector2 direction = transform.position - _deliveryBag.transform.position;
            _rigidbody.velocity = -direction.normalized * _flySpeed;
        }

        private void OnBagRetrieved()
        {
            _bagIsRetrievedByPlayer = true;
            transform.DOKill();
            _rigidbody.velocity = Vector3.zero;
            transform.DOMoveY(1000, 3f);
            StartCoroutine(DestroyWithDelay());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<DeliveryBag>(out DeliveryBag bag))
            {
                bag.transform.SetParent(transform, true);
                bag.GetComponent<Collider2D>().enabled = false;
                bag.transform.localPosition = new Vector3(0, -1, 0);
                _bagIsRetrievedByBird = true;
            }
        }

        private IEnumerator DestroyWithDelay()
        {
            yield return new WaitForSeconds(4);
            Destroy(gameObject);
        }

        #endregion
    }
}
