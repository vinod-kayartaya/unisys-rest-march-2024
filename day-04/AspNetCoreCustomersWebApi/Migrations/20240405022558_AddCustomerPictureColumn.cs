using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreCustomersWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPictureColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Customers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Customers");
        }
    }
}
