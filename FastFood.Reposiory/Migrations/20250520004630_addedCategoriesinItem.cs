using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Reposiory.Migrations
{
    /// <inheritdoc />
    public partial class addedCategoriesinItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_SubCategories_SubCategoryId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SubCategoryId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SubCategoryId1",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemSubCategory",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSubCategory", x => new { x.ItemsId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_ItemSubCategory_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSubCategory_SubCategories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ItemId",
                table: "Categories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSubCategory_SubCategoriesId",
                table: "ItemSubCategory",
                column: "SubCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Items_ItemId",
                table: "Categories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Items_ItemId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "ItemSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ItemId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_SubCategoryId1",
                table: "Items",
                column: "SubCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_SubCategories_SubCategoryId1",
                table: "Items",
                column: "SubCategoryId1",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }
    }
}
