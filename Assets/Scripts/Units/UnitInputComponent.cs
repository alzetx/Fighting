using RPG.Units.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Units
{
    public class UnitInputComponent : MonoBehaviour
    {
        protected Vector3 _movement;
        public ref Vector3 MoveDirection => ref _movement;
        public delegate void OnAttackHandler(string weapon);
        public event OnAttackHandler OnAttackEvent;
        protected void CallOnAttackEvent(string weapon) => OnAttackEvent?.Invoke(weapon);
    }
}
