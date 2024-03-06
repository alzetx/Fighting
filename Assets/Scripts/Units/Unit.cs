using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Units
{
    [RequireComponent(typeof(Animator))]
    public class Unit : MonoBehaviour
    {
        private bool _inAnimationAttack;
        private Animator _animator;
        private UnitInputComponent _inputs;
        private UnitStatsComponent _stats;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _stats = GetComponent<UnitStatsComponent>();
            _inputs = GetComponent<UnitInputComponent>();
            if (_inputs == null)
            {
                return;
            }
            _inputs.OnAttackEvent += OnAttack;
        }

        private void OnAttack(string weapon)
        {
            if (_inAnimationAttack) return;
            _animator.SetTrigger(weapon + "Attack");
            _inAnimationAttack = true;
        }

        private void Update()
        {
            OnMove();
        }
        private void OnMove()
        {
            if (_inAnimationAttack) return;
            ref var movement = ref _inputs.MoveDirection;
            _animator.SetFloat("ForwardMove", movement.z);
            _animator.SetFloat("SideMove", movement.x);
            if (movement.z == 0f && movement.x == 0f)
            {
                _animator.SetBool("Moving", false);
            }
            else
            {
                _animator.SetBool("Moving", true);
                transform.position += movement * Time.deltaTime * _stats.MoveSpeed;
            }
        }
        private void OnAnimationEnd_UnityEvent(AnimationEvent data)
        {
            _inAnimationAttack = false;
        }
    }
}
