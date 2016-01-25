using System.Collections.Generic;
using System.Globalization;

namespace VideoStore
{
    class Customer
    {
        private readonly string _name;
        private readonly List<Rental> _rentals = new List<Rental>(); 

        public Customer(string name)
        {
            _name = name;
        }

        public void AddRental(Rental rental)
        {
            _rentals.Add(rental);
        }

        public override string ToString()
        {
            return _name;
        }

        public string GetStatement()
        {
            double totalAmount = 0;
            int frequentRenterPoints = 0;

            string result = string.Format(CultureInfo.InvariantCulture, "Rental record for '{0}'\r\n", ToString());

            double amount = 0;
            foreach (Rental rental in _rentals)
            {
                int moviePriceCode = rental.GetMovie().GetPriceCode();

                switch (moviePriceCode)
                {
                    case Movie.REGULAR:
                        amount += 2;
                        if (rental.GetDaysRented() > 2)
                        {
                            amount += (rental.GetDaysRented() - 2)*1.5;
                        }
                        break;
                    case Movie.NEW_RELEASE:
                        amount += rental.GetDaysRented()*3;
                        break;
                    case Movie.CHILDRENS:
                        amount += 1.5;
                        if (rental.GetDaysRented() > 3)
                        {
                            amount += (rental.GetDaysRented() - 3)*1.5;
                        }
                        break;
                }
                frequentRenterPoints++;

                if ((rental.GetMovie().GetPriceCode() == Movie.NEW_RELEASE) &&
                    rental.GetDaysRented() > 1)
                {
                    frequentRenterPoints++;
                }

                result += string.Format(CultureInfo.InvariantCulture, "\t'{0}'\t'{1}'\n", rental.GetMovie(), amount);
                totalAmount += amount;
            }

            result += string.Format(CultureInfo.InvariantCulture, "Total owed is '{0}'\n", totalAmount);
            result += string.Format(CultureInfo.InvariantCulture, "You earned '{0}' frequent renter points",
                frequentRenterPoints);

            return result;
        }
    }
}