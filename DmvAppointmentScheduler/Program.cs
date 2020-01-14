using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            // Your code goes here .....
            // Re-write this method to be more efficient instead of a random assignment
            var type0 = new TellerList();
            var type1 = new TellerList();
            var type2 = new TellerList();
            var type3 = new TellerList();
            tellers.Teller.ForEach(teller =>
            {
                var type = teller.specialtyType;
                if (type == "0")
                {
                    type0.Teller.Add(teller);
                } else if (type == "1")
                {
                    type1.Teller.Add(teller);
                } else if (type == "2")
                {
                    type2.Teller.Add(teller);
                } else if (type == "3")
                {
                    type3.Teller.Add(teller);
                }
            });
            var type0Sorted = type0.Teller.OrderBy(x => x.multiplier).ToList();
            var type1Sorted = type1.Teller.OrderBy(x => x.multiplier).ToList();
            var type2Sorted = type2.Teller.OrderBy(x => x.multiplier).ToList();
            var type3Sorted = type3.Teller.OrderBy(x => x.multiplier).ToList();


            var cusType0 = new CustomerList();
            var cusType1 = new CustomerList();
            var cusType2 = new CustomerList();
            var cusType3 = new CustomerList();
            var cusType4 = new CustomerList();
            customers.Customer.ForEach(customer =>
            {
                var type = customer.type;
                if (type == "0")
                {
                    cusType0.Customer.Add(customer);
                }
                else if (type == "1")
                {
                    cusType1.Customer.Add(customer);
                }
                else if (type == "2")
                {
                    cusType2.Customer.Add(customer);
                }
                else if (type == "3")
                {
                    cusType3.Customer.Add(customer);
                }
                else
                {
                    cusType4.Customer.Add(customer);
                }
            });
           // var cusType0Sorted = cusType0.Customer.OrderByDescending(x => x.duration);
            var cusType1Sorted = cusType1.Customer.OrderByDescending(x => x.duration).ToList();
            var cusType2Sorted = cusType2.Customer.OrderByDescending(x => x.duration).ToList();
            var cusType3Sorted = cusType3.Customer.OrderByDescending(x => x.duration).ToList();
            var cusType4Sorted = cusType4.Customer.OrderByDescending(x => x.duration).ToList();

            AssignApp(cusType4Sorted, type0Sorted);
            AssignApp(cusType1Sorted, type1Sorted);
            AssignApp(cusType2Sorted, type2Sorted);
            AssignApp(cusType3Sorted, type3Sorted);

            //foreach (Customer customer in customers.Customer)
            //{
            //    var appointment = new Appointment(customer, tellers.Teller[random.Next(150)]);
            //    appointmentList.Add(appointment);
            //}
        }

        static void AssignApp(List<Customer> customers, List<Teller> tellers)
        {
            var custPerTeller = (customers.Count / tellers.Count) + 1;
            var custAssigned = 0;
            var tellerIndex = 0;
            for (var i = 0; i < customers.Count; i++)
            {
                var appointment = new Appointment(customers.ElementAt(i), tellers.ElementAt(tellerIndex));
                appointmentList.Add(appointment);
                custAssigned++;
                if (custAssigned == custPerTeller)
                {
                    custAssigned = 0;
                    if (tellerIndex < tellers.Count - 1)
                    {
                        tellerIndex++;
                    }
                }
            }
        }

        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
       }

    }
}
