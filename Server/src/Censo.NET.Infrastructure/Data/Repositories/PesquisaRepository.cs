using Censo.NET.Domain.Common.Enums;
using Censo.NET.Domain.Interfaces.Data;
using Censo.NET.Domain.Model;
using Dapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Censo.NET.Infrastructure.Data.Repositories
{
    public class PesquisaRepository : RepositoryBase<Pesquisa>, IPesquisaRepository
    {
        public PesquisaRepository(CensoContext censoContext) : base(censoContext)
        {
        }

        public override async Task<Pesquisa> Obter(Guid id) =>
            await _censoContext.Pesquisas
            .Include(x => x.Parentes)
            .ThenInclude(x => x.Parente)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Pesquisa> CriarPesquisa(Pesquisa model, IEnumerable<Pesquisa> pais, IEnumerable<Pesquisa> filhos)
        {
            await using var transaction = _censoContext.Database.BeginTransaction();

            try
            {
                var entity = await ObterPorNome(model.Nome, model.Sobrenome);

                if (entity == null)
                {
                    await Criar(model);
                }
                else
                {
                    model.Id = entity.Id;

                    _censoContext.Entry(entity).CurrentValues.SetValues(model);
                    await _censoContext.SaveChangesAsync();
                }

                if (pais != null)
                {
                    foreach (var pai in pais)
                    {
                        var updateEntity = await ObterPorNome(pai.Nome, pai.Sobrenome) ?? await Criar(pai);
                        var updateEntityParent = _censoContext.PesquisasPaisFilhos.FirstOrDefault(c => c.GrauParentesco == GrauParentesco.Pai && c.ParenteId == updateEntity.Id && c.PesquisaId == model.Id);

                        if (updateEntityParent == null)
                        {
                            _censoContext.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Id = Guid.NewGuid(), GrauParentesco = GrauParentesco.Pai, ParenteId = updateEntity.Id, PesquisaId = model.Id });
                        }
                        else
                        {
                            _censoContext.Entry(updateEntityParent).CurrentValues.SetValues(new PesquisaPaiFilho
                            {
                                Id = updateEntityParent.Id,
                                GrauParentesco = GrauParentesco.Pai,
                                ParenteId = updateEntity.Id,
                                PesquisaId = model.Id,
                                Parente = updateEntity,
                                Pesquisa = model
                            });
                        }

                        await _censoContext.SaveChangesAsync();
                    }
                }

                if (filhos != null)
                {
                    foreach (var filho in filhos)
                    {
                        var updateEntity = await ObterPorNome(filho.Nome, filho.Sobrenome) ?? await Criar(filho);
                        var updateEntityParent = _censoContext.PesquisasPaisFilhos.FirstOrDefault(c => c.GrauParentesco == GrauParentesco.Filho && c.ParenteId == updateEntity.Id && c.PesquisaId == model.Id);

                        if (updateEntityParent == null)
                        {
                            _censoContext.PesquisasPaisFilhos.Add(new PesquisaPaiFilho { Id = Guid.NewGuid(), GrauParentesco = GrauParentesco.Filho, ParenteId = updateEntity.Id, PesquisaId = model.Id });
                        }
                        else
                        {
                            _censoContext.Entry(updateEntityParent).CurrentValues.SetValues(new PesquisaPaiFilho
                            {
                                Id = updateEntityParent.Id,
                                GrauParentesco = GrauParentesco.Filho,
                                ParenteId = updateEntity.Id,
                                PesquisaId = model.Id,
                                Parente = updateEntity,
                                Pesquisa = model
                            });
                        }

                        await _censoContext.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return model;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Erro ao tentar salvar a pesquisa. " + ex.ToString(), ex);
            }
        }

        public async Task<List<List<Pesquisa>>> ArvoreGenealogica(Guid id, int nivelMaximo = 0)
        {
            var arvore = new List<List<Pesquisa>> { new List<Pesquisa>() };
            arvore.First().Add(await Obter(id));

            var filhosIds = new[] { id };

            for (var i = 0; i < nivelMaximo; i++)
            {
                var ids = await ObterPais(filhosIds);

                if (ids.Any())
                {
                    arvore.Add(_censoContext.Pesquisas.Include(x => x.Parentes).ThenInclude(x => x.Parente).Where(x => ids.Contains(x.Id)).ToList());
                }

                filhosIds = ids;
            }

            return arvore;
        }

        public async Task<(int quantidade, int total)> Filtrar(string nome, Regiao? regiao, Genero? genero, Etnia? etnia, Escolaridade? escolaridade)
        {
            var total = await _censoContext.Pesquisas.CountAsync();

            var predicate = PredicateBuilder.New<Pesquisa>(true);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                predicate = predicate.And(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }

            if (genero.HasValue)
            {
                predicate = predicate.And(x => x.Genero == genero);
            }
            if (escolaridade.HasValue)
            {
                predicate = predicate.And(x => x.Escolaridade == escolaridade);
            }
            if (regiao.HasValue)
            {
                predicate = predicate.And(x => x.Regiao == regiao);
            }
            if (etnia.HasValue)
            {
                predicate = predicate.And(x => x.Etnia == etnia);
            }

            var quantidade = await _censoContext.Pesquisas.CountAsync(predicate);

            return (quantidade, total);
        }

        public async Task<IEnumerable<IEnumerable<Dashboard>>> ObterDashboard()
        {
            var etnia = await _censoContext.Database.GetDbConnection().QueryAsync<Dashboard>("select count(intEtnia) as Quantidade, intEtnia as Chave from TB_PESQUISAS group by (intEtnia)");
            var escolaridade = await _censoContext.Database.GetDbConnection().QueryAsync<Dashboard>("select count(intEscolaridade) as Quantidade, intEscolaridade as Chave from TB_PESQUISAS group by (intEscolaridade)");
            var regiao = await _censoContext.Database.GetDbConnection().QueryAsync<Dashboard>("select count(intRegiao) as Quantidade, intRegiao as Chave from TB_PESQUISAS group by (intRegiao)");
            var genero = await _censoContext.Database.GetDbConnection().QueryAsync<Dashboard>("select count(intGenero) as Quantidade, intGenero as Chave from TB_PESQUISAS group by (intGenero)");
            
            return new[] { regiao, genero, escolaridade, etnia };
        }

        private async Task<Pesquisa> ObterPorNome(string firstName, string lastName) =>
            await _censoContext.Pesquisas.FirstOrDefaultAsync(x => x.Nome == firstName && x.Sobrenome == lastName);

        private async Task<Guid[]> ObterPais(Guid[] ids) =>
            await _censoContext.PesquisasPaisFilhos.Where(x => ids.ToList().Contains(x.PesquisaId) && x.GrauParentesco == GrauParentesco.Pai).Select(x => x.ParenteId).ToArrayAsync();
    }
}
