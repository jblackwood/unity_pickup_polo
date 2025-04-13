using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update
{
    public static class AiAthletes
    {

        public static void runUpdateRules(GameState state)
        {
            if (!state.game_allowAthletesToMove)
                return;
            var nonControlledAthleteIds =
                state.athlete_gameObject.Keys.Where(id => id != state.game_userAthleteId);
            foreach (var athleteId in nonControlledAthleteIds)
            {
                if (state.athlete_teamId[athleteId] == state.game_offenseTeamId)
                {
                    AiAthleteRules.Offense.runUpdateRules(state, athleteId);
                    moveAthlete(state, athleteId);
                }
                else if (state.athlete_teamId[athleteId] != state.game_offenseTeamId && state.game_offenseTeamId != null)
                {
                    AiAthleteRules.Defense.runUpdateRules(state, athleteId);
                    moveAthlete(state, athleteId);
                }
                else
                {
                    AiAthleteRules.FreeBall.runUpdateRules(state, athleteId);
                    moveAthlete(state, athleteId);
                }
            }
        }

        private static void moveAthlete(GameState state, string id)
        {
            var athObj = state.athlete_gameObject[id];
            athObj.transform.position += state.entity_velocity[id] * Time.deltaTime;
        }
    }
}