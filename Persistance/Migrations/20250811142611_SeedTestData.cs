using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AppRoles ekle
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "AppRoleId", "AppRoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Personnel" }
                });

            // Departments ekle
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

            // Users ekle
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Username", "PasswordHash", "AppRoleId", "DepartmentId" },
                values: new object[,]
                {
                    { 1, "Admin User", "admin", "admin123", 1, null },
                    { 2, "Test User", "user", "user123", 2, null },
                    { 3, "Mehmet Usta", "personnel", "personnel123", 3, 1 },
                    { 4, "Ahmet Tekniker", "ahmet", "ahmet123", 3, 2 },
                    { 5, "Fatma Mühendis", "fatma", "fatma123", 3, 3 }
                });

            // Test Complaints ekle
            migrationBuilder.InsertData(
                table: "Complaints",
                columns: new[] { "Id", "Title", "Description", "CreatedAt", "Status", "UserId", "DepartmentId" },
                values: new object[,]
                {
                    { 1, "Çukur Şikayeti", "Mahallemizde derin çukurlar var, araçlar zarar görüyor.", DateTime.Now.AddDays(-5), "Talep Alındı", 2, 1 },
                    { 2, "Çöp Toplama Sorunu", "Çöp kutusu taşmış, temizlik yapılması gerekiyor.", DateTime.Now.AddDays(-3), "Personel Atandı", 2, 2 },
                    { 3, "Park Sorunu", "Park alanında ağaçlar kesilmiş, yeniden dikilmesi gerekiyor.", DateTime.Now.AddDays(-1), "İşlem Tamamlandı", 2, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Test verilerini sil
            migrationBuilder.DeleteData(
                table: "Complaints",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 });

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "AppRoleId",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}
