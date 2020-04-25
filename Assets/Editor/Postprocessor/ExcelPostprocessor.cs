// ***********************************************************************
// Company          : Kunpo
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************

namespace Game
{
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.UserModel;
    using System.Collections.Generic;

    /// <summary>
    /// ExcelPostprocessor
    /// </summary>
    class ExcelPostprocessor : AssetPostprocessor
    {
        #region STATIC
        private static string[] Match3Config = new string[] { "ElementConfig", "MatchEffectConfig" };
        private static string Match3ConfigPath = "Assets/GameRes/Prefabs/Global/Config/";

        private static StringBuilder _HelpBuffer = new StringBuilder();

        private static StringBuilder GetHelpBuffer()
        {
            _HelpBuffer.Length = 0;
            return _HelpBuffer;
        }

        /// <summary>Checks the sheet.</summary>
        /// <param name="sheet">The sheet.</param>
        /// <returns></returns>
        private static bool CheckSheet(string sheet)
        {
            return sheet.StartsWith("_");
        }
        /// <summary>Gets the name of the file.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>Gets the file name without extension.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>Gets the name of the text file.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string GetTextFileName(string path)
        {
            return string.Format("{0}.txt", Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>Gets the text full path.</summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns></returns>
        private static string GetTextFullPath(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            foreach (var item in Match3Config)
            {
                if (item == fileName)
                    return string.Format("{0}/GameRes/Excel/{1}.txt", Application.dataPath, fileName);
            }
            //if (fileName.Contains("Data"))
            //{
            //    return Application.dataPath + "/Prefabs/System/" + fileName + ".txt";
            //}
            //return Path.ChangeExtension(assetPath, ".txt");
            return Application.dataPath + "/Res/Table/" + fileName + ".txt";
        }

        /// <summary>Gets the text asset path.</summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns></returns>
        private static string GetTextAssetPath(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            //if (fileName.Contains("Data"))
            //{
            //    return "Assets/Prefabs/System/" + fileName + ".txt";
            //}
            foreach (var item in Match3Config)
            {
                if (item == fileName)
                    return string.Format(Match3ConfigPath + "{0}.txt", fileName);
            }

            return Path.ChangeExtension(assetPath, ".txt");
        }

        /// <summary>
        /// 自由列
        /// </summary>
        /// <param name="fileName"></param>
        private static void ProcessExcel2(string fileName)
        {
            try
            {
                var jsonBuffer = GetHelpBuffer();
                using (var fs = File.OpenRead(fileName))
                {
                    var workBook = new HSSFWorkbook(fs);
                    if (workBook == null)
                    {
                        return;
                    }

                    jsonBuffer.Append('{');
                    foreach (ISheet sheet in workBook)
                    {
                        bool align = true;
                        //注释表
                        var sheetName = sheet.SheetName;
                        if (sheetName.StartsWith("_"))
                        {
                            continue;
                        }
                        else if (sheetName.StartsWith("!"))
                        {
                            align = false;
                            sheetName = sheetName.Substring(1);
                        }
                        else
                        {
                            align = true;
                        }

                        jsonBuffer.Append('\n');
                        jsonBuffer.Append('"');
                        jsonBuffer.Append(sheetName);
                        jsonBuffer.Append('"');

                        jsonBuffer.Append(':');
                        jsonBuffer.Append('[');

                        #region 表数据 

                        var fRowNum = sheet.FirstRowNum;
                        var lRowNum = sheet.LastRowNum;

                        var fRow = sheet.GetRow(fRowNum);
                        // 这个地方是<= 
                        for (int i = fRowNum; i <= lRowNum; i++)
                        {
                            IRow cRow = sheet.GetRow(i);
                            if (cRow == null)
                            {
                                continue;
                            }

                            var fCRowCell = cRow.GetCell(cRow.FirstCellNum);
                            if (fCRowCell == null || string.IsNullOrEmpty(fCRowCell.ToString()))
                            {
                                continue;
                            }

                            jsonBuffer.Append('\n');
                            jsonBuffer.Append('[');

                            var fCellNum = align ? fRow.FirstCellNum : cRow.FirstCellNum;
                            var lCellNum = align ? fRow.LastCellNum : cRow.LastCellNum;

                            //这个地方是 <
                            for (int j = fCellNum; j < lCellNum; j++)
                            {
                                ICell cell = cRow.GetCell(j);
                                if (cell != null)
                                {
                                    if (cell.CellType == CellType.Numeric)
                                    {
                                        jsonBuffer.Append(cell.ToString());
                                    }
                                    else if (cell.CellType == CellType.Formula)
                                    {
                                        jsonBuffer.Append(cell.NumericCellValue);
                                    }
                                    else
                                    {
                                        string cellString = cell.ToString();
                                        if (cellString.Length > 0 && (cellString[0] == '{' || cellString[0] == '[' || cellString[0] == '"'))
                                        {
                                            jsonBuffer.Append(cellString);
                                        }
                                        else
                                        {
                                            jsonBuffer.Append('"');
                                            jsonBuffer.Append(cell.ToString());
                                            jsonBuffer.Append('"');
                                        }
                                    }
                                }
                                else
                                {
                                    jsonBuffer.Append('"');
                                    jsonBuffer.Append('"');
                                }
                                jsonBuffer.Append(',');
                            }

                            if (jsonBuffer[jsonBuffer.Length - 1] == ',')
                            {
                                jsonBuffer[jsonBuffer.Length - 1] = ']';
                            }
                            else
                            {
                                jsonBuffer.Append(']');
                            }

                            jsonBuffer.Append(',');
                        }

                        #endregion

                        if (jsonBuffer[jsonBuffer.Length - 1] == ',')
                        {
                            jsonBuffer[jsonBuffer.Length - 1] = ']';
                        }
                        else
                        {
                            jsonBuffer.Append(']');
                        }

                        jsonBuffer.Append(',');
                    }

                    if (jsonBuffer[jsonBuffer.Length - 1] == ',')
                    {
                        jsonBuffer[jsonBuffer.Length - 1] = '}';
                    }
                    else
                    {
                        jsonBuffer.Append('}');
                    }

                    string txtFile = GetTextFullPath(fileName);

                    File.WriteAllText(txtFile, jsonBuffer.ToString(), Encoding.UTF8);
                    AssetDatabase.Refresh();
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// 对齐列
        /// </summary>
        /// <param name="fileName"></param>
        private static void ProcessExcel(string fileName)
        {
            if (fileName.Contains("_"))
            {
                Debug.Log("[ProcessExcel] fileName Contains  '_'  return!");
                return;
            }

            try
            {
                var jsonBuffer = GetHelpBuffer();
                using (var fs = File.OpenRead(fileName))
                {
                    jsonBuffer.Append('{');

                    bool isFirstSheet = true;
                    var workBook = new HSSFWorkbook(fs);
                    foreach (ISheet sheet in workBook)
                    {
                        //注释表
                        if (sheet.SheetName.StartsWith("_"))
                        {
                            continue;
                        }
                        if (sheet.LastRowNum == 0)
                        {
                            continue;
                        }

                        List<int> ignores = new List<int>();

                        if (!isFirstSheet)
                        {
                            jsonBuffer.Append(',');
                        }
                        isFirstSheet = false;

                        jsonBuffer.Append('\n');
                        jsonBuffer.Append('"');
                        jsonBuffer.Append(sheet.SheetName);
                        jsonBuffer.Append('"');

                        jsonBuffer.Append(':');
                        jsonBuffer.Append('[');

                        #region 表数据 

                        var fRowNum = sheet.FirstRowNum;
                        var lRowNum = sheet.LastRowNum;

                        var fRow = sheet.GetRow(fRowNum);

                        var fCellNum = fRow.FirstCellNum;
                        var lCellNum = fRow.LastCellNum;

                        // 这个地方是<= 
                        for (int i = fRowNum; i <= lRowNum; i++)
                        {
                            var rowIndex = i - fRowNum;
                            if (rowIndex == 1 || rowIndex == 2)
                            {
                                continue;
                            }
                            IRow row = sheet.GetRow(i);
                            if (row == null)
                            {
                                continue;
                            }

                            ICell fCell = row.GetCell(fCellNum);
                            if (fCell == null || string.IsNullOrEmpty(fCell.ToString()))
                            {
                                continue;
                            }

                            if (i != fRowNum)
                            {
                                jsonBuffer.Append(',');
                            }

                            jsonBuffer.Append('\n');
                            jsonBuffer.Append('[');

                            //这个地方是 <
                            for (int j = fCellNum; j < lCellNum; j++)
                            {
                                ICell cell = row.GetCell(j);
                                if (rowIndex == 0 && (cell == null || string.IsNullOrEmpty(cell.ToString()) || cell.ToString().ToLower().Contains("ignore")))
                                {
                                    ignores.Add(j);
                                    continue;
                                }
                                if (ignores.Contains(j))
                                {
                                    continue;
                                }
                                if (j != fCellNum)
                                {
                                    jsonBuffer.Append(',');
                                }

                                if (cell != null)
                                {
                                    string cellString = cell.ToString();
                                    if (cell.CellType == CellType.Numeric || (cellString.Length > 0 && (
                                        (cellString[0] == '{' && cellString[cellString.Length - 1] == '}') ||
                                        (cellString[0] == '[' && cellString[cellString.Length - 1] == ']') ||
                                        (cellString[0] == '"' && cellString[cellString.Length - 1] == '"'))))
                                    {
                                        jsonBuffer.Append(cellString);
                                    }
                                    else if (cell.CellType == CellType.Formula)
                                    {
                                        jsonBuffer.Append(cell.NumericCellValue);
                                    }
                                    else
                                    {
                                        jsonBuffer.Append('"');
                                        jsonBuffer.Append(cellString);
                                        jsonBuffer.Append('"');
                                    }
                                }
                                else
                                {
                                    jsonBuffer.Append('"');
                                    jsonBuffer.Append('"');
                                }
                            }

                            jsonBuffer.Append(']');
                        }

                        #endregion

                        jsonBuffer.Append(']');
                    }

                    jsonBuffer.Append('\n');
                    jsonBuffer.Append('}');

                    workBook.Close();
                    fs.Close();
                    fs.Dispose();

                    string txtFile = GetTextFullPath(fileName);
                    File.WriteAllText(txtFile, jsonBuffer.ToString(), Encoding.UTF8);
                    AssetDatabase.Refresh();

                    Debug.Log(">>>Excel自动打表成功（老的规则）: " + fileName);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("error! excel:" + fileName + "\n" + ex);
            }
        }

        #endregion

        #region UNITY

        /// <summary>Called when [postprocess all assets].</summary>
        /// <param name="importedAssets">The imported assets.</param>
        /// <param name="deletedAssets">The deleted assets.</param>
        /// <param name="movedAssets">The moved assets.</param>
        /// <param name="movedFromAssetPaths">The moved from asset paths.</param>
        public static void OnPostprocessAllAssets(
                     string[] importedAssets,
                     string[] deletedAssets,
                     string[] movedAssets,
                     string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                if (".xls" == Path.GetExtension(importedAsset))
                {
                    ProcessExcel(importedAsset);
                }
            }

            foreach (var deletedAsset in deletedAssets)
            {
                if (".xls" == Path.GetExtension(deletedAsset))
                {
                    string txtFile = GetTextAssetPath(deletedAsset);
                    AssetDatabase.DeleteAsset(txtFile);
                    AssetDatabase.Refresh();
                }
            }

            for (var i = 0; i < movedAssets.Length; i++)
            {
                if (".xls" == Path.GetExtension(movedAssets[i]))
                {
                    AssetDatabase.RenameAsset(GetTextAssetPath(movedFromAssetPaths[i]), GetFileNameWithoutExtension(movedAssets[i]));
                }
            }
        }

        #endregion
    }
}
