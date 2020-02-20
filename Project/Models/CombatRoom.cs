namespace ConsoleAdventure.Project.Models
{
  class CombatRoom : Room
  {
    public bool Defeated { get; set; } = false;

    public CombatRoom(string name, string description) : base(name, description)
    {
    }
  }
}