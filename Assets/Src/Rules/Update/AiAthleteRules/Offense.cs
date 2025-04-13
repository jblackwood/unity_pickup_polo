
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update.AiAthleteRules
{
    public static class Offense
    {
        private static System.Random random = new System.Random();

        public static void runUpdateRules(GameState state, string id)
        {
            passBall(state, id);
            shootBall(state, id);
            calculateLocationObjective(state, id);
            setVelocity(state, id);
            setRotation(state, id);

            pumpBallAnimation(state, id);
            swimAnimation(state, id);
            idleAnimation(state, id);

        }

        private static void passBall(GameState state, string id)
        {
            if (
                state.game_athleteWithBall == id
                && state.game_timeAthleteWithBall > Constants.secondsBetweenAIPasses
            )
            {
                List<string> teammates =
                    state.athlete_teamId
                        .Where(kv => kv.Value == state.game_offenseTeamId && kv.Key != id)
                        .Select(kv => kv.Key)
                        .ToList();
                var receivingAthleteId = teammates[random.Next(teammates.Count)];
                state.game_athleteReceivingPass = receivingAthleteId;
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

        private static void shootBall(GameState state, string id)
        {
            if (
                state.game_athleteWithBall == id
                && state.game_shotClock <= Constants.aiShootWhenShotClock
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

        private static void calculateLocationObjective(GameState state, string id)
        {
            var athObj = state.athlete_gameObject[id];
            Vector3 locationToBe;
            if
            (
                state.ball_isBeingPassed
                && state.game_athleteReceivingPass == id
            )
            {
                if(state.entity_velocity.ContainsKey(Constants.ball_id))
                {
                    locationToBe = state.ball_passedLocation;
                } else 
                {
                    locationToBe = state.ball.transform.position;
                }
            }
            else if (
                state.game_athleteWithBall == id
            )
            {
                locationToBe = athObj.transform.position;
            }
            else
            {
                var athRole = state.athlete_offenseRole[id];
                var teamId = state.athlete_teamId[id];
                if (state.team_goalToDefend[teamId] == state.goalWest)
                {
                    locationToBe = Constants.offenseLocations_defendGoalWest[athRole];
                }
                else
                {
                    locationToBe = Constants.offenseLocations_defendGoalEast[athRole];
                }
            }
            state.athlete_locationObjective[id] = locationToBe;
        }

        private static void setVelocity(GameState state, string id)
        {
            var athObj = state.athlete_gameObject[id];
            var toLocation = state.athlete_locationObjective[id];
            var vectorToGo = toLocation - athObj.transform.position;
            var velocity = Vector3.zero;
            if (vectorToGo.magnitude >= 0.1f)
                velocity = Constants.athlete_speed * vectorToGo.normalized;
            state.entity_velocity[id] = velocity;
        }

        private static void setRotation(GameState state, string id)
        {
            if
            (
                Time.frameCount % Constants.frameIntervalAthleteRotationAndAnimation != 0
            )
            {
                return;
            }
            var ath = state.athlete_gameObject[id];
            if (
                state.entity_velocity[id] != Vector3.zero
            )
            {
                ath.transform.rotation = Quaternion.LookRotation(state.entity_velocity[id]);
            }
            else if
            (
                state.athlete_teamId[id] == state.game_offenseTeamId
            )
            {
                var teamId = state.game_offenseTeamId;
                var goal = state.team_goalToDefend.Where(kv => kv.Key != teamId).First().Value;
                var lookAtGoal = goal.transform.position - ath.transform.position;
                state.athlete_gameObject[id].transform.rotation = Quaternion.LookRotation(lookAtGoal);
            }
            else
            {
                var athToDefendId = state.athlete_defenseAssignment[id];
                var athToDefend = state.athlete_gameObject[athToDefendId];
                var lookAtPlayerToDefend = athToDefend.transform.position - ath.transform.position;
                state.athlete_gameObject[id].transform.rotation = Quaternion.LookRotation(lookAtPlayerToDefend);
            }
        }

        private static void pumpBallAnimation(GameState state, string id)
        {
            if (
                state.game_athleteWithBall == id
                && state.entity_animation[state.game_athleteWithBall] != Constants.athlete_pump_ball
            )
            {
                var athlete = state.athlete_gameObject[id];
                var athleteAnimator = athlete.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_pump_ball);
                state.entity_animation[id] = Constants.athlete_pump_ball;
            }
        }

        private static void swimAnimation(GameState state, string id)
        {
            if (
                state.game_athleteWithBall != id
                && state.entity_animation[id] != Constants.athlete_swim
                && state.entity_velocity[id].magnitude >= 0.1f
            )
            {
                state.entity_animation[id] = Constants.athlete_swim;
                var athleteAnimator = state.athlete_gameObject[id].GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_swim);
            }
        }

        private static void idleAnimation(GameState state, string id)
        {
            if (
                state.game_athleteWithBall != id
                && state.entity_animation[id] != Constants.athlete_idle
                && state.entity_velocity[id].magnitude < 0.1f
            )
            {
                var athleteAnimator = state.athlete_gameObject[id].GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_idle);
                state.entity_animation[id] = Constants.athlete_idle;
            }
        }
    }
}