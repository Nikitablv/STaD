using NUnit.Framework;
using Moq;
using LW3;

namespace LW3Tests
{
    [TestFixture]
    public class CureTests
    {
        [TestCase("Nurofen")]
        [TestCase("")]
        [TestCase("01234567890123456789012345678")]
        [TestCase("\n")]
        public void Cure_name_normal_when_length_less_than_max(string name)
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);

            // Act
            cure.CureName = name;

            // Assert
            Assert.AreEqual(name, cure.CureName);
        }

        [Test]
        public void Cure_name_unknown_when_length_more_than_max()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            string expected = "Unknown";

            // Act
            cure.CureName = "012345678901234567890123456789";

            // Assert
            Assert.AreEqual(expected, cure.CureName);
        }

        [TestCase(0)]
        [TestCase(500)]
        [TestCase(1000)]
        public void Cure_quantity_normal_when_value_in_range(int quant)
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);

            // Act
            cure.Quantity = quant;

            // Assert
            Assert.AreEqual(quant, cure.Quantity);
        }

        [Test]
        public void Cure_quantity_limiting_when_value_in_upper_limit()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 1000;

            // Act
            cure.Quantity = expected + 1;

            // Assert
            Assert.AreEqual(expected, cure.Quantity);
        }

        [Test]
        public void Cure_quantity_limiting_when_value_in_lower_limit()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 0;

            // Act
            cure.Quantity = expected - 1;

            // Assert
            Assert.AreEqual(expected, cure.Quantity);
        }

        [TestCase(0)]
        [TestCase(5000)]
        [TestCase(10000)]
        public void Cure_price_normal_when_value_in_range(int price)
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);

            // Act
            cure.Price = price;

            // Assert
            Assert.AreEqual(price, cure.Price);
        }

        [Test]
        public void Cure_price_limiting_when_value_in_upper_limit()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 10000;

            // Act
            cure.Price = expected + 1;

            // Assert
            Assert.AreEqual(expected, cure.Price);
        }

        [Test]
        public void Cure_price_limiting_when_value_in_lower_limit()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 0;

            // Act
            cure.Price = expected - 1;

            // Assert
            Assert.AreEqual(expected, cure.Price);
        }

        [Test]
        public void Fill_cure_when_pharmacy_has_inventory()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            var mockPharmacy = new Mock<IPharmacy>();
            mockPharmacy.Setup(repo => repo.HasInventory(cure.CureName, cure.Quantity)).Returns(true);
            mockPharmacy.Setup(repo => repo.RemoveFromDB(cure.CureName, cure.Quantity));

            // Act
            cure.Fill(mockPharmacy.Object);

            // Assert
            Assert.IsTrue(cure.IsFilled);
            mockPharmacy.VerifyAll();
        }

        [Test]
        public void Remove_cure_to_pharmacy()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            var mockPharmacy = new Mock<IPharmacy>();
            mockPharmacy.Setup(repo => repo.AddToDB(cure.CureName, cure.Quantity));

            // Act
            cure.Remove(mockPharmacy.Object);

            // Assert
            Assert.IsFalse(cure.IsFilled);
            mockPharmacy.VerifyAll();
        }

        [Test]
        public void Calculating_total_price()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 500;

            // Act
            int result = cure.TotalPrice();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Calculating_total_price_with_discond()
        {
            // Arrange
            Cure cure = new Cure("Nurofen", 5, 100);
            int expected = 450;

            // Act
            int result = cure.DiscondCard();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Information_about_cure()
        {
            // Arrange
            string name = "Nurofen";
            int count = 5;
            int price = 100;
            Cure cure = new Cure(name, count, price);
            string expected = $"{name}:\n  Quantity: {count}\n  Price per one: {price}\n  Total price: {cure.TotalPrice()}\n  With discond: {cure.DiscondCard()}\n" ;

            // Act
            string result = cure.CureToString();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}