using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViaAPI.Migrations
{
    /// <inheritdoc />
    public partial class TravelHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Viagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Viagem",
                columns: table => new
                {
                    IdViagem = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataChegada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataPartida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HorarioChegada = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HorarioPartida = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagem", x => x.IdViagem);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
