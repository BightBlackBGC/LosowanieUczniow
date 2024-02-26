namespace LosowanieUcznia;

public partial class EditPeoplePage : ContentPage
{
    private List<Person> peopleList;

    // Zdarzenie wywo�ywane po zaktualizowaniu listy os�b
    internal event EventHandler<List<Person>> PersonListUpdated;

    internal EditPeoplePage(List<Person> people)
    {
        InitializeComponent();
        peopleList = people;
        peopleListView.ItemsSource = peopleList;
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var selectedPerson = (sender as Button).CommandParameter as Person;

        // Wy�wietl okna dialogowe umo�liwiaj�ce edycj� danych
        string newNumber = await DisplayPromptAsync("Edycja Numerka", "Wpisz nowy numerek (b�d� zostaw stary):", initialValue: selectedPerson.Number);
        if (newNumber == null)
        {
            newNumber = selectedPerson.Number;
        }
        string newName = await DisplayPromptAsync("Edycja imienia", "Wpisz nowe imie (b�d� zostaw stare):", initialValue: selectedPerson.Name);
        if (newName == null)
        {
            newName = selectedPerson.Name;
        }
        string newSurname = await DisplayPromptAsync("Edycja Nazwiska", "Wpisz nowe nazwisko (b�d� zostaw stare):", initialValue: selectedPerson.Surname);
        if (newSurname == null)
        {
            newSurname = selectedPerson.Surname;
        }
        string newClass = await DisplayPromptAsync("Edycja Klasy", "Wpisz now� klase (b�d� zostaw star�):", initialValue: selectedPerson.Class);
        if (newClass == null)
        {
            newClass = selectedPerson.Class;
        }

        // Sprawd�, czy numer jest ju� przypisany do innej osoby w danej klasie
        var personWithSameNumber = peopleList.FirstOrDefault(p => p.Class == newClass && p.Number == newNumber);
        if (personWithSameNumber != null && personWithSameNumber != selectedPerson)
        {
            await DisplayAlert("Informacja", "Ten numerek zosta� ju� przypisany do innej osoby w tej klasie", "OK");
            return;
        }

        // Zaktualizuj dane wybranej osoby
        selectedPerson.Name = newName;
        selectedPerson.Surname = newSurname;
        selectedPerson.Class = newClass;
        selectedPerson.Number = newNumber;

        // Wywo�aj zdarzenie informuj�ce o aktualizacji listy os�b
        PersonListUpdated?.Invoke(this, peopleList);

        // Od�wie� widok listy os�b
        peopleListView.ItemsSource = null;
        peopleListView.ItemsSource = peopleList;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
