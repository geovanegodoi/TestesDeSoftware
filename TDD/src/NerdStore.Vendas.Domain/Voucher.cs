using System;
using FluentValidation;
using FluentValidation.Results;

namespace NerdStore.Vendas.Domain
{
    public enum TipoDesconto
    {
        Porcentagem = 0, Valor = 1
    }

    public class Voucher
    {
        public string Codigo { get; private set; }

        public decimal? ValorDesconto { get; private set; }

        public decimal? PercentualDesconto { get; private set; }

        public int Quantidade { get; private set; }

        public DateTime DataValidade { get; private set; }

        public bool Ativo { get; private set; }

        public bool Utilizado { get; private set; }

        public TipoDesconto TipoDesconto { get; private set; }

        public Voucher(string codigo, decimal? valorDesconto, decimal? percentualDesconto, int quantidade,
                       DateTime validade, bool ativo, bool utilizado, TipoDesconto tipoDesconto)
        {
            Codigo              = codigo;
            ValorDesconto       = valorDesconto;
            PercentualDesconto  = percentualDesconto;
            Quantidade          = quantidade;
            DataValidade        = validade;
            Ativo               = ativo;
            Utilizado           = utilizado;
            TipoDesconto        = tipoDesconto;
        }

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public static string CodigoErroMsg => "Voucher sem código válido";

        public static string DataValidadeErroMsg => "Este Voucher está expirado";

        public static string AtivoErroMsg => "Este Voucher não é mais valido";

        public static string UtilizadoErroMsg => "Este Voucher já foi utilizado";

        public static string QuantidadeErroMsg => "Este Voucher não está mais disponível";

        public static string ValorDescontoErroMsg => "O valor do desconto precisa ser superior a 0";

        public static string PercentualDescontoErroMsg => "O percentual de desconto precisa ser superior a 0";

        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty()
                .WithMessage(CodigoErroMsg);

            RuleFor(c => c.DataValidade)
                .Must(DataVencimentoSuperiorAtual)
                .WithMessage(DataValidadeErroMsg);

            RuleFor(c => c.Ativo)
                .Equal(true)
                .WithMessage(AtivoErroMsg);

            RuleFor(c => c.Utilizado)
                .Equal(false)
                .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeErroMsg);

            When(f => f.TipoDesconto == TipoDesconto.Valor, () =>
            {
                RuleFor(f => f.ValorDesconto)
                    .NotNull()
                    .WithMessage(ValorDescontoErroMsg)
                    .GreaterThan(0)
                    .WithMessage(ValorDescontoErroMsg);
            });

            When(f => f.TipoDesconto == TipoDesconto.Porcentagem, () =>
            {
                RuleFor(f => f.PercentualDesconto)
                    .NotNull()
                    .WithMessage(PercentualDescontoErroMsg)
                    .GreaterThan(0)
                    .WithMessage(PercentualDescontoErroMsg);
            });
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}