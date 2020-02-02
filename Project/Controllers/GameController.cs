using System;
using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;
using ConsoleAdventure.Project.Models;

namespace ConsoleAdventure.Project.Controllers
{

  public class GameController : IGameController
  {
    private GameService _gameService = new GameService();

    //NOTE Makes sure everything is called to finish Setup and Starts the Game loop
    private bool _playing = true;
    public void Run()
    {
      Console.WriteLine("Welcome to Console Adventure!\nWhat's you're name?");
      string playerName = Console.ReadLine();
      _gameService.Setup(playerName);
      Console.Clear();

      Console.WriteLine("On a brisk dawn while hiking along the base of the Misty Mountains,you slip & fall into a pit! You then attempt to climb out but are unable to. With nothing but a firestarter kit, you must figure out how to escape before nightfall comes when the mountainside will be swarming with orcs!\n");
      Console.WriteLine("Type \"help\" at any time to view your options.\n");
      _gameService.CurrentRoom();
      while (_playing)
      {
        Print();
        GetUserInput();
      }
    }

    //NOTE Gets the user input, calls the appropriate command, and passes on the option if needed.
    public void GetUserInput()
    {
      Console.WriteLine("What would you like to do?");
      string input = Console.ReadLine().ToLower() + " ";
      string command = input.Substring(0, input.IndexOf(" "));
      string option = input.Substring(input.IndexOf(" ") + 1).Trim();
      //NOTE this will take the user input and parse it into a command and option.
      //IE: take silver key => command = "take" option = "silver key"
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

    //NOTE this should print your messages for the game.
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