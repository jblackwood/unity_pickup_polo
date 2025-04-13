using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update
{
    public static class RestartAfterGoal
    {

        public static void runUpdateRules(GameState state)
        {
            setAthleteWithBallOnRestart(state);
            positionAthletes(state);
            state.game_restartAfterGoal = false;
        }


        private static void setAthleteWithBallOnRestart(GameState state)
        {
            if (
                state.game_restartAfterGoal
            )
            {
                state.game_athleteWithBall =
                    state.athlete_offenseRole
                        .Where(athKV =>
                            athKV.Value == Constants.position_offense_center
                            && state.athlete_teamId[athKV.Key] == state.game_offenseTeamId
                    ).First().Key;
            }
        }

       


        private static void positionAthletes(GameState state)
        {
            if (
                state.game_restartAfterGoal
            )
            {
                foreach (var athKV in state.athlete_gameObject)
                {
                    var athId = athKV.Key;
                    var athObj = athKV.Value;
                    var teamId = state.athlete_teamId[athId];
                    var goalToDefend = state.team_goalToDefend[teamId];
                    if (
                        state.athlete_teamId[athId] == state.game_offenseTeamId
                        && goalToDefend == state.goalWest
                    )
                    {
                        athObj.transform.position =
                            Constants.restartOffenseLocations_defendGoalWest[state.athlete_offenseRole[athId]];
                    } else if (
                        state.athlete_teamId[athId] == state.game_offenseTeamId
                        && goalToDefend == state.goalEast
                    )
                    {
                        athObj.transform.position =
                            Constants.restartOffenseLocations_defendGoalEast[state.athlete_offenseRole[athId]];


                    } else if (
                        goalToDefend == state.goalEast
                    )
                    {
                        var athToDefend = state.athlete_defenseAssignment[athId];
                        var locationOfAthToDefend =
                            Constants.restartOffenseLocations_defendGoalWest[state.athlete_offenseRole[athToDefend]];
                        athObj.transform.position = locationOfAthToDefend + Constants.restartAfterGoalDefenseOffset;
                    } else if (
                        goalToDefend == state.goalWest
                    )
                    {
                        var athToDefend = state.athlete_defenseAssignment[athId];
                        var locationOfAthToDefend =
                            Constants.restartOffenseLocations_defendGoalEast[state.athlete_offenseRole[athToDefend]];
                        athObj.transform.position = locationOfAthToDefend - Constants.restartAfterGoalDefenseOffset;
                    }

                }
            }

        }
    }
}