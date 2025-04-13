using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace PickupPolo
{

    public static class Queries
    {
        public static Transform getRightHandOfAthlete(GameObject athlete)
        {
            return (athlete.transform
                    .Find("athlete")
                    .Find("mixamorig:Hips")
                    .Find("mixamorig:Spine")
                    .Find("mixamorig:Spine1")
                    .Find("mixamorig:Spine2")
                    .Find("mixamorig:RightShoulder")
                    .Find("mixamorig:RightArm")
                    .Find("mixamorig:RightForeArm")
                    .Find("mixamorig:RightHand")
                    .Find("mixamorig:RightHand_end"));

        }

        public static Renderer getAthleteRenderer(GameObject ath)
        {
            return ath.transform.Find("athelete_mesh").gameObject.GetComponent<Renderer>();
        }

        public static Dictionary<string, string> calculateDefenseAssignments(GameState state)
        {
            var ans = new Dictionary<string, string>();
            foreach (var athKv in state.athlete_offenseRole)
            {
                var roleToDefend = Constants.roleToDefenseMapping[athKv.Value];
                var idToDefend =
                    state.athlete_offenseRole
                        .Where(toDefendKV =>
                            toDefendKV.Value == roleToDefend
                            && state.athlete_teamId[toDefendKV.Key] != state.athlete_teamId[athKv.Key]
                        ).First().Key;
                ans.Add(athKv.Key, idToDefend);
            }
            return ans;
        }


        private struct ReceivingCalculationStats
        {
            public string athleteId;
            public Vector3 position;
            public float angleToPassVector;
            public float distanceToPasser;
        }
        public static string getReceivingAthleteId(GameState state, Transform passingAthleteTransform, Vector3 joystickDirection)
        {
            Ray ray = new Ray(passingAthleteTransform.position, passingAthleteTransform.forward);

            var athletesToReceivePass =
                state.athlete_gameObject
                    .Where(kv =>
                        kv.Key != state.game_athleteWithBall
                        && state.athlete_teamId[kv.Key] == state.athlete_teamId[state.game_athleteWithBall]
                    ).Select(kv =>
                    {
                        ReceivingCalculationStats s = new ReceivingCalculationStats();
                        s.athleteId = kv.Key;
                        s.position = kv.Value.transform.position;
                        s.angleToPassVector =
                            Vector3.Angle(
                                passingAthleteTransform.forward,
                                s.position - passingAthleteTransform.position
                            );
                        s.distanceToPasser = (s.position - passingAthleteTransform.position).magnitude;
                        return s;
                    });
            // Filter for athletes with < 10 degree angle to pass vector
            var filteredAthletes = athletesToReceivePass.Where(s => s.angleToPassVector < 10f);
            // If just 1 athlete use them
            if (filteredAthletes.Count() == 1)
            {
                return filteredAthletes.First().athleteId;
            }
            // If multiple athletes divide joystick into number of athlete increments 
            // and pick closest to actual joystick mangitdue
            if (filteredAthletes.Count() > 1)
            {

                float joystickIncrement = 1f / filteredAthletes.Count(); // assumes joystick max magnitude is 1
                int indexToUse = Mathf.FloorToInt(((joystickDirection.magnitude) / joystickIncrement));
                // In case joystick magnitude was near 1f
                if (indexToUse >= filteredAthletes.Count()) indexToUse = filteredAthletes.Count() - 1;
                return filteredAthletes.OrderBy(s => s.distanceToPasser).ElementAt(indexToUse).athleteId;
            }
            // Else get athlete with min angle
            else
            {
                float minAngle = 180f;
                string closestAthlete = null;
                foreach (var s in athletesToReceivePass)
                {
                    if (s.angleToPassVector < minAngle)
                    {
                        minAngle = s.angleToPassVector;
                        closestAthlete = s.athleteId;
                    }
                }
                return closestAthlete;
            }
        }

        public static Vector3 calculateBallVelocityForPass(
            GameState state
        )
        {
            var receivingAthlete = state.athlete_gameObject[state.game_athleteReceivingPass];

            var ball = state.ball;
            Vector3 distanceToThrow =
                receivingAthlete.transform.position
                + Vector3.up * 0.02f
                - ball.transform.position;

            float timeToArrive =
                distanceToThrow.magnitude * Constants.pass_time_arrive_vs_distance_ratio;

            timeToArrive = Mathf.Max(timeToArrive, Constants.min_pass_time_to_arrive);

            Vector3 ballVelocity =
                distanceToThrow / timeToArrive
                - Constants.ball_gravity * timeToArrive / 2;
            return ballVelocity;

        }


        public static Vector3 calculateProjectileVelocity(
            Vector3 startPosition,
            Vector3 endPosition,
            float initialSpeed,
            float gravity
        )
        {
            Debug.Log(startPosition);
            Debug.Log(endPosition);


            float deltaX = endPosition.x - startPosition.x;
            float deltaZ = endPosition.z - startPosition.z;
            float deltaY = endPosition.y - startPosition.y;

            // Quadratic equation
            float a = Mathf.Pow(gravity, 2f) * 0.25f;
            float b = -1f * Mathf.Pow(initialSpeed, 2f) + gravity * deltaY;
            float c = Mathf.Pow(deltaX, 2f) + Mathf.Pow(deltaZ, 2f) + Mathf.Pow(deltaY, 2f);

            float w1 = (-b + Mathf.Sqrt(Mathf.Pow(b, 2f) - 4 * a * c)) / (2 * a);
            float w2 = (-b - Mathf.Sqrt(Mathf.Pow(b, 2f) - 4 * a * c)) / (2 * a);

            float bestW = Mathf.Max(0f, w1, w2);
            // Vector3 velocity;
            // if (bestW > 0f)
            // {
            float t1 = Mathf.Sqrt(w1);
            float t2 = Mathf.Sqrt(w2);
            Debug.Log("t1: " + t1);
            Debug.Log("t2: " + t2);

            float t = Math.Min(t1, t2);
            float v0x = deltaX / t;
            float v0z = deltaZ / t;
            float v0y = (deltaY - gravity * Mathf.Pow(t, 2) / 2) / t;
            Vector3 velocity = new Vector3(v0x, v0y, v0z);
            // }
            // else
            // {
            //     // Use 30 degree angle
            //     float a1 = (1 + Mathf.Pow(deltaZ, 2) / Mathf.Pow(deltaX, 2));
            //     float b1 = Mathf.Tan(Mathf.PI / 6f) * Mathf.Sqrt(a1);
            //     float c1 = -Mathf.Pow(initialSpeed,2);
            //     float v0x = (-b1 - Mathf.Sqrt(Mathf.Pow(b1, 2f) - 4 * a1 * c1)) / (2 * a1);
            //     float v0z = deltaZ / deltaX * v0x;
            //     float v0y = Mathf.Tan(Mathf.PI / 6f) * Mathf.Sqrt(Mathf.Pow(v0x, 2) * Mathf.Pow(v0z, 2));

            //     Debug.Log(a1);
            //     Debug.Log(b1);
            //     Debug.Log(c1);
            //     Debug.Log(v0x);
            //     Debug.Log(v0z);
            //     Debug.Log(v0y);
            //     velocity = new Vector3(v0x, v0y, v0z);
            // }

            Debug.Log(velocity);
            return velocity;
        }

        public static float calculateProjectileAngle(
            float deltaHorizontal,
            float deltaY,
            float initialSpeed,
            float gravity
        )
        // Formula copied from
        // https://math.stackexchange.com/questions/3019313/finding-projectile-angle-with-different-elevation-when-velocity-and-range-are-kn
        {
            float term1 = Mathf.Pow(initialSpeed, 2);
            float term2 = Mathf.Sqrt(
                Mathf.Pow(initialSpeed, 4) + gravity * (-gravity * Mathf.Pow(deltaHorizontal, 2) + 2 * deltaY * Mathf.Pow(initialSpeed, 2))
            );
            float term3 = -gravity * deltaHorizontal;

            float theta1 = Mathf.Atan((term1 + term2)/term3); 
            float theta2 = Mathf.Atan((term1 - term2)/term3); 

            if(float.IsNaN(theta1) || float.IsNaN(theta2))
            {
                // At least one of them is NaN
                return float.IsNaN(theta1) ? theta2 : theta1;
            } else
            {
                return Mathf.Min(theta1, theta2);
            }
        }

        public static Vector3 throwBallVelocity(
            Vector3 startPosition,
            Vector3 endPosition,
            float initialSpeed,
            float gravity,
            float defaultAngle
        ){
            float deltaX = endPosition.x - startPosition.x;
            float deltaZ = endPosition.z - startPosition.z;
            float deltaHorizontal = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaZ, 2));
            float deltaY = endPosition.y - startPosition.y;
            float theta = calculateProjectileAngle(
                deltaHorizontal,
                deltaY,
                initialSpeed,
                gravity
            );
            if(float.IsNaN(theta))
            {
                theta = defaultAngle;
            }
            float v0y = Mathf.Sin(theta)*initialSpeed;
            float SpeedXZ = Mathf.Cos(theta)*initialSpeed;

            Vector3 velocityXZ = new Vector3(deltaX, 0f,deltaZ).normalized * SpeedXZ;
            return new Vector3(velocityXZ.x, v0y, velocityXZ.z);
        }
    }
}