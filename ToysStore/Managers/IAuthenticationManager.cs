using ToysStore.DataContracts;

namespace ToysStore.Managers
{
    public interface IAuthenticationManager
    {
        Errors AddUser(RegisterUserRequest registerUserRequest);
        Errors Login(LoginRequest loginRequest);
        Errors LogOut();      
    }
}