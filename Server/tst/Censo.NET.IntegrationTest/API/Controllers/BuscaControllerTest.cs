using Censo.NET.API;
using Censo.NET.Application.Dtos;
using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Model;
using Censo.NET.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Censo.NET.IntegrationTest.API.Controllers
{
    public class BuscaControllerTest : IDisposable
    {
        private readonly HttpClient _client;
        private readonly CensoContext _context;
        private string _address;

        public BuscaControllerTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {new KeyValuePair<string, string>("databaseName", "busca")});

            var server = new TestServer(new WebHostBuilder()
                .UseConfiguration(builder.Build())
                .UseEnvironment("Teste")
                .UseStartup<Startup>());

            _context = server.Host.Services.GetService(typeof(CensoContext)) as CensoContext;
            _client = server.CreateClient();
        }

        [Fact]
        public async Task ArvoreGenealogica()
        {
            var model = new Pesquisa
            {
                Nome = "Nome",
                Sobrenome = "Sobrenome"
            };
            _context.Pesquisas.Add(model);
            await _context.SaveChangesAsync();

            var parent1Model = new Pesquisa
            {
                Nome = "Pai1 Nome",
                Sobrenome = "Pai1 Sobrenome",
            };
            _context.Pesquisas.Add(parent1Model);
            await _context.SaveChangesAsync();

            var parent2Model = new Pesquisa
            {
                Nome = "Pai2 Nome",
                Sobrenome = "Pai2 Sobrenome",
            };
            _context.Pesquisas.Add(parent2Model);
            await _context.SaveChangesAsync();

            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = parent1Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = parent2Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = model });
            await _context.SaveChangesAsync();

            var grandParent1Model = new Pesquisa
            {
                Nome = "Avo1 Nome",
                Sobrenome = "Avo1 Sobrenome",
            };
            _context.Pesquisas.Add(grandParent1Model);
            await _context.SaveChangesAsync();

            var grandParent2Model = new Pesquisa
            {
                Nome = "Avo2 Nome",
                Sobrenome = "Avo2 Sobrenome",
            };
            _context.Pesquisas.Add(grandParent2Model);
            await _context.SaveChangesAsync();

            var grandParent3Model = new Pesquisa
            {
                Nome = "Avo3 Nome",
                Sobrenome = "Avo3 Sobrenome",
            };
            _context.Pesquisas.Add(grandParent3Model);
            await _context.SaveChangesAsync();

            var grandParent4Model = new Pesquisa
            {
                Nome = "Avo4 Nome",
                Sobrenome = "Avo4 Sobrenome",
            };
            _context.Pesquisas.Add(grandParent4Model);
            await _context.SaveChangesAsync();

            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = grandParent1Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = parent1Model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = grandParent2Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = parent1Model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = grandParent3Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = parent2Model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Parente = grandParent4Model, GrauParentesco = GrauParentesco.Pai, Pesquisa = parent2Model });
            await _context.SaveChangesAsync();

            _address = "api/busca/arvoreGenealogica";

            var response = await _client.GetAsync($"{_address}?id={model.Id}&nivel=2");

            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PesquisaDto>>>(await response.Content.ReadAsStringAsync());

            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            Assert.Single(resultList[0].ToList());
            Assert.Equal(2, resultList[1].ToList().Count);
            Assert.Equal(4, resultList[2].ToList().Count);
        }

        [Theory]
        [InlineData(null, Regiao.CentroOeste, null, null, null, 20)]
        [InlineData("a", Regiao.Nordeste, null, null, null, 20)]
        [InlineData("s", null, null, null, null, 20)]
        [InlineData(null, null, null, null, null, 20)]
        [InlineData(null, Regiao.CentroOeste, Genero.Feminino, Etnia.Branco, Escolaridade.Doutorado, 20)]
        [InlineData("Maria", null, null, null, null, 20)]
        [InlineData("a", null, Genero.Feminino, null, null, 20)]
        public async Task Filter(string nome, Regiao? regiao, Genero? genero, Etnia? etnia, Escolaridade? escolaridade, int expectedTotal)
        {
            _address = "api/busca/filtro";
            await SeedFilter();

            HttpResponseMessage response;
            if (string.IsNullOrWhiteSpace(nome))
            {
                response = await _client.GetAsync($"{_address}?regiao={regiao}&genero={genero}&etnia={etnia}&escolaridade={escolaridade}");
            }
            else
            {
                response = await _client.GetAsync($"{_address}?nome={nome}&regiao={regiao}&genero={genero}&etnia={etnia}&escolaridade={escolaridade}");
            }

            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<ResultadoBuscaDto>(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedTotal, result.Total);
        }

        private async Task SeedFilter()
        {
            var nomes = new string[] { "João", "Maria", "Pedro", "Rafael", "Cristina", "Priscila" };
            var sobrenomes = new string[] { "Silveira", "Souza", "Silva", "Guedes", "Maia", "Monte Mor" };
            var regioes = new Regiao[] { Regiao.Nordeste, Regiao.Sul, Regiao.Sudeste, Regiao.Norte, Regiao.CentroOeste };
            var etnias = new Etnia[] { Etnia.Branco, Etnia.Caboclo, Etnia.Cafuzo, Etnia.Indigena, Etnia.Mulato, Etnia.Negro, Etnia.Pardo };
            var generos = new Genero[] { Genero.Feminino, Genero.Masculino, Genero.Indefinido, Genero.Outro };
            var escolaridades = new Escolaridade[] { Escolaridade.Outro, Escolaridade.MediolIncompleto, Escolaridade.MedioCompleto, Escolaridade.PosGraduado, Escolaridade.SuperiorCompleto };

            for (int i = 0; i < 20; i++)
            {
                _context.Pesquisas.Add(new Pesquisa
                {
                    Nome = nomes[new Random().Next(6)],
                    Sobrenome = sobrenomes[new Random().Next(6)],
                    Regiao = regioes[new Random().Next(5)],
                    Etnia = etnias[new Random().Next(7)],
                    Genero = generos[new Random().Next(4)],
                    Escolaridade = escolaridades[new Random().Next(5)]
                });
            }

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _context.Database.EnsureDeleted();
            _context?.Dispose();
        }
    }
}
