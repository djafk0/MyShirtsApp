namespace MyShirtsApp.Test.Mocks
{
    using System;
    using MyShirtsApp.Data;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseMock
    {
        public static MyShirtsAppDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<MyShirtsAppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new MyShirtsAppDbContext(dbContextOptions);
            }
        }
    }
}
