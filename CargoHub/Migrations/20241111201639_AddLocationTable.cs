using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CargoHub.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WarehouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: true),
                    ExtraReference = table.Column<string>(type: "TEXT", nullable: true),
                    OrderStatus = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    ShippingNotes = table.Column<string>(type: "TEXT", nullable: true),
                    PickingNotes = table.Column<string>(type: "TEXT", nullable: true),
                    WarehouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipTo = table.Column<int>(type: "INTEGER", nullable: false),
                    BillTo = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalSurcharge = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Orderdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShipmentType = table.Column<string>(type: "TEXT", nullable: true),
                    ShipmentStatus = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CarrierCode = table.Column<string>(type: "TEXT", nullable: true),
                    CarrierDescription = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceCode = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentType = table.Column<string>(type: "TEXT", nullable: true),
                    TransferMode = table.Column<string>(type: "TEXT", nullable: true),
                    TotalPackageCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPackageWeight = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uid = table.Column<string>(type: "TEXT", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    UpcCode = table.Column<string>(type: "TEXT", nullable: true),
                    ModelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CommodityCode = table.Column<string>(type: "TEXT", nullable: true),
                    ItemLine = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemType = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPurchaseQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitOrderQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PackOrderQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierCode = table.Column<string>(type: "TEXT", nullable: true),
                    SupplierPartNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShipmentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Code", "CommodityCode", "CreatedAt", "Description", "ItemGroup", "ItemLine", "ItemType", "ModelNumber", "PackOrderQuantity", "ShipmentId", "ShortDescription", "SupplierCode", "SupplierId", "SupplierPartNumber", "Uid", "UnitOrderQuantity", "UnitPurchaseQuantity", "UpcCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Item1", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "First item", 0, 0, 0, null, 0, null, null, null, 0, null, "P007435", 23, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Item2", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Second item", 0, 0, 0, null, 0, null, null, null, 0, null, "P009557", 1, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Item3", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Third item", 0, 0, 0, null, 0, null, null, null, 0, null, "P009553", 50, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "BillTo", "CreatedAt", "ExtraReference", "Notes", "OrderDate", "OrderStatus", "PickingNotes", "Reference", "RequestDate", "ShipTo", "ShipmentId", "ShippingNotes", "SourceId", "TotalAmount", "TotalDiscount", "TotalSurcharge", "TotalTax", "UpdatedAt", "WarehouseId" },
                values: new object[] { 1, 0, new DateTime(2019, 4, 3, 11, 33, 15, 0, DateTimeKind.Unspecified), "Bedreven arm straffen bureau.", "Voedsel vijf vork heel.", new DateTime(2019, 4, 3, 11, 33, 15, 0, DateTimeKind.Unspecified), "Delivered", "Ademen fijn volgorde scherp aardappel op leren.", "ORD00001", new DateTime(2019, 4, 7, 11, 33, 15, 0, DateTimeKind.Unspecified), 0, 1, "Buurman betalen plaats bewolkt.", 33, 9905.13m, 150.77m, 77.6m, 372.72m, new DateTime(2019, 4, 5, 7, 33, 15, 0, DateTimeKind.Unspecified), 18 });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "ItemId", "OrderId", "Amount" },
                values: new object[,]
                {
                    { 1, 1, 23 },
                    { 2, 1, 1 },
                    { 3, 1, 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ShipmentId",
                table: "Items",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemId",
                table: "OrderItems",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses",
                column: "ContactId",
                unique: true);
        }
    }
}
