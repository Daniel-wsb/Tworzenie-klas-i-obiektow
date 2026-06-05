using System;
using System.Collections.Generic;
using System.Linq;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public int Age { get; set; }
    public bool Adopted { get; set; }
    public int? VolunteerId { get; set; }

    public override string ToString()
    {
        string guardian = VolunteerId.HasValue ? $"opiekun ID {VolunteerId.Value}" : "brak opiekuna";
        string status = Adopted ? "zaadoptowane" : "dostępne";
        return $"[{Id}] {Name} ({Species}, {Age} lat) | {status} | {guardian}";
    }
}

public class Volunteer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public List<int> AssignedAnimals { get; set; } = new List<int>();

    public override string ToString()
    {
        string animals = AssignedAnimals.Count > 0 ? string.Join(", ", AssignedAnimals) : "brak";
        return $"[{Id}] {Name}, tel. {Phone} | zwierzęta: {animals}";
    }
}

public class Adoption
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public string AnimalName { get; set; } = "";
    public string AdopterName { get; set; } = "";
    public string Contact { get; set; } = "";
    public string ProcessedBy { get; set; } = "";
    public DateTime Date { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Date:yyyy-MM-dd HH:mm} | {AnimalName} (ID {AnimalId}) | adoptujący: {AdopterName}, kontakt: {Contact}, obsłużył: {ProcessedBy}";
    }
}

public class Shelter
{
    private readonly List<Animal> animals = new List<Animal>();
    private readonly List<Volunteer> volunteers = new List<Volunteer>();
    private readonly List<Adoption> adoptions = new List<Adoption>();

    private int nextAnimalId = 1;
    private int nextVolunteerId = 1;
    private int nextAdoptionId = 1;

    public void AddAnimal(string name, string species, int age)
    {
        animals.Add(new Animal
        {
            Id = nextAnimalId++,
            Name = name,
            Species = species,
            Age = age,
            Adopted = false,
            VolunteerId = null
        });
    }

    public void AddVolunteer(string name, string phone)
    {
        volunteers.Add(new Volunteer
        {
            Id = nextVolunteerId++,
            Name = name,
            Phone = phone
        });
    }

    private Animal? GetAnimal(int id) => animals.FirstOrDefault(a => a.Id == id);
    private Volunteer? GetVolunteer(int id) => volunteers.FirstOrDefault(v => v.Id == id);

    public string AssignVolunteer(int animalId, int volunteerId)
    {
        Animal? animal = GetAnimal(animalId);
        Volunteer? volunteer = GetVolunteer(volunteerId);

        if (animal == null) return "Nie znaleziono zwierzęcia.";
        if (volunteer == null) return "Nie znaleziono wolontariusza.";
        if (animal.Adopted) return "Nie można przypisać opiekuna do zwierzęcia po adopcji.";

        animal.VolunteerId = volunteer.Id;
        if (!volunteer.AssignedAnimals.Contains(animal.Id))
        {
            volunteer.AssignedAnimals.Add(animal.Id);
        }

        return "Opiekun został przypisany.";
    }

    public string AdoptAnimal(int animalId, string adopterName, string contact)
    {
        Animal? animal = GetAnimal(animalId);

        if (animal == null) return "Nie znaleziono zwierzęcia.";
        if (animal.Adopted) return "To zwierzę jest już adoptowane.";

        string processedBy = "system";
        if (animal.VolunteerId.HasValue)
        {
            Volunteer? volunteer = GetVolunteer(animal.VolunteerId.Value);
            if (volunteer != null)
            {
                processedBy = volunteer.Name;
            }
        }

        animal.Adopted = true;

        adoptions.Add(new Adoption
        {
            Id = nextAdoptionId++,
            AnimalId = animal.Id,
            AnimalName = animal.Name,
            AdopterName = adopterName,
            Contact = contact,
            ProcessedBy = processedBy,
            Date = DateTime.Now
        });

        return $"Zwierzę '{animal.Name}' zostało zaadoptowane.";
    }

    public void ShowAnimals()
    {
        if (animals.Count == 0)
        {
            Console.WriteLine("Brak zwierząt.");
            return;
        }

        foreach (Animal animal in animals)
        {
            Console.WriteLine(animal);
        }
    }

    public void ShowVolunteers()
    {
        if (volunteers.Count == 0)
        {
            Console.WriteLine("Brak wolontariuszy.");
            return;
        }

        foreach (Volunteer volunteer in volunteers)
        {
            Console.WriteLine(volunteer);
        }
    }

    public void ShowAdoptions()
    {
        if (adoptions.Count == 0)
        {
            Console.WriteLine("Brak adopcji.");
            return;
        }

        foreach (Adoption adoption in adoptions)
        {
            Console.WriteLine(adoption);
        }
    }
}

public class Program
{
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Podaj poprawną liczbę całkowitą.");
        }
    }

    public static void Main()
    {
        Shelter shelter = new Shelter();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== SCHRONISKO DLA ZWIERZĄT ===");
            Console.WriteLine("1. Dodaj zwierzę");
            Console.WriteLine("2. Dodaj wolontariusza");
            Console.WriteLine("3. Pokaż zwierzęta");
            Console.WriteLine("4. Pokaż wolontariuszy");
            Console.WriteLine("5. Przydziel opiekuna do zwierzęcia");
            Console.WriteLine("6. Przeprowadź adopcję");
            Console.WriteLine("7. Pokaż adopcje");
            Console.WriteLine("0. Wyjście");

            Console.Write("Wybierz opcję: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Imię zwierzęcia: ");
                    string animalName = Console.ReadLine() ?? "";
                    Console.Write("Gatunek: ");
                    string species = Console.ReadLine() ?? "";
                    int age = ReadInt("Wiek: ");
                    shelter.AddAnimal(animalName, species, age);
                    Console.WriteLine("Dodano zwierzę.");
                    break;

                case "2":
                    Console.Write("Imię i nazwisko wolontariusza: ");
                    string volunteerName = Console.ReadLine() ?? "";
                    Console.Write("Telefon: ");
                    string phone = Console.ReadLine() ?? "";
                    shelter.AddVolunteer(volunteerName, phone);
                    Console.WriteLine("Dodano wolontariusza.");
                    break;

                case "3":
                    shelter.ShowAnimals();
                    break;

                case "4":
                    shelter.ShowVolunteers();
                    break;

                case "5":
                    int animalIdForVolunteer = ReadInt("ID zwierzęcia: ");
                    int volunteerId = ReadInt("ID wolontariusza: ");
                    Console.WriteLine(shelter.AssignVolunteer(animalIdForVolunteer, volunteerId));
                    break;

                case "6":
                    int animalIdForAdoption = ReadInt("ID zwierzęcia: ");
                    Console.Write("Imię i nazwisko adoptującego: ");
                    string adopterName = Console.ReadLine() ?? "";
                    Console.Write("Kontakt: ");
                    string contact = Console.ReadLine() ?? "";
                    Console.WriteLine(shelter.AdoptAnimal(animalIdForAdoption, adopterName, contact));
                    break;

                case "7":
                    shelter.ShowAdoptions();
                    break;

                case "0":
                    Console.WriteLine("Koniec programu.");
                    return;

                default:
                    Console.WriteLine("Niepoprawna opcja.");
                    break;
            }
        }
    }
}