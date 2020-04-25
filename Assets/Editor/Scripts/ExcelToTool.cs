/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/24 14:27:51
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
namespace Game
{
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// ExcelPostprocessor
    /// </summary>
    public class ExcelToTool
    {
        [MenuItem("Tools/Excel/Excel生成CS文件和数据", false, 10)]
        private static void ExcelTo()
        {
            UnityEngine.Object[] objects = Selection.objects;
            for (int i = 0; i < objects.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(objects[i]);
                if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)// 如果是目录
                {
                    string[] fileEntries = Directory.GetFiles(path, "*.xls", SearchOption.AllDirectories);
                    if (fileEntries.Length > 0)
                    {
                        foreach (var item in fileEntries)
                        {
                            //Debug.Log(item);
                            ProcessExcel(item);
                        }
                    }
                }
                else if (".xls" == Path.GetExtension(path))
                {
                    //Debug.Log(path);
                    ProcessExcel(path);
                }
            }
            //Debug.Log("没有选中的Excel文件!");
        }

        #region STATIC

        private static StringBuilder _HelpBuffer = new StringBuilder();

        private static StringBuilder GetHelpBuffer()
        {
            _HelpBuffer.Length = 0;
            return _HelpBuffer;
        }

        /// <summary>Gets the text full path.</summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns></returns>
        private static string GetTextFullPath(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            //return Path.ChangeExtension(assetPath, ".txt");
            return Application.dataPath + "/Res/Table/" + fileName + ".txt";
        }

