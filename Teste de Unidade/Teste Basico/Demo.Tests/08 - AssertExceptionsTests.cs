using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertExceptionsTests
    {
        [Fact(Skip = "Does not throw exception")]
        public void Calculadora_Dividir_DeveRetornarErroDivisaoPorZero()
        {
            //Arrange
            var calculadora = new Calculadora();

            //Act & Assert
            Assert.Throws<DivideByZeroException>(testCode: () => calculadora.Dividir(10, 0));
        }

        [Fact]
        public void Funcionario_Salario_DeveRetornarErroValorInferiorAoPermitido()
        {
            //Arrange & Act & Assert
            var exception = Assert.Throws<Exception>(testCode: () => FuncionarioFactory.Criar("Geovane", 100));

            Assert.Equal(expected: "Salario inferior ao permitido", actual: exception.Message);
        }
    }
}
