namespace SpeakWithMe.Library.Services.IServices;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILogService {
    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="filename">文件名</param>
    /// <param name="logType">日志类型</param>
    /// <param name="funcIndex">模块名方法名索引，如UploadToPJ.Services.Implementation.LogService.XXX方法</param>
    /// <param name="fileContent">文件内容</param>
    void WriteLog(string filename, LogType logType, string funcIndex,
        string fileContent);
}

/// <summary>
/// 日志类型枚举
/// </summary>
public enum LogType {
    None,
    Info,
    Warn,
    Error
}