using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Remake_Kafka_Example_01.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DBTEST1");

            migrationBuilder.CreateTable(
                name: "TABLE_PRODUCT_V2",
                schema: "DBTEST1",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(200)", unicode: false, nullable: false),
                    NAME = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    PRICE = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "NUMBER(38)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_PRODUCT_V2", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TABLE_PRODUCT_V2",
                schema: "DBTEST1");
        }
    }
}
