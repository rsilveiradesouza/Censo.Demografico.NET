using Censo.NET.API.Controllers;
using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Censo.NET.UnitTest.API.Controllers
{
    public class BuscaControllerTest
    {
        private readonly Mock<IBuscaService> _service;
        private readonly BuscaController _controller;

        public BuscaControllerTest()
        {
            _service = new Mock<IBuscaService>();
            _controller = new BuscaController(_service.Object);
        }

        [Fact]
        public async Task ArvoreGenealogica()
        {
            // arrange
            var model = new List<List<PesquisaDto>>
            {
                new List<PesquisaDto> {
                    new PesquisaDto { }
                },
                new List<PesquisaDto> {
                    new PesquisaDto { },
                    new PesquisaDto {}
                }
            };
            _service.Setup(x => x.ArvoreGenealogica(It.IsAny<Guid>(), It.IsAny<int>())).Returns(Task.FromResult<IEnumerable<IEnumerable<PesquisaDto>>>(model));

            var guid = Guid.NewGuid();

            // act
            var result = await _controller.ArvoreGenealogica(guid, 1);

            // assert
            _service.Verify(x => x.ArvoreGenealogica(guid, 1), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsAssignableFrom<IEnumerable<IEnumerable<PesquisaDto>>>((result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ArvoreGenealogicaInternalError()
        {
            // arrange
            _service.Setup(x => x.ArvoreGenealogica(It.IsAny<Guid>(), It.IsAny<int>())).Throws(new Exception("erro teste"));

            var guid = Guid.NewGuid();

            // act
            var result = await _controller.ArvoreGenealogica(guid, 1);

            // assert
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("erro teste", (result.Result as ObjectResult).Value);
        }

        [Fact]
        public async Task Filtro()
        {
            // arrange
            ResultadoBuscaDto resultado = new ResultadoBuscaDto { Fraction = 1, Total = 2 };
            _service.Setup(x => x.Filtro(It.IsAny<string>(), It.IsAny<Regiao?>(), It.IsAny<Genero?>(), It.IsAny<Etnia?>(), It.IsAny<Escolaridade?>())).Returns(Task.FromResult(resultado));

            // act
            var result = await _controller.Filtro("teste", Regiao.CentroOeste, Genero.Masculino, Etnia.Negro, Escolaridade.Doutorado);

            // assert
            _service.Verify(x => x.Filtro("teste", Regiao.CentroOeste, Genero.Masculino, Etnia.Negro, Escolaridade.Doutorado), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(resultado.Fraction, ((result.Result as OkObjectResult).Value as ResultadoBuscaDto).Fraction);
            Assert.Equal(resultado.Total, ((result.Result as OkObjectResult).Value as ResultadoBuscaDto).Total);
        }

        [Fact]
        public async Task FiltroInternalError()
        {
            // arrange
            ResultadoBuscaDto resultado = new ResultadoBuscaDto { Fraction = 1, Total = 2 };
            _service.Setup(x => x.Filtro(It.IsAny<string>(), It.IsAny<Regiao?>(), It.IsAny<Genero?>(), It.IsAny<Etnia?>(), It.IsAny<Escolaridade?>())).Throws(new Exception("erro teste"));

            // act
            var result = await _controller.Filtro("");

            // assert
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("erro teste", (result.Result as ObjectResult).Value);
        }
    }
}
