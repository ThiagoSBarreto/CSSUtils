using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Logger
{
    public interface ILogger
    {
        /// <summary>
        /// Logger configuration
        /// </summary>
        /// <param name="caminho">Path to the log folder. Default: The assembly execution path</param>
        /// <param name="tamanhoMaximoLog">Max Log Size. Default: 30</param>
        /// <param name="tipoTamanhoLog">Storage units for log size. Default: MB</param>
        /// <param name="tempoPersistenciaLog">Max log persistency time. Default: 3</param>
        /// <param name="tipoPersistencia">Time unit for log persistency. Default: DAYS</param>
        void Configurar(string logPath = "", int logMaxSize = 30, LogStorageUnit storateUnit = LogStorageUnit.MB, int logMaxPersistency = 3, LogPersistencyType logPersistencyType = LogPersistencyType.DAYS);

        /// <summary>
        /// Creates a Log with a message that will be put into the MESSAGE Log folder
        /// </summary>
        /// <param name="message">The log message</param>
        void CreateLog(string message);

        /// <summary>
        /// Create a Log and put into the selected LogType folder
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="type">The LogType</param>
        void CreateLog(string message, LogType type);

        /// <summary>
        /// Creates a Log and put the STACKTRACE and INNEREXCEPTION STACKTRACE and Error Message into the ERROR LOG folder
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="ex">Exception</param>
        void CreateLog(string message, Exception ex);

        /// <summary>
        /// Creates a Log and put into the selected LogType folder with the STACKTRACE and INNEREXCEPTION STACKTRACE and Error Messages
        /// </summary>
        /// <param name="message">The log message</param>
        /// <param name="ex">Exception</param>
        /// <param name="type">The LogType</param>
        void CreateLog(string message, Exception ex, LogType type);
    }

    /// <summary>
    /// LogTypes folders
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// DEBUG LogType, doesn't generates logs in RELEASE mode
        /// </summary>
        DEBUG,
        /// <summary>
        /// WARNING LogType
        /// </summary>
        WARNING,
        /// <summary>
        /// ERROR LogType
        /// </summary>
        ERROR,
        /// <summary>
        /// CRITICAL LogType
        /// </summary>
        CRITICAL,
        /// <summary>
        /// MESSAGE LogType, similar to DEBUG LogType, but is generated in RELEASE mode
        /// </summary>
        MESSAGE
    }

    /// <summary>
    /// Storage Units used into the configurations
    /// </summary>
    public enum LogStorageUnit
    {
        /// <summary>
        /// KiloByte size: LogMaxSize * 1
        /// </summary>
        KB = 1,
        /// <summary>
        /// MegaByte size: LogMaxSize * 1024
        /// </summary>
        MB = 1024,
        /// <summary>
        /// GigaByte size: LogMaxSize * 1048576
        /// </summary>
        GB = 1048576,
        /// <summary>
        /// TeraByte size: LogMaxSize * 1073741824
        /// </summary>
        TB = 1073741824
    }

    /// <summary>
    /// Time unit used into the configurations
    /// </summary>
    public enum LogPersistencyType
    {
        /// <summary>
        /// Persists for HOURS
        /// </summary>
        HOURS,
        /// <summary>
        /// Persists for DAYS
        /// </summary>
        DAYS,
        /// <summary>
        /// Persists for Months
        /// </summary>
        MONTHS,
        /// <summary>
        /// Persists for Years
        /// </summary>
        YEARS
    }
}
