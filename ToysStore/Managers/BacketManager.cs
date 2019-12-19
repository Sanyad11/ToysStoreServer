using ToysStore.DataContracts;
using ToysStore.Entities;
using Serilog;
using System;

namespace ToysStore.Managers
{
    public class BacketManager : IBacketManager
    {
        private readonly IDataBaseManager _dbManager;
        private ILogger _logger;

        public BacketManager(IDataBaseManager DbManager, ILogger logger)
        {
            _dbManager = DbManager;
            _logger = logger;
        }

        /// <summary>
        /// Added new record to data base or updated existing
        /// </summary>
        /// <param name="backetData"></param>
        /// <returns></returns>
        public Errors UpdateBacket(BacketData backetData)
        {
            try
            {
                _dbManager.UpdateBacket(backetData);
                return Errors.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Errors.DATA_BASE_ERROR;
            }
        }

        /// <summary>
        /// Get all choised toys in backet and calculated all their price
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BacketResponce GetBucketList(int userId)
        {
            try
            {
                return new BacketResponce()
                {
                    ToysData = _dbManager.GetBucketListBacketData(userId),
                    Price = _dbManager.GetBucketListPrice(userId)
                };           
            }
            catch (Exception ex)
            {
                _logger.Error("DataBase error", ex);
                return null;
            }
        }

        /// <summary>
        /// Removes record from data base
        /// </summary>
        /// <param name="backetData"></param>
        /// <returns></returns>
        public Errors RemoveFromBacket(int userId, int toyId)
        {
            try
            {           
                _dbManager.RemoveFromBacket(userId, toyId);
                return Errors.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Errors.DATA_BASE_ERROR;
            }
        }

        /// <summary>
        /// Remove all rows with current userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Errors BuyAllToysFromBacket(int userId)
        {
            try
            {
                _dbManager.BuyAllToysFromBacket(userId);
                return Errors.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Errors.DATA_BASE_ERROR;
            }
        }

    }
}