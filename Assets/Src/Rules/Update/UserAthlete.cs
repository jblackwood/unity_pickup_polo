
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update
{
    public static class UserAthlete
    {


        public static void runUpdateRules(GameState state)
        {
            checkKeyboard(state);
            calculateJoystickDirection(state);
            if(state.game_userTeamId == state.game_offenseTeamId)
            {
                UserControlRules.Offense.runUpdateRules(state);
            } 
            else 
            {
                UserControlRules.Defense.runUpdateRules(state);
            }
            controlledAthleteIcon(state);
        }


        private static void checkKeyboard(GameState state)
        {
            if (Input.GetKeyDown(KeyCode.A) && state.leftButton.activeSelf)
            {
                state.ui_leftButtonDown = true;
            }
            if (Input.GetKeyUp(KeyCode.A) && state.leftButton.activeSelf)
            {
                state.ui_leftButtonDown = false;
            }
            if (Input.GetKeyUp(KeyCode.S) && state.middleButton.activeSelf)
            {
                state.ui_middleButtonClick = true;
            }
            if (Input.GetKeyUp(KeyCode.D) && state.rightButton.activeSelf)
            {
                state.ui_rightButtonClick = true;
            }
        }


        private static void calculateJoystickDirection(GameState state)
        {
            state.ui_joystickDirection =
                Vector3.forward * state.joystick.Vertical + Vector3.right * state.joystick.Horizontal;
        }




        private static void controlledAthleteIcon(GameState state)
        {
            GameObject controlledAthlete = state.athlete_gameObject[state.game_userAthleteId];

            if (
                state.controlledAthleteIcon.transform.parent
                != controlledAthlete.transform
            )
            {
                state.controlledAthleteIcon.transform.parent = controlledAthlete.transform;

                state.controlledAthleteIcon.transform.position =
                    controlledAthlete.transform.position +
                    controlledAthlete.transform.rotation * Constants.controlledAthleteIconOffset;
                state.controlledAthleteIcon.transform.rotation =
                    controlledAthlete.transform.rotation * Constants.athleteIconRotation;
            }
        }
    }
}