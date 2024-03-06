using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace RPG.Units.Player
{
    public class PlayerInputComponent : UnitInputComponent
    {
        private PlayerControls _controls;
        private void Awake()
        {
            _controls = new PlayerControls();
        }
        private void OnEnable()
        {
            _controls.Unit.Enable();
            _controls.Unit.SwordAttack.performed += _ => OnAttack("Sword");
            _controls.Unit.ShieldAttack.performed += _ => OnAttack("Shield");
        }

        private void OnAttack(string weapon)
        {
            CallOnAttackEvent(weapon);
        }

        private void Update()
        {
            var direction = _controls.Unit.Move.ReadValue<Vector2>();
            _movement = new Vector3(direction.x, 0f, direction.y);
        }
        private void OnDisable()
        {
            _controls.Unit.SwordAttack.performed -= _ => OnAttack("Sword");
            _controls.Unit.ShieldAttack.performed -= _ => OnAttack("Shield");
            _controls.Unit.Disable();
        }
        private void OnDestroy()
        {
            _controls.Dispose();
        }
    }
}
