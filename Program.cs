using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIO.Series.Classes;
using DIO.Series.Enum;

namespace DIO.Series
{
    class Program
    {
        static SerieRepositorio repositorio = new SerieRepositorio();

        static void Main(string[] args)
        {
            string opcaoUsuario = ObterOpcaoUsuario();
            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarSeries();
                        break;
                    case "2":
                        InserirSerie();
                        break;
                    case "3":
                        AtualizarSerie();
                        break;
                    case "4":
                        ExcluirSerie();
                        break;
                    case "5":
                        VisualizarSerie();
                        break;
                    case "C":
                        Console.Clear();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }
            Console.WriteLine("Obrigado por utilizar nossos serviços.");
            Console.ReadLine();
        }

        private static void ExcluirSerie()
        {
            Console.Write("Digite o id da série: ");

            string indiceSerieString = Console.ReadLine();

            if (!isNumeric(indiceSerieString))
            {
                Console.Write("O valor informado não é um número válido!");
                return;
            }

            int indiceSerie = int.Parse(indiceSerieString);

            if (isSerieNotInList(indiceSerie))
            {
                Console.Write("Não foi possível a exclusão pois não foi encontrada uma série para o ID informado.");
                return;
            }

            repositorio.Exclui(indiceSerie);
            Console.Write("A série foi excluída com sucesso.");

        }

        private static void VisualizarSerie()
        {
            Console.Write("Digite o id da série: ");

            string indiceSerieString = Console.ReadLine();

            if (!isNumeric(indiceSerieString))
            {
                Console.Write("O valor informado não é um número válido!");
                return;
            }

            int indiceSerie = int.Parse(indiceSerieString);

            if (isSerieNotInList(indiceSerie))
            {
                Console.Write("Não foi possível encontrar uma série para o ID informado.");
                return;
            }

            var serie = repositorio.RetornaPorId(indiceSerie);

            Console.WriteLine(serie);
        }

        private static void AtualizarSerie()
        {
            Console.Write("Digite o id da série: ");
            string indiceSerieString = Console.ReadLine();

            if (!isNumeric(indiceSerieString))
            {
                Console.Write("ID informado inválido!");
                return;
            }
            int indiceSerie = int.Parse(indiceSerieString);

            if (repositorio.RetornaPorId(indiceSerie) == null)
            {
                Console.Write("Não existe uma série para o ID informado.");
                return;
            }

            foreach (int i in System.Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0}-{1}", i, System.Enum.GetName(typeof(Genero), i));
            }
            Console.Write("Digite o gênero entre as opções acima: ");

            string generoEntrada = Console.ReadLine();

            if (!isNumeric(generoEntrada))
            {
                Console.Write("Opção inválida. Digite um valor de 1 a 13.");
                return;
            }

            int entradaGenero = int.Parse(generoEntrada);

            Console.Write("Digite o Título da Série: ");
            string entradaTitulo = Console.ReadLine();

            Console.Write("Digite o Ano de Início da Série: ");
            int entradaAno = int.Parse(Console.ReadLine());

            Console.Write("Digite a Descrição da Série: ");
            string entradaDescricao = Console.ReadLine();

            Serie atualizaSerie = new Serie(id: indiceSerie,
                                        genero: (Genero)entradaGenero,
                                        titulo: entradaTitulo,
                                        ano: entradaAno,
                                        descricao: entradaDescricao);

            repositorio.Atualiza(indiceSerie, atualizaSerie);
            Console.Write("A série foi atualizada com sucesso.");

        }
        private static void ListarSeries()
        {
            Console.WriteLine("Listar séries");

            var lista = repositorio.Lista();

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhuma série cadastrada.");
                return;
            }

            foreach (var serie in lista)
            {
                var excluido = serie.retornaExcluido();

                Console.WriteLine("#ID {0}: - {1} {2}", serie.retornaId(), serie.retornaTitulo(), (excluido ? "*Excluído*" : ""));
            }
        }

        private static void InserirSerie()
        {
            Console.WriteLine("Inserir uma nova série");


            foreach (int i in System.Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0}-{1}", i, System.Enum.GetName(typeof(Genero), i));
            }
            Console.Write("Digite o gênero entre as opções acima: ");
            string generoEntrada = Console.ReadLine();

            if (!isNumeric(generoEntrada))
            {
                Console.Write("Opção inválida. Digite um valor de 1 a 13.");
                return;
            }

            int entradaGenero = int.Parse(generoEntrada);

            if (System.Enum.GetValues(typeof(Genero)).Length < entradaGenero)
            {
                Console.Write("Opção inválida. Digite um valor de 1 a 13.");
                return;
            }

            Console.Write("Digite o Título da Série: ");
            string entradaTitulo = Console.ReadLine();

            Console.Write("Digite o Ano de Início da Série: ");
            int entradaAno = int.Parse(Console.ReadLine());

            Console.Write("Digite a Descrição da Série: ");
            string entradaDescricao = Console.ReadLine();

            Serie novaSerie = new Serie(id: repositorio.ProximoId(),
                                        genero: (Genero)entradaGenero,
                                        titulo: entradaTitulo,
                                        ano: entradaAno,
                                        descricao: entradaDescricao);

            repositorio.Insere(novaSerie);
        }

        private static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            Console.WriteLine("DIOFlix a seu dispor!!!");
            Console.WriteLine("Informe a opção desejada:");

            Console.WriteLine("1- Listar séries");
            Console.WriteLine("2- Inserir nova série");
            Console.WriteLine("3- Atualizar série");
            Console.WriteLine("4- Excluir série");
            Console.WriteLine("5- Visualizar série");
            Console.WriteLine("C- Limpar Tela");
            Console.WriteLine("X- Sair");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }

        private static bool isNumeric(string valor)
        {
            if (valor == null || !int.TryParse(valor, out _))
            {
                return false;
            }
            return true;
        }

        private static bool isSerieNotInList(int indiceSerie)
        {
            return repositorio.RetornaPorId(indiceSerie) == null ? true : false;
        }
    }
}

