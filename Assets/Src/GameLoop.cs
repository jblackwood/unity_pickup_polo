using UnityEngine;
using System.Linq;

namespace PickupPolo
{
    public class GameLoop : MonoBehaviour
    {

        public GameObject athletePrefab;
        public GameObject camObj;
        public Joystick joystick;
        public GameObject leftButton;
        public GameObject middleButton;
        public GameObject rightButton;
        public GameObject shotClockUI;
        public GameObject ballPrefab;
        public GameObject boundaryEast;
        public GameObject boundaryWest;
        public GameObject boundaryNorth;
        public GameObject boundarySouth;
        public GameObject goalEast;
        public GameObject goalWest;
        public GameObject controlledAthleteIconPrefab;
        public GameObject receivingAthleteIconPrefab;



        public static GameState state = new GameState();

       
        // Start is called before the first frame update
        void Start()

        {
            state.athlete_teamId = Constants.athleteTeamIds;
            for (int i = 0; i < Constants.athlete_ids.Count; i++)
            {
                string athleteId = Constants.athlete_ids[i];
                GameObject athlete =
                    Instantiate(athletePrefab);
                athlete.name = athleteId;
                state.athlete_gameObject[athleteId] = athlete;
                state.entity_animation[athleteId] = Constants.athlete_idle;
                if (state.athlete_teamId[athleteId] == Constants.team_2)
                {
                    var athleteRenderer = Queries.getAthleteRenderer(athlete);
                    athleteRenderer.material.SetColor("_Color", Color.grey);
                }
            }
            state.athlete_offenseRole = Constants.athleteOffenseRoleIds;

            state.camera = camObj;
            state.joystick = joystick;
            state.leftButton = leftButton;
            state.middleButton = middleButton;
            state.rightButton = rightButton;
            state.shotClockUI = shotClockUI;
            state.goalWest = goalWest;
            state.goalEast = goalEast;
            state.controlledAthleteIcon = Instantiate(controlledAthleteIconPrefab);
            state.receivingAthleteIcon = Instantiate(receivingAthleteIconPrefab);
            state.boundaryEast = boundaryEast.transform.position.x - Constants.boundary_buffer;
            state.boundaryWest = boundaryWest.transform.position.x + Constants.boundary_buffer;
            state.boundaryNorth = boundaryNorth.transform.position.z - Constants.boundary_buffer;
            state.boundarySouth = boundarySouth.transform.position.z + Constants.boundary_buffer;

            var ball =
                Instantiate(ballPrefab);
            ball.name = Constants.ball_id;
            state.ball = ball;

            state.game_userTeamId = Constants.team_1;
            state.game_userAthleteId = Constants.athlete_ids[2];
            state.game_offenseTeamId = Constants.team_1;
            state.team_goalToDefend[Constants.team_1] = goalWest;
            state.team_goalToDefend[Constants.team_2] = goalEast;
            state.team_goalToScore[Constants.team_1] = goalEast;
            state.team_goalToScore[Constants.team_2] = goalWest;
            state.athlete_defenseAssignment = Queries.calculateDefenseAssignments(state);

            state.game_restartAfterGoal = true;

        }

        // Update is called once per frame
        void Update()
        {
            CollisionRules.runCollisionRules(state);
            UpdateRules.runUpdateRules(state);
        }
    }
}