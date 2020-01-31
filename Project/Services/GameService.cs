using System;
using System.Collections.Generic;
using ConsoleAdventure.Project.Interfaces;
using ConsoleAdventure.Project.Models;

namespace ConsoleAdventure.Project
{
  public class GameService : IGameService
  {
    private IGame _game { get; set; }
    public List<string> Messages { get; set; }
    public GameService()
    {
      _game = new Game();
      Messages = new List<string>();
    }

    //default messages
    public void CurrentRoom()
    {
      Console.WriteLine($"You're {_game.CurrentRoom.Name}.\n");
    }

    public void Go(string direction)
    {
      if (_game.CurrentRoom.Exits.ContainsKey(direction))
      {
        _game.CurrentRoom = _game.CurrentRoom.Exits[direction];
      }
      else
      {
        Console.WriteLine("Congratulations, you walked into a wall. Perhaps try another direction?");
      }

      // if (_game.CurrentPlayer.Inventory.Contains("Torch"))
      // {
      //   Messages.Add(_game.CurrentRoom.Description);
      // }
      // else
      // {
      //   Messages.Add("It's pitch black.");
      // }

      CurrentRoom();
      Console.WriteLine();
    }
    public void Help()
    {
      throw new System.NotImplementedException();
    }

    public void Inventory()
    {
      if (_game.CurrentPlayer.Inventory.Count > 0)
      {
        Console.Write("Inventory:\n");
        foreach (Item item in _game.CurrentPlayer.Inventory)
        {
          Console.WriteLine(item.Name);
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("Your inventory is empty.");
      }
      CurrentRoom();
      Console.WriteLine();
    }

    public void Look()
    {
      Messages.Add(_game.CurrentRoom.Description);
    }

    public void Quit()
    {
      Console.WriteLine("You gave in to despair & were found by orcs after nightfall...");
      Environment.Exit(0);
    }
    ///<summary>
    ///Restarts the game 
    ///</summary>
    public void Reset()
    {
      throw new System.NotImplementedException();
    }

    public void Setup(string playerName)
    {
      _game.CurrentPlayer.Name = playerName;
    }
    ///<summary>When taking an item be sure the item is in the current room before adding it to the player inventory, Also don't forget to remove the item from the room it was picked up in</summary>
    public void TakeItem(string itemName)
    {
      Item activeItem = _game.CurrentRoom.Items.Find(i => i.Name.ToLower() == itemName);
      if (activeItem == null)
      {
        Messages.Add($"No {itemName} in this room.");
        return;
      }
      _game.CurrentPlayer.AddToInventory(activeItem);
      _game.CurrentRoom.Items.Remove(activeItem);
      CurrentRoom();
    }
    ///<summary>
    ///No need to Pass a room since Items can only be used in the CurrentRoom
    ///Make sure you validate the item is in the room or player inventory before
    ///being able to use the item
    ///</summary>
    public void UseItem(string itemName)
    {
      switch (itemName)
      {
        case "ladder":
          if (_game.CurrentRoom.Name == "in the pit")
          {
            Messages.Add($"You use the ladder to escape! Well done, {_game.CurrentPlayer.Name}!");
          }
          else
          {
            Messages.Add("You can't find a good use for that in this room.\n");
          }
          break;
        default:
          Messages.Add("You can't use that.\n");
          break;
      }
    }
  }
}