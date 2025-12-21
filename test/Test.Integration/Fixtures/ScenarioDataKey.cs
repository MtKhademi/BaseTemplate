namespace Test.Integration.Fixtures;

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


    AccessControll_PermissionGetPaginatedResponse,
    AccessControll_ModuleGetPaginatedResponse,
    AccessControll_FeatureGetPaginatedResponse,
    CacheClearAllResponse,
    CacheGetByKeyResponse,
    CacheSetResponse,

    // Order scenarios
    Orders,
    OrderCreate,
    OrderGetByIdResponse,
    OrderGetPaginated,
    OrderMarkAsPaidResponse,
}
