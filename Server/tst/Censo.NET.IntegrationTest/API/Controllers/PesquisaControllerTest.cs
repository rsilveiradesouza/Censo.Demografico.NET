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
    public class PesquisaControllerTest : IDisposable
    {
        private readonly HttpClient _client;
        private readonly CensoContext _context;
        private readonly string _address;

        public PesquisaControllerTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {new KeyValuePair<string, string>("databaseName", "pesquisa")});

            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Teste")
                .UseConfiguration(builder.Build())
                .UseStartup<Startup>());

            _context = server.Host.Services.GetService(typeof(CensoContext)) as CensoContext;
            _address = "/api/pesquisa";
            _client = server.CreateClient();
        }

        [Fact]
        public async Task Obter()
        {
            var modelChild = new Pesquisa
            {
                Nome = "Nome1",
                Sobrenome = "Sobrenome1",
                Genero = Genero.Feminino
            };
            _context.Pesquisas.Add(modelChild);

            var modelParent = new Pesquisa
            {
                Nome = "Nome2",
                Sobrenome = "Sobrenome2",
                Genero = Genero.Masculino
            };
            _context.Pesquisas.Add(modelParent);

            var model = new Pesquisa
            {
                Nome = "Nome3",
                Sobrenome = "Sobrenome3",
                Genero = Genero.Masculino
            };
            _context.Pesquisas.Add(model);

            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { GrauParentesco = GrauParentesco.Filho, Parente = modelChild, Pesquisa = model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { GrauParentesco = GrauParentesco.Pai, Parente = modelParent, Pesquisa = model });

            await _context.SaveChangesAsync();

            var response = await _client.GetAsync($"{_address}/{model.Id}");

            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<PesquisaDto>(await response.Content.ReadAsStringAsync());

            Assert.Equal(model.Id, result.Pesquisa.Id);
            Assert.Equal(modelParent.Id, result.Pais.First().Id);
            Assert.Equal(modelChild.Id, result.Filhos.First().Id);
        }

        [Fact]
        public async Task ObterTodos()
        {
            var modelChild = new Pesquisa
            {
                Nome = "Nome1",
                Sobrenome = "Sobrenome1",
                Genero = Genero.Masculino
            };
            _context.Pesquisas.Add(modelChild);

            var modelParent = new Pesquisa
            {
                Nome = "Nome2",
                Sobrenome = "Sobrenome2",
                Genero = Genero.Masculino
            };
            _context.Pesquisas.Add(modelParent);

            var model = new Pesquisa
            {
                Nome = "Nome3",
                Sobrenome = "Sobrenome3",
                Genero = Genero.Masculino
            };
            _context.Pesquisas.Add(model);

            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { GrauParentesco = GrauParentesco.Filho, Parente = modelChild, Pesquisa = model });
            _context.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { GrauParentesco = GrauParentesco.Pai, Parente = modelParent, Pesquisa = model });

            await _context.SaveChangesAsync();

            var response = await _client.GetAsync($"{_address}");

            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<List<PesquisaDto>>(await response.Content.ReadAsStringAsync()).ToList();
            Assert.True(result.Exists(x => x.Pesquisa.Id == modelChild.Id));
            Assert.True(result.Exists(x => x.Pesquisa.Id == modelParent.Id));
            Assert.True(result.Exists(x => x.Pesquisa.Id == model.Id));
        }

        public void Dispose()
        {
            _client?.Dispose();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
