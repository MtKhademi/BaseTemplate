using ECommerceModule.Contract.Cart.Commands;

namespace ECommerceModule.Cart.Features.DeleteCart;

internal class DeleteCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapDelete("/ecommerce/api/v{apiVersion:apiVersion}/cart/{cartId}", async (
                [FromRoute(Name = "cartId")] int cartId,
                [FromServices] ISender sender,
                [FromServices] ILogger<DeleteCartEndpoint> logger,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(DeleteCartCommand.Create(cartId), cancellationToken);
                return Results.Ok(true.ToApiResultSuccess());
            })
            .WithMetadata(new ApiVersion(1, 0))
            .WithApiVersionSet(versionSet)
            .WithGroupName("ECOMMERCE-V1")
            .MapToApiVersion(1)
            .WithTags("CART")
            .IncludeInOpenApi()
            .Produces<ApiResult<bool>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete cart")
            .WithDescription("Delete a cart by id");
    }
}
