namespace Common.Interfaces;

public interface IJWTHandlerService<TModel> where TModel : class
{
    string GenerateToken(TModel model);
    TModel GetModelFromToken(string token);
}
