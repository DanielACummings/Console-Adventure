using System;
using ConsoleAdventure.Project.Interfaces;

namespace ConsoleAdventure.Project.Controllers
{

  public class GameController : IGameController
  {
    private GameService _gameService = new GameService();

    private bool _playing = true;
    public void Run()
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Welcome to Console Adventure!\nWhat's you're name?");
      string playerName = Console.ReadLine();
      _gameService.Setup(playerName);
      Console.Clear();

      Console.WriteLine("On a brisk dawn while hiking along the base of the Misty Mountains,you slip & fall into a pit! You then attempt to climb out but are unable to.\nYou must figure out how to escape before nightfall when the mountainside will be swarming with orcs!\n");
      Console.WriteLine("Type \"help\" at any time to view your options.\n");
      _gameService.RoomInfo();
      while (_playing)
      {
        Print();
        GetUserInput();
      }
    }

    public void GetUserInput()
    {
      Console.WriteLine("\nWhat would you like to do?");
      string input = Console.ReadLine().ToLower() + " ";
      string command = input.Substring(0, input.IndexOf(" "));
      string option = input.Substring(input.IndexOf(" ") + 1).Trim();

      Console.Clear();
      switch (command)
      {
        case "look":
          _gameService.Look();
          break;
        case "take":
          _gameService.TakeItem(option);
          break;
        case "inventory":
          _gameService.Inventory();
          break;
        case "use":
          _gameService.UseItem(option);
          break;
        case "go":
          _gameService.Go(option);
          break;
        case "help":
          _gameService.Help();
          break;
        case "quit":
          _gameService.Quit();
          break;
        default:
          Console.WriteLine("You can't do that.");
          break;
      }
    }

    private void Print()
    {
      foreach (var message in _gameService.Messages)
      {
        Console.WriteLine(message);
      }
      _gameService.Messages.Clear();
    }

  }
}