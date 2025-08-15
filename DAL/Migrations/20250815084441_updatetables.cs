using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingDate",
                table: "TbShippments",
                newName: "ShipingDate");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserSebders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefualt",
                table: "TbUserSebders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserSebders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserSebders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DelivryDate",
                table: "TbShippments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ShipingPackgingId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbShipingPackging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipingPackgingAname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipingPackgingEname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipingPackging", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShipingPackgingId",
                table: "TbShippments",
                column: "ShipingPackgingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShipingPackging_ShipingPackgingId",
                table: "TbShippments",
                column: "ShipingPackgingId",
                principalTable: "TbShipingPackging",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShipingPackging_ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropTable(
                name: "TbShipingPackging");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserSebders");

            migrationBuilder.DropColumn(
                name: "IsDefualt",
                table: "TbUserSebders");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserSebders");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserSebders");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "DelivryDate",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.RenameColumn(
                name: "ShipingDate",
                table: "TbShippments",
                newName: "ShippingDate");
        }
    }
}
