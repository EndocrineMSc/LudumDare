using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class Player : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        private readonly string HURT_TRIGGER = "Hurt";
        private readonly string WALK_TRIGGER = "Walk";
        private readonly string IDLE_TRIGGER = "Idle";
        private readonly string ATTACK_TRIGGER = "Attack";
        private readonly string DEATH_TRIGGER = "Death";
        private readonly string JUMP_Trigger = "Jump";


        private float _hurtAnimationDuration;
        private Animator _playerAnimator;


        #endregion

        #region Functions

        private void Awake()
        {
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerAnimator.SetTrigger(JUMP_Trigger);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                _playerAnimator.SetTrigger(WALK_TRIGGER);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _playerAnimator.SetTrigger(ATTACK_TRIGGER);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _playerAnimator.SetTrigger(HURT_TRIGGER);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _playerAnimator.SetTrigger(DEATH_TRIGGER);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                _playerAnimator.SetTrigger(IDLE_TRIGGER);
            }
        }




        public void TakeDamage()
        {
            _playerAnimator.SetTrigger(HURT_TRIGGER);
            
        }

        private void HandleDeath()
        {

        }

        #endregion
    }
}
