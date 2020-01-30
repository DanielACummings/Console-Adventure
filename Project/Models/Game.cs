using ConsoleAdventure.Project.Interfaces;

namespace ConsoleAdventure.Project.Models
{
  public class Game : IGame
  {
    public IRoom CurrentRoom { get; set; }
    public IPlayer CurrentPlayer { get; set; }

    public void Setup()
    {
      //makes rooms
      var pit = new Room("in the pit", "There's nothing but dirt wall around you.");
      // & an unlit torch near the cave entrance.
      var firstCaveRoom = new Room("one room deep", "You see dwarf & orc skeletons around the room from a past battle.");
      //"An orc is running at you from the north side of the room with his sword drawn!\nWhat will you do?"
      var secondCaveRoom = new Room("two rooms deep", "There's a deep chasm on the south side of the room.\nWhat will you do?");
      var thirdCaveRoom = new Room("three rooms deep", "It's a dead end! There's no way to go but back.");

      //makes relationships for movement between rooms
      pit.AddExit(firstCaveRoom, "east");
      firstCaveRoom.AddExit(pit, "west");
      firstCaveRoom.AddExit(secondCaveRoom, "east");
      secondCaveRoom.AddExit(firstCaveRoom, "west");
      secondCaveRoom.AddExit(thirdCaveRoom, "east");
      thirdCaveRoom.AddExit(secondCaveRoom, "west");

      CurrentRoom = pit;
      CurrentPlayer = new Player("");
    }

    public Game()
    {
      Setup();
    }
  }
}