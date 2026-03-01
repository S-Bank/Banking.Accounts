using System;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Transaction;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Accounts.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:currency", "eur,rub,usd")
                .Annotation("Npgsql:Enum:status", "active,frozen")
                .Annotation("Npgsql:Enum:transaction_type", "deposit,withdraw");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<Currency>(type: "currency", nullable: false),
                    Status = table.Column<Status>(type: "status", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outboxs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "jsonb", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ErrorCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outboxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outboxs_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<TransactionType>(type: "transaction_type", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    currency = table.Column<Currency>(type: "currency", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outboxs_AccountId",
                table: "Outboxs",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Outboxs_ProcessedOn_OccurredOn",
                table: "Outboxs",
                columns: new[] { "ProcessedOn", "OccurredOn" },
                filter: "\"ProcessedOn\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId_CreatedAt",
                table: "Transactions",
                columns: new[] { "AccountId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedAt",
                table: "Transactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReferenceId",
                table: "Transactions",
                column: "ReferenceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outboxs");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
