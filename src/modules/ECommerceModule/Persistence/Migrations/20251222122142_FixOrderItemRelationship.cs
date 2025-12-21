using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItemRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems",
                column: "OrderEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderEntityId",
                schema: "ecommerce",
                table: "OrderItems",
                column: "OrderEntityId",
                principalSchema: "ecommerce",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
