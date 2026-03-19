using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NextOps.Api.Migrations
{
    /// <inheritdoc />
    public partial class DataFlowProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_claim_user_user_id",
                table: "user_claim");

            migrationBuilder.DropForeignKey(
                name: "fk_user_login_user_user_id",
                table: "user_login");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_user_user_id",
                table: "user_role");

            migrationBuilder.DropForeignKey(
                name: "fk_user_token_user_user_id",
                table: "user_token");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "user");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "user");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "user");

            migrationBuilder.DropColumn(
                name: "access_failed_count",
                table: "user");

            migrationBuilder.DropColumn(
                name: "email",
                table: "user");

            migrationBuilder.DropColumn(
                name: "email_confirmed",
                table: "user");

            migrationBuilder.DropColumn(
                name: "lockout_enabled",
                table: "user");

            migrationBuilder.DropColumn(
                name: "lockout_end",
                table: "user");

            migrationBuilder.DropColumn(
                name: "normalized_email",
                table: "user");

            migrationBuilder.DropColumn(
                name: "normalized_user_name",
                table: "user");

            migrationBuilder.DropColumn(
                name: "phone_number_confirmed",
                table: "user");

            migrationBuilder.DropColumn(
                name: "user_name",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                table: "user",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                table: "user",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "user",
                newName: "second_lastname");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "user",
                newName: "middle_name");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "user",
                newName: "last_name");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: true),
                    brand = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    average_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    stock = table.Column<int>(type: "integer", nullable: true),
                    categoty_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_category_id",
                table: "product",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_asp_net_users_id",
                table: "user",
                column: "id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claim_asp_net_users_user_id",
                table: "user_claim",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_login_asp_net_users_user_id",
                table: "user_login",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_asp_net_users_user_id",
                table: "user_role",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_token_asp_net_users_user_id",
                table: "user_token",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_asp_net_users_id",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "fk_user_claim_asp_net_users_user_id",
                table: "user_claim");

            migrationBuilder.DropForeignKey(
                name: "fk_user_login_asp_net_users_user_id",
                table: "user_login");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_asp_net_users_user_id",
                table: "user_role");

            migrationBuilder.DropForeignKey(
                name: "fk_user_token_asp_net_users_user_id",
                table: "user_token");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "user",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "user",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "second_lastname",
                table: "user",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "middle_name",
                table: "user",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "user",
                newName: "concurrency_stamp");

            migrationBuilder.AddColumn<int>(
                name: "access_failed_count",
                table: "user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "user",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "email_confirmed",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "lockout_enabled",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "lockout_end",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "normalized_email",
                table: "user",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "normalized_user_name",
                table: "user",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "phone_number_confirmed",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "user_name",
                table: "user",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "user",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claim_user_user_id",
                table: "user_claim",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_login_user_user_id",
                table: "user_login",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_user_user_id",
                table: "user_role",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_token_user_user_id",
                table: "user_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
