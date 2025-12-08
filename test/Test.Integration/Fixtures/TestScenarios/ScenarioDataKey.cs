namespace Test.Integration.Fixtures.TestScenarios;

public enum ScenarioDataKey
{
    None,
    CurrentUser,
    Register,
    ChangePasswordLoggedUserResponse,
    SignOut,

    Users,
    UserCreate,
    UserGetByIdResponse,
    UserDeleteResponse,
    UserRoleGetsResponse,
    UserRoleChangeResponse,
    UserUpdate,

    Roles,
    RoleCreate,
    RoleDelete,
    RoleGetPaginated,
    RoleGetByIdResponse,
    RoleUpdate,
    UserGetPaginated,
}
