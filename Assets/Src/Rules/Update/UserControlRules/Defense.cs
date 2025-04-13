using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update.UserControlRules
{
    public static class Defense
    {


        public static void runUpdateRules(GameState state)
        {
            setButtonText(state);
            turnOffReceivingPlayerIcon(state);
            switchPlayer(state);
            stealBall(state);
            idleAfterStealBall(state);
            idleAfterPumpBall(state);
            idleAfterSwimming(state);
            startSwimmingIfIdle(state);
            rotateWithJoystick(state);
            moveSwimmer(state);
            keepControlledAthleteWithinBoundary(state);

        }


        private static void setButtonText(GameState state)
        {
            Text middleButtonText = state.middleButton.GetComponentInChildren<Text>();
            if
            (
                middleButtonText.text != Constants.middle_button_text_defense
            )
            {
                Text leftButtonText = state.leftButton.GetComponentInChildren<Text>();
                Text rightButtonText = state.rightButton.GetComponentInChildren<Text>();
                leftButtonText.text = Constants.left_button_text_defense;
                middleButtonText.text = Constants.middle_button_text_defense;
                rightButtonText.text = Constants.right_button_text_defense;
            }
        }


        private static void turnOffReceivingPlayerIcon(GameState state)
        {
            if
            (
                state.receivingAthleteIcon.activeSelf
            )
            {
                state.receivingAthleteIcon.SetActive(false);
            }
        }


        private static void switchPlayer(GameState state)
        {
            if(
                state.ui_middleButtonClick
            )
            {
                // get closest non-user controlled player to ball
                string closestAthleteId = null;
                float closestDistanceToBall = 1000f;
                var ballPosition =
                    state.ball.transform.position;
                foreach (KeyValuePair<string, GameObject> kv in state.athlete_gameObject)
                {
                    string athleteId = kv.Key;
                    string teamId = state.athlete_teamId[athleteId];
                    if(
                        teamId != state.game_userTeamId
                        || athleteId == state.game_userAthleteId
                    )
                    {
                        continue;
                    }
                    var athPosition = kv.Value.transform.position;
                    var athDistanceToBall = Vector3.Distance(athPosition, ballPosition);
                    if(athDistanceToBall < closestDistanceToBall)
                    {
                        closestDistanceToBall = athDistanceToBall;
                        closestAthleteId = athleteId;
                    }
                }
                state.game_userAthleteId = closestAthleteId;
            }
        }


        private static void stealBall(GameState state)
        {
            if(
                state.ui_rightButtonClick
                && (
                    state.entity_animation[state.game_userAthleteId] == Constants.athlete_swim
                    || state.entity_animation[state.game_userAthleteId] == Constants.athlete_idle
                )
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_steal_ball);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_steal_ball;
            }
        }


        private static void idleAfterStealBall(GameState state)
        {
            if(
                state.entity_animation[state.game_userAthleteId] != Constants.athlete_steal_ball
            ){
                return;
            }
            var athlete = state.athlete_gameObject[state.game_userAthleteId];
            var athleteAnimator = athlete.GetComponent<Animator>();
            var animatorState = athleteAnimator.GetCurrentAnimatorStateInfo(0);
            if(
                animatorState.normalizedTime>=1
                && animatorState.IsName(Constants.athlete_steal_ball)
            )
            {
                athleteAnimator.Play(Constants.athlete_idle);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_idle;
            }
        }
        
        
        private static void idleAfterPumpBall(GameState state)
        {
            if(
                state.entity_animation[state.game_userAthleteId] == Constants.athlete_pump_ball
            ){
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_idle);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_idle;
            }
        }


        private static void idleAfterSwimming(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] == Constants.athlete_swim
                && state.ui_joystickDirection.magnitude <= 0f
                && state.game_athleteWithBall != state.game_userAthleteId
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_idle);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_idle;
            }
        }


        private static void startSwimmingIfIdle(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] == Constants.athlete_idle
                && state.game_athleteWithBall != state.game_userAthleteId
                && state.ui_joystickDirection.magnitude > 0f
                && state.game_allowAthletesToMove
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_swim);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_swim;
            }
        }


        private static void rotateWithJoystick(GameState state)
        {
            if (
                state.ui_joystickDirection.sqrMagnitude > 0f
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                athlete.transform.rotation = Quaternion.LookRotation(state.ui_joystickDirection);
            }

        }

        private static void moveSwimmer(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] == Constants.athlete_swim
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                athlete.transform.position +=
                    state.ui_joystickDirection * Constants.athlete_speed * Time.deltaTime;
            }
        }


        private static void keepControlledAthleteWithinBoundary(GameState state)
        {
            var athlete = state.athlete_gameObject[state.game_userAthleteId];
            var athletePos = athlete.transform.position;

            if (athletePos.x < state.boundaryWest)
            {
                athletePos = new Vector3(state.boundaryWest, athletePos.y, athletePos.z);
            }
            if (athletePos.x > state.boundaryEast)
            {
                athletePos = new Vector3(state.boundaryEast, athletePos.y, athletePos.z);
            }
            if (athletePos.z < state.boundarySouth)
            {
                athletePos = new Vector3(athletePos.x, athletePos.y, state.boundarySouth);
            }
            if (athletePos.z > state.boundaryNorth)
            {
                athletePos = new Vector3(athletePos.x, athletePos.y, state.boundaryNorth);
            }
            athlete.transform.position = athletePos;
        }
    }
}