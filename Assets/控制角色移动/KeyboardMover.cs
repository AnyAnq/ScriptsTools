// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables
{
    public class KeyboardMover : MonoBehaviour
    {
        public float speed = 10;
        public MoveAxis Vertical = new MoveAxis(KeyCode.W, KeyCode.S);
        public MoveAxis Horizontal = new MoveAxis(KeyCode.D, KeyCode.A);

        private void Update()
        {
            var moveNormal = new Vector3(Horizontal, Vertical, 0.0f).normalized;

            transform.position += moveNormal * Time.deltaTime * speed;
        }

        [Serializable]
        public class MoveAxis
        {
            public KeyCode Negative;
            public KeyCode Positive;

            public MoveAxis(KeyCode positive, KeyCode negative)
            {
                Positive = positive;
                Negative = negative;
            }

            public static implicit operator float(MoveAxis axis)
            {
                return (Input.GetKey(axis.Positive)? 1.0f: 0.0f) -
                       (Input.GetKey(axis.Negative)? 1.0f: 0.0f);
            }
        }
    }
}