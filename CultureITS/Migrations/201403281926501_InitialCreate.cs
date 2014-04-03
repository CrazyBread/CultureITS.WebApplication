namespace CultureITS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserRole = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Photo = c.Binary(),
                        PhotoMime = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exhibits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Location = c.String(nullable: false),
                        ApplicationType = c.String(),
                        ApplicationData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        ApplicationType = c.String(),
                        ApplicationData = c.Binary(),
                        Exhibit_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exhibits", t => t.Exhibit_Id, cascadeDelete: true)
                .Index(t => t.Exhibit_Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.String(),
                        Date = c.DateTime(nullable: false),
                        Percent = c.Double(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        QuestionsLeft = c.Int(nullable: false),
                        Student_Id = c.Int(nullable: false),
                        Test_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.TestMains", t => t.Test_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Test_Id);
            
            CreateTable(
                "dbo.TestMains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Topic = c.String(nullable: false),
                        Author = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        ApplicationType = c.String(),
                        ApplicationData = c.Binary(),
                        Test_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestMains", t => t.Test_Id, cascadeDelete: true)
                .Index(t => t.Test_Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Right = c.Boolean(nullable: false),
                        ApplicationType = c.String(),
                        ApplicationData = c.Binary(),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.GameItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Area = c.String(),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        AdditionalUrl = c.String(),
                        Order = c.Int(nullable: false),
                        AccessMask = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccessRights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Area = c.String(),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        Role = c.Int(nullable: false),
                        IsAllowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExhibitStudents",
                c => new
                    {
                        Exhibit_Id = c.Int(nullable: false),
                        Student_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exhibit_Id, t.Student_Id })
                .ForeignKey("dbo.Exhibits", t => t.Exhibit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Exhibit_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.GameItemStudents",
                c => new
                    {
                        GameItem_Id = c.Int(nullable: false),
                        Student_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameItem_Id, t.Student_Id })
                .ForeignKey("dbo.GameItems", t => t.GameItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.GameItem_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Telephone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        University = c.String(nullable: false),
                        Department = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Course = c.Int(nullable: false),
                        Group = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", new[] { "Id" });
            DropIndex("dbo.Teachers", new[] { "Id" });
            DropIndex("dbo.Administrators", new[] { "Id" });
            DropIndex("dbo.GameItemStudents", new[] { "Student_Id" });
            DropIndex("dbo.GameItemStudents", new[] { "GameItem_Id" });
            DropIndex("dbo.ExhibitStudents", new[] { "Student_Id" });
            DropIndex("dbo.ExhibitStudents", new[] { "Exhibit_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Questions", new[] { "Test_Id" });
            DropIndex("dbo.Sessions", new[] { "Test_Id" });
            DropIndex("dbo.Sessions", new[] { "Student_Id" });
            DropIndex("dbo.Articles", new[] { "Exhibit_Id" });
            DropForeignKey("dbo.Students", "Id", "dbo.Users");
            DropForeignKey("dbo.Teachers", "Id", "dbo.Users");
            DropForeignKey("dbo.Administrators", "Id", "dbo.Users");
            DropForeignKey("dbo.GameItemStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.GameItemStudents", "GameItem_Id", "dbo.GameItems");
            DropForeignKey("dbo.ExhibitStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.ExhibitStudents", "Exhibit_Id", "dbo.Exhibits");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Test_Id", "dbo.TestMains");
            DropForeignKey("dbo.Sessions", "Test_Id", "dbo.TestMains");
            DropForeignKey("dbo.Sessions", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Articles", "Exhibit_Id", "dbo.Exhibits");
            DropTable("dbo.Students");
            DropTable("dbo.Teachers");
            DropTable("dbo.Administrators");
            DropTable("dbo.GameItemStudents");
            DropTable("dbo.ExhibitStudents");
            DropTable("dbo.AccessRights");
            DropTable("dbo.MenuItems");
            DropTable("dbo.GameItems");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
            DropTable("dbo.TestMains");
            DropTable("dbo.Sessions");
            DropTable("dbo.Articles");
            DropTable("dbo.Exhibits");
            DropTable("dbo.Users");
        }
    }
}
