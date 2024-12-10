using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Feedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoLocal",
                table: "Turismo");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Turismo",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Turismo",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Localizacao",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Localizacao",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Comentario = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avaliacao = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Turismo");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Turismo");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Localizacao");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Localizacao");

            migrationBuilder.AddColumn<string>(
                name: "DescricaoLocal",
                table: "Turismo",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
