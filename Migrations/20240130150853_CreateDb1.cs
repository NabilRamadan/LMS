using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Years = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    UniversityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.UniversityId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CourseSemesters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSemesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSemesters_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSemesters_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSemesters_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesRoleId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Gpa = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInfos_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_StudentInfos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    LectureId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCycleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.LectureId);
                    table.ForeignKey(
                        name: "FK_Lectures_CourseSemesters_CourseCycleId",
                        column: x => x.CourseCycleId,
                        principalTable: "CourseSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: true),
                    CourseCycleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_CourseSemesters_CourseCycleId",
                        column: x => x.CourseCycleId,
                        principalTable: "CourseSemesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quizzes_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCycleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentEnrollments_CourseSemesters_CourseCycleId",
                        column: x => x.CourseCycleId,
                        principalTable: "CourseSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentEnrollments_Users_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCycleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_CourseSemesters_CourseCycleId",
                        column: x => x.CourseCycleId,
                        principalTable: "CourseSemesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "LectureFiles",
                columns: table => new
                {
                    LectureFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureFiles", x => x.LectureFileId);
                    table.ForeignKey(
                        name: "FK_LectureFiles_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "LectureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId");
                });

            migrationBuilder.CreateTable(
                name: "StudentQuizGrades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentQuizGrades_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentQuizGrades_Users_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TaskAnswers",
                columns: table => new
                {
                    TaskAnswerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAnswers", x => x.TaskAnswerId);
                    table.ForeignKey(
                        name: "FK_TaskAnswers_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId");
                    table.ForeignKey(
                        name: "FK_TaskAnswers_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    AnswerNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionAnswersId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAnswers_QuestionAnswers_QuestionAnswersId",
                        column: x => x.QuestionAnswersId,
                        principalTable: "QuestionAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizAnswers_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemesters_CourseId",
                table: "CourseSemesters",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemesters_InstructorId",
                table: "CourseSemesters",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemesters_SemesterId",
                table: "CourseSemesters",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureFiles_LectureId",
                table: "LectureFiles",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseCycleId",
                table: "Lectures",
                column: "CourseCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_QuestionAnswersId",
                table: "QuizAnswers",
                column: "QuestionAnswersId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_StudentId",
                table: "QuizAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CourseCycleId",
                table: "Quizzes",
                column: "CourseCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_InstructorId",
                table: "Quizzes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersUserId",
                table: "RoleUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_CourseCycleId",
                table: "StudentEnrollments",
                column: "CourseCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_StudentUserId",
                table: "StudentEnrollments",
                column: "StudentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_DepartmentId",
                table: "StudentInfos",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_UserId",
                table: "StudentInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizGrades_QuizId",
                table: "StudentQuizGrades",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizGrades_StudentUserId",
                table: "StudentQuizGrades",
                column: "StudentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_TaskId",
                table: "TaskAnswers",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CourseCycleId",
                table: "Tasks",
                column: "CourseCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_InstructorId",
                table: "Tasks",
                column: "InstructorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LectureFiles");

            migrationBuilder.DropTable(
                name: "QuizAnswers");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "StudentEnrollments");

            migrationBuilder.DropTable(
                name: "StudentInfos");

            migrationBuilder.DropTable(
                name: "StudentQuizGrades");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "TaskAnswers");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "CourseSemesters");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
