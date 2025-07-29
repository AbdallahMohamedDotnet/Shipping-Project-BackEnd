using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create VwCities view
            migrationBuilder.Sql(@"
                CREATE VIEW VwCities AS
                SELECT 
                    c.Id,
                    c.CityAName,
                    c.CityEName,
                    c.CountryId,
                    co.CountryAName,
                    co.CountryEName,
                    c.CreatedBy,
                    c.CreatedDate,
                    c.UpdatedBy,
                    c.UpdatedDate,
                    c.CurrentState
                FROM TbCities c
                INNER JOIN TbCountries co ON c.CountryId = co.Id
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop VwCities view
            migrationBuilder.Sql("DROP VIEW IF EXISTS VwCities");
        }
    }
}
