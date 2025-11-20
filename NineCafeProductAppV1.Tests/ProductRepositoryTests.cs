using Microsoft.EntityFrameworkCore;
using NineCafeProductAppV1.Data;
using NineCafeProductAppV1.Models;
using NineCafeProductAppV1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCafeProductAppV1.Tests
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase("ProductDb")
               .Options;
        }
            
        private AppDbContext CreateDbContext() => new AppDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            var db = CreateDbContext();
            var repos = new ProductRepository(db);
            var product = new ProductPosting
            {
                Title = "Test title adding",
                Price = 1.25M,
                Description = "Test desc adding",
                Category = "Test cate adding",
                ImageUrl = "Test url adding",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url adding"

            };
            await repos.AddAsync(product);

            var result = db.productPostings.SingleOrDefault(x => x.Title == "Test title adding");
            Assert.NotNull(result);
            Assert.Equal("Test title adding", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            var db = CreateDbContext();
            var repos = new ProductRepository(db);
            var product = new ProductPosting
            {
                Title = "Test title",
                Price = 1.25M,
                Description = "Test desc",
                Category = "Test cate",
                ImageUrl = "Test url",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url"
            };
            await repos.AddAsync(product);
            var result = await repos.GetByIdAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal("Test title", result.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
        {
            var db = CreateDbContext();
            var repos = new ProductRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repos.GetByIdAsync(99999)
                );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            var db = CreateDbContext();
            var repos = new ProductRepository(db);

            var product1 = new ProductPosting
            {
                Title = "Test title adding1",
                Price = 1.25M,
                Description = "Test desc adding1",
                Category = "Test cate adding1",
                ImageUrl = "Test url adding1",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url adding1"
            };
            var product2 = new ProductPosting
            {
                Title = "Test title adding2",
                Price = 1.25M,
                Description = "Test desc adding2",
                Category = "Test cate adding2",
                ImageUrl = "Test url adding2",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url adding2"
            };

            await db.productPostings.AddRangeAsync(product1, product2);
            await db.SaveChangesAsync();

            var result = await repos.GetAllAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            var db = CreateDbContext();
            var repos = new ProductRepository(db);
            var product = new ProductPosting
            {
                Title = "Test title",
                Price = 1.25M,
                Description = "Test desc",
                Category = "Test cate",
                ImageUrl = "Test url",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url"
            };
            await repos.AddAsync(product);
            await db.SaveChangesAsync();

            product.Title = "Updated title";
            await repos.UpdateAsync(product);
            var result = db.productPostings.Find(product.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated title", product.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            var db = CreateDbContext();
            var repso = new ProductRepository(db);
            var product = new ProductPosting
            {
                Title = "Test title",
                Price = 1.25M,
                Description = "Test desc",
                Category = "Test cate",
                ImageUrl = "Test url",
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                FoodPandaUrl = "Test url"
            };
            await repso.AddAsync(product);
            await db.SaveChangesAsync();

            await repso.DeleteAsync(product.Id);
            var result = db.productPostings.Find(product.Id);

            Assert.Null(result);
        }
    }
}
