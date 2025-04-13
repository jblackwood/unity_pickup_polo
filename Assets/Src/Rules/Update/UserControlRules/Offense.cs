using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update.UserControlRules
{
    public static class Offense
    {


        public static void runUpdateRules(GameState state)
        {
            setButtonText(state);
            startSwimmingWithBall(state);
            startSwimmingWithoutBall(state);
            rotateWithJoystick(state);
            moveSwimmer(state);
            idleIfNoBallAndJoystick(state);
            pumpIfBallAndNoJoystick(state);
            shootBall(state);
            calculateReceivingPlayer(state);
            showIconForReceivingPlayer(state);
            passBall(state);
            keepControlledAthleteWithinBoundary(state);

        }
        
        
        private static void setButtonText(GameState state)
        {
            Text middleButtonText = state.middleButton.GetComponentInChildren<Text>();
            if
            (
                middleButtonText.text != Constants.middle_button_text_offense
            )
            {
                Text leftButtonText = state.leftButton.GetComponentInChildren<Text>();
                Text rightButtonText = state.rightButton.GetComponentInChildren<Text>();
                leftButtonText.text = Constants.left_button_text_offense;
                middleButtonText.text = Constants.middle_button_text_offense;
                rightButtonText.text = Constants.right_button_text_offense;
            }
        }

        private static void startSwimmingWithBall(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] != Constants.athlete_swim
                && state.game_athleteWithBall == state.game_userAthleteId
                && state.ui_leftButtonDown
                && state.game_allowAthletesToMove
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_swim);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_swim;
            }
        }


        private static void startSwimmingWithoutBall(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] != Constants.athlete_swim
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


        private static void idleIfNoBallAndJoystick(GameState state)
        {
            if (
                state.entity_animation[state.game_userAthleteId] != Constants.athlete_idle
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


        private static void pumpIfBallAndNoJoystick(GameState state)
        {
            if (
                state.game_athleteWithBall == state.game_userAthleteId
                && state.entity_animation[state.game_athleteWithBall] != Constants.athlete_pump_ball
                && !state.ui_leftButtonDown
            )
            {
                var athlete = state.athlete_gameObject[state.game_userAthleteId];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_pump_ball);
                state.entity_animation[state.game_userAthleteId] = Constants.athlete_pump_ball;
            }
        }

        private static void shootBall(GameState state)
        {
            if (
                state.ui_rightButtonClick
                && state.game_athleteWithBall == state.game_userAthleteId
            )
            {
                GameObject goal = state.team_goalToScore[state.game_offenseTeamId];
                var shotPosition =
                    goal == state.goalWest ?
                        goal.transform.position + Constants.shotOffsetFromGoal
                        : goal.transform.position - Constants.shotOffsetFromGoal;
                Vector3 ballVelocity =
                    Queries.throwBallVelocity(
                        startPosition: state.ball.transform.position,
                        endPosition: shotPosition,
                        initialSpeed: Constants.shotSpeed,
                        gravity: Constants.ball_gravity.y,
                        defaultAngle: Constants.defaultThrowingAngle
                    );
                state.game_athleteWithBall = null;
                state.game_offenseTeamId = null;
                state.entity_velocity.Add(Constants.ball_id, ballVelocity);
            }
        }


        private static void calculateReceivingPlayer(GameState state)
        {
            if (
                state.game_athleteWithBall != null
            )
            {
                var ball = state.ball;
                var controlledAthlete = state.athlete_gameObject[state.game_userAthleteId];
                state.game_athleteReceivingPass =
                    Queries.getReceivingAthleteId(state, controlledAthlete.transform, state.ui_joystickDirection);
            }
        }

        private static void showIconForReceivingPlayer(GameState state)
        {
            if (
                state.game_athleteReceivingPass != null
                && state.game_userAthleteId == state.game_athleteWithBall
            )
            {
                state.receivingAthleteIcon.SetActive(true);
                GameObject receivingAthlete = state.athlete_gameObject[state.game_athleteReceivingPass];
                state.receivingAthleteIcon.transform.parent = receivingAthlete.transform;
                state.receivingAthleteIcon.transform.position =
                    receivingAthlete.transform.position + Constants.receivingAthleteIconOffset;
                state.receivingAthleteIcon.transform.rotation = Constants.athleteIconRotation;
            }
            else if (
                state.receivingAthleteIcon.activeSelf
            )
            {
                state.receivingAthleteIcon.SetActive(false);
            }
        }

        private static void passBall(GameState state)
        {
            if (
                state.ui_middleButtonClick
                && state.game_athleteWithBall == state.game_userAthleteId
            )
            {
                var receivingAthleteId = state.game_athleteReceivingPass;
                var defendingAthId = 
                    state.athlete_defenseAssignment.Where(p => p.Value==receivingAthleteId).First().Key;
                var receivingAthPosition = state.athlete_gameObject[receivingAthleteId].transform.position;
                var defendingAthPosition = state.athlete_gameObject[defendingAthId].transform.position;

                var passLocation = 
                    receivingAthPosition 
                    + Quaternion.LookRotation(receivingAthPosition - defendingAthPosition)*Constants.passOffset;

                Vector3 ballVelocity = Queries.throwBallVelocity(
                    startPosition: state.ball.transform.position,
                    endPosition: passLocation,
                    initialSpeed: Constants.passSpeed,
                    gravity: Constants.ball_gravity.y,
                    defaultAngle: Constants.defaultThrowingAngle
                );
                state.ball_passedLocation = passLocation;
                state.entity_velocity.Add(Constants.ball_id, ballVelocity);
                state.game_athleteWithBall = null;
                state.ball_isBeingPassed = true;
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