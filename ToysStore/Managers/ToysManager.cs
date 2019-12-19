using ToysStore.DataContracts;
using ToysStore.Entities;
using Serilog;
using System;
using System.Collections.Generic;

namespace ToysStore.Managers
{
    public class ToysManager : IToysManager
    {
        private readonly IDataBaseManager _dbManager;
        private ILogger _logger;

        public ToysManager(IDataBaseManager DbManager, ILogger logger)
        {
            _dbManager = DbManager;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all toys from data base
        /// </summary>
        /// <returns>
        /// </returns>
        public List<AllToysRequest> GetAllToys()
        {
            List<AllToysRequest> allToys = new List<AllToysRequest>();
            try
            {
                allToys = _dbManager.GetAllToys();
                return allToys;
            }
            catch (Exception ex)
            {
                _logger.Error("Data base error", ex);
                return null;
            }
        }

        /// <summary>
        /// Check is that ID is correct and if it is. return toy with that ID
        /// </summary>
        /// <param name="toyId"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public ToysData GetToyById(int toyId, out Errors errorCode)
        {
            try
            {
                if (!_dbManager.IsExistToy(toyId))
                {
                    errorCode = Errors.INVALID_TOY_ID;
                    return null;
                }
                ToysData toy = _dbManager.GetToyById(toyId);
                errorCode = Errors.OK;
                return toy;

            }
            catch (Exception ex)
            {
                _logger.Error("Data base error", ex);
                errorCode = Errors.DATA_BASE_ERROR;
                return null;
            }
        }

    }
}