using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureVault.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditCardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CcType = table.Column<bool>(type: "bit", nullable: false),
                    CcName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CcNumber = table.Column<byte[]>(type: "varbinary(128)", maxLength: 128, nullable: false),
                    CcExpiration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVV = table.Column<byte[]>(type: "varbinary(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCards_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_userId",
                table: "CreditCards",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");
        }
    }
}
