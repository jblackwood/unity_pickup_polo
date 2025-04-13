using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update
{
    public static class TeamAI
    {

        public static void runUpdateRules(GameState state)
        {
            setSwimmersForFreeBall(state);

        }

        private static void setSwimmersForFreeBall(GameState state)
        {
            if (
                state.game_offenseTeamId == null
                && state.game_allowAthletesToMove
                && !state.entity_velocity.ContainsKey(Constants.ball_id)
                && state.athlete_isSwimmingForBall.Count == 0
            )
            {
                Dictionary<string, string> team_closestAthleteToBall =
                    new Dictionary<string, string>();

                Dictionary<string, float> team_distanceClosesAthleteToBall =
                    new Dictionary<string, float>();

                foreach (KeyValuePair<string, GameObject> kv in state.athlete_gameObject)
                {
                    string athleteId = kv.Key;
                    string teamId = state.athlete_teamId[athleteId];
                    float distanceToBall =
                        Vector3.Distance(kv.Value.transform.position, state.ball.transform.position);
                    if (
                        !team_distanceClosesAthleteToBall.ContainsKey(teamId)
                        || distanceToBall < team_distanceClosesAthleteToBall[teamId]
                    )
                    {
                        team_distanceClosesAthleteToBall[teamId] = distanceToBall;
                        team_closestAthleteToBall[teamId] = athleteId;
                    }
                }

                foreach (string athleteId in team_closestAthleteToBall.Values)
                {
                    state.athlete_isSwimmingForBall.Add(athleteId);
                }
            }
        }
    }
}