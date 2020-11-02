using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringTools_Unir_RetornarNomeCompleto()
        {
            // Arrange
            var st = new StringTools();

            // Act
            var nomeCompleto = st.Unir("Geovane", "Godoi");

            // Assert
            Assert.Equal(expected: "Geovane Godoi", actual: nomeCompleto);
        }

        [Fact]
        public void StringTools_Unir_DeveIgnorarCase()
        {
            // Arrange
            var st = new StringTools();

            // Act
            var nomeCompleto = st.Unir("Geovane", "Godoi");

            // Assert
            Assert.Equal(expected: "GEOVANE GODOI", actual: nomeCompleto, ignoreCase: true);
        }

        [Fact]
        public void StringTools_Unir_DeveConterTrecho()
        {
            // Arrange
            var st = new StringTools();

            // Act
            var nomeCompleto = st.Unir("Geovane", "Godoi");

            // Assert
            Assert.Contains(expectedSubstring: "Geovane", actualString: nomeCompleto);
        }

        [Fact]
        public void StringTools_Unir_DeveComecarCom()
        {
            // Arrange
            var st = new StringTools();

            // Act
            var nomeCompleto = st.Unir("Geovane", "Godoi");

            // Assert
            Assert.StartsWith(expectedStartString: "Geo", actualString: nomeCompleto);
        }

        [Fact]
        public void StringTools_Unir_DeveTerminarCom()
        {
            // Arrange
            var st = new StringTools();

            // Act
            var nomeCompleto = st.Unir("Geovane", "Godoi");

            // Assert
            Assert.EndsWith(expectedEndString: "doi", actualString: nomeCompleto);
        }
    }
}
