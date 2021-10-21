using System;
using System.Collections.Generic;
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
        private Timer _logRoutine;

        public void Configurar(string caminho = "", int tamanhoMaximoLog = 30, TipoTamanhoLog tipoTamanhoLog = TipoTamanhoLog.MB, int tempoPersistenciaLog = 3, TipoPersistencia tipoPersistencia = TipoPersistencia.DIAS)
        {
            _config = new ConfiguracaoLog();
            _config.Caminho = string.IsNullOrWhiteSpace(caminho) ? Path.Combine(Environment.CurrentDirectory, "LOG") : caminho;
            _config.TamanhoLog = tamanhoMaximoLog;
            _config.TipoTamanho = tipoTamanhoLog;
            _config.TempoPersistencia = tempoPersistenciaLog;
            _config.TipoPersistencia = tipoPersistencia;

            _logRoutine = new Timer(new TimerCallback(LogRoutine), null, 0, 900);
        }

        public void CreateLog(Exception ex = null, string message = "", LogType logType = LogType.INFO)
        {
            if (_config == null) Configurar();

            string caminhoRaiz = _config.Caminho;
            string caminhoTipo = Path.Combine(caminhoRaiz, logType.ToString().ToUpper());
            string caminhoAno = Path.Combine(caminhoTipo, DateTime.Now.ToString("yyyy"));
            string caminhoMes = Path.Combine(caminhoAno, DateTime.Now.ToString("MMMM").ToUpper());
            string caminhoArquivo = Path.Combine(caminhoMes, $"{DateTime.Now.ToString("dd-MM-yyyy")} - Log.txt");

            try
            {
                if (!Directory.Exists(caminhoRaiz)) Directory.CreateDirectory(caminhoRaiz);
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

            using (FileStream fs = new FileStream(caminhoArquivo, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(sb.ToString());
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
                case TipoTamanhoLog.MB:
                    maxFileSize = maxFileSize * 1024;
                    break;
                case TipoTamanhoLog.GB:
                    maxFileSize = maxFileSize * 1048576;
                    break;
                case TipoTamanhoLog.TB:
                    maxFileSize = maxFileSize * 1073741824;
                    break;
            }

            long maxTime = _config.TempoPersistencia;
            switch (_config.TipoPersistencia)
            {
                case TipoPersistencia.HORAS:
                    maxTime = maxTime * 3600;
                    break;
                case TipoPersistencia.DIAS:
                    maxTime = maxTime * 86400;
                    break;
                case TipoPersistencia.MESES:
                    maxTime = maxTime * 2629800;
                    break;
                case TipoPersistencia.ANOS:
                    maxTime = maxTime * 31557600;
                    break;
            }

            try
            {
                foreach (string logFolder in Directory.GetDirectories(_config.Caminho))
                {
                    foreach (string tipoLog in Directory.GetDirectories(logFolder))
                    {
                        foreach (string anoLog in Directory.GetDirectories(tipoLog))
                        {
                            foreach (string logFile in Directory.GetDirectories(anoLog))
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
                                        using (StreamReader sr = new StreamReader(logFile))
                                        {
                                            using (StreamWriter sw = new StreamWriter(tempFile))
                                            {
                                                string line;
                                                int ignoreLines = 0;
                                                while ((line = sr.ReadLine()) != null)
                                                {
                                                    if (ignoreLines > 5)
                                                    {
                                                        sw.WriteLine(line);
                                                    }
                                                    ignoreLines++;
                                                }
                                                File.Delete(logFile);
                                                File.Move(tempFile, logFile);
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
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
            public TipoTamanhoLog TipoTamanho { get; set; }
            public int TempoPersistencia { get; set; }
            public TipoPersistencia TipoPersistencia { get; set; }
        }
    }
}
