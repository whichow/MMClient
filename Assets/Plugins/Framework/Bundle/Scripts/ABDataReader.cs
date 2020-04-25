// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using System.Collections;
using System.IO;

namespace K.AB
{

    public class ABDataReader
    {
        public virtual ABData[] Read(Stream stream)
        {
            var sr = new StreamReader(stream);
            var text = sr.ReadToEnd();
            sr.Close();

            var table = KJson.ToJsonTable(text);
            var package = table.GetArrayList(ABDefine.kPackageKey);
            int index = 0;
            var abDataArray = new ABData[package.Count - 1];
            for (int i = abDataArray.Length - 1; i >= 0; i--)
            {
                abDataArray[i] = new ABData();
            }
            KJson.Resolve(package, (t) =>
            {
                abDataArray[index++].Load(t);
            });
            return abDataArray;
        }

        public virtual ABData[] Read(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                var table = KJson.ToJsonTable(bytes);
                var package = table.GetArrayList(ABDefine.kPackageKey);
                int index = 0;
                var abDataArray = new ABData[package.Count - 1];
                for (int i = abDataArray.Length - 1; i >= 0; i--)
                {
                    abDataArray[i] = new ABData();
                }
                KJson.Resolve(package, (t) =>
                 {
                     abDataArray[index++].Load(t);
                 });
                return abDataArray;
            }
            return null;
        }

        public virtual ABData[] Read(Hashtable table)
        {
            if (table != null)
            {
                var package = table.GetArrayList(ABDefine.kPackageKey);
                int index = 0;
                var abDataArray = new ABData[package.Count - 1];
                for (int i = abDataArray.Length - 1; i >= 0; i--)
                {
                    abDataArray[i] = new ABData();
                }
                KJson.Resolve(package, (t) =>
                {
                    abDataArray[index++].Load(t);
                });
                return abDataArray;
            }
            return null;
        }
    }
}