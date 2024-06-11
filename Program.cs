using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CarParkApplication
{
    internal class Program
    {
        static Dictionary<int, string> carParkDictionary;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome! Please initialize a car park.");
            while (true)
            {
                Console.Write("Enter the capacity of your car park: ");
                int capacity;

                if (int.TryParse(Console.ReadLine(), out capacity))
                {
                    if (capacity > 0)
                    {
                        carParkDictionary = InitalizeCarPark(capacity);
                        PrintCarPark(carParkDictionary);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter an integer greater than 0.");
                    }
                }
                else
                {
                    Console.WriteLine("Enter an integer greater than 0.");
                }
            }

            while (true)
            {
                ShowMenu();
                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (choice)
                {
                    case '1':
                        // Add vehicle
                        Console.Write("Enter licence to add (XXX-XXX): ");
                        string licenceNumber = Console.ReadLine();
                        int stall = AddVehicle(carParkDictionary, licenceNumber);
                        if (stall == -1)
                        {
                            Console.WriteLine("Invalid licence number.");
                        }
                        else if (stall == -2)
                        {
                            Console.WriteLine("No stalls available.");
                        }
                        else
                        {
                            Console.WriteLine($"Vehicle with licence {licenceNumber} added to stall {stall}.");
                        }
                        break;
                    case '2':
                        Console.Write("Enter stall number to vacate: ");
                        int stallNumber;
                        if (int.TryParse(Console.ReadLine(), out stallNumber))
                        {
                            if (VacateStall(carParkDictionary, stallNumber))
                            {
                                Console.WriteLine($"Stall {stallNumber} vacated.");
                            }
                            else
                            {
                                Console.WriteLine($"Stall {stallNumber} is already empty or invalid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid stall number.");
                        }
                        break;
                    case '3':
                        // Leave parkade
                        Console.Write("Enter licence to remove (XXX-XXX): ");
                        string licenceNum = Console.ReadLine();
                        int stallToLeave = LeaveParkade(carParkDictionary, licenceNum);
                        if (stallToLeave == -1)
                        {
                            Console.WriteLine($"Vehicle with licence {licenceNum.ToUpper()} does not exist or is invalid.");
                        }
                        else
                        {
                            Console.WriteLine($"Vehicle with licence {licenceNum.ToUpper()} removed at stall {stallToLeave}.");
                        }
                        break;
                    case '4':
                        string manifest = Manifest(carParkDictionary);
                        Console.WriteLine(manifest);
                        break;
                    case '5':
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }

        static Dictionary<int, string> InitalizeCarPark(int capacity)
        {
            Dictionary<int, string> carPark = new Dictionary<int, string>();

            for (int i = 1; i <= capacity; i++)
            {
                carPark[i] = null;
            }

            return carPark;
        }

        static void PrintCarPark(Dictionary<int, string> carPark)
        {
            Console.WriteLine($"Car park initialized with {carPark.Count} stalls.");
            Console.Write("{");
            int i = 0;
            foreach (KeyValuePair<int, string> kvp in carPark)
            {
                // If null, print "Unoccupied"
                Console.Write($"{kvp.Key}: {kvp.Value ?? "Unoccupied"}");
                if (i < carPark.Count - 1)
                {
                    Console.Write(", ");
                }
                i++;
            }
            Console.WriteLine("}");
        }

        static int AddVehicle(Dictionary<int, string> carPark, string licence)
        {
            licence = licence.ToUpper();
            
            // Check if the licence is valid
            if (!Regex.IsMatch(licence, @"^[A-Za-z0-9]{3}-[A-Za-z0-9]{3}$"))
            {
                return -1;
            }

            // Find the first unoccupied stall
            foreach (KeyValuePair<int, string> kvp in carPark)
            {
                if (kvp.Value == null)
                {
                    // Park the vehicle in the unoccupied stall and return the stall number.
                    carPark[kvp.Key] = licence;
                    return kvp.Key;
                }
            }

            // If there are no unoccupied stalls, return -2
            return -2;
        }

        static bool VacateStall(Dictionary<int, string> carPark, int stallNumber)
        {
            if (carPark.ContainsKey(stallNumber))
            {
                if (carPark[stallNumber] != null)
                {
                    carPark[stallNumber] = null;
                    return true;
                }
            }
            return false;
        }

        static int LeaveParkade(Dictionary<int, string> carPark, string licenceNumber)
        {
            licenceNumber = licenceNumber.ToUpper();

            if (carPark.ContainsValue(licenceNumber))
            {
                foreach (KeyValuePair<int, string> kvp in carPark)
                {
                    if (kvp.Value == licenceNumber)
                    {
                        carPark[kvp.Key] = null;
                        return kvp.Key;
                    }
                }
            }
            return -1;
        }

        static string Manifest(Dictionary<int, string> carPark)
        {
            string manifest = "Current car park manifest:";
            foreach (KeyValuePair<int, string> kvp in carPark)
            {
                manifest += $"\nStall {kvp.Key}: {kvp.Value ?? "Unoccupied"}";
            }
            return manifest;
        }

        static void ShowMenu()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("1. Add vehicle");
            Console.WriteLine("2. Vacate stall");
            Console.WriteLine("3. Leave parkade");
            Console.WriteLine("4. Print manifest");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
