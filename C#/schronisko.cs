using System;
using System.Collections.Generic;

namespace SchroniskoInteraktywne
{
    public class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public Volunteer Caretaker { get; set; }
        public bool IsAdopted { get; set; }

        public Animal(string name, string species, int age)
        {
            Name = name;
            Species = species;
            Age = age;
            Caretaker = null;
            IsAdopted = false;
        }

        public override string ToString()
        {
            string caretakerName = Caretaker != null ? Caretaker.Name : "Brak";
            string status = IsAdopted ? "Adoptowany" : "Czeka na dom";
            return $"[{Species}] {Name}, {Age} lat | Opiekun: {caretakerName} | Status: {status}";
        }
    }

    public class Volunteer
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Volunteer(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{Name} (Tel: {PhoneNumber})";
        }
    }

    public class Adoption
    {
        public Animal AnimalToAdopt { get; set; }
        public string AdopterName { get; set; }

        public Adoption(Animal animal, string adopterName)
        {
            AnimalToAdopt = animal;
            AdopterName = adopterName;
        }

        public void ProcessAdoption()
        {
            if (AnimalToAdopt.IsAdopted)
            {
                Console.WriteLine($"\n[BŁĄD] {AnimalToAdopt.Name} już został adoptowany!");
            }
            else
            {
                AnimalToAdopt.IsAdopted = true;
                Console.WriteLine($"\n[SUKCES] {AdopterName} adoptował(a) zwierzaka o imieniu {AnimalToAdopt.Name}.");
            }
        }
    }

    class Program
    {
        static List<Animal> animals = new List<Animal>();
        static List<Volunteer> volunteers = new List<Volunteer>();

        static void Main(string[] args)
        {
            // Dane startowe
            animals.Add(new Animal("Reksio", "Pies", 3));
            animals.Add(new Animal("Mruczek", "Kot", 1));
            volunteers.Add(new Volunteer("Anna Kowalska", "123-456-789"));

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== SYSTEM ZARZĄDZANIA SCHRONISKIEM ===");
                Console.WriteLine("1. Zarejestruj nowe zwierzę");
                Console.WriteLine("2. Zarejestruj nowego wolontariusza");
                Console.WriteLine("3. Przydziel opiekuna do zwierzęcia");
                Console.WriteLine("4. Przeprowadź proces adopcji");
                Console.WriteLine("5. Wyświetl listę zwierząt");
                Console.WriteLine("6. Wyjdź");
                Console.Write("Wybierz opcję: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n--- REJESTRACJA ZWIERZĘCIA ---");
                        Console.Write("Imię: "); string name = Console.ReadLine();
                        Console.Write("Gatunek: "); string species = Console.ReadLine();
                        Console.Write("Wiek: "); int age = int.Parse(Console.ReadLine());
                        animals.Add(new Animal(name, species, age));
                        Console.WriteLine("Dodano zwierzaka!");
                        break;

                    case "2":
                        Console.WriteLine("\n--- REJESTRACJA WOLONTARIUSZA ---");
                        Console.Write("Imię i nazwisko: "); string vName = Console.ReadLine();
                        Console.Write("Telefon: "); string phone = Console.ReadLine();
                        volunteers.Add(new Volunteer(vName, phone));
                        Console.WriteLine("Dodano wolontariusza!");
                        break;

                    case "3":
                        Console.WriteLine("\n--- PRZYDZIELANIE OPIEKUNA ---");
                        if (animals.Count == 0 || volunteers.Count == 0)
                        {
                            Console.WriteLine("Błąd: Brak zwierząt lub wolontariuszy w systemie.");
                            break;
                        }

                        for (int i = 0; i < animals.Count; i++)
                            Console.WriteLine($"{i}. {animals[i]}");
                        Console.Write("Wybierz numer zwierzęcia: ");
                        int aIdx = int.Parse(Console.ReadLine());

                        for (int i = 0; i < volunteers.Count; i++)
                            Console.WriteLine($"{i}. {volunteers[i]}");
                        Console.Write("Wybierz numer wolontariusza: ");
                        int vIdx = int.Parse(Console.ReadLine());

                        animals[aIdx].Caretaker = volunteers[vIdx];
                        Console.WriteLine("Pomyślnie przypisano opiekuna.");
                        break;

                    case "4":
                        Console.WriteLine("\n--- PROCES ADOPCJI ---");
                        List<Animal> available = animals.FindAll(a => !a.IsAdopted);
                        if (available.Count == 0)
                        {
                            Console.WriteLine("Brak zwierząt gotowych do adopcji.");
                            break;
                        }

                        for (int i = 0; i < available.Count; i++)
                            Console.WriteLine($"{i}. {available[i]}");
                        Console.Write("Wybierz numer zwierzęcia: ");
                        int adoptIdx = int.Parse(Console.ReadLine());

                        Console.Write("Imię i nazwisko adoptującego: ");
                        string adopter = Console.ReadLine();

                        Adoption adoption = new Adoption(available[adoptIdx], adopter);
                        adoption.ProcessAdoption();
                        break;

                    case "5":
                        Console.WriteLine("\n--- LISTA ZWIERZĄT W SCHRONISKU ---");
                        foreach (var animal in animals)
                            Console.WriteLine(animal);
                        break;

                    case "6":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Niepoprawny wybór!");
                        break;
                }
            }
        }
    }
}