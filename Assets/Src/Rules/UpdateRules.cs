
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo
{
    public static class UpdateRules
    {

        public static void runUpdateRules(GameState state)
        {
            Update.RestartAfterGoal.runUpdateRules(state);
            updatePossesionTime(state);
            updateShotClock(state);
            updateTimeAthleteWithBall(state);
            allowMovement(state); 
            userAthleteWithBall(state);
            Update.TeamAI.runUpdateRules(state);
            Update.UserAthlete.runUpdateRules(state);
            Update.AiAthletes.runUpdateRules(state);
            Update.Ball.runUpdateRules(state);
            updateCameraPosition(state);
            nullOneFrameFields(state);

        }


        private static void userAthleteWithBall(GameState state)
        {
            if (
                state.game_athleteWithBall != null
                && state.game_offenseTeamId == state.game_userTeamId
                && state.game_athleteWithBall != state.game_userAthleteId
            )
            {
                state.game_userAthleteId = state.game_athleteWithBall;
            }
        }

        private static void allowMovement(GameState state)
        {
            if (
                !state.game_allowAthletesToMove
                && Time.time > 1.5f
            )
            {
                state.game_allowAthletesToMove = true;
            }

        }

        private static void updateCameraPosition(GameState state)
        {
            Vector3 cameraFocus;
            if (
                state.game_athleteWithBall != null
            )
            {
                cameraFocus =
                    state.athlete_gameObject[state.game_athleteWithBall].transform.position;

            }
            else
            {
                cameraFocus =
                    new Vector3(state.ball.transform.position.x, 0, state.ball.transform.position.z);
            }
            var camObj = state.camera;
            camObj.transform.position =
                cameraFocus + Constants.cameraOffsetFromBall;

        }

        private static void nullOneFrameFields(GameState state)
        {
            state.ui_middleButtonClick = false;
            state.ui_rightButtonClick = false;
        }
        
        private static void updatePossesionTime(GameState state)
        {
            if(state.game_offenseTeamId != null)
            {
                state.game_possesionTime += Time.deltaTime;
            } else {
                state.game_possesionTime = 0f;
            }
        }
        
        private static void updateShotClock(GameState state)
        {
            state.game_shotClock = 
                (int) Math.Round(Constants.shotClock - state.game_possesionTime, 0);
            Text t = state.shotClockUI.GetComponent<Text>();
            t.text = $"Shot clock: {state.game_shotClock}";
        }
        
        private static void updateTimeAthleteWithBall(GameState state)
        {
            if(state.game_athleteWithBall != null)
            {
                state.game_timeAthleteWithBall += Time.deltaTime;
            }
        }

    }
}