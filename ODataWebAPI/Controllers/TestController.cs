using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataWebAPI.Context;
using ODataWebAPI.Models;

namespace ODataWebAPI.Controllers
{
    [Route("odata")]
    [ApiController]
    public sealed class TestController(ApplicationDbContext context) : ODataController
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new();
            builder.EntitySet<Category>("Categories");
            return builder.GetEdmModel();
        }

        [HttpGet("Categories")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All & ~AllowedQueryOptions.Select)]
        public IQueryable<Category> Categories()
        {
            var categories = context.Categories.AsQueryable();

            return categories;
        }
    }
}