using Bogus;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using ODataWebAPI.Context;
using ODataWebAPI.Controllers;
using ODataWebAPI.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString = "Data Source=BAYDEMIRPC\\SQLEXPRESS;Initial Catalog=ODataDB ;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers().AddOData(opt=>
        opt
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", TestController.GetEdmModel())
);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapGet("seed-data/categories", async (ApplicationDbContext dbContext) =>
{
    Faker faker = new();

    var categoryNames = faker.Commerce.Categories(100);
    List<Category> categories = categoryNames.Select(s => new Category
    {
        Name = s
    }).ToList();
    dbContext.AddRange(categories);

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
}).Produces(204).WithTags("SeedCategories");

app.MapScalarApiReference();

app.MapControllers();

app.Run();
