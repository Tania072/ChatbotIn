using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba2.Migrations
{
    /// <inheritdoc />
    public partial class AddChatbotFieldsToProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionCorta",
                table: "Productos",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Destacado",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnPromocion",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Etiquetas",
                table: "Productos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioDescuento",
                table: "Productos",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAgregado",
                table: "CarritoItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Destacado",
                table: "Productos",
                column: "Destacado");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_EnPromocion",
                table: "Productos",
                column: "EnPromocion");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Nombre_DescripcionCorta_Etiquetas",
                table: "Productos",
                columns: new[] { "Nombre", "DescripcionCorta", "Etiquetas" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Productos_Destacado",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_EnPromocion",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Nombre_DescripcionCorta_Etiquetas",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "DescripcionCorta",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Destacado",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "EnPromocion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Etiquetas",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioDescuento",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Productos",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAgregado",
                table: "CarritoItems",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");
        }
    }
}
