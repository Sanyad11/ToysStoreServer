using AutoMapper;
using ToysStore.DataContracts;
using ToysStore.Entities;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToysStore.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IDataBaseManager _dbManager;
        private ILogger _logger;
        private ISessionStateManager _sessionManager;
        private IMapper _mapper;

        public AuthenticationManager(IDataBaseManager DbManager, ILogger logger, ISessionStateManager sessionManager, IMapper mapper)
        {
            _sessionManager = sessionManager;
            _dbManager = DbManager;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>
        /// </returns>
        public Errors Login(LoginRequest loginRequest)
        {
            try
            {
                loginRequest.Password = Hash(loginRequest.Password);
                if (_dbManager.IsExistUser(loginRequest.Login, loginRequest.Password))
                {
                    _sessionManager.AddKey("login", loginRequest.Login);
                    _sessionManager.AddKey("password", loginRequest.Password);
                    return Errors.OK;
                }
                return Errors.INVALID_AUTHENTIFICATION_VALUES;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Errors.DATA_BASE_ERROR;
            }
        }

        /// <summary>
        /// Log out user
        /// </summary>
        /// <returns>
        /// </returns>
        public Errors LogOut()
        {
            try
            {
                _sessionManager.AbandonSession();
                return Errors.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Errors.SYSTEM_ERROR;
            }
        }

        /// <summary>
        /// Add user to Data base
        /// </summary>
        /// <param name="registerUserRequest"></param>
        /// <returns>
        /// </returns>
        public Errors AddUser(RegisterUserRequest registerUserRequest)
        {
            registerUserRequest.Password = Hash(registerUserRequest.Password);
            UserData user = _mapper.Map<RegisterUserRequest, UserData>(registerUserRequest);
            try
            {
                if (_dbManager.isExistUser(user))
                    return Errors.USER_ALREADY_EXIST;

                _dbManager.AddUser(user);
                return Errors.OK;
            }
            catch (Exception ex)
            {
                _logger.Error("Authentication error", ex);
                return Errors.DATA_BASE_ERROR;
            }

        }
        
        /// <summary>
        /// This function encodes user password using algorythm SHA1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("X2")));
        }
    }
}