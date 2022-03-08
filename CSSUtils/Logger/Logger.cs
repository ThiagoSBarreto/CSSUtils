using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSSUtils.Logger
{
    public class Logger : ILogger
    {
        private ConfiguracaoLog _config;
        private object _logLocker = new object();
        private Timer _routineTimer;

        public void Configurar(string logPath = "", int logMaxSize = 30, LogStorageUnit storateUnit = LogStorageUnit.MB, int logMaxPersistency = 3, LogPersistencyType logPersistencyType = LogPersistencyType.DAYS)
        {
            _config = new ConfiguracaoLog();
            _config.Caminho = string.IsNullOrWhiteSpace(logPath) ? Path.Combine(Environment.CurrentDirectory, "LOG") : logPath;
            _config.TamanhoLog = logMaxSize;
            _config.TipoTamanho = storateUnit;
            _config.TempoPersistencia = logMaxPersistency;
            _config.TipoPersistencia = logPersistencyType;

            if (!Directory.Exists(_config.Caminho)) Directory.CreateDirectory(_config.Caminho);

            _routineTimer = new Timer(new TimerCallback(LogRoutine), null, 0, 86400);
        }

        public void CreateLog(string message)
        {
            GenerateLog(null, LogType.MESSAGE, message);
        }

        public void CreateLog(string message, LogType type)
        {
            GenerateLog(null, type, message);
        }

        public void CreateLog(string message, Exception ex)
        {
            GenerateLog(ex, LogType.ERROR, message);
        }

        public void CreateLog(string message, Exception ex, LogType type)
        {
            GenerateLog(ex, type, message);
        }

        private void GenerateLog(Exception ex = null, LogType logType = LogType.MESSAGE, string message = "")
        {
            if (_config == null) Configurar();

#if DEBUG

#else
            if (logType == LogType.DEBUG) return;
#endif

            string caminhoRaiz = _config.Caminho;
            string caminhoTipo = Path.Combine(caminhoRaiz, logType.ToString().ToUpper());
            string caminhoAno = Path.Combine(caminhoTipo, DateTime.Now.ToString("yyyy"));
            string caminhoMes = Path.Combine(caminhoAno, DateTime.Now.ToString("MMMM").ToUpper());
            string caminhoArquivo = Path.Combine(caminhoMes, $"{DateTime.Now.ToString("dd-MM-yyyy")} - Log.txt");

            try
            {
                if (!Directory.Exists(caminhoTipo)) Directory.CreateDirectory(caminhoTipo);
                if (!Directory.Exists(caminhoAno)) Directory.CreateDirectory(caminhoAno);
                if (!Directory.Exists(caminhoMes)) Directory.CreateDirectory(caminhoMes);
            }
            catch (AccessViolationException e)
            {
                throw new Exception($"Sem permissão para criar Diretórios. Veja a InnerException para mais detalhes", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao criar Diretórios de LOGS. Veja a InnerException para mais detalhes", e);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"= LOG GERADO: {DateTime.Now.ToString("HH-mm-ss-fffff")} =========================================");
            if (!string.IsNullOrWhiteSpace(message))
            {
                sb.AppendLine("============================= COMENTARIO =============================");
                sb.AppendLine(message);
            }
            if (ex != null)
            {
                if (!string.IsNullOrWhiteSpace(ex.Message))
                {
                    sb.AppendLine("========================= EXCEPTION MESSAGE ==========================");
                    sb.AppendLine(ex.Message);
                }
                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    sb.AppendLine("============================= STACKTRACE =============================");
                    sb.AppendLine(ex.StackTrace);
                }
                if (ex.InnerException != null)
                {
                    sb.AppendLine(InnerException(ex.InnerException));
                }
            }
            sb.AppendLine("============================= FIM DO LOG =============================");

            lock (_logLocker)
            {
                if (!File.Exists(caminhoArquivo))
                {
                    File.Create(caminhoArquivo).Close();
                }
                using (StreamWriter sw = new StreamWriter(caminhoArquivo, true))
                {
                    sw.WriteLine(sb.ToString());
                }
            }
        }

        private string InnerException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                sb.AppendLine("====================== INNER EXCEPTION MESSAGE ========================");
                sb.AppendLine(ex.Message);
            }
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                sb.AppendLine("===================== INNER EXCEPTION STACKTRACE ======================");
                sb.AppendLine(ex.StackTrace);
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine(InnerException(ex.InnerException));
            }
            return sb.ToString();
        }

        private void LogRoutine(object state)
        {
            string tempFile = Path.Combine(_config.Caminho, "log.tmp");

            long maxFileSize = _config.TamanhoLog;
            switch (_config.TipoTamanho)
            {
                case LogStorageUnit.KB:
                    maxFileSize = maxFileSize * 1024;
                    break;
                case LogStorageUnit.MB:
                    maxFileSize = maxFileSize * 1048576;
                    break;
                case LogStorageUnit.GB:
                    maxFileSize = maxFileSize * 1073741824;
                    break;
                case LogStorageUnit.TB:
                    maxFileSize = maxFileSize * 1099511627776;
                    break;
            }

            long maxTime = _config.TempoPersistencia;
            switch (_config.TipoPersistencia)
            {
                case LogPersistencyType.HOURS:
                    maxTime = maxTime * 3600;
                    break;
                case LogPersistencyType.DAYS:
                    maxTime = maxTime * 86400;
                    break;
                case LogPersistencyType.MONTHS:
                    maxTime = maxTime * 2629800;
                    break;
                case LogPersistencyType.YEARS:
                    maxTime = maxTime * 31557600;
                    break;
            }

            try
            {
                Task task = new Task(() =>
                {
                    foreach (string logFolder in Directory.GetDirectories(_config.Caminho))
                    {
                        foreach (string anoLog in Directory.GetDirectories(logFolder))
                        {
                            foreach (string mesLog in Directory.GetDirectories(anoLog))
                            {
                                foreach (string logFile in Directory.GetFiles(mesLog))
                                {
                                    while (true)
                                    {
                                        FileInfo fi = new FileInfo(logFile);
                                        if ((DateTime.Now - fi.CreationTime).TotalSeconds > maxTime)
                                        {
                                            File.Delete(logFile);
                                        }
                                        else if (fi.Length > maxFileSize)
                                        {
                                            lock (_logLocker)
                                            {
                                                using (StreamReader sr = new StreamReader(logFile))
                                                {
                                                    using (StreamWriter sw = new StreamWriter(tempFile))
                                                    {
                                                        string line;
                                                        int ignoreLines = 0;
                                                        while ((line = sr.ReadLine()) != null)
                                                        {
                                                            if (ignoreLines > 2)
                                                            {
                                                                sw.WriteLine(line);
                                                            }
                                                            ignoreLines++;
                                                        }
                                                        sw.Close();
                                                        sr.Close();
                                                        File.Delete(logFile);
                                                        File.Move(tempFile, logFile);
                                                        fi = new FileInfo(logFile);
                                                        if (fi.Length <= maxFileSize)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                });
                task.Start();
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception($"O arquivo não existe", ex);
            }
            catch(DirectoryNotFoundException ex)
            {
                throw new Exception($"O caminho não existe", ex);
            }
            catch(AccessViolationException ex)
            {
                throw new Exception($"Sem permissão para ler/gravar", ex);
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao gerar arquivo de LOG", ex);
            }
        }

        public class ConfiguracaoLog
        {
            public string Caminho { get; set; }
            public int TamanhoLog { get; set; }
            public LogStorageUnit TipoTamanho { get; set; }
            public int TempoPersistencia { get; set; }
            public LogPersistencyType TipoPersistencia { get; set; }
        }
    }
}
