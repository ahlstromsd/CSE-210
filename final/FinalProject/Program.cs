using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Item class representing a collectible item in the game
class Item
{
    public string Name { get; set; }
    public int AttackBoost { get; set; }
    public int DefenseBoost { get; set; }
    public int HealthBoost { get; set; }

    public Item(string name, int attackBoost, int defenseBoost, int healthBoost)
    {
        Name = name;
        AttackBoost = attackBoost;
        DefenseBoost = defenseBoost;
        HealthBoost = healthBoost;
    }
}

// Monster class representing a monster in the game
class Monster
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }

    public Monster(string name, int health, int damage)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public void Attack(Player player)
    {
        player.Health -= Damage - player.Defense;
        Console.WriteLine($"{Name} attacks you for {Damage - player.Defense} damage!");
    }
}

// Player class representing the player character
class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    private List<Item> Inventory { get; set; }

    public Player(string name)
    {
        Name = name;
        Health = 100;
        Attack = 10;
        Defense = 0;
        Inventory = new List<Item>();
    }

    public void PickUp(Item item)
    {
        Inventory.Add(item);
        Attack += item.AttackBoost;
        Defense += item.DefenseBoost;
        Health += item.HealthBoost;
        Console.WriteLine($"You picked up {item.Name}!");
    }

    public void UsePotion()
    {
        Health = 100;
        Console.WriteLine("You used a potion and restored your health to 100!");
    }

    public bool HasPotion()
    {
        foreach (var item in Inventory)
        {
            if (item.Name.ToLower().Contains("potion"))
            {
                return true;
            }
        }
        return false;
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"Player: {Name} | Health: {Health} | Attack: {Attack} | Defense: {Defense}");
    }

    public void DisplayInventory()
    {
        if (Inventory.Count == 0)
        {
            Console.WriteLine("You have no items in your inventory.");
            return;
        }
        Console.WriteLine("Inventory:");
        foreach (var item in Inventory)
        {
            Console.WriteLine($"- {item.Name}");
        }
    }
}

// Room class representing a room in the dungeon
class Room
{
    public string Description { get; set; }
    public Monster Monster { get; private set; }
    public List<Item> Items { get; private set; }
    public Room InnerRoom { get; set; }

    public Room(string description, Monster monster = null, List<Item> items = null, Room innerRoom = null)
    {
        Description = description;
        Monster = monster;
        Items = items ?? new List<Item>();
        InnerRoom = innerRoom;
    }

