CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [DisplayName] nvarchar(max) NOT NULL DEFAULT ((N'')),
    [CurrentUserRole] nvarchar(max) NOT NULL DEFAULT ((N'')),
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Role] (
    [role_ID] varchar(50) NOT NULL,
    [name] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Role__760F998498577A3D] PRIMARY KEY ([role_ID])
);
GO


CREATE TABLE [University] (
    [university_ID] varchar(50) NOT NULL,
    [name] nvarchar(255) NOT NULL,
    [Logo_path] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Universi__F24EBB78CFEB19C5] PRIMARY KEY ([university_ID])
);
GO


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [UserAddress] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ApplicationUserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_UserAddress] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserAddress_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Calender] (
    [calender_ID] varchar(50) NOT NULL,
    [user_ID] varchar(50) NULL,
    [start_date] datetime2 NOT NULL,
    [end_date] datetime2 NOT NULL,
    [body] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Calender] PRIMARY KEY ([calender_ID])
);
GO


CREATE TABLE [Course] (
    [course_ID] varchar(50) NOT NULL,
    [name] varchar(255) NOT NULL,
    [hours] int NULL DEFAULT (((0))),
    [created_at] datetime NULL DEFAULT ((getdate())),
    [image_path] varchar(255) NULL,
    [faculty_ID] varchar(50) NULL,
    CONSTRAINT [PK__Course__8F1FF3A6DB1504CD] PRIMARY KEY ([course_ID])
);
GO


CREATE TABLE [Course_semester] (
    [cycle_ID] varchar(50) NOT NULL,
    [Semester_ID] varchar(50) NOT NULL,
    [Course_ID] varchar(50) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Course_s__5D924C1901EB9452] PRIMARY KEY ([cycle_ID]),
    CONSTRAINT [FK__Course_se__Cours__2739D489] FOREIGN KEY ([Course_ID]) REFERENCES [Course] ([course_ID])
);
GO


CREATE TABLE [Lecture] (
    [lecture_ID] varchar(50) NOT NULL,
    [course_cycle_ID] varchar(50) NOT NULL,
    [title] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    [type] nvarchar(50) NULL,
    CONSTRAINT [PK__Lecture__797923ED7F7AD417] PRIMARY KEY ([lecture_ID]),
    CONSTRAINT [FK__Lecture__course___2BFE89A6] FOREIGN KEY ([course_cycle_ID]) REFERENCES [Course_semester] ([cycle_ID])
);
GO


CREATE TABLE [Lecture_file] (
    [Lecture_file_ID] int NOT NULL,
    [lecture_ID] varchar(50) NOT NULL,
    [file_path] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    [name] nvarchar(200) NULL,
    CONSTRAINT [PK__Lecture___69F00415E4FDD298] PRIMARY KEY ([Lecture_file_ID]),
    CONSTRAINT [FK__Lecture_f__lectu__2FCF1A8A] FOREIGN KEY ([lecture_ID]) REFERENCES [Lecture] ([lecture_ID])
);
GO


CREATE TABLE [Department] (
    [department_ID] varchar(50) NOT NULL,
    [faculty_ID] varchar(50) NULL,
    [name] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Departme__C22220EA01691B04] PRIMARY KEY ([department_ID])
);
GO


CREATE TABLE [Faculty] (
    [faculty_ID] varchar(50) NOT NULL,
    [university_ID] varchar(50) NULL,
    [name] nvarchar(255) NOT NULL,
    [levels] int NOT NULL,
    [Logo_path] varchar(255) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    [last_semester_ID] varchar(50) NULL,
    CONSTRAINT [PK__Faculty__7B06AD84DD5251FD] PRIMARY KEY ([faculty_ID]),
    CONSTRAINT [FK__Faculty__univers__06CD04F7] FOREIGN KEY ([university_ID]) REFERENCES [University] ([university_ID])
);
GO


CREATE TABLE [Semester] (
    [Semester_ID] varchar(50) NOT NULL,
    [faculty_ID] varchar(50) NULL,
    [start_Date] date NOT NULL,
    [end_Date] date NOT NULL,
    [years] varchar(50) NOT NULL,
    [number] int NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Semester__12459B949BF7782D] PRIMARY KEY ([Semester_ID]),
    CONSTRAINT [FK__Semester__facult__22751F6C] FOREIGN KEY ([faculty_ID]) REFERENCES [Faculty] ([faculty_ID])
);
GO


