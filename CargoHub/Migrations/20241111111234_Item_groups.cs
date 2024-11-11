using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoHub.Migrations
{
    /// <inheritdoc />
    public partial class Item_groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6197), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6200) });

            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6203), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6206), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6206) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6319), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6320) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6323), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6324) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6326), new DateTime(2024, 11, 11, 11, 12, 34, 507, DateTimeKind.Utc).AddTicks(6327) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1678), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1680) });

            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1683), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1684) });

            migrationBuilder.UpdateData(
                table: "ItemGroups",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1686), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1687) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1797), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1798) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1801), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1802) });

            migrationBuilder.UpdateData(
                table: "ItemLines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1804), new DateTime(2024, 11, 11, 11, 5, 2, 939, DateTimeKind.Utc).AddTicks(1804) });
        }
    }
}
