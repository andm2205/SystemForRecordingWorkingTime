using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemForRecordingWorkingTime.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    ApplicantUserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "date", nullable: false),
                    ApprovingUserId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayOffAtTheExpenseOfVacationRequest_ReplacementUserId = table.Column<int>(type: "int", nullable: true),
                    DayOffAtTheExpenseOfWorkingOutRequest_ReplacementUserId = table.Column<int>(type: "int", nullable: true),
                    ReplacementUserId = table.Column<int>(type: "int", nullable: true),
                    VacationType = table.Column<int>(type: "int", nullable: true),
                    VacationRequest_ReplacementUserId = table.Column<int>(type: "int", nullable: true),
                    Movable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_Users_ApplicantUserId",
                        column: x => x.ApplicantUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_Users_ApprovingUserId",
                        column: x => x.ApprovingUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_Users_DayOffAtTheExpenseOfVacationRequest_ReplacementUserId",
                        column: x => x.DayOffAtTheExpenseOfVacationRequest_ReplacementUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_Users_DayOffAtTheExpenseOfWorkingOutRequest_ReplacementUserId",
                        column: x => x.DayOffAtTheExpenseOfWorkingOutRequest_ReplacementUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_Users_ReplacementUserId",
                        column: x => x.ReplacementUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_Users_VacationRequest_ReplacementUserId",
                        column: x => x.VacationRequest_ReplacementUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatedDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<DateTime>(type: "date", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatedDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatedDates_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingOutDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<DateTime>(type: "date", nullable: false),
                    DayOffAtTheExpenseOfWorkingOutRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingOutDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingOutDates_Request_DayOffAtTheExpenseOfWorkingOutRequestId",
                        column: x => x.DayOffAtTheExpenseOfWorkingOutRequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemoteWorkRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlans_Request_RemoteWorkRequestId",
                        column: x => x.RemoteWorkRequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_ApplicantUserId",
                table: "Request",
                column: "ApplicantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ApprovingUserId",
                table: "Request",
                column: "ApprovingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_DayOffAtTheExpenseOfVacationRequest_ReplacementUserId",
                table: "Request",
                column: "DayOffAtTheExpenseOfVacationRequest_ReplacementUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_DayOffAtTheExpenseOfWorkingOutRequest_ReplacementUserId",
                table: "Request",
                column: "DayOffAtTheExpenseOfWorkingOutRequest_ReplacementUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ReplacementUserId",
                table: "Request",
                column: "ReplacementUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_VacationRequest_ReplacementUserId",
                table: "Request",
                column: "VacationRequest_ReplacementUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatedDates_RequestId",
                table: "StatedDates",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingOutDates_DayOffAtTheExpenseOfWorkingOutRequestId",
                table: "WorkingOutDates",
                column: "DayOffAtTheExpenseOfWorkingOutRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_RemoteWorkRequestId",
                table: "WorkPlans",
                column: "RemoteWorkRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatedDates");

            migrationBuilder.DropTable(
                name: "WorkingOutDates");

            migrationBuilder.DropTable(
                name: "WorkPlans");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
