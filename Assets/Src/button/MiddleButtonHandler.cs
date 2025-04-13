using UnityEngine;

namespace PickupPolo
{

    public class MiddleButtonHandler : MonoBehaviour
    {
        public void handleClick()
        {
            GameLoop.state.ui_middleButtonClick = true;
        }
    }
}