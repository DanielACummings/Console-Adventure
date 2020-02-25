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
    public bool running { get; private set; }

    public GameService()
    {
      _game = new Game();
      Messages = new List<string>();
    }

    //default messages
    public void RoomInfo()
    {
      Console.WriteLine($"You're {_game.CurrentRoom.Name}.");
      if (_game.CurrentRoom.Name == "in the pit")
      {
        Messages.Add(_game.CurrentRoom.Description);
      }
      else if (torchEquiped == true)
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
      //walking into pit
      if (_game.CurrentRoom.Name == "two rooms deep" && direction == "south")
      {
        Console.WriteLine("You plummet down the chasm to your death...");
        Console.ForegroundColor = ConsoleColor.White;
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

      //confronting orc
      if (_game.CurrentRoom.Name == "one room deep" && orcDefeated == false)
      {
        confrontOrcWithoutTorch();
      }
      else
      {
        RoomInfo();
        Console.WriteLine();
      }

      if (_game.CurrentRoom.Name == "two rooms deep" && orcDefeated == false)
      {
        if (swordEquiped == true)
        {
          Messages.Add("An orc runs at you with sword drawn! He attacks you but after a struggle, you defeat the him with your sword.");
          orcDefeated = true;
        }
        else
        {
          Messages.Add("You need more than a torch &your fists to defeat an armed orc. You are slain in battle.");
          Console.ForegroundColor = ConsoleColor.White;
          Environment.Exit(0);
        }
      }
    }

    public void Help()
    {
      Messages.Add("\nYou can type the following commands:\nlook - gives room description & lists room items\ntake [item name] - adds item to inventory\ninventory - shows items in inventory\n use [item name] - uses item\ngo [north, south, east, or west] - makes you travel that direction\nquit - ends the game\n");
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
      Console.WriteLine();
    }

    public void Look()
    {
      RoomInfo();
      //shows items in cave rooms if torch is equipped
      if (torchEquiped == false && _game.CurrentRoom.Name == "in the pit")
      {
        Messages.Add("You spot a torch on the ground.");
      };

      if (torchEquiped == true)
      {
        foreach (var item in _game.CurrentRoom.Items)
        {
          Messages.Add($"You spot a {item.Name.ToLower()}! {item.Description}");
        }
      }
    }

    public void Quit()
    {
      Console.WriteLine("You gave in to despair & were found by orcs after nightfall...");
      Console.ForegroundColor = ConsoleColor.White;
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
      Item activeItem = _game.CurrentRoom.Items.Find(i => i.Name.ToLower() == itemName.ToLower());
      if (activeItem == null)
      {
        Messages.Add($"There's no {itemName.ToLower()} in this room.");
        return;
      }

      if (activeItem.Name.ToString() == "Torch")
      {
        torchEquiped = true;
      }

      if (activeItem.Name.ToString() == "Sword")
      {
        swordEquiped = true;
        Messages.Add("O-----{:::::::::::::::>");
      }

      _game.CurrentPlayer.AddToInventory(activeItem);
      _game.CurrentRoom.Items.Remove(activeItem);
      Messages.Add($"You added the {itemName.ToLower()} to your inventory.");

      if (activeItem.Name.ToLower() == "torch")
      {
        Messages.Add("You use your firestarter kit to light the torch.");
      };
    }

    //# region
    ///<summary>
    ///No need to Pass a room since Items can only be used in the CurrentRoom
    ///Make sure you validate the item is in the room or player inventory before
    ///being able to use the item
    ///</summary>
    //# endregion
    public void UseItem(string itemName)
    {
      switch (itemName)
      {
        case "ladder":
          if (_game.CurrentRoom.Name == "in the pit")
          {
            Messages.Add($"You use the ladder to escape! Well done, {_game.CurrentPlayer.Name}!");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(0);
            // change to running = false;
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

    //bools
    public bool torchEquiped = false;
    public bool swordEquiped = false;
    public bool orcDefeated = false;

    public void confrontOrcWithoutTorch()
    {
      if (torchEquiped == false)
      {
        Console.WriteLine("You enter the cave without any light & an orc attacks & kills you!");
        Console.ForegroundColor = ConsoleColor.White;
        Environment.Exit(0);
      }
    }
  }
}