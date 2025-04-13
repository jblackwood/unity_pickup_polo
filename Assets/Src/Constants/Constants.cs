using UnityEngine;
using System.Collections.Generic;

namespace PickupPolo
{
    public static class Constants
    {
        public const string camera_id = "Main Camera";
        public const string joystick_id = "Joystick";
        public const string pool_id = "pool";
        public const string ball_id = "ball";
        public const string goal_east_id = "goal_east";
        public const string goal_west_id = "goal_west";
        public const string boundary_east_id = "boundary_east";
        public const string boundary_west_id = "boundary_west";
        public const string boundary_north_id = "boundary_north";
        public const string boundary_south_id = "boundary_south";
        public const float boundary_buffer = 0.1f;

        public const string left_button_text_offense = "Swim";
        public const string middle_button_text_offense = "Pass";
        public const string right_button_text_offense = "Shoot";
        public const string left_button_text_defense = "TBD";
        public const string middle_button_text_defense = "Switch";
        public const string right_button_text_defense = "Steal";

        public const string athlete_swim = "athlete|swim";
        public const string athlete_idle = "athlete|idle";
        public const string athlete_pump_ball = "athlete|pump_ball";
        public const string athlete_steal_ball = "athlete|steal_ball";

        public const float athlete_speed = 1.5f;
        
        public const float pass_time_arrive_vs_distance_ratio = 0.2f;
        public const float min_pass_time_to_arrive = 0.5f;
        public static Vector3 ball_gravity = new Vector3(0f, -10f,0f);


        public static Quaternion athleteIconRotation =
            Quaternion.Euler(90f,0f,0f);
        public static Vector3 controlledAthleteIconOffset = new Vector3(0f,0.1f,0.4f);
        public static Vector3 receivingAthleteIconOffset = new Vector3(0f,0.1f,0f);

        public static Vector3 swimmingAthleteBallOffset = new Vector3(0f,0.05f,0.55f); 
        public static Vector3 cameraOffsetFromBall = new Vector3(0.0f,7f,-7f);

        public static string position_offense_1 = "offense_1";
        public static string position_offense_2 = "offense_2";
        public static string position_offense_3 = "offense_3";
        public static string position_offense_4 = "offense_4";
        public static string position_offense_5 = "offense_5";
        public static string position_offense_center = "offense_center";

        public static List<string> athlete_ids =
            new List<string>(){
                "athlete_0",
                "athlete_1",
                "athlete_2",
                "athlete_3",
                "athlete_4",
                "athlete_5",

                "athlete_6",
                "athlete_7",
                "athlete_8",
                "athlete_9",
                "athlete_10",
                "athlete_11"
            };

        public static string team_1 = "team_1";
        public static string team_2 = "team_2";

        // <athleteId, teamId>
        public static Dictionary<string, string> athleteTeamIds = 
            new Dictionary<string, string>(){
                {athlete_ids[0], Constants.team_1},
                {athlete_ids[1], Constants.team_1},
                {athlete_ids[2], Constants.team_1},
                {athlete_ids[3], Constants.team_1},
                {athlete_ids[4], Constants.team_1},
                {athlete_ids[5], Constants.team_1},
                
                {athlete_ids[6], Constants.team_2},
                {athlete_ids[7], Constants.team_2},
                {athlete_ids[8], Constants.team_2},
                {athlete_ids[9], Constants.team_2},
                {athlete_ids[10], Constants.team_2},
                {athlete_ids[11], Constants.team_2}
            };
        
        public static Dictionary<string, string> athleteOffenseRoleIds =
            new Dictionary<string, string>(){
                {athlete_ids[0], position_offense_1},
                {athlete_ids[1], position_offense_2},
                {athlete_ids[2], position_offense_3},
                {athlete_ids[3], position_offense_4},
                {athlete_ids[4], position_offense_5},
                {athlete_ids[5], position_offense_center},
                
                {athlete_ids[6], position_offense_1},
                {athlete_ids[7], position_offense_2},
                {athlete_ids[8], position_offense_3},
                {athlete_ids[9], position_offense_4},
                {athlete_ids[10], position_offense_5},
                {athlete_ids[11], position_offense_center}
            };