    public void DisplayRoomDetails()
    {
        Console.WriteLine(Description);
        if (Monster != null)
        {
            Console.WriteLine($"There is a {Monster.Name} here!");
        }
        if (Items.Count > 0)
        {
            Console.WriteLine("Items in this room:");
            foreach (var item in Items)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
    }

    public void RemoveMonster()
    {
        Monster = null;
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}

// Dungeon class representing the entire dungeon
class Dungeon
{
    private List<Room> Rooms { get; set; }
    private Player Player { get; set; }
    private Room CurrentRoom { get; set; }
    private bool HasLookedAround { get; set; }

    public Dungeon(Player player)
    {
        Player = player;
        Rooms = new List<Room>
        {
            new Room("Room 1: A dimly lit room with cobwebs everywhere.", new Monster("Goblin", 30, 10)),
            new Room("Room 2: A dusty library with ancient books.", null, new List<Item> { new Item("Shield", 0, 10, 0) }),
            new Room("Room 3: A cold room with an eerie silence.", new Monster("Zombie", 40, 15)),
            new Room("Room 4: A brightly lit room with high ceilings.", new Monster("Dragon", 100, 20))
        };

        // Add an inner room with a sword to room 1
        Rooms[0].InnerRoom = new Room("Inner Room: A small armory with a shining sword.", null, new List<Item> { new Item("Sword", 10, 0, 0) });
        CurrentRoom = Rooms[0];

        Rooms[2].InnerRoom = new Room("Inner Room: A small closet with broken bottles on shelves. Only one is intact, it's a health potion!", null, new List<Item> { new Item("Health Potion", 0, 0, 30)});
        CurrentRoom = Rooms[2];

        Rooms[3].InnerRoom = new Room("Inner Room: A room filled with gold and jewels. You've won the game!");
        CurrentRoom = Rooms[3];
    }
    

//FIX ABOVE^^^ add inner room to room 3 with health potion. also add health potion to items class











    private void MovePlayer(int roomIndex)
    {
        CurrentRoom = Rooms[roomIndex];
        HasLookedAround = false;
        EnterRoom();
    }

    private void EnterRoom()
    {
        Console.WriteLine($"\nYou enter {CurrentRoom.Description}");

        while (Player.Health > 0)
        {
            Player.DisplayStatus();
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Look around");
            if (HasLookedAround)
            {
                Console.WriteLine("2. Pick up item");
            }
            else
            {
                Console.WriteLine("2. (Look around first to see items)");
            }
            Console.WriteLine("3. Attack monster");
            Console.WriteLine("4. Enter inner room");
            Console.WriteLine("5. Exit the room");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CurrentRoom.DisplayRoomDetails();
                    HasLookedAround = true;
                    break;

                case "2":
                    if (HasLookedAround && CurrentRoom.Items.Count > 0)
                    {
                        Console.WriteLine("Which item would you like to pick up?");
                        for (int i = 0; i < CurrentRoom.Items.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {CurrentRoom.Items[i].Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= CurrentRoom.Items.Count)
                        {
                            var item = CurrentRoom.Items[itemIndex - 1];
                            Player.PickUp(item);
                            CurrentRoom.RemoveItem(item);
                        }
                        else
                        {
                            Console.WriteLine("Invalid item index.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You need to look around first or there are no items to pick up.");
                    }
                    break;

                case "3":
                    if (CurrentRoom.Monster != null)
                    {
                        Player.DisplayStatus();
                        Console.WriteLine("You attack the monster!");
                        CurrentRoom.Monster.Health -= Player.Attack;
                        Console.WriteLine($"You dealt {Player.Attack} damage to the {CurrentRoom.Monster.Name}!");

                        if (CurrentRoom.Monster.Health > 0)
                        {
                            CurrentRoom.Monster.Attack(Player);
                        }
                        else
                        {
                            Console.WriteLine($"You have defeated the {CurrentRoom.Monster.Name}!");
                            CurrentRoom.RemoveMonster();
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no monsters to attack.");
                    }
                    break;

                case "4":
                    if ((CurrentRoom.InnerRoom != null) && (CurrentRoom.Monster == null))
                    {
                        Console.WriteLine("You discover an inner room!");
                        EnterInnerRoom(CurrentRoom.InnerRoom);
                    }
                    else
                    {
                        Console.WriteLine("There is no inner room to enter or you need to defeat the monster first.");
                    }
                    break;

                case "5":
                    Console.WriteLine("You exit the room.");
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            if (Player.Health <= 0)
            {
                Console.WriteLine("You have died. Game over.");
                Environment.Exit(0);
            }
            else if (CurrentRoom == Rooms[3].InnerRoom)
            {
                Console.WriteLine("You have entered the inner room filled with gold and jewels. You have won the game!");
                Environment.Exit(0);
            }
        }
    }

    private void EnterInnerRoom(Room innerRoom)
    {
        Console.WriteLine("You enter the inner room...");
        innerRoom.DisplayRoomDetails();

        while (Player.Health > 0)
        {
            Player.DisplayStatus();
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Pick up item");
            Console.WriteLine("2. Exit inner room");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (innerRoom.Items.Count > 0)
                    {
                        Console.WriteLine("Which item would you like to pick up?");
                        for (int i = 0; i < innerRoom.Items.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {innerRoom.Items[i].Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= innerRoom.Items.Count)
                        {
                            var item = innerRoom.Items[itemIndex - 1];
                            Player.PickUp(item);
                            innerRoom.RemoveItem(item);
                        }
                        else
                        {
                            Console.WriteLine("Invalid item index.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no items to pick up.");
                    }
                    break;

                case "2":
                    Console.WriteLine("You exit the inner room.");
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            if (Player.Health <= 0)
            {
                Console.WriteLine("You have died. Game over.");
                Environment.Exit(0);
            }
        }
    }

    public void Play()
    {
        Console.WriteLine("Welcome to the Dungeon Explorer Game!");
        while (true)
        {
            Player.DisplayStatus();
            Console.WriteLine("Choose a room to enter:");
            for (int i = 0; i < Rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Rooms[i].Description}");
            }

            if (int.TryParse(Console.ReadLine(), out int roomIndex) && roomIndex > 0 && roomIndex <= Rooms.Count)
            {
                MovePlayer(roomIndex - 1);
            }
            else
            {
                Console.WriteLine("Invalid room index.");
            }
        }
    }
}

// Main program class
class Program
{
    static void Main(string[] args)
    {
        var player = new Player("Adventurer");
        var dungeon = new Dungeon(player);
        dungeon.Play();
    }
}