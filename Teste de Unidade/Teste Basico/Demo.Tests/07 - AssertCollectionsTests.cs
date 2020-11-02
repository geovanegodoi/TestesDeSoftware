using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertCollectionsTests
    {
        [Fact]
        public void Funcionario_Habilidades_NaoDevePossuirHabilidadesVazias()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", salario: 1000);

            //Assert
            Assert.All(collection: funcionario.Habilidades, habilidade => Assert.False(string.IsNullOrEmpty(habilidade)));
        }

        [Fact]
        public void Funcionario_Habilidades_JuniorDevePossuirHabilidadesBasicas()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Junior);

            //Assert
            Assert.Contains(expected: "OOP", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_PlenoDevePossuirHabilidadesIntermediarias()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Pleno);

            //Assert
            Assert.Contains(expected: "Testes", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_SeniorDevePossuirHabilidadesAvancadas()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Senior);

            //Assert
            Assert.Contains(expected: "Microservices", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_JuniorNaoDevePossuirHabilidadesAvancadas()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Junior);

            //Assert
            Assert.DoesNotContain(expected: "Testes", funcionario.Habilidades);
            Assert.DoesNotContain(expected: "Microservices", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_PlenoNaoDevePossuirHabilidadesAvancadas()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Pleno);

            //Assert
            Assert.DoesNotContain(expected: "Microservices", funcionario.Habilidades);
        }

        [Fact]
        public void Funcionario_Habilidades_SeniorDevePossuirTodasHabilidades()
        {
            //Arrange & Act
            var funcionario = FuncionarioFactory.Criar(nome: "Geovane", nivel: NivelProfissional.Senior);

            var todasHabilidades = new []
            {
                "Lógica de Programação",
                "OOP",
                "Testes",
                "Microservices"
            };

            //Assert
            Assert.Equal(expected: todasHabilidades, funcionario.Habilidades);
        }
    }
}
