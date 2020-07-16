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


    //bools
    public bool torchEquiped = false;
    public bool swordEquiped = false;
    public bool orcDefeated = false;


    //default messages
    public void RoomInfo()
    {
      Messages.Add($"You're {_game.CurrentRoom.Name}.");
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
      // SPECIAL TRAVEL

      //entering cave without a torch
      if (_game.CurrentRoom.Name == "in the pit" && direction == "east")
      {
        if (torchEquiped == false)
        {
          this.gameOver("You were attacked by an orc you couldn't see because you entered the dark cave without a light...");
        }
      }

      //walking into pit
      if (_game.CurrentRoom.Name == "two rooms deep" && direction == "south")
      {
        this.gameOver("You plummet down a chasm to your death...");
      }

      //BASIC TRAVEL
      if (_game.CurrentRoom.Exits.ContainsKey(direction))
      {
        _game.CurrentRoom = _game.CurrentRoom.Exits[direction];
        RoomInfo();
      }
      else
      {
        Messages.Add("Congratulations, you walked into a wall. Perhaps try another direction?");
      }
      // //confronting orc
      // if (_game.CurrentRoom.Name == "two rooms deep" && orcDefeated == false)
      // {
      //   if (swordEquiped == true)
      //   {
      //     Messages.Add("An orc runs at you with sword drawn! He attacks you but after a struggle, you defeat the him with your sword.");
      //     orcDefeated = true;
      //   }
      //   else
      //   {
      //     this.gameOver("You need more than a torch & your fists to defeat an armed orc. You have been slain...");
      //   }
      // }
    }

    public void Help()
    {
      Messages.Add("\nYou can type the following commands:\nlook - gives room description & lists room items\ntake [item name] - adds item to inventory\ninventory - shows items in inventory\nuse [item name] - uses item\ngo [north, south, east, or west] - makes you travel that direction\nquit - ends the game\n");
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
      this.gameOver("You gave in to despair & were found by orcs after nightfall...");
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
      RoomInfo();

      Item activeItem = _game.CurrentRoom.Items.Find(i => i.Name.ToLower() == itemName.ToLower());
      if (activeItem == null)
      {
        Messages.Add($"There's no {itemName.ToLower()} in this room.");
        return;
      }

      if (activeItem.Name.ToLower() == "torch")
      {
        torchEquiped = true;
      }

      if (activeItem.Name.ToLower() == "sword")
      {
        swordEquiped = true;
        Messages.Add("O-----{:::::::::::::::>");
      }

      _game.CurrentPlayer.AddToInventory(activeItem);
      _game.CurrentRoom.Items.Remove(activeItem);
      Messages.Add($"You add the {itemName.ToLower()} to your inventory.");

      if (activeItem.Name.ToLower() == "torch")
      {
        Messages.Add("You take the torch and light it with the some flit, steel, and char you found beside it.");
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
            this.gameOver($"You use the ladder to escape! Well done, {_game.CurrentPlayer.Name}!");
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

    public void gameOver(string msg)
    {
      Console.WriteLine(msg);
      Console.ForegroundColor = ConsoleColor.White;
      Environment.Exit(0);
    }
  }
}