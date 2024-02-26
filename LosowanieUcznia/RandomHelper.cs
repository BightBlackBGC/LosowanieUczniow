using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosowanieUcznia
{
    internal class RandomHelper
    {
        public int RandomNumber { get; set; }

        public static implicit operator RandomHelper(int v)
        {
            throw new NotImplementedException();
        }

        // Metoda generująca losową liczbę
        public static int GenerateRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 16);

            return randomNumber;
        }
    }
}
