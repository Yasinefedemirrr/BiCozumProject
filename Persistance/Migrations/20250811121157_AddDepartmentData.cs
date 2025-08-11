using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Department verilerini ekle
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fen İşleri Müdürlüğü" },
                    { 2, "Temizlik İşleri Müdürlüğü" },
                    { 3, "İmar ve Şehircilik Müdürlüğü" },
                    { 4, "Sosyal Hizmetler Müdürlüğü" },
                    { 5, "Kültür ve Turizm Müdürlüğü" },
                    { 6, "Çevre Koruma Müdürlüğü" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Department verilerini sil
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
