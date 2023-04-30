using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class EnemyTurnFeeler : MonoBehaviour
    {
        #region Fields and Properties

        private Enemy _enemy;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _enemy = transform.parent.GetComponent<Enemy>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _enemy.Turn();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _enemy.Turn();
            }
        }
    }
}
