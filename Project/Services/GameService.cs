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
    public void RoomInfo()
    {
      Console.WriteLine($"You're {_game.CurrentRoom.Name}.");
      if (torchEquiped == true)
      {
        Messages.Add(_game.CurrentRoom.Description);
      }
      else
      {
        Messages.Add("It's pitch black.");
      };
    }

    public void Go(string direction)
    {
      //special circumstance travel
      if (_game.CurrentRoom.Name == "two rooms deep" && direction == "south")
      {
        Console.WriteLine("You plummet down the chasm to your death...");
        Environment.Exit(0);
      }

      //basic travel
      if (_game.CurrentRoom.Exits.ContainsKey(direction))
      {
        _game.CurrentRoom = _game.CurrentRoom.Exits[direction];
      }
      else
      {
        Messages.Add("Congratulations, you walked into a wall. Perhaps try another direction?");
      }

      RoomInfo();
      Console.WriteLine();
    }

    public void Help()
    {
      RoomInfo();
      Messages.Add("\nYou can type the following commands:\nlook - gives room description\ntake [item name] - adds item to inventory\ninventory - shows items in inventory\n use [item name] - uses item\ngo [north, south, east, or west] - makes you travel that direction\nquit - ends the game\n");
    }

    public void Inventory()
    {
      if (_game.CurrentPlayer.Inventory.Count > 0)
      {
        Console.Write("Inventory:\n");
        foreach (Item item in _game.CurrentPlayer.Inventory)
        {
          Console.WriteLine($"{item.Name} - {item.Description}");
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("Your inventory is empty.");
      }
      RoomInfo();
      Console.WriteLine();
    }

    public void Look()
    {
      RoomInfo();
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

      if (activeItem.Name.ToString() == "Torch")
      {
        torchEquiped = true;
      }

      if (activeItem.Name.ToString() == "Sword")
      {
        swordEquiped = true;
      }

      _game.CurrentPlayer.AddToInventory(activeItem);
      _game.CurrentRoom.Items.Remove(activeItem);
      RoomInfo();

    }

    public bool torchEquiped = false;
    public bool swordEquiped = false;

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