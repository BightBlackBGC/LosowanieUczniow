namespace LosowanieUcznia;

public partial class EditPeoplePage : ContentPage
{
    private List<Person> peopleList;

    // Zdarzenie wywo³ywane po zaktualizowaniu listy osób
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

        // Wyœwietl okna dialogowe umo¿liwiaj¹ce edycjê danych
        string newNumber = await DisplayPromptAsync("Edycja Numerka", "Wpisz nowy numerek (b¹dŸ zostaw stary):", initialValue: selectedPerson.Number);
        if (newNumber == null)
        {
            newNumber = selectedPerson.Number;
        }
        string newName = await DisplayPromptAsync("Edycja imienia", "Wpisz nowe imie (b¹dŸ zostaw stare):", initialValue: selectedPerson.Name);
        if (newName == null)
        {
            newName = selectedPerson.Name;
        }
        string newSurname = await DisplayPromptAsync("Edycja Nazwiska", "Wpisz nowe nazwisko (b¹dŸ zostaw stare):", initialValue: selectedPerson.Surname);
        if (newSurname == null)
        {
            newSurname = selectedPerson.Surname;
        }
        string newClass = await DisplayPromptAsync("Edycja Klasy", "Wpisz now¹ klase (b¹dŸ zostaw star¹):", initialValue: selectedPerson.Class);
        if (newClass == null)
        {
            newClass = selectedPerson.Class;
        }

        // SprawdŸ, czy numer jest ju¿ przypisany do innej osoby w danej klasie
        var personWithSameNumber = peopleList.FirstOrDefault(p => p.Class == newClass && p.Number == newNumber);
        if (personWithSameNumber != null && personWithSameNumber != selectedPerson)
        {
            await DisplayAlert("Informacja", "Ten numerek zosta³ ju¿ przypisany do innej osoby w tej klasie", "OK");
            return;
        }

        // Zaktualizuj dane wybranej osoby
        selectedPerson.Name = newName;
        selectedPerson.Surname = newSurname;
        selectedPerson.Class = newClass;
        selectedPerson.Number = newNumber;

        // Wywo³aj zdarzenie informuj¹ce o aktualizacji listy osób
        PersonListUpdated?.Invoke(this, peopleList);

        // Odœwie¿ widok listy osób
        peopleListView.ItemsSource = null;
        peopleListView.ItemsSource = peopleList;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
