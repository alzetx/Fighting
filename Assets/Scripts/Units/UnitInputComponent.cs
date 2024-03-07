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
        public delegate void OnFocusHandler();
        public OnAttackHandler OnAttackEvent;
        public OnFocusHandler OnTargetEvent;
        protected void CallOnAttackEvent(string weapon) => OnAttackEvent?.Invoke(weapon);
        protected void CallOnTargetEvent() => OnTargetEvent?.Invoke();
    }
}
