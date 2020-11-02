using System;
using Xunit;

namespace Demo.Tests
{
    public class CalculadoraTests
    {
        [Fact]
        public void Calculadora_Somar_RetornarValorDaSoma()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(2, 2);

            //Assert
            Assert.Equal(expected: 4, actual: resultado);
        }

        [Fact]
        public void Calculadora_Subtrair_RetornarValorDaSubtracao()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Subtrair(2, 2);

            //Assert
            Assert.Equal(expected: 0, actual: resultado);
        }

        [Fact]
        public void Calculadora_Dividir_RetornarValorDaDivisao()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Dividir(2, 2);

            //Assert
            Assert.Equal(expected: 1, actual: resultado);
        }

        [Fact]
        public void Calculadora_Multiplicar_RetornarValorDaMultiplicacao()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Multiplicar(2, 2);

            //Assert
            Assert.Equal(expected: 4, actual: resultado);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 6)]
        [InlineData(4, 4, 8)]
        [InlineData(5, 5, 10)]
        public void Calculadora_Somar_RetonarValoresDaSomaCorreto(double v1, double v2, double total)
        {
            // Arrange
            var calculadora = new Calculadora();

            // Act
            var resultado = calculadora.Somar(v1, v2);

            // Assert
            Assert.Equal(expected: total, actual: resultado);
        }
    }
}
