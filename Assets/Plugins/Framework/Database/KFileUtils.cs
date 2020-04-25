// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Name
// Last Modified On : 
// ***********************************************************************
namespace Game
{
    using System.IO;
    using UnityEngine;

    public static class KFileUtils
    {
        /// <summary>Combines the specified path1.合并两个路径字符串.</summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>Gets the extension.返回指定的路径字符串的扩展名.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
        /// <summary>Gets the name of the file.返回指定路径字符串的文件名和扩展名.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
        /// <summary>Gets the file name without extension.返回指定路径字符串的文件名.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>Gets the files.返回指定目录中的文件的名称.</summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        /// <summary>Gets the creation time.返回指定文件的创建日期和时间.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static long GetCreationTime(string path)
        {
            return File.GetCreationTimeUtc(path).Ticks;
        }

        /// <summary>Gets the last access time.返回上次访问指定文件的日期和时间.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static long GetLastAccessTime(string path)
        {
            return File.GetLastAccessTimeUtc(path).Ticks;
        }

        /// <summary>Gets the last write time.返回上次写入指定的文件的日期和时间.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static long GetLastWriteTime(string path)
        {
            return File.GetLastWriteTimeUtc(path).Ticks;
        }
        /// <summary>Compares the last write time.</summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns></returns>
        public static long CompareLastWriteTime(string path1, string path2)
        {
            return (File.GetLastWriteTimeUtc(path1) - File.GetLastWriteTimeUtc(path2)).Ticks;
        }

        /// <summary>Deletes the specified path.</summary>
        /// <param name="path">The path.</param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>Moves the specified source file name.</summary>
        /// <param name="sourFileName">Name of the sour file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        public static void MoveFile(string sourFileName, string destFileName)
        {
            File.Move(sourFileName, destFileName);
        }

        /// <summary>Existses the specified path.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>Reads all bytes.</summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool ReadAllBytes(string path, out byte[] bytes)
        {
            bytes = null;
            try
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                }
                return true;
            }
            catch (System.Exception)
            {
#if DEBUG_MY
                Debug.LogError("Failed read file: " + path);
#endif
                return false;
            }
        }

        /// <summary>Writes all bytes.</summary>
        /// <param name="path">The path.</param>
        /// <param name="bytes">The bytes.</param>
        public static bool WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                using (FileStream fs = File.OpenWrite(path))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                return true;
            }
            catch (System.Exception)
            {
#if DEBUG_MY
                Debug.LogError("Failed write file: " + path);
#endif
                return false;
            }
        }
    }
}
