namespace JsonDiff.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jsons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JsonId = c.String(),
                        Left = c.String(),
                        Right = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jsons");
        }
    }
}
