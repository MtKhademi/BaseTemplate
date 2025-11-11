namespace IAMModule.Contract.Requests;

public class ChangeUserStatusRequest
{
    public string UserId { get; set; }
    public bool Active { get; set; }
}