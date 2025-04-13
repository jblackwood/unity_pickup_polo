using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PickupPolo.Update
{
    public static class Ball
    {


        public static void runUpdateRules(GameState state)
        {
            nullParentIfNoAthleteHasBall(state);
            setBallPositionIfSwimming(state);
            moveFlyingBall(state);
            stopFlyingBallAtWater(state);
            attachBallToRightHandOfNonSwimmingPlayer(state);
        }


        private static void nullParentIfNoAthleteHasBall(GameState state){
            if(
                state.game_athleteWithBall == null
            )
            {
                state.ball.transform.parent = null;
            }
        }

        private static void moveFlyingBall(GameState state)
        {
            var ball = state.ball;
            if(
                ball.transform.position.y > 0
                && state.game_athleteWithBall == null
            ) {
                var ballVelocity = state.entity_velocity[Constants.ball_id];
                ball.transform.position += ballVelocity*Time.deltaTime;
                var newVelocity = ballVelocity + Constants.ball_gravity*Time.deltaTime;
                state.entity_velocity[Constants.ball_id] = newVelocity;
            }
        }
        
        private static void stopFlyingBallAtWater(GameState state)
        {
            var ball = state.ball;
            if(
                ball.transform.position.y <= 0
                && state.game_athleteWithBall == null
            ) {
                ball.transform.position = 
                    new Vector3(ball.transform.position.x, 0, ball.transform.position.z);
                state.entity_velocity.Remove(Constants.ball_id); 
            }
        }

        private static void setBallPositionIfSwimming(GameState state){
            if(
                state.game_athleteWithBall != null
                && state.ball.transform.parent != state.athlete_gameObject[state.game_athleteWithBall].transform
                && state.entity_animation[state.game_athleteWithBall] == Constants.athlete_swim
            )
            {
                var athlete = state.athlete_gameObject[state.game_athleteWithBall];
                state.ball.transform.parent = athlete.transform;
                state.ball.transform.position = 
                    athlete.transform.position + athlete.transform.rotation*Constants.swimmingAthleteBallOffset;
            }
        }

        private static void attachBallToRightHandOfNonSwimmingPlayer(GameState state){
            if(
                state.game_athleteWithBall == null
            )
            {
                return;
            }
            var athleteWithBall = state.athlete_gameObject[state.game_athleteWithBall];
            var rightHand = Queries.getRightHandOfAthlete(athleteWithBall);
            if(
                state.ball.transform.parent != rightHand
                && state.entity_animation[state.game_athleteWithBall] != Constants.athlete_swim
            )
            {
                state.ball.transform.parent = rightHand;

                state.ball.transform.position = rightHand.transform.position;
               
            }
        }
    }
}