        /// <summary>
        /// 对齐列
        /// </summary>
        /// <param name="fileName"></param>
        private static void ProcessExcel(string fileName)
        {
            try
            {
                var jsonBuffer = GetHelpBuffer();
                using (var fs = File.OpenRead(fileName))
                {
                    //Debug.Log(">>>Excel自动打表开始读表: " + fileName);
                    jsonBuffer.Append('{');

                    bool isFirstSheet = true;
                    var workBook = new HSSFWorkbook(fs);
                    foreach (ISheet sheet in workBook)
                    {
                        //注释表
                        //if (sheet.SheetName.StartsWith("_"))
                        if (!sheet.SheetName.Contains("XDM"))
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

                        #region TOCS

                        CSFileHelp.SetClassName(sheet.SheetName);
                        for (int _i = 0; _i < 3; _i++)
                        {
                            IRow row = sheet.GetRow(_i);
                            for (int _j = fCellNum; _j < lCellNum; _j++)
                            {
                                ICell cell = row.GetCell(_j);
                                string cellVal = "";
                                if (cell != null)
                                {
                                    cellVal = cell.ToString();
                                }
                                if (_i == 0)
                                {
                                    if (string.IsNullOrEmpty(cellVal) || cellVal.ToLower().Contains("ignore"))
                                    {
                                        ignores.Add(_j);
                                        continue;
                                    }
                                    CSFileHelp.AddProperty(_j, cellVal);
                                }
                                else if (_i == 1)
                                {
                                    if (ignores.Contains(_j)) continue;
                                    CSFileHelp.AddPropertyNote(_j, cellVal);
                                }
                                else if (_i == 2)
                                {
                                    if (ignores.Contains(_j)) continue;
                                    CSFileHelp.AddPropertyType(_j, cellVal);
                                }
                            }
                        }
                        CSFileHelp.CreateCSFile();

                        #endregion

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
                                //if (rowIndex == 0 && (cell == null || string.IsNullOrEmpty(cell.ToString()) || cell.ToString().Contains("ignore")))
                                //{
                                //    ignores.Add(j);
                                //    continue;
                                //}
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

                    if (jsonBuffer.Length > 10)
                    {
                        string txtFile = GetTextFullPath(fileName);
                        File.WriteAllText(txtFile, jsonBuffer.ToString(), Encoding.UTF8);
                        AssetDatabase.Refresh();
                        CSFileHelp.CreateCSFile();

                        Debug.Log(">>>Excel自动打表成功: " + fileName);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("error! excel:" + fileName + "\n" + ex);
            }
        }

        #endregion

    }

    public class CSFileHelp
    {
        private static readonly string CSFilePath = "Assets/GameLogic/GameConfig/XDM/";
        private static StringBuilder m_csStringBuilder = new StringBuilder();
        private static string m_className;
        private static Dictionary<int, string> m_csPropertyNameDic = new Dictionary<int, string>();
        private static Dictionary<int, string> m_csPropertyTypeDic = new Dictionary<int, string>();
        private static Dictionary<int, string> m_csPropertyNoteDic = new Dictionary<int, string>();

        public static void SetClassName(string className)
        {
            m_className = className;
            m_csStringBuilder.Length = 0;
            m_csPropertyNameDic.Clear();
            m_csPropertyTypeDic.Clear();
            m_csPropertyNoteDic.Clear();
        }

        public static void AddProperty(int index, string propertyName)
        {
            if (index == 0 && propertyName != "ID")
            {
                Debug.LogErrorFormat("[{0}]第一列字段名必须要为【ID】{1}", m_className, propertyName);
            }
            m_csPropertyNameDic.Add(index, propertyName);
        }

        public static void AddPropertyType(int index, string propertyType)
        {
            if (string.IsNullOrEmpty(propertyType))
            {
                propertyType = "string";
                Debug.LogError("字段类型为空,自动转成string! " + m_className + index);
            }
            m_csPropertyTypeDic.Add(index, propertyType);
        }

        public static void AddPropertyNote(int index, string propertyNote)
        {
            m_csPropertyNoteDic.Add(index, propertyNote.Replace("\n", "  "));
        }

        public static void CreateCSFile()
        {
            if (string.IsNullOrEmpty(m_className) || m_csPropertyNameDic.Count == 0)
            {
                return;
            }

            m_csStringBuilder.Append("// Auto Generated Code\n");
            m_csStringBuilder.Append("using System.Collections;\n");
            m_csStringBuilder.Append("using System.Collections.Generic;\n");
            m_csStringBuilder.Append("\n");
            m_csStringBuilder.Append("namespace Game.DataModel\n");
            m_csStringBuilder.Append("{\n");
            m_csStringBuilder.Append("    public partial class ");
            m_csStringBuilder.Append(m_className);
            m_csStringBuilder.Append(" : IXDM\n");
            m_csStringBuilder.Append("    {\n");

            var keys = m_csPropertyNameDic.Keys;
            foreach (var key in keys)
            {
                m_csStringBuilder.Append("        /// <summary>\n");
                m_csStringBuilder.Append("        /// ");
                m_csStringBuilder.Append(m_csPropertyNoteDic[key]);
                m_csStringBuilder.Append("\n        /// </summary>\n");
                m_csStringBuilder.Append("        public ");
                m_csStringBuilder.Append(m_csPropertyTypeDic[key]);
                m_csStringBuilder.Append(" ");
                m_csStringBuilder.Append(m_csPropertyNameDic[key]);
                m_csStringBuilder.Append(" { get; protected set; }\n");
            }

            m_csStringBuilder.Append("\n");
            m_csStringBuilder.Append("        public void Parse(Hashtable table)\n");
            m_csStringBuilder.Append("        {\n");
            foreach (var key in keys)
            {
                OrganizeParseStr(key);
            }
            
            m_csStringBuilder.Append("\n            ParseEx();\n");
            m_csStringBuilder.Append("        }\n");
            m_csStringBuilder.Append("\n        partial void ParseEx();\n\n");
            m_csStringBuilder.Append("    }\n");

            m_csStringBuilder.Append("\n");
            OrganizeXTable();

            m_csStringBuilder.Append("}\n");

            string csFile = string.Format("{0}{1}.cs", CSFilePath, m_className);
            File.WriteAllText(csFile, m_csStringBuilder.ToString(), Encoding.UTF8);
            //Debug.Log("CreateCSFileComplete! " + csFile);
            m_className = "";
            m_csStringBuilder.Length = 0;
            m_csPropertyNameDic.Clear();
            m_csPropertyTypeDic.Clear();
            m_csPropertyNoteDic.Clear();
        }

        private static void OrganizeParseStr(int key)
        {
            string boolStr = "";
            string typeGetstr = "";
            switch (m_csPropertyTypeDic[key])
            {
                case "int":
                    typeGetstr = "GetInt";
                    break;
                case "float":
                    typeGetstr = "GetFloat";
                    break;
                case "string":
                    typeGetstr = "GetString";
                    break;
                case "bool":
                    typeGetstr = "GetInt";
                    boolStr = " == 1";
                    break;
                case "Hashtable":
                    typeGetstr = "GetTable";
                    break;
                case "List<int>":
                    typeGetstr = "GetIntList";
                    break;
                case "List<float>":
                    typeGetstr = "GetFloatList";
                    break;
                case "List<bool>":
                    typeGetstr = "GetBoolList";
                    break;
                case "List<string>":
                    typeGetstr = "GetStringList";
                    break;
                case "List<Hashtable>":
                    typeGetstr = "GetHashtableList";
                    break;
                case "ArrayList":
                    typeGetstr = "GetArrayList";
                    break;
                default:
                    Debug.LogError(m_className + key + "类型错误 " + m_csPropertyTypeDic[key]);
                    break;
            }

            m_csStringBuilder.Append("            ");
            m_csStringBuilder.Append(m_csPropertyNameDic[key]);
            m_csStringBuilder.Append(" = ");
            m_csStringBuilder.Append("table.");
            m_csStringBuilder.Append(typeGetstr);
            m_csStringBuilder.Append("(\"");
            m_csStringBuilder.Append(m_csPropertyNameDic[key]);
            m_csStringBuilder.Append("\")");
            m_csStringBuilder.Append(boolStr);
            m_csStringBuilder.Append(";\n");

        }

        private static void OrganizeXTable()
        {
            string xtableName = m_className.Substring(0, m_className.Length - 3) + "XTable";
            m_csStringBuilder.Append("    public partial class ");
            m_csStringBuilder.Append(xtableName);
            m_csStringBuilder.Append(" : XTable<");
            m_csStringBuilder.Append(m_className);
            m_csStringBuilder.Append(">\n");
            m_csStringBuilder.Append("    {\n");
            m_csStringBuilder.Append("        public override string ResourceName\n");
            m_csStringBuilder.Append("        {\n");
            m_csStringBuilder.Append("            get { return \"");
            m_csStringBuilder.Append(m_className);
            m_csStringBuilder.Append("\";}\n");
            m_csStringBuilder.Append("        }\n");
            m_csStringBuilder.Append("    }\n");

            m_csStringBuilder.Append("\n");
            m_csStringBuilder.Append("    public partial class XTable\n");
            m_csStringBuilder.Append("    {\n");
            m_csStringBuilder.Append("        public static ");
            m_csStringBuilder.Append(xtableName);
            m_csStringBuilder.Append(" ");
            m_csStringBuilder.Append(xtableName);
            m_csStringBuilder.Append(" = new ");
            m_csStringBuilder.Append(xtableName);
            m_csStringBuilder.Append("();\n");
            m_csStringBuilder.Append("    }\n");
        }

    }
}