using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using log4net.Util;
using System.IO;


namespace Relational.Core
{
    public static class Common
    {


       //internal const string RelationalConfigFileName = "Relational.config";
       //internal const string RelationalLogFileName = "Relational.log";
       //internal const string RelationalConfigPath = "Infor\\CPM\\Relational";
       //internal const string RelationalLogPath = "Infor\\CPM\\LogFiles";
    
    }
    public sealed class Logging
    {
        /// <summary>
        /// Name of the default logging repository.
        /// 
        /// </summary>
        private const string DefaultRepositoryName = "Relational";

        /// <summary>
        /// Gets and initializes the logging object.  Our default repository name will be used.
        ///             This is a convenience method.
        /// 
        /// </summary>
        /// <param name="loggerName">Name of the logger to retrieve or configure.</param>
        /// <returns>
        /// The logging object we should use to log information.
        /// </returns>
        public static ILog InitializeLogging(string loggerName)
        {
            return Logging.InitializeLogging(DefaultRepositoryName, loggerName);
        }

        /// <summary>
        /// Gets and initializes the logging object.
        /// 
        /// </summary>
        /// <param name="repositoryName">Name of the logging repository to be used.  If none is provided, a default will be used.</param><param name="loggerName">Name of the logger to retrieve or configure.</param>
        /// <returns>
        /// The logging object we should use to log information.
        /// </returns>
        public static ILog InitializeLogging(string repositoryName, string loggerName)
        {
            string str = string.IsNullOrEmpty(repositoryName) ? DefaultRepositoryName : repositoryName;
            ILoggerRepository iloggerRepository = (ILoggerRepository)null;
            try
            {
                iloggerRepository = LogManager.GetRepository(str);
            }
            catch 
            {
            }
            if (iloggerRepository == null)
                iloggerRepository = LogManager.CreateRepository(str);
            if (!iloggerRepository.Configured)
            {
                GlobalContext.Properties["CommonApplicationData"] = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                FileInfo fileInfo = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Infor\\CPM\\Relational", "Relational.config"));
                XmlConfigurator.ConfigureAndWatch(iloggerRepository, fileInfo);
                iloggerRepository.Configured = true;
            }
            return LogManager.GetLogger(str, loggerName);
        }
    }
}

	
