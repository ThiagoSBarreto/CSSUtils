using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Logger
{
    /// <summary>
    /// Interface utilizada pelo Logger, contendo as funções e auxliares para geração de LOGS
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Configurações do LOGGER
        /// </summary>
        /// <param name="caminho">Caminho onde os arquivos de LOG serão armazenados. Padrão: Cria pasta dos LOGS no local onde a aplicação for executada</param>
        /// <param name="tamanhoMaximoLog">Tamanho Maximo dos arquivos de LOG. Padrão: 30</param>
        /// <param name="tipoTamanhoLog">Define a unidade do tamanho maximo dos arquivos de LOG. Padrão: MB</param>
        /// <param name="tempoPersistenciaLog">Define o tempo que os arquivos de LOG existirão no Sistema. Padrão: 3</param>
        /// <param name="tipoPersistencia">Define a unidade de tempo que os arquivos de LOG existirão no Sistema. Padrão: Dias</param>
        void Configurar(string caminho = "", int tamanhoMaximoLog = 30, TipoTamanhoLog tipoTamanhoLog = TipoTamanhoLog.MB, int tempoPersistenciaLog = 3, TipoPersistencia tipoPersistencia = TipoPersistencia.DIAS);

        /// <summary>
        /// Cria uma mensagem de LOG e grava em Arquivo
        /// </summary>
        /// <param name="ex">Exception Gerada pelo Sistema</param>
        /// <param name="message">Mensagem Opcional sobre o erro</param>
        /// <param name="logType">Tipo do LOG</param>
        void CreateLog(Exception ex = null, string message = "", LogType logType = LogType.INFO);
    }

    /// <summary>
    /// Tipos de LOGS que podem ser gerados
    /// </summary>
    public enum LogType
    {
        DEBUG,
        WARNING,
        ERROR,
        CRITICAL,
        MESSAGE,
        UTIL,
        STACK_TRACE,
        INFO
    }

    /// <summary>
    /// Unidade de tamanho do Arquivo de LOG
    /// </summary>
    public enum TipoTamanhoLog
    {
        KB = 1,
        MB = 1024,
        GB = 1048576,
        TB = 1073741824
    }

    /// <summary>
    /// Unidade de tempo da persistencia dos arquivos
    /// </summary>
    public enum TipoPersistencia
    {
        HORAS,
        DIAS,
        MESES,
        ANOS
    }
}
