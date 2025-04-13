
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update.AiAthleteRules
{
    public static class FreeBall
    {
        public static void runUpdateRules(GameState state, string id)
        {
            calculateLocationObjective(state, id);
            setVelocity(state, id);
            setRotation(state, id);
            setAnimation(state, id);
        }

        private static void calculateLocationObjective(GameState state, string id)
        {
            if
            (
                state.game_offenseTeamId == null
            )
            {
                if (state.athlete_isSwimmingForBall.Contains(id)) {
                    state.athlete_locationObjective[id] = state.ball.transform.position;
                } else {
                    var athObj = state.athlete_gameObject[id];
                    state.athlete_locationObjective[id] = athObj.transform.position;
                }
            }
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
            if (Time.frameCount % Constants.frameIntervalAthleteRotationAndAnimation != 0)
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

        private static void setAnimation(GameState state, string id)
        {
            if (Time.frameCount % Constants.frameIntervalAthleteRotationAndAnimation != 0)
            {
                return;
            }
            var ath = state.athlete_gameObject[id];
            var athPos = ath.transform.position;
            if (
                state.entity_animation[id] != Constants.athlete_swim
                && state.entity_velocity[id].magnitude >= 0.1f
                )
            {
                state.entity_animation[id] = Constants.athlete_swim;
                var athleteAnimator = ath.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_swim);
            }
            else if (
                state.entity_animation[id] != Constants.athlete_idle
                && state.entity_velocity[id].magnitude < 0.1f
            )
            {
                var athleteAnimator = ath.GetComponent<Animator>();
                athleteAnimator.Play(Constants.athlete_idle);
                state.entity_animation[id] = Constants.athlete_idle;
            }
        }
    }
}