using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();
    
        Console.Write("Enter room description: ");
        var description = Console.ReadLine();
    
        var room = new Room
        {
            Name = name,
            Description = description
        };
    
        _context.Rooms.Add(room);
        _context.SaveChanges();
    
        Console.WriteLine($"Room '{name}' added to the game.");
    }
    
    public void AddCharacter()
    {
        var characters = _context.Characters.ToList();
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();
        bool found = false;
    
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();
    
        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine());
    
        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());
    
        var character = new Character
        {
            Name = name,
            Level = level,
            RoomId = roomId
        };
    
        // Moves through the rooms list to find the right ID
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].Id == roomId)
            {
                found = true;
            }
        }
    
    
        // Final results
        if (found)
        {
            _context.Characters.Add(character);
            rooms[roomId - 1].Characters.Add(character);
            _context.SaveChanges();
        }
        else 
        {
            Console.WriteLine("The room you selected does not exist.");
        }
    }
    
    public void FindCharacter()
    {
        var characters = _context.Characters.ToList();
    
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();
    
        try
        {
            // LINQ search for right name
            var result = characters.Where(c => c.Name.ToLower() == name.ToLower()).Select(c => c).First();
            if (result != null)
            {
                var character = result;
                Console.WriteLine($"Character ID: {character.Id}\nName: {character.Name}\nLevel: {character.Level}\nRoom ID: {character.RoomId}");
            }
            else
            {
                Console.WriteLine($"There is no character by the name of {name}.");
            }
        }
        catch (InvalidOperationException e)
        {
            // Don't know why it's catching an exception, but here's the code for catching it
            Console.WriteLine($"There is no character by the name of {name}.");
        }
    }
}
