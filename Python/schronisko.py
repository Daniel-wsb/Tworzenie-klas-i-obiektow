
class Animal:
    def __init__(self, name: str, species: str, age: int):
        self.name = name
        self.species = species
        self.age = age
        self.caretaker = None
        self.is_adopted = False

    def __str__(self):
        caretaker_name = self.caretaker.name if self.caretaker else "Brak"
        status = "Adoptowany" if self.is_adopted else "Czeka na dom"
        return f"[{self.species}] {self.name}, {self.age} lat | Opiekun: {caretaker_name} | Status: {status}"


class Volunteer:
    def __init__(self, name: str, phone_number: str):
        self.name = name
        self.phone_number = phone_number

    def __str__(self):
        return f"{self.name} (Tel: {self.phone_number})"


class Adoption:
    def __init__(self, animal: Animal, adopter_name: str):
        self.animal = animal
        self.adopter_name = adopter_name

    def process_adoption(self):
        if self.animal.is_adopted:
            print(f"\n[BŁĄD] {self.animal.name} został już wcześniej adoptowany!")
            return False
        else:
            self.animal.is_adopted = True
            print(f"\n[SUKCES] {self.adopter_name} adoptował(a) zwierzaka: {self.animal.name}!")
            return True


# Tablice na dane wprowadzane przez użytkownika
animals = []
volunteers = []

def show_menu():
    print("\n=== SYSTEM ZARZĄDZANIA SCHRONISKIEM ===")
    print("1. Zarejestruj nowe zwierzę")
    print("2. Zarejestruj nowego wolontariusza")
    print("3. Przydziel opiekuna (wolontariusza) do zwierzęcia")
    print("4. Przeprowadź proces adopcji")
    print("5. Wyświetl listę zwierząt")
    print("6. Wyjdź z programu")
    return input("Wybierz opcję (1-6): ")

def main():
    # Dane startowe, żeby nie zaczynać od pustego programu
    animals.append(Animal("Reksio", "Pies", 3))
    animals.append(Animal("Mruczek", "Kot", 1))
    volunteers.append(Volunteer("Anna Kowalska", "123-456-789"))

    while True:
        choice = show_menu()

        if choice == "1":
            print("\n--- REJESTRACJA ZWIERZĘCIA ---")
            name = input("Podaj imię: ")
            species = input("Podaj gatunek (np. Pies, Kot): ")
            age = int(input("Podaj wiek: "))
            animals.append(Animal(name, species, age))
            print(f"Dodano zwierzę {name}!")

        elif choice == "2":
            print("\n--- REJESTRACJA WOLONTARIUSZA ---")
            name = input("Podaj imię i nazwisko wolontariusza: ")
            phone = input("Podaj numer telefonu: ")
            volunteers.append(Volunteer(name, phone))
            print(f"Dodano wolontariusza {name}!")

        elif choice == "3":
            print("\n--- PRZYDZIELANIE OPIEKUNA ---")
            if not animals or not volunteers:
                print("Błąd: Musisz mieć zarejestrowane min. 1 zwierzę i 1 wolontariusza!")
                continue
            
            # Wybór zwierzęcia
            for i, anim in enumerate(animals):
                print(f"{i}. {anim}")
            animal_idx = int(input("Wybierz numer zwierzęcia: "))

            # Wybór wolontariusza
            for i, vol in enumerate(volunteers):
                print(f"{i}. {vol}")
            vol_idx = int(input("Wybierz numer wolontariusza: "))

            # Przypisanie
            animals[animal_idx].caretaker = volunteers[vol_idx]
            print(f"Przypisano opiekuna {volunteers[vol_idx].name} do {animals[animal_idx].name}!")

        elif choice == "4":
            print("\n--- PROCES ADOPCJI ---")
            available_animals = [a for a in animals if not a.is_adopted]
            if not available_animals:
                print("Brak zwierząt dostępnych do adopcji.")
                continue

            for i, anim in enumerate(available_animals):
                print(f"{i}. {anim}")
            anim_idx = int(input("Wybierz numer zwierzęcia do adopcji: "))
            
            adopter = input("Podaj imię i nazwisko adoptującego: ")
            
            # Uruchomienie klasy adopcji
            adoption = Adoption(available_animals[anim_idx], adopter)
            adoption.process_adoption()

        elif choice == "5":
            print("\n--- LISTA ZWIERZĄT W SCHRONISKU ---")
            if not animals:
                print("Schronisko jest puste.")
            for anim in animals:
                print(anim)

        elif choice == "6":
            print("Zamykanie programu. Do zobaczenia!")
            break
        else:
            print("Niepoprawna opcja, spróbuj ponownie.")

if __name__ == "__main__":
    main()