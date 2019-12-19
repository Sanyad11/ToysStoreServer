using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToysStore.DataContracts;
using ToysStore.Entities;
using ToysStore.Managers;
using Serilog;
using System;
using System.Collections.Generic;

namespace ToysStore.Tests
{
    [TestClass]
    public class ToysManagerTests
    {
        [TestMethod]
        public void GetAllToys_Return_AllToys()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            AllToysRequest toysData = new AllToysRequest() { ID = 1, Name = "TyranosaurusRex", Price = 99 };
            List<AllToysRequest> toysDatas = new List<AllToysRequest> { toysData };
            mockDb.Setup(db => db.GetAllToys()).Returns(toysDatas);
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new ToysManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetAllToys();

            // Assert
            Assert.AreEqual(toysDatas, result);
            Assert.AreEqual(toysDatas[0].Name, result[0].Name);
        }

        [TestMethod]
        public void GetAllToys_Return_Null()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            AllToysRequest toysData = new AllToysRequest() { ID = 1, Name = "TyranosaurusRex", Price = 99 };
            List<AllToysRequest> toysDatas = new List<AllToysRequest> { toysData };
            mockDb.Setup(db => db.GetAllToys()).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new ToysManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetAllToys();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetToyById_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            ToysData toy = new ToysData() { ID = 1, Name = "TyranosaurusRex", Description = "Roarrrrrr", Price = 99 };
            mockDb.Setup(db => db.IsExistToy(It.IsAny<int>())).Returns(true);
            mockDb.Setup(db => db.GetToyById(It.IsAny<int>())).Returns(toy);
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new ToysManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetToyById(1, out Errors errorCode);

            // Assert
            Assert.AreEqual(Errors.OK, errorCode);
            Assert.AreEqual(toy, result);
            Assert.AreEqual(toy.Name, result.Name);
        }

        [TestMethod]
        public void GetToyById_Return_INVALID_TOY_ID()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            ToysData toy = new ToysData() { ID = 1, Name = "TyranosaurusRex", Description = "Roarrrrrr", Price = 99 };
            mockDb.Setup(db => db.IsExistToy(It.IsAny<int>())).Returns(false);
            mockDb.Setup(db => db.GetToyById(It.IsAny<int>())).Returns(toy);
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new ToysManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetToyById(1, out Errors errorCode);

            // Assert
            Assert.AreEqual(Errors.INVALID_TOY_ID, errorCode);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetToyById_Return_DATA_BASE_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            ToysData toy = new ToysData() { ID = 1, Name = "TyranosaurusRex", Description = "Roarrrrrr", Price = 99 };
            mockDb.Setup(db => db.IsExistToy(It.IsAny<int>())).Returns(true);
            mockDb.Setup(db => db.GetToyById(It.IsAny<int>())).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new ToysManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetToyById(1, out Errors errorCode);

            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, errorCode);
            Assert.IsNull(result);
        }

    }
}