CREATE TABLE [users] (
    [user_ID] varchar(50) NOT NULL,
    [full_name] nvarchar(255) NOT NULL,
    [email] varchar(255) NOT NULL,
    [password] varchar(255) NOT NULL,
    [phone] varchar(20) NULL,
    [image_path] varchar(255) NULL,
    [Faculty_ID] varchar(50) NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    [status] varchar(50) NULL DEFAULT (('ACTIVE')),
    CONSTRAINT [PK__users__B9BF33073B1E48F3] PRIMARY KEY ([user_ID]),
    CONSTRAINT [FK__users__Faculty_I__1332DBDC] FOREIGN KEY ([Faculty_ID]) REFERENCES [Faculty] ([faculty_ID])
);
GO


CREATE TABLE [instructor_course_semester] (
    [instructor_ID] varchar(50) NOT NULL,
    [course_cycle_ID] varchar(50) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__instruct__A27700A8CD14256D] PRIMARY KEY ([course_cycle_ID], [instructor_ID]),
    CONSTRAINT [FK__instructo__cours__24285DB4] FOREIGN KEY ([course_cycle_ID]) REFERENCES [Course_semester] ([cycle_ID]),
    CONSTRAINT [FK__instructo__instr__2334397B] FOREIGN KEY ([instructor_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [news] (
    [news_ID] varchar(50) NOT NULL,
    [content] nvarchar(255) NOT NULL,
    [file_path] varchar(255) NULL,
    [faculty_ID] varchar(50) NULL,
    [user_ID] varchar(50) NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__news__4C22D9E0E1CE1E8E] PRIMARY KEY ([news_ID]),
    CONSTRAINT [FK__news__faculty_ID__2EA5EC27] FOREIGN KEY ([faculty_ID]) REFERENCES [Faculty] ([faculty_ID]),
    CONSTRAINT [FK__news__user_ID__2F9A1060] FOREIGN KEY ([user_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Quiz] (
    [quiz_ID] varchar(50) NOT NULL,
    [title] varchar(255) NOT NULL,
    [notes] varchar(500) NULL,
    [start_date] datetime NULL,
    [end_date] datetime NULL,
    [grade] float NULL,
    [course_cycle_ID] varchar(50) NULL,
    [instructor_ID] varchar(50) NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Quiz__2D7357F4EB5FE8D8] PRIMARY KEY ([quiz_ID]),
    CONSTRAINT [FK__Quiz__course_cyc__42E1EEFE] FOREIGN KEY ([course_cycle_ID]) REFERENCES [Course_semester] ([cycle_ID]),
    CONSTRAINT [FK__Quiz__instructor__43D61337] FOREIGN KEY ([instructor_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Student_Enrollment] (
    [student_ID] varchar(50) NOT NULL,
    [course_cycle_ID] varchar(50) NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Student___718E812A36167AA0] PRIMARY KEY ([student_ID], [course_cycle_ID]),
    CONSTRAINT [FK__Student_E__cours__3493CFA7] FOREIGN KEY ([course_cycle_ID]) REFERENCES [Course_semester] ([cycle_ID]),
    CONSTRAINT [FK__Student_E__stude__339FAB6E] FOREIGN KEY ([student_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Student_info] (
    [academic_ID] varchar(50) NOT NULL,
    [user_ID] varchar(50) NULL,
    [department_ID] varchar(50) NULL,
    [level] int NOT NULL,
    [gpa] float NULL,
    CONSTRAINT [PK__Student___CE78F2BC1C1D708B] PRIMARY KEY ([academic_ID]),
    CONSTRAINT [FK__Student_i__depar__17F790F9] FOREIGN KEY ([department_ID]) REFERENCES [Department] ([department_ID]),
    CONSTRAINT [FK__Student_i__user___17036CC0] FOREIGN KEY ([user_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Task] (
    [task_ID] varchar(50) NOT NULL,
    [title] varchar(255) NOT NULL,
    [start_date] datetime NULL,
    [end_date] datetime NULL,
    [grade] float NULL,
    [file_path] varchar(255) NOT NULL,
    [course_cycle_ID] varchar(50) NULL,
    [instructor_ID] varchar(50) NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Task__049318B59168F721] PRIMARY KEY ([task_ID]),
    CONSTRAINT [FK__Task__course_cyc__3864608B] FOREIGN KEY ([course_cycle_ID]) REFERENCES [Course_semester] ([cycle_ID]),
    CONSTRAINT [FK__Task__instructor__395884C4] FOREIGN KEY ([instructor_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [User_role] (
    [role_ID] varchar(50) NOT NULL,
    [user_ID] varchar(50) NOT NULL,
    CONSTRAINT [PK__User_rol__1D946AB40474E7D3] PRIMARY KEY ([role_ID], [user_ID]),
    CONSTRAINT [FK__User_role__role___1AD3FDA4] FOREIGN KEY ([role_ID]) REFERENCES [Role] ([role_ID]),
    CONSTRAINT [FK__User_role__user___1BC821DD] FOREIGN KEY ([user_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Questions] (
    [question_ID] varchar(50) NOT NULL,
    [quiz_ID] varchar(50) NULL,
    [text] varchar(255) NOT NULL,
    [type] varchar(50) NOT NULL,
    [question_number] int NOT NULL,
    [grade] float NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Question__2EC9C0317399D9F2] PRIMARY KEY ([question_ID]),
    CONSTRAINT [FK__Questions__quiz___47A6A41B] FOREIGN KEY ([quiz_ID]) REFERENCES [Quiz] ([quiz_ID])
);
GO


CREATE TABLE [Student_Quiz_Grade] (
    [student_ID] varchar(50) NOT NULL,
    [quiz_ID] varchar(50) NOT NULL,
    [grade] float NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Student___78DF1E5DF85585A2] PRIMARY KEY ([student_ID], [quiz_ID]),
    CONSTRAINT [FK__Student_Q__quiz___55009F39] FOREIGN KEY ([quiz_ID]) REFERENCES [Quiz] ([quiz_ID]),
    CONSTRAINT [FK__Student_Q__stude__540C7B00] FOREIGN KEY ([student_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE TABLE [Task_answer] (
    [answer_ID] varchar(50) NOT NULL,
    [task_ID] varchar(50) NULL,
    [student_ID] varchar(50) NULL,
    [name] nvarchar(max) NULL,
    [grade] float NULL,
    [file_path] varchar(255) NOT NULL,
    [status] varchar(40) NULL DEFAULT (('PENDING')),
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Task_ans__3371470048AD5EBA] PRIMARY KEY ([answer_ID]),
    CONSTRAINT [FK__Task_answ__stude__3E1D39E1] FOREIGN KEY ([student_ID]) REFERENCES [users] ([user_ID]),
    CONSTRAINT [FK__Task_answ__task___3D2915A8] FOREIGN KEY ([task_ID]) REFERENCES [Task] ([task_ID])
);
GO


CREATE TABLE [Question_answers] (
    [answer_ID] varchar(50) NOT NULL,
    [question_ID] varchar(50) NULL,
    [text] varchar(255) NULL,
    [is_correct] bit NOT NULL,
    [answer_number] int NOT NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Question__33714700833E0B49] PRIMARY KEY ([answer_ID]),
    CONSTRAINT [FK__Question___quest__4B7734FF] FOREIGN KEY ([question_ID]) REFERENCES [Questions] ([question_ID])
);
GO


CREATE TABLE [Quiz_answers] (
    [answer_ID] nvarchar(450) NOT NULL,
    [student_ID] varchar(50) NULL,
    [question_answers_ID] varchar(50) NULL,
    [created_at] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Quiz_ans__33714700A3212EA6] PRIMARY KEY ([answer_ID]),
    CONSTRAINT [FK__Quiz_answ__quest__503BEA1C] FOREIGN KEY ([question_answers_ID]) REFERENCES [Question_answers] ([answer_ID]),
    CONSTRAINT [FK__Quiz_answ__stude__4F47C5E3] FOREIGN KEY ([student_ID]) REFERENCES [users] ([user_ID])
);
GO


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE ([NormalizedName] IS NOT NULL);
GO


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE ([NormalizedUserName] IS NOT NULL);
GO


CREATE INDEX [IX_Calender_user_ID] ON [Calender] ([user_ID]);
GO


CREATE INDEX [IX_Course_faculty_ID] ON [Course] ([faculty_ID]);
GO


CREATE INDEX [IX_Course_semester_Course_ID] ON [Course_semester] ([Course_ID]);
GO


CREATE INDEX [IX_Course_semester_Semester_ID] ON [Course_semester] ([Semester_ID]);
GO


CREATE INDEX [IX_Department_faculty_ID] ON [Department] ([faculty_ID]);
GO


CREATE INDEX [IX_Faculty_last_semester_ID] ON [Faculty] ([last_semester_ID]);
GO


CREATE INDEX [IX_Faculty_university_ID] ON [Faculty] ([university_ID]);
GO


CREATE INDEX [IX_instructor_course_semester_instructor_ID] ON [instructor_course_semester] ([instructor_ID]);
GO


CREATE INDEX [IX_Lecture_course_cycle_ID] ON [Lecture] ([course_cycle_ID]);
GO


CREATE INDEX [IX_Lecture_file_lecture_ID] ON [Lecture_file] ([lecture_ID]);
GO


CREATE INDEX [IX_news_faculty_ID] ON [news] ([faculty_ID]);
GO


CREATE INDEX [IX_news_user_ID] ON [news] ([user_ID]);
GO


CREATE INDEX [IX_Question_answers_question_ID] ON [Question_answers] ([question_ID]);
GO


CREATE INDEX [IX_Questions_quiz_ID] ON [Questions] ([quiz_ID]);
GO


CREATE INDEX [IX_Quiz_course_cycle_ID] ON [Quiz] ([course_cycle_ID]);
GO


CREATE INDEX [IX_Quiz_instructor_ID] ON [Quiz] ([instructor_ID]);
GO


CREATE INDEX [IX_Quiz_answers_question_answers_ID] ON [Quiz_answers] ([question_answers_ID]);
GO


CREATE INDEX [IX_Quiz_answers_student_ID] ON [Quiz_answers] ([student_ID]);
GO


CREATE INDEX [IX_Semester_faculty_ID] ON [Semester] ([faculty_ID]);
GO


CREATE INDEX [IX_Student_Enrollment_course_cycle_ID] ON [Student_Enrollment] ([course_cycle_ID]);
GO


CREATE INDEX [IX_Student_info_department_ID] ON [Student_info] ([department_ID]);
GO


CREATE INDEX [IX_Student_info_user_ID] ON [Student_info] ([user_ID]);
GO


CREATE INDEX [IX_Student_Quiz_Grade_quiz_ID] ON [Student_Quiz_Grade] ([quiz_ID]);
GO


CREATE INDEX [IX_Task_course_cycle_ID] ON [Task] ([course_cycle_ID]);
GO


CREATE INDEX [IX_Task_instructor_ID] ON [Task] ([instructor_ID]);
GO


CREATE INDEX [IX_Task_answer_student_ID] ON [Task_answer] ([student_ID]);
GO


CREATE INDEX [IX_Task_answer_task_ID] ON [Task_answer] ([task_ID]);
GO


CREATE UNIQUE INDEX [UQ__Universi__72E12F1BD77946A1] ON [University] ([name]);
GO


CREATE INDEX [IX_User_role_user_ID] ON [User_role] ([user_ID]);
GO


CREATE UNIQUE INDEX [IX_UserAddress_ApplicationUserId] ON [UserAddress] ([ApplicationUserId]);
GO


CREATE INDEX [IX_users_Faculty_ID] ON [users] ([Faculty_ID]);
GO


CREATE UNIQUE INDEX [UQ__users__AB6E6164094D60C7] ON [users] ([email]);
GO


CREATE UNIQUE INDEX [UQ__users__B43B145FD7D413C2] ON [users] ([phone]) WHERE [phone] IS NOT NULL;
GO


ALTER TABLE [Calender] ADD CONSTRAINT [FK_Calender_users_user_ID] FOREIGN KEY ([user_ID]) REFERENCES [users] ([user_ID]);
GO


ALTER TABLE [Course] ADD CONSTRAINT [FK__Course__faculty___607251E5] FOREIGN KEY ([faculty_ID]) REFERENCES [Faculty] ([faculty_ID]);
GO


ALTER TABLE [Course_semester] ADD CONSTRAINT [FK__Course_se__Semes__2645B050] FOREIGN KEY ([Semester_ID]) REFERENCES [Semester] ([Semester_ID]);
GO


ALTER TABLE [Department] ADD CONSTRAINT [FK__Departmen__facul__0A9D95DB] FOREIGN KEY ([faculty_ID]) REFERENCES [Faculty] ([faculty_ID]);
GO


ALTER TABLE [Faculty] ADD CONSTRAINT [FK__Faculty__last_se__6166761E] FOREIGN KEY ([last_semester_ID]) REFERENCES [Semester] ([Semester_ID]);
GO