        // <offense position id, position id to defend>
        public static Dictionary<string, string> roleToDefenseMapping =
            new Dictionary<string, string>(){
                {position_offense_1, position_offense_4},
                {position_offense_2, position_offense_5},
                {position_offense_3, position_offense_center},
                {position_offense_4, position_offense_1},
                {position_offense_5, position_offense_2},
                {position_offense_center, position_offense_3},
            };

        // <position id, location>
        public static Dictionary<string, Vector3> restartOffenseLocations_defendGoalWest = 
            new Dictionary<string, Vector3>(){
                {Constants.position_offense_1, new Vector3(0f,0f,3f)},
                {Constants.position_offense_2, new Vector3(0f,0f,1.5f)},
                {Constants.position_offense_3, new Vector3(-1.5f,0f,0f)},
                {Constants.position_offense_4, new Vector3(0f,0f,-1.5f)},
                {Constants.position_offense_5, new Vector3(0f,0f,-3f)},
                {Constants.position_offense_center, new Vector3(0f,0f,0f)},
            };
        
        public static Dictionary<string, Vector3> restartOffenseLocations_defendGoalEast = 
            new Dictionary<string, Vector3>(){
                {Constants.position_offense_1, new Vector3(0f,0f,-3f)},
                {Constants.position_offense_2, new Vector3(0f,0f,-1.5f)},
                {Constants.position_offense_3, new Vector3(1.5f,0f,0f)},
                {Constants.position_offense_4, new Vector3(0f,0f,1.5f)},
                {Constants.position_offense_5, new Vector3(0f,0f,3f)},
                {Constants.position_offense_center, new Vector3(0f,0f,0f)},
            };

        // <position id, location>
        public static Dictionary<string, Vector3> offenseLocations_defendGoalWest = 
            new Dictionary<string, Vector3>(){
                {Constants.position_offense_1, new Vector3(9.5f,0f,4f)},
                {Constants.position_offense_2, new Vector3(6f,0f,2.8f)},
                {Constants.position_offense_3, new Vector3(4f,0f,0f)},
                {Constants.position_offense_4, new Vector3(6f,0f,-2.8f)},
                {Constants.position_offense_5, new Vector3(9.5f,0f,-4f)},
                {Constants.position_offense_center, new Vector3(10f,0f,0f)},
            };
        
        public static Dictionary<string, Vector3> offenseLocations_defendGoalEast = 
            new Dictionary<string, Vector3>(){
                {Constants.position_offense_1, new Vector3(-9.5f,0f,-4f)},
                {Constants.position_offense_2, new Vector3(-6f,0f,-2.8f)},
                {Constants.position_offense_3, new Vector3(-4f,0f,0f)},
                {Constants.position_offense_4, new Vector3(-6f,0f,2.8f)},
                {Constants.position_offense_5, new Vector3(-9.5f,0f,4f)},
                {Constants.position_offense_center, new Vector3(-10f,0f,0f)},
            };
        
        public static Vector3 restartAfterGoalDefenseOffset = new Vector3(2.5f,0f,0f);

        public static Vector3 athDefenseOffset = new Vector3(0f,0f,0.5f);

        public static int frameIntervalAthleteRotationAndAnimation = 15;
        public static float secondsBetweenAIPasses = 2f;
        public static float shotClock = 20;
        public static int aiShootWhenShotClock = 2;

        public static Vector3 aiShootLocation = new Vector3(0f,0f,0f);
        public static Vector3 shotOffsetFromGoal = new Vector3(0.5f,0f,2f);
        public static float shotSpeed = 15f;
        public static float passSpeed = 10f;
        public static Vector3 passOffset = new Vector3(0f, 0f, 2f);
        public static float defaultThrowingAngle = Mathf.PI/6f;
    }
}