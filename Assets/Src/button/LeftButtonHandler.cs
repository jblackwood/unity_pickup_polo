using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickupPolo
{


    public class LeftButtonHandler : MonoBehaviour
    {
        public void onPointerDown()
        {
            GameLoop.state.ui_leftButtonDown = true;
        }

        public void onPointerUp()
        {
            GameLoop.state.ui_leftButtonDown = false;
        }
    }
}