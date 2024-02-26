using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosowanieUcznia
{
    internal class FileManager
    {
        // Metoda wczytująca dane osób z pliku
        public static List<Person> LoadPeopleFromFile()
        {
            List<Person> loadedPeople = new List<Person>();
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "people.txt");
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Rozdzielenie linii na części
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            // Pobranie danych o osobie i dodanie do listy
                            string name = parts[0];
                            string surname = parts[1];
                            string personClass = parts[2];
                            int number = int.Parse(parts[3]);
                            loadedPeople.Add(new Person { Name = name, Surname = surname, Class = personClass, Number = number.ToString(), ExclusionTurns = 0 });
                        }
                    }
                }
            }
            return loadedPeople;
        }

        // Zapisywanie danych o osobach do pliku
        public static void SavePeopleToFile(List<Person> people)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "people.txt");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Person person in people)
                {
                    // Zapisywanie danych o osobie do pliku
                    writer.WriteLine($"{person.Name},{person.Surname},{person.Class},{person.Number.ToString()}");
                }
            }
        }
    }
}
