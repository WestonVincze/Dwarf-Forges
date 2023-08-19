using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
        public struct InputStates
        {
            public bool W, S, A, D, Q, E;
            public bool QDown, EDown;
        }

        virtual public InputStates GetInputStates()
        {
            InputStates states;

            states.W = Input.GetKey(KeyCode.W);
            states.S = Input.GetKey(KeyCode.S);
            states.A = Input.GetKey(KeyCode.A);
            states.D = Input.GetKey(KeyCode.D);
            states.Q = Input.GetKey(KeyCode.Q);
            states.E = Input.GetKey(KeyCode.E);
            states.QDown = Input.GetKeyDown(KeyCode.Q);
            states.EDown = Input.GetKeyDown(KeyCode.E);

            return states;
        }
}

