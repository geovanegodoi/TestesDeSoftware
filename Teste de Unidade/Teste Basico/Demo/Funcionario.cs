using System;
using System.Collections.Generic;

namespace Demo
{
    public enum NivelProfissional
    {
        Junior, Pleno, Senior
    }

    public class Pessoa
    {
        public string Nome { get; protected set; }

        public string Apelido { get; set; }
    }

    public class Funcionario : Pessoa
    {
        public double Salario { get; private set; }

        public NivelProfissional NivelProfissional { get; private set; }

        public IList<string> Habilidades { get; private set; }

        public Funcionario(string nome, double salario)
        {
            this.Nome = string.IsNullOrEmpty(nome) ? "Fulano" : nome;
            DefinirSalario(salario);
            DefinirHabilidades();
        }

        private void DefinirSalario(double salario)
        {
            if (salario < 500) throw new Exception(message: "Salario inferior ao permitido");

            this.Salario = salario;

            if (salario < 2000) NivelProfissional = NivelProfissional.Junior;
            else if (salario >= 8000) NivelProfissional = NivelProfissional.Senior;
            else NivelProfissional = NivelProfissional.Pleno;
        }

        private void DefinirHabilidades()
        {
            var habilidadesBasicas = new List<string>
            {
                "Lógica de Programação",
                "OOP"
            };

            this.Habilidades = habilidadesBasicas;

            switch(this.NivelProfissional)
            {
                case NivelProfissional.Pleno:
                    this.Habilidades.Add("Testes");
                    break;

                case NivelProfissional.Senior:
                    this.Habilidades.Add("Testes");
                    this.Habilidades.Add("Microservices");
                    break;
            }
        }
    }

    public class FuncionarioFactory
    {
        public static Funcionario Criar(string nome, double salario)
        {
            return new Funcionario(nome, salario);
        }

        public static Funcionario Criar(string nome, NivelProfissional nivel)
        {
            var salario = 1000;

            if (nivel == NivelProfissional.Pleno)
                salario = 5000;
            else if (nivel == NivelProfissional.Senior)
                salario = 10000;

            return new Funcionario(nome, salario);
        }
    }
}
