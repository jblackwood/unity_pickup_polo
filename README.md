# Unity Pickup Polo
This is a small water polo video game I made in Unity as a personal project. It was a fun project for learning Unity with a topic I'm passionate about.


The main game loop entry point is in [Assets/Src/Gameloop.cs](https://github.com/jblackwood/unity_pickup_polo/blob/2fe51fd6383384ef7e8857bfaac9cb837086cb2e/Assets/Src/GameLoop.cs#L86):
```
// Update is called once per frame
void Update()
{
   CollisionRules.runCollisionRules(state);
   UpdateRules.runUpdateRules(state);
}
```


The game logic is entirely contained in the static functions `CollisionRules.runCollisionRules` and `UpdateRules.runUpdateRules(state)` for how the game state should update each frame. For example [Assets/Src/Rules/UpdatesRules.cs](https://github.com/jblackwood/unity_pickup_polo/blob/2fe51fd6383384ef7e8857bfaac9cb837086cb2e/Assets/Src/Rules/UpdateRules.cs#L13) looks like:
```
namespace PickupPolo
{
   public static class UpdateRules
   {


       public static void runUpdateRules(GameState state)
       {
           Update.RestartAfterGoal.runUpdateRules(state);
           updatePossesionTime(state);
           updateShotClock(state);
           updateTimeAthleteWithBall(state);
           allowMovement(state);
           userAthleteWithBall(state);
           Update.TeamAI.runUpdateRules(state);
           Update.UserAthlete.runUpdateRules(state);
           Update.AiAthletes.runUpdateRules(state);
           Update.Ball.runUpdateRules(state);
           updateCameraPosition(state);
           nullOneFrameFields(state);


       }


       // definitions of the static functions used above
```


I tend to prefer functional-style programming which is why I'm using almost exclusively static functions in this project. I didn't find a need to encapsulate state in objects any further than Unity APIs already encapsulate input and graphics states.

