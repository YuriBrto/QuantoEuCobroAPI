using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuantoEuCubro.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CorPrimaria = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CorSecundaria = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Fonte = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Premium = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", nullable: true),
                    HtmlBase = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    TemplateId = table.Column<int>(type: "INTEGER", nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DescricaoCliente = table.Column<string>(type: "TEXT", nullable: true),
                    NomeCliente = table.Column<string>(type: "TEXT", nullable: true),
                    EmailCliente = table.Column<string>(type: "TEXT", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublicToken = table.Column<string>(type: "TEXT", nullable: false),
                    QuantidadeVisualizacoes = table.Column<int>(type: "INTEGER", nullable: false),
                    UltimaVisualizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Propostas_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Propostas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CamposPersonalizados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropostaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    TipoCampo = table.Column<int>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamposPersonalizados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CamposPersonalizados_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "Categoria", "CorPrimaria", "CorSecundaria", "Fonte", "HtmlBase", "Nome", "Premium", "Thumbnail" },
                values: new object[,]
                {
                    { 1, "Tecnologia", "#6C63FF", "#F3F4F6", "Poppins", null, "Moderno", false, "/thumbnails/moderno.png" },
                    { 2, "Design", "#1A1A2E", "#E8C547", "Playfair Display", null, "Elegante", false, "/thumbnails/elegante.png" },
                    { 3, "Agência", "#0F172A", "#38BDF8", "Montserrat", null, "Premium Pro", true, "/thumbnails/premium.png" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CamposPersonalizados_PropostaId",
                table: "CamposPersonalizados",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_PublicToken",
                table: "Propostas",
                column: "PublicToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_TemplateId",
                table: "Propostas",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_UsuarioId",
                table: "Propostas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CamposPersonalizados");

            migrationBuilder.DropTable(
                name: "Propostas");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
