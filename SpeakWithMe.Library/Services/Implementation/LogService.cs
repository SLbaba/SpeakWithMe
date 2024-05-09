using SpeakWithMe.Library.Services.IServices;

namespace SpeakWithMe.Library.Services.Implementation;

/// <summary>
/// 日志服务
/// </summary>
public class LogService : ILogService {
    // ******** 公开变量

    // ******** 私有变量

    /// <summary>
    /// 字典锁，防止多个线程同时操作下面的字典
    /// </summary>
    private readonly object _dicLocker = new();

    /// <summary>
    /// 锁
    /// </summary>
    private readonly Dictionary<string, object> _locker = new();

    // ******** 继承方法

    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="filename">文件名</param>
    /// <param name="logType">日志类型</param>
    /// <param name="funcIndex">模块名方法名索引，如UploadToPJ.Services.Implementation.LogService.XXX方法</param>
    /// <param name="fileContent">文件内容</param>
    public void WriteLog(string filename, LogType logType, string funcIndex,
        string fileContent) {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        var dateTime = DateTime.Now; //时间要放在lock外边 
        try {
            fileContent =
                $"{dateTime:yyyy-MM-dd HH:mm:ss.fff} [{logType}] [{funcIndex}] {fileContent} {Environment.NewLine}";
            var lockerName = filename.ToLower();
            lock (_dicLocker) {
                if (!_locker.ContainsKey(lockerName))
                    _locker[lockerName] = new object();
            }

            lock (_locker[lockerName]) {
                //拼接日志文件的目录
                var tempPath =
                    Path.Combine(path, dateTime.ToString("yyyy-MM-dd"));
                //检查是否存在Log文件夹，不存在则创建
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                //拼接日志文件的名称
                var tempName = Path.Combine(tempPath,
                    $"{filename}_{dateTime:yyyy-MM-dd}.log");
                //写入文件
                File.AppendAllText(tempName, fileContent);
            }
        } catch {
            throw;
        }
    }

    // ******** 公开方法

    // ******** 私有方法
}