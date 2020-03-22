using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotnet.Linq.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleBasic();
            ExampleWhereClause();
            ExampleOrderClause();
            ExampleGroupClause();
            ExampleJoinClause();
            ExampleCustomExtensionMethods();
            ExampleParallel();
        }

        private static void ExampleBasic()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic iteration of linq");

            var customers = from customer in GetCustomers()
                            select customer;
            foreach (var c in customers)
                Console.WriteLine($"-> {c.ToString()}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleWhereClause()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic where clause");

            var customers = from customer in GetCustomers()
                            where customer.Name.Length > 5 &&
                                (customer.Id > 10 || customer.Email.Contains("@"))
                            select customer;
            foreach (var c in customers)
                Console.WriteLine($"-> {c.ToString()}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleOrderClause()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic order clause");

            var customers = from customer in GetCustomers()
                            where customer.Name.Length > 5 &&
                                (customer.Id > 10 || customer.Email.Contains("@"))
                            orderby customer.Name descending
                            select customer;
            foreach (var c in customers)
                Console.WriteLine($"-> {c.ToString()}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleGroupClause()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic group clause");

            var customers = from customer in GetCustomers()
                            where customer.Name.Length > 5 &&
                                (customer.Id > 10 || customer.Email.Contains("@"))
                            group customer by customer.Name into newCustomer
                            select newCustomer;
            foreach (var c in customers)
                Console.WriteLine($"-> {c.Key}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleJoinClause()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic join clause");

            var customers = new List<Customer>()
            {
                new Customer() { Id = 1, Name = "Fulano", Email = "x@z.com", AddressId = 1 },
                new Customer() { Id = 2, Name = "Ciclano", Email = "r@a.com", AddressId = 2 },
                new Customer() { Id = 3, Name = "Pafuncio", Email = "q@f.com", AddressId = 3 }
            };

            var addreses = new List<Address>()
            {
                new Address() { Id = 1, Street = "Rua do Fulano", Street_Number = "1", District = "Fulano", City = "Fulano", Country = "Fulano" },
                new Address() { Id = 2, Street = "Rua do Ciclano", Street_Number = "2", District = "Ciclano", City = "Ciclano", Country = "Ciclano" },
                new Address() { Id = 3, Street = "Rua do Pafuncio", Street_Number = "3", District = "Pafuncio", City = "Pafuncio", Country = "Pafuncio" }
            };

            var query = from customer in customers
                        join address in addreses on customer.AddressId equals address.Id
                        select new Customer()
                        {
                            Id = customer.Id,
                            Name = customer.Name,
                            Email = customer.Email,
                            AddressId = address.Id,
                            Address = address
                        };

            foreach (var c in query)
                Console.WriteLine($"-> {c.ToString()}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleCustomExtensionMethods()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Basic extension methods how to add funcionalities of linq");

            var ints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19, 20 };
            Console.WriteLine($"-> numbers: {string.Join(',', ints)}");
            var intsGreaterThanFive = ints.GreaterThanOrEqual(5);
            Console.WriteLine($"-> number greater of than 5: {string.Join(',', intsGreaterThanFive)}");
            var intsLessThanFifteen = ints.LessThanOrEqual(15);
            Console.WriteLine($"-> number less of than 5: {string.Join(',', intsLessThanFifteen)}");

            Console.WriteLine("########################################################################################");
        }

        private static void ExampleParallel()
        {
            Console.WriteLine("########################################################################################");
            Console.WriteLine("Example parallel");
            Console.WriteLine($"Started at: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");

            var customers = from customer in GetCustomers(999).AsParallel()
                            select customer;
            foreach (var c in customers)
                Console.WriteLine($"-> {c.ToString()}");

            Console.WriteLine($"Finished at: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            Console.WriteLine("########################################################################################");
        }

        private static IQueryable<Customer> GetCustomers(int? numberOffake = null)
        {
            numberOffake ??= new Random().Next(10, 100);
            var fakes = new List<Customer>();
            for (var i = 0; i < numberOffake; i++)
            {
                var customer = new Faker<Customer>()
                    .RuleFor(x => x.Id, f => i)
                    .RuleFor(u => u.Name, (f, u) => f.Name.FirstName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                    .Generate();
                customer.Address = new Faker<Address>()
                    .RuleFor(x => x.Street, f => f.Address.StreetName())
                    .RuleFor(x => x.Street_Number, f => f.Random.Number(1, 9999).ToString())
                    .RuleFor(x => x.District, f => f.Address.State())
                    .RuleFor(x => x.City, f => f.Address.City())
                    .RuleFor(x => x.Country, f => f.Address.Country())
                    .Generate();
                fakes.Add(customer);
            }

            return fakes.AsQueryable();
        }
    }

    public static class LinqCustomExtensionMethods
    {
        public static IEnumerable<int> GreaterThanOrEqual(this IEnumerable<int> ints, int comparable)
        {
            foreach (var i in ints)
                if (i >= comparable)
                    yield return i;
        }

        public static IEnumerable<int> LessThanOrEqual(this IEnumerable<int> ints, int comparable)
        {
            foreach (var i in ints)
                if (i <= comparable)
                    yield return i;
        }
    }
}
