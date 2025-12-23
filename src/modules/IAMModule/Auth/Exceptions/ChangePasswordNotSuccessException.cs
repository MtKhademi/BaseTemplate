namespace IAMModule.Auth.Exceptions;

internal class ChangePasswordNotSuccessException : NotValidDataException<ChangePasswordNotSuccessException>
{
    public ChangePasswordNotSuccessException(IdentityResult result) :
        base(result.Errors.Select(e => e.Description).ToList())
    {
    }
}
