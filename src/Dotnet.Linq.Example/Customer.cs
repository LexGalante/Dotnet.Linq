using System;
using System.Collections.Generic;
using System.Text;

namespace Dotnet.Linq.Example
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public Customer()
        {

        }

        public Customer(Address address)
        {
            Address = address;
        }

        public Customer(int id, string name, string email, Address address)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Id = ");
            builder.Append(Id);
            builder.Append(", Name = ");
            builder.Append(Name);
            builder.Append(", Email = ");
            builder.Append(Email);
            builder.Append(", Address");
            builder.Append(Address.ToString());

            return builder.ToString();
        }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Street_Number { get; set; } = "01";
        public string District { get; set; } = "Centro";
        public string City { get; set; } = "Curitiba";
        public string Country { get; set; } = "Brasil";

        public Address()
        {

        }

        public Address(string street, string street_Number, string district, string city, string country)
        {
            Street = street;
            Street_Number = street_Number;
            District = district;
            City = city;
            Country = country;
        }

        public override string ToString() => $"{Street}, {Street_Number} - {District} - {City} - {Country}";
    }
}
