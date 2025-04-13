using UnityEngine;

namespace PickupPolo
{

    public class RightButtonHandler : MonoBehaviour
    {
        public void handleClick()
        {
            GameLoop.state.ui_rightButtonClick = true;
        }
    }
}