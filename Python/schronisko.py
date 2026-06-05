from dataclasses import dataclass
from datetime import datetime
from typing import Optional, List


@dataclass
class Animal:
    id: int
    name: str
    species: str
    age: int
    adopted: bool = False
    volunteer_id: Optional[int] = None

    def __str__(self) -> str:
        guardian = f"opiekun ID {self.volunteer_id}" if self.volunteer_id else "brak opiekuna"
        status = "zaadoptowane" if self.adopted else "dostępne"
        return f"[{self.id}] {self.name} ({self.species}, {self.age} lat) | {status} | {guardian}"


@dataclass
class Volunteer:
    id: int
    name: str
    phone: str
    assigned_animals: List[int]

    def __str__(self) -> str:
        animals = ", ".join(map(str, self.assigned_animals)) if self.assigned_animals else "brak"
        return f"[{self.id}] {self.name}, tel. {self.phone} | zwierzęta: {animals}"


@dataclass
class Adoption:
    id: int
    animal_id: int
    animal_name: str
    adopter_name: str
    contact: str
    processed_by: str
    date: str

    def __str__(self) -> str:
        return (
            f"[{self.id}] {self.date} | {self.animal_name} (ID {self.animal_id}) "
            f"| adoptujący: {self.adopter_name}, kontakt: {self.contact}, "
            f"obsłużył: {self.processed_by}"
        )


class Shelter:
    def __init__(self) -> None:
        self.animals: List[Animal] = []
        self.volunteers: List[Volunteer] = []
        self.adoptions: List[Adoption] = []
        self.next_animal_id = 1
        self.next_volunteer_id = 1
        self.next_adoption_id = 1

    def add_animal(self, name: str, species: str, age: int) -> None:
        self.animals.append(Animal(self.next_animal_id, name, species, age))
        self.next_animal_id += 1

    def add_volunteer(self, name: str, phone: str) -> None:
        self.volunteers.append(Volunteer(self.next_volunteer_id, name, phone, []))
        self.next_volunteer_id += 1

    def get_animal(self, animal_id: int) -> Optional[Animal]:
        return next((a for a in self.animals if a.id == animal_id), None)

    def get_volunteer(self, volunteer_id: int) -> Optional[Volunteer]:
        return next((v for v in self.volunteers if v.id == volunteer_id), None)

    def assign_volunteer(self, animal_id: int, volunteer_id: int) -> str:
        animal = self.get_animal(animal_id)
        volunteer = self.get_volunteer(volunteer_id)

        if animal is None:
            return "Nie znaleziono zwierzęcia."
        if volunteer is None:
            return "Nie znaleziono wolontariusza."
        if animal.adopted:
            return "Nie można przypisać opiekuna do zwierzęcia po adopcji."

        animal.volunteer_id = volunteer.id
        if animal.id not in volunteer.assigned_animals:
            volunteer.assigned_animals.append(animal.id)
        return "Opiekun został przypisany."

    def adopt_animal(self, animal_id: int, adopter_name: str, contact: str) -> str:
        animal = self.get_animal(animal_id)
        if animal is None:
            return "Nie znaleziono zwierzęcia."
        if animal.adopted:
            return "To zwierzę jest już adoptowane."

        processed_by = "system"
        if animal.volunteer_id is not None:
            volunteer = self.get_volunteer(animal.volunteer_id)
            if volunteer:
                processed_by = volunteer.name

        animal.adopted = True
        adoption = Adoption(
            self.next_adoption_id,
            animal.id,
            animal.name,
            adopter_name,
            contact,
            processed_by,
            datetime.now().strftime("%Y-%m-%d %H:%M")
        )
        self.adoptions.append(adoption)
        self.next_adoption_id += 1
        return f"Zwierzę '{animal.name}' zostało zaadoptowane."

    def show_animals(self) -> None:
        if not self.animals:
            print("Brak zwierząt.")
            return
        for animal in self.animals:
            print(animal)

    def show_volunteers(self) -> None:
        if not self.volunteers:
            print("Brak wolontariuszy.")
            return
        for volunteer in self.volunteers:
            print(volunteer)

    def show_adoptions(self) -> None:
        if not self.adoptions:
            print("Brak adopcji.")
            return
        for adoption in self.adoptions:
            print(adoption)


def read_int(prompt: str) -> int:
    while True:
        try:
            return int(input(prompt))
        except ValueError:
            print("Podaj poprawną liczbę całkowitą.")


def main() -> None:
    shelter = Shelter()

    while True:
        print("\n=== SCHRONISKO DLA ZWIERZĄT ===")
        print("1. Dodaj zwierzę")
        print("2. Dodaj wolontariusza")
        print("3. Pokaż zwierzęta")
        print("4. Pokaż wolontariuszy")
        print("5. Przydziel opiekuna do zwierzęcia")
        print("6. Przeprowadź adopcję")
        print("7. Pokaż adopcje")
        print("0. Wyjście")

        choice = input("Wybierz opcję: ").strip()

        if choice == "1":
            name = input("Imię zwierzęcia: ")
            species = input("Gatunek: ")
            age = read_int("Wiek: ")
            shelter.add_animal(name, species, age)
            print("Dodano zwierzę.")

        elif choice == "2":
            name = input("Imię i nazwisko wolontariusza: ")
            phone = input("Telefon: ")
            shelter.add_volunteer(name, phone)
            print("Dodano wolontariusza.")

        elif choice == "3":
            shelter.show_animals()

        elif choice == "4":
            shelter.show_volunteers()

        elif choice == "5":
            animal_id = read_int("ID zwierzęcia: ")
            volunteer_id = read_int("ID wolontariusza: ")
            print(shelter.assign_volunteer(animal_id, volunteer_id))

        elif choice == "6":
            animal_id = read_int("ID zwierzęcia: ")
            adopter_name = input("Imię i nazwisko adoptującego: ")
            contact = input("Kontakt: ")
            print(shelter.adopt_animal(animal_id, adopter_name, contact))

        elif choice == "7":
            shelter.show_adoptions()

        elif choice == "0":
            print("Koniec programu.")
            break

        else:
            print("Niepoprawna opcja.")


if __name__ == "__main__":
    main()