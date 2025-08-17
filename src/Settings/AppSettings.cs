using System;
using System.IO;

namespace MiniPos.Settings;

public class AppSettings
{
    #region static factory
    private static AppSettings? instance;
    public static AppSettings Instance
    {
        get
        {
            instance ??= new AppSettings();
            return instance;
        }
    }
    private AppSettings() { }
    #endregion

    public string GetDefaultFolder()
    {
        var envDefaultFolder = Environment.GetEnvironmentVariable("MINIPOS_DEFAULT_FOLDER");

        if (!string.IsNullOrWhiteSpace(envDefaultFolder))
        {
            return envDefaultFolder;
        }

        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    }

    public string GetProductsSpreadsheetFilepath()
    {
        var envfilepath = Environment.GetEnvironmentVariable("MINIPOS_SRC_FILEPATH");

        if (!string.IsNullOrWhiteSpace(envfilepath))
        {
            return envfilepath;
        }

        var filename = Environment.GetEnvironmentVariable("MINIPOS_SRC_FILENAME") ?? "MINIPOS_DATA.xlsx";
        return Path.Combine(GetDefaultFolder(), filename);
    }

    public string GenerateReceiptFilepath()
    {
        var filename = "MINIPOS-" + DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + ".xlsx";
        return Path.Combine(GetDefaultFolder(), filename);
    }
}
