using FeatureManagementTracker.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;
using System;

namespace FeatureManagementTracker.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") // Angular app URL
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var conStr = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddDbContext<FeatureDBContext>(options =>
            options.UseSqlServer(conStr));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FeatureDBContext>();
                context.Database.Migrate();
                SeedData(context);

            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use CORS policy
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void SeedData(FeatureDBContext context)
        {
            // I am seeding data to insert records into the table, this assumes the DB already exists.

            // SQL Query to create DB tabel features in a new DB : FeatureManagementDB

            //CREATE TABLE Features(
            //    Id INT PRIMARY KEY IDENTITY,
            //    Title NVARCHAR(1000) NOT NULL,
            //    Description NVARCHAR(MAX),
            //    EstimatedComplexity NVARCHAR(2),
            //    Status NVARCHAR(20),
            //    TargetCompletionDate DATETIME,
            //    ActualCompletionDate DATETIME
            //);

            if (!context.Features.Any())
            {
                context.Features.AddRange(
                    new Feature
                    {
                        Title = "Feature 1",
                        Description = "This is a description for Feature 1",
                        EstimatedComplexity = "M",
                        Status = "New",
                        TargetCompletionDate = null,
                        ActualCompletionDate = null
                    },
                    new Feature
                    {
                        Title = "Feature 2",
                        Description = "This is a description for Feature 2",
                        EstimatedComplexity = "L",
                        Status = "Active",
                        TargetCompletionDate = new DateTime(2024, 12, 31),
                        ActualCompletionDate = null
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
