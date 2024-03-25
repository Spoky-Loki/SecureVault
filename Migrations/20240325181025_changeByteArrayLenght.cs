using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureVault.Migrations
{
    /// <inheritdoc />
    public partial class changeByteArrayLenght : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "varbinary(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CcNumber",
                table: "CreditCards",
                type: "varbinary(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CVV",
                table: "CreditCards",
                type: "varbinary(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(128)",
                oldMaxLength: 128);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "varbinary(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CcNumber",
                table: "CreditCards",
                type: "varbinary(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<byte[]>(
                name: "CVV",
                table: "CreditCards",
                type: "varbinary(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(32)",
                oldMaxLength: 32);
        }
    }
}
