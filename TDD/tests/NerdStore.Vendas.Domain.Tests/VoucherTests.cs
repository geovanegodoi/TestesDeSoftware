using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {     
        [Fact(DisplayName = "Validar Voucher Tipo Valor Valido")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher(codigo: "PROMO-15-REAIS", valorDesconto: 15, percentualDesconto: null, quantidade: 1,
                                      validade: DateTime.Now.AddDays(1), ativo: true, utilizado: false, tipoDesconto: TipoDesconto.Valor);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Tipo Valor Invalido")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher(codigo: "", valorDesconto: null, percentualDesconto: null, quantidade: 0,
                          validade: DateTime.Now.AddDays(-1), ativo: false, utilizado: true, tipoDesconto: TipoDesconto.Valor);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expected: 6, result.Errors.Count);
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Válido")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarValido()
        {
            var voucher = new Voucher(codigo: "PROMO-15-OFF", valorDesconto: null, percentualDesconto: 15, quantidade: 1,
                                      validade: DateTime.Now.AddDays(15), ativo: true, utilizado: false, tipoDesconto: TipoDesconto.Porcentagem);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Inválido")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarInvalido()
        {
            var voucher = new Voucher(codigo: "", valorDesconto: null, percentualDesconto: null, quantidade: 0,
                                      validade: DateTime.Now.AddDays(-1), ativo: false, utilizado: true, tipoDesconto: TipoDesconto.Porcentagem);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expected: 6, result.Errors.Count);
        }
    }
}
