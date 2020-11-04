using Censo.NET.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Censo.NET.Infrastructure.Migrations
{
    [DbContext(typeof(CensoContext))]
    [Migration("20201027190815_Criar")]
    public partial class _20201027190815_Criar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_PESQUISAS",
                columns: table => new
                {
                    idPesquisa = table.Column<Guid>(nullable: false),
                    chNome = table.Column<string>(nullable: false),
                    chSobrenome = table.Column<string>(nullable: true),
                    intRegiao = table.Column<int>(nullable: true),
                    intEtnia = table.Column<int>(nullable: true),
                    intGenero = table.Column<int>(nullable: true),
                    intEscolaridade = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PESQUISAS", x => x.idPesquisa);
                });

            migrationBuilder.CreateTable(
                name: "TB_PESQUISAS_PAISFILHOS",
                columns: table => new
                {
                    idPesquisaPaiFilho = table.Column<Guid>(nullable: false),
                    idPesquisa = table.Column<Guid>(nullable: false),
                    idPesquisaParente = table.Column<Guid>(nullable: false),
                    intGrauParentesco = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PESQUISAS_PAISFILHOS", x => x.idPesquisaPaiFilho);
                    table.ForeignKey(
                        name: "FK_TB_PESQUISAS_PAISFILHOS_TB_PESQUISAS_idPesquisa",
                        column: x => x.idPesquisa,
                        principalTable: "TB_PESQUISAS",
                        principalColumn: "idPesquisa",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TB_PESQUISAS_PAISFILHOS_TB_PESQUISAS_idPesquisaParente",
                        column: x => x.idPesquisaParente,
                        principalTable: "TB_PESQUISAS",
                        principalColumn: "idPesquisa",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_PESQUISAS_PAISFILHOS_idPesquisaParente",
                table: "TB_PESQUISAS_PAISFILHOS",
                column: "idPesquisaParente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_PESQUISAS_PAISFILHOS");

            migrationBuilder.DropTable(
                name: "TB_PESQUISAS");
        }
    }
}
