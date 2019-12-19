using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToysStore.DataContracts;
using ToysStore.Entities;
using ToysStore.Managers;
using Serilog;
using System;

namespace ToysStore.Tests
{
    [TestClass]
    public class AutenticationManagerTests
    {
        [TestMethod]
        public void AddUser_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = "fghjkl", Email = "ghjknnb@gf@vb", Password = "ertykjhjb" };
            UserData userData = new UserData() { Login = "fghjkl1", Email = "ghjknnb1@gf@vb", Password = "ertykjhjb1" };
            mockMapper.Setup(mp => mp.Map<RegisterUserRequest, UserData>(It.IsAny<RegisterUserRequest>())).Returns(userData);
            mockDb.Setup(db => db.isExistUser(It.Is<UserData>(ud => userData.Equals(ud)))).Returns(false);
            mockDb.Setup(db => db.AddUser(It.Is<UserData>(ud => userData.Equals(ud))));
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);


            // Act
            var result = manager.AddUser(registerUserRequest);


            // Assert
            Assert.AreEqual(Errors.OK, result);
        }

        [TestMethod]
        public void AddUser_Return_UserAlreadyExist()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = "fghjkl", Email = "ghjknnb@gf@vb", Password = "ertykjhjb" };
            UserData userData = new UserData() { Login = "fghjkl1", Email = "ghjknnb1@gf@vb", Password = "ertykjhjb1" };
            mockMapper.Setup(mp => mp.Map<RegisterUserRequest, UserData>(It.IsAny<RegisterUserRequest>())).Returns(userData);
            mockDb.Setup(db => db.isExistUser(It.Is<UserData>(ud => userData.Equals(ud)))).Returns(true);
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);


            // Act
            var result = manager.AddUser(registerUserRequest);


            // Assert
            Assert.AreEqual(Errors.USER_ALREADY_EXIST, result);
        }

        [TestMethod]
        public void AddUser_Return_DATA_BASE_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = "fghjkl", Email = "ghjknnb@gf@vb", Password = "ertykjhjb" };
            UserData userData = new UserData() { Login = "fghjkl1", Email = "ghjknnb1@gf@vb", Password = "ertykjhjb1" };
            mockMapper.Setup(mp => mp.Map<RegisterUserRequest, UserData>(It.IsAny<RegisterUserRequest>())).Returns(userData);
            mockDb.Setup(db => db.isExistUser(It.Is<UserData>(ud => userData.Equals(ud)))).Throws(new Exception());
            mockDb.Setup(db => db.AddUser(It.Is<UserData>(ud => userData.Equals(ud)))).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);


            // Act
            var result = manager.AddUser(registerUserRequest);


            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, result);
        }

        [TestMethod]
        public void Login_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();
                       
            LoginRequest loginRequest = new LoginRequest() { Login = "fghjkl1", Password = "ertykjhjb1" };
            mockDb.Setup(db => db.IsExistUser(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);           

            // Act
            var result = manager.Login(loginRequest);


            // Assert
            Assert.AreEqual(Errors.OK, result);
        }

        [TestMethod]
        public void Login_Return_INVALID_AUTHENTIFICATION_VALUES()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();

            LoginRequest loginRequest = new LoginRequest() { Login = "fghjkl1", Password = "ertykjhjb1" };
            mockDb.Setup(db => db.IsExistUser(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);

            // Act
            var result = manager.Login(loginRequest);

            // Assert
            Assert.AreEqual(Errors.INVALID_AUTHENTIFICATION_VALUES, result);
        }

        [TestMethod]
        public void Login_Return_DATA_BASE_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();

            LoginRequest loginRequest = new LoginRequest() { Login = "fghjkl1", Password = "ertykjhjb1" };
            mockDb.Setup(db => db.IsExistUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);

            // Act
            var result = manager.Login(loginRequest);

            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, result);
        }

        [TestMethod]
        public void LogOut_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();
            
            int countOfAbadonSessions = 0;
            mockSessionManager.Setup(sm => sm.AbandonSession()).Callback(() => countOfAbadonSessions++);

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);

            // Act
            var result = manager.LogOut();

            // Assert
            Assert.AreEqual(Errors.OK, result);
            Assert.AreEqual(1, countOfAbadonSessions);
        }

        [TestMethod]
        public void LogOut_Return_SYSTEM_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            var mockSessionManager = new Mock<ISessionStateManager>();
            var mockMapper = new Mock<IMapper>();
            
            mockSessionManager.Setup(sm => sm.AbandonSession()).Throws(new Exception());

            var manager = new AuthenticationManager(mockDb.Object, mockLogger.Object, mockSessionManager.Object, mockMapper.Object);

            // Act
            var result = manager.LogOut();

            // Assert
            Assert.AreEqual(Errors.SYSTEM_ERROR, result);
        }


    }
}
