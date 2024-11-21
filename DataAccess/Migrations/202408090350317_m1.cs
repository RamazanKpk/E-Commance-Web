namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandId = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.ProductCategories", t => t.ParentId)
                .Index(t => t.BrandId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsVatInclude = c.Boolean(nullable: false),
                        VatRate = c.Int(nullable: false),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        StockAmount = c.Int(nullable: false),
                        CriticalStockAmount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(),
                        ProductAmount = c.Int(nullable: false),
                        ProductUnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCompleated = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserGroupId = c.Int(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        BirthDate = c.DateTime(),
                        PhoneNumber = c.String(),
                        EmailAddress = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        AccountType = c.Short(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupId, cascadeDelete: true)
                .Index(t => t.UserGroupId);
            
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(),
                        PhoneNumber = c.String(),
                        City = c.String(),
                        Distirct = c.String(),
                        Address = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDefaultAddress = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserFavoriteProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DiscountRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        ImagePath = c.String(),
                        IsDefaultImage = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.Users", "UserGroupId", "dbo.UserGroups");
            DropForeignKey("dbo.UserFavoriteProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserFavoriteProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UserContacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "ParentId", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductCategories", "BrandId", "dbo.Brands");
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropIndex("dbo.UserFavoriteProducts", new[] { "UserId" });
            DropIndex("dbo.UserFavoriteProducts", new[] { "ProductId" });
            DropIndex("dbo.UserContacts", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UserGroupId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategories", new[] { "ParentId" });
            DropIndex("dbo.ProductCategories", new[] { "BrandId" });
            DropTable("dbo.OrderStatus");
            DropTable("dbo.ProductImages");
            DropTable("dbo.UserGroups");
            DropTable("dbo.UserFavoriteProducts");
            DropTable("dbo.UserContacts");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Brands");
        }
    }
}
