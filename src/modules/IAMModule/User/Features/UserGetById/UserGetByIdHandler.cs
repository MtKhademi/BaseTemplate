namespace IAMModule.User.Features.UserGetById;

internal class UserGetByIdHandler(UserManager<ApplicationUser> userManager) :
    IQueryHandler<UserGetByIdQuery, ApplicationUserModel>
{
    public async Task<ApplicationUserModel> Handle(UserGetByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(query.UserId, cancellationToken);

        return user.ToModel();

    }
}
