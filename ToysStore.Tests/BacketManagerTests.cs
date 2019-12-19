using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToysStore.DataContracts;
using ToysStore.Entities;
using ToysStore.Managers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToysStore.Tests
{
    [TestClass]
    public class BacketManagerTests
    {

        [TestMethod]
        public void RemoveFromBacket_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            
            mockDb.Setup(db => db.RemoveFromBacket(It.IsAny<int>(),It.IsAny<int>()));
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.RemoveFromBacket(1, 1);

            // Assert
            Assert.AreEqual(Errors.OK, result);
        }

        [TestMethod]
        public void RemoveFromBacket_Return_DATA_BASE_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            mockDb.Setup(db => db.RemoveFromBacket(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.RemoveFromBacket(1, 1);

            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, result);
        }

        [TestMethod]
        public void UpdateBacket_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            BacketData backetData = new BacketData() { UserId = 1, ToyId = 1, Count = 3};
            mockDb.Setup(db => db.UpdateBacket(It.IsAny<BacketData>()));
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.UpdateBacket(backetData);

            // Assert
            Assert.AreEqual(Errors.OK, result);
        }

        [TestMethod]
        public void UpdateBacket_Return_DATA_BASE_ERROR()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            BacketData backetData = new BacketData() { UserId = 1, ToyId = 1, Count = 3 };
            mockDb.Setup(db => db.UpdateBacket(It.IsAny<BacketData>())).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.UpdateBacket(backetData);

            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, result);
        }

        [TestMethod]
        public void GetBucketList_Return_BacketResponce()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            List<AllToysRequest> allToysRequest = new List<AllToysRequest>();
            int price = 3;
            allToysRequest.Add(new AllToysRequest { ID = 1, Name ="Belka", Price = 1 });
            BacketResponce backetResponce = new BacketResponce() { ToysData = allToysRequest, Price = price };
            mockDb.Setup(db => db.GetBucketListBacketData(It.IsAny<int>())).Returns(allToysRequest);
            mockDb.Setup(db => db.GetBucketListPrice(It.IsAny<int>())).Returns(price);
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetBucketList(1);

            // Assert
            Assert.AreEqual(backetResponce.Price, result.Price);
            Assert.AreEqual(backetResponce.ToysData.First(), result.ToysData.First());
        }

        [TestMethod]
        public void GetBucketList_Return_Exeption()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            List<AllToysRequest> allToysRequest = new List<AllToysRequest>();
            int price = 3;
            allToysRequest.Add(new AllToysRequest { ID = 1, Name = "Belka", Price = 1 });
            BacketResponce backetResponce = new BacketResponce() { ToysData = allToysRequest, Price = price };
            mockDb.Setup(db => db.GetBucketListBacketData(It.IsAny<int>())).Throws(new Exception());
            mockDb.Setup(db => db.GetBucketListPrice(It.IsAny<int>())).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.GetBucketList(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BuyAllToysFromBacket_Return_Exeption()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();
            
            mockDb.Setup(db => db.BuyAllToysFromBacket(It.IsAny<int>())).Throws(new Exception());
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.BuyAllToysFromBacket(1);

            // Assert
            Assert.AreEqual(Errors.DATA_BASE_ERROR, result);
        }

        [TestMethod]
        public void BuyAllToysFromBacket_Return_OK()
        {
            // Arrange
            var mockDb = new Mock<IDataBaseManager>();
            var mockLogger = new Mock<ILogger>();

            mockDb.Setup(db => db.BuyAllToysFromBacket(It.IsAny<int>()));
            mockLogger.Setup(db => db.Error(It.IsAny<string>()));

            var manager = new BacketManager(mockDb.Object, mockLogger.Object);

            // Act
            var result = manager.BuyAllToysFromBacket(1);

            // Assert
            Assert.AreEqual(Errors.OK, result);
        }
    }
}
