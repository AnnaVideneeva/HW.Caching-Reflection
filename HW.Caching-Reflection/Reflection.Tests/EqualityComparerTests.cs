using System.Collections.Generic;
using NUnit.Framework;

namespace Reflection.Tests
{
    [TestFixture]
    public class EqualityComparerTests
    {
        internal class Email
        {
            public string Address { get; set; }
        }

        internal class PersonData
        {
            public string Name { get; set; }
        }

        internal class Person
        {
            public int Id { get; set; }
            public PersonData PersonData { get; set; }
            public List<Email> Emails { get; set; }
        }

        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(-11, -11, ExpectedResult = true)]
        [TestCase(1, 11, ExpectedResult = false)]
        public bool EqualityComparerInt_Tests(int lhs, int rhs)
        {
            return EqualityComparer.Equals(lhs, rhs);
        }

        [TestCase("test", null, ExpectedResult = false)]
        [TestCase(null, null, ExpectedResult = true)]
        public bool CompareNull_Tests(string lhs, string rhs)
        {
            return EqualityComparer.Equals(lhs, rhs);
        }

        [TestCase("Anna", "Anna", "anna11@gmail.ru", "anna11@gmail.ru", ExpectedResult = true)]
        [TestCase("Anna", "Anna", null, null, ExpectedResult = true)]
        [TestCase("Anna", "Tanya", "anna99@gmail.ru", "anna99@gmail.ru", ExpectedResult = false)]
        public bool CompareClassesAndEnumerables_Tests(string lhsName, string rhsName, string lhsEmail, string rhsEmail)
        {
            var lhsPerson = new Person()
            {
                Id = 1,
                PersonData = new PersonData()
                {
                    Name = lhsName
                },
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Address = lhsEmail
                    }
                }
            };

            var rhsPerson = new Person()
            {
                Id = 1,
                PersonData = new PersonData()
                {
                    Name = rhsName
                },
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Address = rhsEmail
                    }
                }
            };

            return EqualityComparer.Equals(lhsPerson, rhsPerson);
        }
    }
}