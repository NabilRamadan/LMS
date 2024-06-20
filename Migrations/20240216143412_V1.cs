using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDApi.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "(N'')"),
                    CurrentUserRole = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "(N'')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__760F998498577A3D", x => x.role_ID);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__student__3213E83FDCFE20BD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    university_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Logo_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Universi__F24EBB78CFEB19C5", x => x.university_ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddress_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    course_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    hours = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    image_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course__8F1FF3A6DB1504CD", x => x.course_ID);
                });

            migrationBuilder.CreateTable(
                name: "Course_semester",
                columns: table => new
                {
                    cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Semester_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Course_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course_s__5D924C1901EB9452", x => x.cycle_ID);
                    table.ForeignKey(
                        name: "FK__Course_se__Cours__2739D489",
                        column: x => x.Course_ID,
                        principalTable: "Course",
                        principalColumn: "course_ID");
                });

            migrationBuilder.CreateTable(
                name: "Lecture",
                columns: table => new
                {
                    lecture_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    course_cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lecture__797923ED7F7AD417", x => x.lecture_ID);
                    table.ForeignKey(
                        name: "FK__Lecture__course___2BFE89A6",
                        column: x => x.course_cycle_ID,
                        principalTable: "Course_semester",
                        principalColumn: "cycle_ID");
                });

            migrationBuilder.CreateTable(
                name: "LectureFile",
                columns: table => new
                {
                    Lecture_file_ID = table.Column<int>(type: "int", nullable: false),
                    lecture_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    file_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lecture___69F00415E4FDD298", x => x.Lecture_file_ID);
                    table.ForeignKey(
                        name: "FK__Lecture_f__lectu__2FCF1A8A",
                        column: x => x.lecture_ID,
                        principalTable: "Lecture",
                        principalColumn: "lecture_ID");
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    department_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departme__C22220EA01691B04", x => x.department_ID);
                });

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    university_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    levels = table.Column<int>(type: "int", nullable: false),
                    Logo_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    last_semester_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Faculty__7B06AD84DD5251FD", x => x.faculty_ID);
                    table.ForeignKey(
                        name: "FK__Faculty__univers__06CD04F7",
                        column: x => x.university_ID,
                        principalTable: "University",
                        principalColumn: "university_ID");
                });

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    Semester_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    start_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    years = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    number = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Semester__12459B949BF7782D", x => x.Semester_ID);
                    table.ForeignKey(
                        name: "FK__Semester__facult__22751F6C",
                        column: x => x.faculty_ID,
                        principalTable: "Faculty",
                        principalColumn: "faculty_ID");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    image_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "('ACTIVE')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__B9BF33073B1E48F3", x => x.user_ID);
                    table.ForeignKey(
                        name: "FK__users__Faculty_I__1332DBDC",
                        column: x => x.Faculty_ID,
                        principalTable: "Faculty",
                        principalColumn: "faculty_ID");
                });

            migrationBuilder.CreateTable(
                name: "InstructorCourseSemester",
                columns: table => new
                {
                    instructor_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    course_cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__instruct__A27700A8CD14256D", x => new { x.course_cycle_ID, x.instructor_ID });
                    table.ForeignKey(
                        name: "FK__instructo__cours__24285DB4",
                        column: x => x.course_cycle_ID,
                        principalTable: "Course_semester",
                        principalColumn: "cycle_ID");
                    table.ForeignKey(
                        name: "FK__instructo__instr__2334397B",
                        column: x => x.instructor_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    news_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    faculty_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    user_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__news__4C22D9E0E1CE1E8E", x => x.news_ID);
                    table.ForeignKey(
                        name: "FK__news__faculty_ID__2EA5EC27",
                        column: x => x.faculty_ID,
                        principalTable: "Faculty",
                        principalColumn: "faculty_ID");
                    table.ForeignKey(
                        name: "FK__news__user_ID__2F9A1060",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    quiz_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    notes = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    grade = table.Column<double>(type: "float", nullable: true),
                    course_cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    instructor_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quiz__2D7357F4EB5FE8D8", x => x.quiz_ID);
                    table.ForeignKey(
                        name: "FK__Quiz__course_cyc__42E1EEFE",
                        column: x => x.course_cycle_ID,
                        principalTable: "Course_semester",
                        principalColumn: "cycle_ID");
                    table.ForeignKey(
                        name: "FK__Quiz__instructor__43D61337",
                        column: x => x.instructor_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollment",
                columns: table => new
                {
                    student_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    course_cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student___718E812A36167AA0", x => new { x.student_ID, x.course_cycle_ID });
                    table.ForeignKey(
                        name: "FK__Student_E__cours__3493CFA7",
                        column: x => x.course_cycle_ID,
                        principalTable: "Course_semester",
                        principalColumn: "cycle_ID");
                    table.ForeignKey(
                        name: "FK__Student_E__stude__339FAB6E",
                        column: x => x.student_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "StudentInfo",
                columns: table => new
                {
                    academic_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    user_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    department_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    level = table.Column<int>(type: "int", nullable: false),
                    gpa = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student___CE78F2BC1C1D708B", x => x.academic_ID);
                    table.ForeignKey(
                        name: "FK__Student_i__depar__17F790F9",
                        column: x => x.department_ID,
                        principalTable: "Department",
                        principalColumn: "department_ID");
                    table.ForeignKey(
                        name: "FK__Student_i__user___17036CC0",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    task_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    grade = table.Column<double>(type: "float", nullable: true),
                    file_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    course_cycle_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    instructor_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Task__049318B59168F721", x => x.task_ID);
                    table.ForeignKey(
                        name: "FK__Task__course_cyc__3864608B",
                        column: x => x.course_cycle_ID,
                        principalTable: "Course_semester",
                        principalColumn: "cycle_ID");
                    table.ForeignKey(
                        name: "FK__Task__instructor__395884C4",
                        column: x => x.instructor_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    role_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    user_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_rol__1D946AB40474E7D3", x => new { x.role_ID, x.user_ID });
                    table.ForeignKey(
                        name: "FK__User_role__role___1AD3FDA4",
                        column: x => x.role_ID,
                        principalTable: "Role",
                        principalColumn: "role_ID");
                    table.ForeignKey(
                        name: "FK__User_role__user___1BC821DD",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    question_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    quiz_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    question_number = table.Column<int>(type: "int", nullable: false),
                    grade = table.Column<double>(type: "float", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__2EC9C0317399D9F2", x => x.question_ID);
                    table.ForeignKey(
                        name: "FK__Questions__quiz___47A6A41B",
                        column: x => x.quiz_ID,
                        principalTable: "Quiz",
                        principalColumn: "quiz_ID");
                });

            migrationBuilder.CreateTable(
                name: "StudentQuizGrade",
                columns: table => new
                {
                    student_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    quiz_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    grade = table.Column<double>(type: "float", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student___78DF1E5DF85585A2", x => new { x.student_ID, x.quiz_ID });
                    table.ForeignKey(
                        name: "FK__Student_Q__quiz___55009F39",
                        column: x => x.quiz_ID,
                        principalTable: "Quiz",
                        principalColumn: "quiz_ID");
                    table.ForeignKey(
                        name: "FK__Student_Q__stude__540C7B00",
                        column: x => x.student_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateTable(
                name: "TaskAnswer",
                columns: table => new
                {
                    answer_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    task_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    student_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    file_path = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true, defaultValueSql: "('PENDING')"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Task_ans__3371470048AD5EBA", x => x.answer_ID);
                    table.ForeignKey(
                        name: "FK__Task_answ__stude__3E1D39E1",
                        column: x => x.student_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                    table.ForeignKey(
                        name: "FK__Task_answ__task___3D2915A8",
                        column: x => x.task_ID,
                        principalTable: "Task",
                        principalColumn: "task_ID");
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    answer_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    question_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    text = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    is_correct = table.Column<bool>(type: "bit", nullable: false),
                    answer_number = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__33714700833E0B49", x => x.answer_ID);
                    table.ForeignKey(
                        name: "FK__Question___quest__4B7734FF",
                        column: x => x.question_ID,
                        principalTable: "Question",
                        principalColumn: "question_ID");
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswer",
                columns: table => new
                {
                    answer_ID = table.Column<int>(type: "int", nullable: false),
                    student_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    question_answers_ID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quiz_ans__33714700A3212EA6", x => x.answer_ID);
                    table.ForeignKey(
                        name: "FK__Quiz_answ__quest__503BEA1C",
                        column: x => x.question_answers_ID,
                        principalTable: "QuestionAnswer",
                        principalColumn: "answer_ID");
                    table.ForeignKey(
                        name: "FK__Quiz_answ__stude__4F47C5E3",
                        column: x => x.student_ID,
                        principalTable: "User",
                        principalColumn: "user_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Course_faculty_ID",
                table: "Course",
                column: "faculty_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_semester_Course_ID",
                table: "Course_semester",
                column: "Course_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_semester_Semester_ID",
                table: "Course_semester",
                column: "Semester_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Department_faculty_ID",
                table: "Department",
                column: "faculty_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_last_semester_ID",
                table: "Faculty",
                column: "last_semester_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_university_ID",
                table: "Faculty",
                column: "university_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseSemester_instructor_ID",
                table: "InstructorCourseSemester",
                column: "instructor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_course_cycle_ID",
                table: "Lecture",
                column: "course_cycle_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LectureFile_lecture_ID",
                table: "LectureFile",
                column: "lecture_ID");

            migrationBuilder.CreateIndex(
                name: "IX_news_faculty_ID",
                table: "news",
                column: "faculty_ID");

            migrationBuilder.CreateIndex(
                name: "IX_news_user_ID",
                table: "news",
                column: "user_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_quiz_ID",
                table: "Question",
                column: "quiz_ID");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_question_ID",
                table: "QuestionAnswer",
                column: "question_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_course_cycle_ID",
                table: "Quiz",
                column: "course_cycle_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_instructor_ID",
                table: "Quiz",
                column: "instructor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswer_question_answers_ID",
                table: "QuizAnswer",
                column: "question_answers_ID");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswer_student_ID",
                table: "QuizAnswer",
                column: "student_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Semester_faculty_ID",
                table: "Semester",
                column: "faculty_ID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_course_cycle_ID",
                table: "StudentEnrollment",
                column: "course_cycle_ID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfo_department_ID",
                table: "StudentInfo",
                column: "department_ID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfo_user_ID",
                table: "StudentInfo",
                column: "user_ID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizGrade_quiz_ID",
                table: "StudentQuizGrade",
                column: "quiz_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Task_course_cycle_ID",
                table: "Task",
                column: "course_cycle_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Task_instructor_ID",
                table: "Task",
                column: "instructor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswer_student_ID",
                table: "TaskAnswer",
                column: "student_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswer_task_ID",
                table: "TaskAnswer",
                column: "task_ID");

            migrationBuilder.CreateIndex(
                name: "UQ__Universi__72E12F1BD77946A1",
                table: "University",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Faculty_ID",
                table: "User",
                column: "Faculty_ID");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164094D60C7",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__B43B145FD7D413C2",
                table: "User",
                column: "phone",
                unique: true,
                filter: "[phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_ApplicationUserId",
                table: "UserAddress",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_user_ID",
                table: "UserRole",
                column: "user_ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Course__faculty___607251E5",
                table: "Course",
                column: "faculty_ID",
                principalTable: "Faculty",
                principalColumn: "faculty_ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Course_se__Semes__2645B050",
                table: "Course_semester",
                column: "Semester_ID",
                principalTable: "Semester",
                principalColumn: "Semester_ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Departmen__facul__0A9D95DB",
                table: "Department",
                column: "faculty_ID",
                principalTable: "Faculty",
                principalColumn: "faculty_ID");

            migrationBuilder.AddForeignKey(
                name: "FK__Faculty__last_se__6166761E",
                table: "Faculty",
                column: "last_semester_ID",
                principalTable: "Semester",
                principalColumn: "Semester_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Semester__facult__22751F6C",
                table: "Semester");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "InstructorCourseSemester");

            migrationBuilder.DropTable(
                name: "LectureFile");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "QuizAnswer");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "StudentEnrollment");

            migrationBuilder.DropTable(
                name: "StudentInfo");

            migrationBuilder.DropTable(
                name: "StudentQuizGrade");

            migrationBuilder.DropTable(
                name: "TaskAnswer");

            migrationBuilder.DropTable(
                name: "UserAddress");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Lecture");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "Course_semester");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Faculty");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "University");
        }
    }
}
