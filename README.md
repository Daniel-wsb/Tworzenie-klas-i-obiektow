# Tworzenie-klas-i-obiektów

System Zarządzania Schroniskiem dla Zwierząt (Python & C#)

Projekt przedstawia implementację obiektową (OOP) prostego systemu obsługi schroniska dla zwierząt. Program został napisany w dwóch wersjach językowych: **Python** oraz **C#**.

## Funkcje programu
Aplikacja działa w trybie konsolowym i oferuje interaktywne menu wyboru, które pozwala użytkownikowi na pełne sterowanie procesem:
1. **Rejestracja zwierząt** – dodawanie nowych podopiecznych (imię, gatunek, wiek).
2. **Rejestracja wolontariuszy** – tworzenie profili opiekunów z danymi kontaktowymi.
3. **Przydzielanie opiekunów** – dynamiczne łączenie wolontariusza z konkretnym zwierzęciem.
4. **Proces adopcji** – przypisywanie zwierzęcia do nowego właściciela, automatyczna zmiana statusu i blokada ponownej adopcji.
5. **Podgląd stanu** – wyświetlanie aktualnej listy zwierząt wraz z ich opiekunami i statusem.

## Architektura projektu
Logika aplikacji została oparta na trzech powiązanych ze sobą klasach:
* `Animal` – reprezentuje zwierzę w schronisku.
* `Volunteer` – reprezentuje wolontariusza/opiekuna.
* `Adoption` – klasa odpowiedzialna za przeprowadzenie i walidację transakcji adopcyjnej.
