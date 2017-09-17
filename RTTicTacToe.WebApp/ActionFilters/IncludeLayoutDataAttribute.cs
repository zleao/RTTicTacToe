using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RTTicTacToe.WebApp.ActionFilters
{
    public class IncludeLayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ViewResult)
            {
                var bag = (context.Result as ViewResult).ViewData;

                bag["Games"] = Domain.GameQueries.GetAllGames(); 
            }
        }
    }
}
