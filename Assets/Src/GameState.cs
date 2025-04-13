using System.Collections.Generic;

using UnityEngine;

namespace PickupPolo
{
    public class GameState
    {
        public Joystick joystick;
        public float boundaryEast;
        public float boundaryWest;
        public float boundaryNorth;
        public float boundarySouth;
        
        public GameObject ball;
        public GameObject camera;
        public GameObject leftButton;
        public GameObject middleButton;
        public GameObject rightButton;
        public GameObject shotClockUI;
        public GameObject controlledAthleteIcon;
        public GameObject receivingAthleteIcon;
        public GameObject goalWest;
        public GameObject goalEast;

        public Dictionary<string, GameObject> athlete_gameObject =
            new Dictionary<string,GameObject>();

        public Dictionary<string,string> entity_animation = 
            new Dictionary<string, string>();
        
        public Dictionary<string, Vector3> entity_velocity =
            new Dictionary<string,Vector3>();

        public List<(string, string)> entity_collisions = 
            new List<(string, string)>();

        //<athleteId, offensePositionId> 
        public Dictionary<string, string> athlete_offenseRole =
            new Dictionary<string, string>();
        
        //<athleteId, athleteId> 
        public Dictionary<string, string> athlete_defenseAssignment =
            new Dictionary<string, string>();

        // <athleteId, TeamId>
        public Dictionary<string, string> athlete_teamId =
            new Dictionary<string, string>();

        // <athleteId, location to swimTo>
        public Dictionary<string, Vector3> athlete_locationObjective =
            new Dictionary<string,Vector3>();

        // <athleteId>
        public HashSet<string> athlete_isSwimmingForBall =
            new HashSet<string>();

        // <teamId, goal gameobject>
        public Dictionary<string, GameObject>  team_goalToDefend = 
            new Dictionary<string, GameObject>();
        
        // <teamId, goal gameobject>
        public Dictionary<string, GameObject>  team_goalToScore = 
            new Dictionary<string, GameObject>();


        public bool ball_isBeingPassed = false;
        public Vector3 ball_passedLocation = Vector3.zero;

        public string game_userTeamId = null;
        public string game_userAthleteId = null;
        public string game_offenseTeamId = null;
        public string game_athleteWithBall = null;
        public string game_athleteReceivingPass = null;
        public bool game_allowAthletesToMove = false;
        public bool game_restartAfterGoal = false;
        public float game_possesionTime = 0f;
        public int game_shotClock = 0;
        public float game_timeAthleteWithBall = 0f;
        
        // One frame variables 
        public Vector3 ui_joystickDirection;
        public bool ui_leftButtonDown = false;
        public bool ui_middleButtonClick = false;
        public bool ui_rightButtonClick = false;

        
        public GameState(){

        }

    }

}