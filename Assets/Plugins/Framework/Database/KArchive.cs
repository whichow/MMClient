// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
#define USE_OBFUSCATE

namespace Game
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// 文件存储方式
    /// </summary>
    public class KArchive
    {
        #region Field

        private string _archiveID;
        private string _archiveKey;

        private Dictionary<string, int> _intTable = new Dictionary<string, int>();
        private Dictionary<string, float> _floatTable = new Dictionary<string, float>();
        private Dictionary<string, string> _stringTable = new Dictionary<string, string>();

        private Dictionary<int, Dictionary<string, int>> _sIntTable = new Dictionary<int, Dictionary<string, int>>();
        private Dictionary<int, Dictionary<string, float>> _sFloatTable = new Dictionary<int, Dictionary<string, float>>();
        private Dictionary<int, Dictionary<string, string>> _sStringTable = new Dictionary<int, Dictionary<string, string>>();

        private int _modifyCount;

        #endregion

        #region PROPERTY

        private string fileName
        {
            get { return KUtils.GenerateHash(_archiveID) + ".gxx"; }
        }

        private bool modified
        {
            get { return _modifyCount > 0; }
            set { _modifyCount = value ? 1 : 0; }
        }

        #endregion

        #region Constructor

        private KArchive(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _archiveID = "supercalifragilisticexpiadocious";
            }
            else
            {
                _archiveID = id;
            }

            _archiveKey = "afkdieolspfsfopeocpmnlghskfjlghw";
        }

        #endregion

        #region Get Set Key 

        /// <summary>Gets the int.</summary>
        public int GetInt(string key, int defaultValue)
        {
            int tmp;
            if (_intTable.TryGetValue(key, out tmp))
            {
                return tmp;
            }
            return defaultValue;
        }

        /// <summary>Gets the int.</summary>
        public int GetInt(int index, string key, int defaultValue)
        {
            Dictionary<string, int> tmpDictionary;
            if (_sIntTable.TryGetValue(index, out tmpDictionary))
            {
                int tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    return tmp;
                }
            }
            return defaultValue;
        }

        /// <summary>Sets the int.</summary>
        public void SetInt(string key, int value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            int tmp;
            if (_intTable.TryGetValue(key, out tmp))
            {
                if (value != tmp)
                {
                    _intTable[key] = value;
                    _modifyCount++;
                }
            }
            else
            {
                _intTable.Add(key, value);
                _modifyCount++;
            }
        }

        /// <summary>Sets the int.</summary>
        public void SetInt(int index, string key, int value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Dictionary<string, int> tmpDictionary;
            if (_sIntTable.TryGetValue(index, out tmpDictionary))
            {
                int tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    if (value != tmp)
                    {
                        tmpDictionary[key] = value;
                        _modifyCount++;
                    }
                }
                else
                {
                    tmpDictionary.Add(key, value);
                    _modifyCount++;
                }
            }
            else
            {
                tmpDictionary = new Dictionary<string, int>();
                tmpDictionary.Add(key, value);
                _sIntTable.Add(index, tmpDictionary);
                _modifyCount++;
            }
        }

        /// <summary>Gets the float.</summary>
        public float GetFloat(string key, float defaultValue)
        {
            float tmp;
            if (_floatTable.TryGetValue(key, out tmp))
            {
                return tmp;
            }
            return defaultValue;
        }

        /// <summary>Gets the float.</summary>
        public float GetFloat(int index, string key, float defaultValue)
        {
            Dictionary<string, float> tmpDictionary;
            if (_sFloatTable.TryGetValue(index, out tmpDictionary))
            {
                float tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    return tmp;
                }
            }

            return defaultValue;
        }

        /// <summary>Sets the float.</summary>
        public void SetFloat(string key, float value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            float tmp;
            if (_floatTable.TryGetValue(key, out tmp))
            {
                if (value != tmp)
                {
                    _floatTable[key] = value;
                    _modifyCount++;
                }
            }
            else
            {
                _floatTable.Add(key, value);
                _modifyCount++;
            }
        }

        /// <summary>Sets the float.</summary>
        public void SetFloat(int index, string key, float value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Dictionary<string, float> tmpDictionary;
            if (_sFloatTable.TryGetValue(index, out tmpDictionary))
            {
                float tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    if (value != tmp)
                    {
                        tmpDictionary[key] = value;
                        _modifyCount++;
                    }
                }
                else
                {
                    tmpDictionary.Add(key, value);
                    _modifyCount++;
                }
            }
            else
            {
                tmpDictionary = new Dictionary<string, float>();
                tmpDictionary.Add(key, value);
                _sFloatTable.Add(index, tmpDictionary);
                _modifyCount++;
            }
        }

        /// <summary>Gets the string.</summary>
        public string GetString(string key, string defaultValue)
        {
            string tmp;
            if (_stringTable.TryGetValue(key, out tmp))
            {
                return tmp;
            }
            return defaultValue;
        }
        /// <summary>Gets the string.</summary>
        public string GetString(int index, string key, string defaultValue)
        {
            Dictionary<string, string> tmpDictionary;
            if (_sStringTable.TryGetValue(index, out tmpDictionary))
            {
                string tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    return tmp;
                }
            }
            return defaultValue;
        }

        /// <summary>Sets the string.</summary>
        public void SetString(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            string tmp;
            if (_stringTable.TryGetValue(key, out tmp))
            {
                if (value != tmp)
                {
                    _stringTable[key] = value;
                    _modifyCount++;
                }
            }
            else
            {
                _stringTable.Add(key, value);
                _modifyCount++;
            }
        }

        /// <summary>Sets the string.</summary>
        public void SetString(int index, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Dictionary<string, string> tmpDictionary;
            if (_sStringTable.TryGetValue(index, out tmpDictionary))
            {
                string tmp;
                if (tmpDictionary.TryGetValue(key, out tmp))
                {
                    if (value != tmp)
                    {
                        tmpDictionary[key] = value;
                        _modifyCount++;
                    }
                }
                else
                {
                    tmpDictionary.Add(key, value);
                    _modifyCount++;
                }
            }
            else
            {
                tmpDictionary = new Dictionary<string, string>();
                tmpDictionary.Add(key, value);
                _sStringTable.Add(index, tmpDictionary);
                _modifyCount++;
            }
        }

        #endregion

        #region READ AND WRITE

        /// <summary>Reads the profiles.��д�����ͽṹһ��.</summary>
        private void ReadFromStream(MemoryStream stream)
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                var tmpUid = binaryReader.ReadString();

                if (_archiveID != tmpUid)
                {
                    return;
                }

                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                int length = binaryReader.ReadInt32();
                _intTable = new Dictionary<string, int>(length);
                for (int i = 0; i < length; i++)
                {
                    var key = binaryReader.ReadString();
                    var value = binaryReader.ReadInt32();
                    _intTable.Add(key, value);
                }

                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                length = binaryReader.ReadInt32();
                _floatTable = new Dictionary<string, float>(length);
                for (int i = 0; i < length; i++)
                {
                    var key = binaryReader.ReadString();
                    var value = binaryReader.ReadSingle();
                    _floatTable.Add(key, value);
                }

                //string
                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                length = binaryReader.ReadInt32();
                _stringTable = new Dictionary<string, string>(length);
                for (int i = 0; i < length; i++)
                {
                    var key = binaryReader.ReadString();
                    var value = binaryReader.ReadString();
                    _stringTable.Add(key, value);
                }

                // sInt
                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                length = binaryReader.ReadInt32();
                _sIntTable = new Dictionary<int, Dictionary<string, int>>(length);
                for (int i = 0; i < length; i++)
                {
                    var index = binaryReader.ReadInt32();

                    int length2 = binaryReader.ReadInt32();
                    var tmpDictionary = new Dictionary<string, int>(length2);
                    for (int j = 0; j < length2; j++)
                    {
                        var key = binaryReader.ReadString();
                        var value = binaryReader.ReadInt32();
                        tmpDictionary.Add(key, value);
                    }
                    _sIntTable.Add(index, tmpDictionary);
                }

                // sFloat
                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                length = binaryReader.ReadInt32();
                _sFloatTable = new Dictionary<int, Dictionary<string, float>>(length);
                for (int i = 0; i < length; i++)
                {
                    var index = binaryReader.ReadInt32();

                    int length2 = binaryReader.ReadInt32();
                    var tmpDictionary = new Dictionary<string, float>(length2);
                    for (int j = 0; j < length2; j++)
                    {
                        var key = binaryReader.ReadString();
                        var value = binaryReader.ReadSingle();
                        tmpDictionary.Add(key, value);
                    }
                    _sFloatTable.Add(index, tmpDictionary);
                }

                // sString
                if (stream.Length - stream.Position < 4L)
                {
                    return;
                }
                length = binaryReader.ReadInt32();
                _sStringTable = new Dictionary<int, Dictionary<string, string>>(length);
                for (int i = 0; i < length; i++)
                {
                    var index = binaryReader.ReadInt32();

                    int length2 = binaryReader.ReadInt32();
                    var tmpDictionary = new Dictionary<string, string>(length2);
                    for (int j = 0; j < length2; j++)
                    {
                        var key = binaryReader.ReadString();
                        var value = binaryReader.ReadString();
                        tmpDictionary.Add(key, value);
                    }
                    _sStringTable.Add(index, tmpDictionary);
                }
            }
        }

        /// <summary>Writes to stream.��д�����ͽṹһ��.</summary>
        private void WriteToStream(MemoryStream stream)
        {
            using (var binaryWriter = new BinaryWriter(stream))
            {
                binaryWriter.Write(_archiveID);

                // int
                binaryWriter.Write(_intTable.Count);
                foreach (var current in _intTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value);
                }

                //float
                binaryWriter.Write(_floatTable.Count);
                foreach (var current in _floatTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value);
                }

                //string
                binaryWriter.Write(_stringTable.Count);
                foreach (var current in _stringTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value);
                }

                // sInt
                binaryWriter.Write(_sIntTable.Count);
                foreach (var current in _sIntTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value.Count);
                    foreach (var current1 in current.Value)
                    {
                        binaryWriter.Write(current1.Key);
                        binaryWriter.Write(current1.Value);
                    }
                }

                // sFloat
                binaryWriter.Write(_sFloatTable.Count);
                foreach (var current in _sFloatTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value.Count);
                    foreach (var current1 in current.Value)
                    {
                        binaryWriter.Write(current1.Key);
                        binaryWriter.Write(current1.Value);
                    }
                }

                // sString
                binaryWriter.Write(_sStringTable.Count);
                foreach (var current in _sStringTable)
                {
                    binaryWriter.Write(current.Key);
                    binaryWriter.Write(current.Value.Count);
                    foreach (var current1 in current.Value)
                    {
                        binaryWriter.Write(current1.Key);
                        binaryWriter.Write(current1.Value);
                    }
                }
            }
        }

        /// <summary>Encrypts the specified udid.</summary>
        private void Encrypt(byte[] data)
        {
            Obfuscate(_archiveKey, data, 0, data.Length);
        }

        /// <summary>Decrypts the specified udid.</summary>
        private void Decrypt(byte[] data)
        {
            Obfuscate(_archiveKey, data, 0, data.Length);
        }

        #endregion

        #region STATIC

        /// <summary>The g_local documents path</summary>
        private static string localDocumentsPath
        {
            get { return Application.persistentDataPath; }
        }

        /// <summary>The g_backup documents path</summary>
        private static string backupDocumentsPath
        {
            get { return Application.temporaryCachePath; }
        }

        private static byte[] ReadAllBytesLocalOrBackup(string fileName)
        {
            if (!string.IsNullOrEmpty(localDocumentsPath))
            {
                string localFile = KFileUtils.Combine(localDocumentsPath, fileName);

                if (KFileUtils.ExistsFile(localFile))
                {
                    byte[] result;
                    if (!KFileUtils.ReadAllBytes(localFile, out result))
                    {
                        Debug.LogError("Failed to load local file: " + localFile);
                    }
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(backupDocumentsPath))
            {
                string backupFile = KFileUtils.Combine(backupDocumentsPath, fileName);

                if (KFileUtils.ExistsFile(backupFile))
                {
                    byte[] result;
                    if (!KFileUtils.ReadAllBytes(backupFile, out result))
                    {
                        Debug.LogError("Failed to load backup file: " + backupFile);
                    }
                    return result;
                }
            }

            return null;
        }

        private static void WriteAllBytesLocalAndBackup(string fileName, byte[] bytes)
        {
            if (!string.IsNullOrEmpty(localDocumentsPath))
            {
                string localFile = KFileUtils.Combine(localDocumentsPath, fileName);

                if (!KFileUtils.WriteAllBytes(localFile, bytes))
                {
                    Debug.LogError("Failed to save local file: " + localFile);
                }
            }

            if (!string.IsNullOrEmpty(backupDocumentsPath))
            {
                string backupFile = KFileUtils.Combine(backupDocumentsPath, fileName);

                if (!KFileUtils.WriteAllBytes(backupFile, bytes))
                {
                    Debug.LogError("Failed to save backup file: " + backupFile);
                }
            }
        }

        /// <summary>Obfuscates the specified udid.</summary>
        private static void Obfuscate(string udid, byte[] data, int offset, int length)
        {
#if USE_OBFUSCATE
            int dl = data.Length;
            int ul = udid.Length;

            for (int i = 0, j = 0; i < dl; i++, j++)
            {
                j = j < ul ? j : 0;
                data[i] = (byte)(data[i] ^ udid[j]);
            }
#endif
        }

        #endregion

        #region Merges

        /// <summary>Merges the specified other.</summary>
        /// <param name="other">The other.</param>
        public void Merge(KArchive other)
        {
            foreach (var current in other._intTable)
            {
                int tmpValue;
                if (this._intTable.TryGetValue(current.Key, out tmpValue))
                {
                    if (current.Value > tmpValue)
                    {
                        this._intTable[current.Key] = current.Value;
                        this._modifyCount++;
                    }
                }
                else
                {
                    this._intTable[current.Key] = current.Value;
                    this._modifyCount++;
                }
            }

            foreach (var current in other._floatTable)
            {
                float tmpValue;
                if (this._floatTable.TryGetValue(current.Key, out tmpValue))
                {
                    if (current.Value > tmpValue)
                    {
                        this._floatTable[current.Key] = current.Value;
                        this._modifyCount++;
                    }
                }
                else
                {
                    this._floatTable[current.Key] = current.Value;
                    this._modifyCount++;
                }
            }

            foreach (var current in other._stringTable)
            {
                string tmpValue;
                if (!this._stringTable.TryGetValue(current.Key, out tmpValue))
                {
                    this._stringTable[current.Key] = current.Value;
                    this._modifyCount++;
                }
            }
        }

        /// <summary>Migrates the int.</summary>
        /// <param name="key">The key.</param>
        private void MigrateInt(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                this.SetInt(key, PlayerPrefs.GetInt(key));
                PlayerPrefs.DeleteKey(key);
            }
        }

        /// <summary>Unmigrate the int.</summary>
        /// <param name="key">The key.</param>
        private void UnmigrateInt(string key)
        {
            int tmpValue;
            if (_intTable.TryGetValue(key, out tmpValue))
            {
                PlayerPrefs.SetInt(key, tmpValue);
            }
        }

        /// <summary>Migrates the float.</summary>
        /// <param name="key">The key.</param>
        private void MigrateFloat(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                this.SetFloat(key, PlayerPrefs.GetFloat(key));
                PlayerPrefs.DeleteKey(key);
            }
        }

        /// <summary>Unmigrate the float.</summary>
        /// <param name="key">The key.</param>
        private void UnmigrateFloat(string key)
        {
            float tmpValue;
            if (_floatTable.TryGetValue(key, out tmpValue))
            {
                PlayerPrefs.SetFloat(key, tmpValue);
            }
        }

        /// <summary>Migrates the string.</summary>
        /// <param name="key">The key.</param>
        private void MigrateString(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                this.SetString(key, PlayerPrefs.GetString(key));
                PlayerPrefs.DeleteKey(key);
            }
        }

        /// <summary>Unmigrates the string.</summary>
        /// <param name="key">The key.</param>
        private void UnmigrateString(string key)
        {
            string tmpValue;
            if (_stringTable.TryGetValue(key, out tmpValue))
            {
                PlayerPrefs.SetString(key, tmpValue);
            }
        }

        #endregion

        /// <summary>Loads this instance.</summary>
        private void LoadFile()
        {
            _modifyCount = 0;

            var bytes = ReadAllBytesLocalOrBackup(this.fileName);
            if (bytes != null && bytes.Length > 0)
            {
                this.Decrypt(bytes);
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    ReadFromStream(memoryStream);
                }
            }
        }

        /// <summary>Saves this instance.</summary>
        private void SaveFile()
        {
            if (_modifyCount > 0)
            {
                _modifyCount = 0;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    this.WriteToStream(memoryStream);

                    var bytes = memoryStream.ToArray();
                    this.Encrypt(bytes);

                    WriteAllBytesLocalAndBackup(this.fileName, bytes);
                }
            }
        }

        #region MANAGER 

        public static KArchive Load(string uid)
        {
            _Instance = new KArchive(SystemInfo.deviceUniqueIdentifier);
            return _Instance;
        }

        public static void Save()
        {
            if (_Instance != null)
            {
                _Instance.SaveFile();
            }
        }

        public static void Log()
        {
            if (_Instance != null)
            {
            }
        }

        private static KArchive _Instance;

        public static KArchive Instance
        {
            get { return _Instance; }
        }

        #endregion
    }
}
