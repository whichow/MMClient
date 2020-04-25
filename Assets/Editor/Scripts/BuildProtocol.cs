using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

public class BuildProtocol
{
    #region Const

    private const string BAT_FILE = @"../public_message/clientProtoGen.bat";

    #endregion

    [MenuItem("Tools/Protocol/Generate CSFile", false, 10)]
    public static void BuildAndCopy()
    {
        Build();
        Copy();
    }

    private static void Build()
    {
        try
        {
            var batFile = new FileInfo(BAT_FILE);

            var process = new Process();
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = batFile.FullName;
            process.StartInfo.WorkingDirectory = batFile.Directory.FullName;

            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(string.Format("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString()));
        }
    }

    private static void Copy()
    {
        var batFile = new FileInfo(BAT_FILE);
        var src = Path.Combine(batFile.Directory.FullName, "csproto");
        var dst = "Assets/GameLogic/GameNet/Protocol";
        FileUtil.ReplaceDirectory(src, dst);
    } 
}