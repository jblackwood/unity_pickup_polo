
using UnityEngine;
using System.Collections.Generic;

namespace PickupPolo
{
    public static class CollisionRules
    {

        public static void runCollisionRules(GameState state)
        {
          
            foreach (var coll in state.entity_collisions)
            {
                handleCollision(coll.Item1, coll.Item2, state);

            }
            state.entity_collisions.Clear();
        }

        public static void handleCollision(string e1, string e2, GameState state)
        {
            receivingAthleteCatchesBall(e1,e2,state);
            athleteShouldPickupBall(e1, e2, state);
        }

        private static void receivingAthleteCatchesBall(string e1, string e2, GameState state)
        {
            if (
                // Assume e1 collidedWith c
                state.ball_isBeingPassed
                && state.game_athleteReceivingPass == e1
                && e2 == Constants.ball_id
            )
            {
                state.entity_velocity.Remove(Constants.ball_id);
                state.game_athleteWithBall = e1;
                state.game_athleteReceivingPass = null;
                state.ball_isBeingPassed = false;
                state.game_timeAthleteWithBall = 0f;
            }
        }
        
        private static void athleteShouldPickupBall(string e1, string e2, GameState state)
        {
            if (
                // Assume e1 collidedWith c
                state.athlete_gameObject.ContainsKey(e1)
                && e2 == Constants.ball_id
                && state.game_athleteWithBall == null
                && state.game_offenseTeamId != state.athlete_teamId[e1]
                && !state.entity_velocity.ContainsKey(Constants.ball_id)
            )
            {
                state.athlete_isSwimmingForBall.Clear();
                state.game_athleteWithBall = e1;
                state.game_offenseTeamId = state.athlete_teamId[e1];
                state.game_possesionTime = 0f;
                state.game_timeAthleteWithBall = 0f;
                state.ball_isBeingPassed = false;
                state.game_athleteReceivingPass = null;
            }
        }

    }
}