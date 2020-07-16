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
      var pit = new Room("in the pit", "The dirt wall around you is too tall to scale, and the only direction to travel is east into a cave...");
      var firstCaveRoom = new Room("one room deep", "You see dwarf & orc skeletons around the room from a past battle.");
      var secondCaveRoom = new Room("two rooms deep", "There's a deep chasm on the south side of the room.");
      var thirdCaveRoom = new Room("three rooms deep", "It's a dead end!");

      //makes relationships for movement between rooms
      pit.AddExit(firstCaveRoom, "east");
      firstCaveRoom.AddExit(pit, "west");
      firstCaveRoom.AddExit(secondCaveRoom, "east");
      secondCaveRoom.AddExit(firstCaveRoom, "west");
      secondCaveRoom.AddExit(thirdCaveRoom, "east");
      thirdCaveRoom.AddExit(secondCaveRoom, "west");

      //adds items to rooms
      pit.AddItem("Torch", "It would be good for traveling through perilous caves.");
      firstCaveRoom.AddItem("Sword", "Could come in handy for orc slaying.");
      thirdCaveRoom.AddItem("Ladder", "Escape is now possible!");

      CurrentRoom = pit;
      CurrentPlayer = new Player("");
    }

    public Game()
    {
      Setup();
    }
  }
}