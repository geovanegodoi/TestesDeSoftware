using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertNumbersTests
    {
        [Fact]
        public void Calculadora_Somar_DeverSerIgual()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(2, 2);

            //Assert
            Assert.Equal(expected: 4, actual: resultado);
        }

        [Fact]
        public void Calculadora_Somar_NaoDeverSerIgual()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(1.131231, 2.231231);

            //Assert
            Assert.NotEqual(expected: 4, actual: resultado, precision: 1);
        }
    }
}
