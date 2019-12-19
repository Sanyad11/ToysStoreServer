using ToysStore.Attributes;

public enum Errors
{
    [Description("OK result")]
    OK = 200,
    [Description("Email invalid")]
    INCORRECT_EMAIL = 1201,
    [Description("Login lenght less than 6")]
    INCORRECT_LOGIN_LENGHT = 1202,
    [Description("Password lenght less than 6 or more than 20")]
    INCORRECT_PASSWORD_LENGHT = 1203,
    [Description("Login or password was wrong")]
    INVALID_AUTHENTIFICATION_VALUES = 1204,
    [Description("User already exist")]
    USER_ALREADY_EXIST = 1205,
    [Description("One or more fields are empty.")]
    EMPTY_ONE_OR_MORE_FIELD = 1206,
    [Description("Toy Id is not valid")]
    INVALID_TOY_ID = 1401,
    [Description("User Id is not valid")]
    INVALID_USER_ID = 1402,
    [Description("Paramether is not valid")]
    INVALID_PARAMETHER = 1403,
    [Description("Database error")]
    DATA_BASE_ERROR = 2201,
    [Description("Something in system went wrong")]
    SYSTEM_ERROR = 2202   
}