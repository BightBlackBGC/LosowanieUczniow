using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LosowanieUcznia
{
    public partial class MainPage : ContentPage
    {
        private int initialRandomNumber; // Zmienna przechowująca wylosowany szczesliwy numerek
        private List<Person> peopleList; // Lista przechowująca dane o osobach

        public MainPage()
        {
            InitializeComponent();

            // Inicjalizacja szczesliwego numerka
            initialRandomNumber = RandomHelper.GenerateRandomNumber();

            // Wczytanie danych osób z pliku
            peopleList = FileManager.LoadPeopleFromFile();

            RandomHelper randomNumberObj = new RandomHelper();
            randomNumberObj.RandomNumber = initialRandomNumber;
            this.BindingContext = randomNumberObj;
        }

        // Metoda zwracająca losową osobę z listy osób
        private Person GetRandomPersonFromList(List<Person> people)
        {
            Random random = new Random();
            int selectedIndex = random.Next(0, people.Count);
            return people[selectedIndex];
        }

        // Metoda aktualizująca liczbę pominięć dla każdej osoby
        private void UpdateExclusionTurns(List<Person> people)
        {
            foreach (var person in people)
            {
                // Zmniejszenie liczby tur wykluczenia
                if (person.ExclusionTurns > 0)
                    person.ExclusionTurns--;
            }
        }

        // Metoda filtrująca osoby według klasy
        private List<Person> FilterPeopleByClass(string filterClass)
        {
            return peopleList.Where(p => p.Class == filterClass && p.ExclusionTurns == 0).ToList();
        }

        // Obsługa przycisku dodawania nowej osoby
        private void OnAddPersonClicked(object sender, EventArgs e)
        {
            // Pobranie danych z interfejsu użytkownika
            string name = entryName.Text;
            string surname = entrySurname.Text;
            string personClass = entryClass.Text;

            // Wczytanie istniejących danych z pliku
            List<Person> loadedPeople = FileManager.LoadPeopleFromFile();

            // Filtracja osób z danej klasy
            List<Person> peopleInClass = loadedPeople.FindAll(p => p.Class == personClass);

            // Sprawdzenie liczby osób w klasie
            int numberOfPeopleInClass = peopleInClass.Count;

            //Dodanie osoby
            Person newPerson = new Person { Name = name, Surname = surname, Class = personClass, Number = (numberOfPeopleInClass + 1).ToString() };

            loadedPeople.Add(newPerson);

            // Wyczyszczenie pól wprowadzania
            entryName.Text = "";
            entrySurname.Text = "";
            entryClass.Text = "";

            // Zapisanie danych do pliku
            FileManager.SavePeopleToFile(loadedPeople);

        }

        // Obsługa przycisku losowania osoby z danej klasy
        private void OnRandomPersonFromClassClicked(object sender, EventArgs e)
        {
            // Pobranie klasy do filtrowania
            string filterClass = entryFilterClass.Text;

            // Filtracja osób z danej klasy
            List<Person> filteredPeople = FilterPeopleByClass(filterClass);

            // Sprawdzenie, czy są osoby do losowania
            if (filteredPeople.Count > 0)
            {
                // Usunięcie osoby ze szczesliwym numerkiem z listy do losowania
                int initialRandomPersonIndex = filteredPeople.FindIndex(p => p.Number == initialRandomNumber.ToString());
                if (initialRandomPersonIndex != -1)
                {
                    filteredPeople.RemoveAt(initialRandomPersonIndex);
                }

                // Losowanie osoby
                Person randomPerson = GetRandomPersonFromList(filteredPeople);

                // Wyświetlenie informacji o wylosowanej osobie
                DisplayAlert("Losowa Osoba", $"Numerek: {randomPerson.Number}\nImie: {randomPerson.Name}\nNazwisko: {randomPerson.Surname}\nKlasa: {randomPerson.Class}", "OK");

                // Ustawienie liczby tur wykluczenia dla wylosowanej osoby
                randomPerson.ExclusionTurns = 4;

                // Aktualizacja liczników wykluczenia dla wszystkich osób
                UpdateExclusionTurns(peopleList);

                // Zapisanie danych do pliku
                FileManager.SavePeopleToFile(peopleList);
            }
            else
            {
                // Komunikat o braku osób do losowania
                DisplayAlert("Informacja", "Nie znaleziono żadnej osoby w tej klasie", "OK");
            }
        }
        private async void OnEditPeopleListClicked(object sender, EventArgs e)
        {
            string filterClass = entryFilterClass.Text;

            // Pobranie wszystkich osób z danej klasy
            List<Person> filteredPeople = peopleList.Where(p => p.Class == filterClass).ToList();

            if (filteredPeople.Any())
            {
                // Utwórz nowe okno dialogowe z listą osób do edycji
                var editPage = new EditPeoplePage(filteredPeople);

                // Oczekuj na zamknięcie okna dialogowego
                editPage.PersonListUpdated += (s, updatedPeopleList) =>
                {
                    // Zapisz zaktualizowaną listę osób do głównej listy
                    foreach (var updatedPerson in updatedPeopleList)
                    {
                        var personToUpdate = peopleList.FirstOrDefault(p => p.Name == updatedPerson.Name && p.Surname == updatedPerson.Surname && p.Class == updatedPerson.Class && p.Number == updatedPerson.Number);
                        if (personToUpdate != null)
                        {
                            personToUpdate.Name = updatedPerson.Name;
                            personToUpdate.Surname = updatedPerson.Surname;
                            personToUpdate.Class = updatedPerson.Class;
                            personToUpdate.Number = updatedPerson.Number;
                        }
                    }

                    // Zapisz zmiany do pliku
                    FileManager.SavePeopleToFile(peopleList);
                };

                // Wyświetl okno dialogowe
                await Navigation.PushModalAsync(editPage);
            }
            else
            {
                await DisplayAlert("Informacja", "Nie znaleziono żadnej osoby w tej klasie", "OK");
            }
        }
    }
}