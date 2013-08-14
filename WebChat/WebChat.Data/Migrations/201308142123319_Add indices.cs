namespace WebChat.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addindices : DbMigration
    {
        public override void Up()
        {
            this.CreateIndex("Users", "Username", unique: true, name: "IX_UQ_UserUsername");
        }
        
        public override void Down()
        {
            this.DropIndex("Users", "Username");
        }
    }
}
