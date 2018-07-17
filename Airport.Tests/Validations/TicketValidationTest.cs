using System;
using NUnit.Framework;
using Airport.DAL.Entities;
using Airport.BLL.Validators;

namespace Airport.Tests.Validations
{
    [TestFixture]
    public class TicketValidationTest
    {
        [Test]
        public void WhenFlightIsNull_ThenIsNotValid()
        {
            TicketValidator validator = new TicketValidator();

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Price = 23
            };

            // Act
            var validationResult = validator.Validate(ticket);

            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(2)]
        public void WhenPriceSmallerThan2_ThenIsNotValid(decimal value)
        {
            TicketValidator validator = new TicketValidator();

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                Price = value
            };

            // Act
            var validationResult = validator.Validate(ticket);

            {
                Assert.IsFalse(validationResult.IsValid);
            }
        }
    }
}
