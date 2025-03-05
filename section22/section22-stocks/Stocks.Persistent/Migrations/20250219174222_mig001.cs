using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stocks.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class mig001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrder",
                columns: table => new
                {
                    BuyOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrder", x => x.BuyOrderID);
                    table.CheckConstraint("priceCheck", "Price >= 1 and Price <= 10000");
                    table.CheckConstraint("quantityCheck", "Quantity >= 1 and Quantity <= 100000");
                });

            migrationBuilder.CreateTable(
                name: "SellOrder",
                columns: table => new
                {
                    SellOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateAndTimeOfOrder = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrder", x => x.SellOrderID);
                    table.CheckConstraint("priceCheck1", "Price >= 1 and Price <= 10000");
                    table.CheckConstraint("quantityCheck1", "Quantity >= 1 and Quantity <= 100000");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrder");

            migrationBuilder.DropTable(
                name: "SellOrder");
        }
    }
}
