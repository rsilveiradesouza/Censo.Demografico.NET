using Censo.NET.API.Controllers;
using Censo.NET.Application.Dtos;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Censo.NET.UnitTest.API.Controllers
{
    public class PesquisaControllerTest
    {
        private readonly Mock<IPesquisaService> _service;
        private readonly PesquisaController _controller;

        public PesquisaControllerTest()
        {
            _service = new Mock<IPesquisaService>();
            _controller = new PesquisaController(_service.Object);
        }

        [Fact]
        public async Task CadastrarPesquisa()
        {
            // arrange
            var vm = new PesquisaDto { Pesquisa = new PesquisaInfoDto() };
            _service.Setup(x => x.CadastrarPesquisa(It.IsAny<PesquisaDto>())).Returns(Task.FromResult(new PesquisaDto()));

            // act
            var result = await _controller.CadastrarPesquisa(vm);

            // assert
            _service.Verify(x => x.CadastrarPesquisa(It.IsAny<PesquisaDto>()), Times.Once);
        }

        [Fact]
        public async Task CadastrarPesquisaInternalError()
        {
            // arrange
            _service.Setup(x => x.CadastrarPesquisa(It.IsAny<PesquisaDto>())).Throws(new Exception("erro teste"));

            // act
            var result = await _controller.CadastrarPesquisa(new PesquisaDto { Pesquisa = new PesquisaInfoDto() });

            // assert
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("erro teste", (result.Result as ObjectResult).Value);
        }

        [Fact]
        public async Task Obter()
        {
            // arrange
            var model = new PesquisaDto();
            _service.Setup(x => x.Obter(It.IsAny<Guid>())).Returns(Task.FromResult(model));

            var guid = Guid.NewGuid();

            // act
            var result = await _controller.Obter(guid);

            // assert
            _service.Verify(x => x.Obter(guid), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<PesquisaDto>((result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ObterInternalError()
        {
            // arrange
            _service.Setup(x => x.Obter(It.IsAny<Guid>())).Throws(new Exception("erro teste"));

            var guid = Guid.NewGuid();

            // act
            var result = await _controller.Obter(guid);

            // assert
            Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("erro teste", (result.Result as ObjectResult).Value);
        }

        [Fact]
        public async Task ObterNotFound()
        {
            // arrange
            var model = new Pesquisa();
            _service.Setup(x => x.Obter(It.IsAny<Guid>())).Returns(Task.FromResult((PesquisaDto)null));

            var guid = Guid.NewGuid();

            // act
            var result = await _controller.Obter(guid);

            // assert
            _service.Verify(x => x.Obter(guid), Times.Once);
        }

        [Fact]
        public async Task ObterTudo()
        {
            // arrange
            var model = new List<PesquisaDto>() {
                new PesquisaDto(),
                new PesquisaDto()
            };
            _service.Setup(x => x.ObterTudo()).Returns(Task.FromResult<IEnumerable<PesquisaDto>>(model));

            // act
            var result = await _controller.ObterTudo();

            // assert
            _service.Verify(x => x.ObterTudo(), Times.Once);
            Assert.IsAssignableFrom<IEnumerable<PesquisaDto>>((result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ObterTudoInternalError()
        {
            // arrange
            _service.Setup(x => x.ObterTudo()).Throws(new Exception("erro teste"));

            // act
            var result = await _controller.ObterTudo();

            // assert
            Assert.Equal(StatusCodes.Status500InternalServerError, (result.Result as ObjectResult).StatusCode);
            Assert.Equal("erro teste", (result.Result as ObjectResult).Value);
        }
    }
}
