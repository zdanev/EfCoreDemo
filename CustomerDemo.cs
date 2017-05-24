namespace EfCoreDemo
{
    public static class CustomerDemo
    {
        public static void Execute()
        {
            using (var db = new CustomerDb())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}