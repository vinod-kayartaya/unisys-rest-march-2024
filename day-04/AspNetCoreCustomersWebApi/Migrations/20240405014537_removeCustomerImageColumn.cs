using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreCustomersWebApi.Migrations
{
    /// <inheritdoc />
    public partial class removeCustomerImageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Customers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
