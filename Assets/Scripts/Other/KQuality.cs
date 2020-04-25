// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KQuality" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using UnityEngine;

namespace Game
{
    public class KQuality : MonoBehaviour
    {
        public enum Level
        {
            Low = 0,
            Medium = 1,
            High = 2
        }

        public static KQuality Instance;

        public static int DeviceLevel
        {
            get { return Instance ? Instance._deviceLevel : 0; }
        }

        public int _deviceLevel;
        private int _targetFrameRate;

        public int deviceLevel
        {
            get { return this._deviceLevel; }
            set
            {
                //            QualitySettings.SetQualityLevel(value, true);
                this._deviceLevel = value;
                PlayerPrefs.GetInt("quality", _deviceLevel);
            }
        }

        public int targetFrameRate
        {
            get { return this._targetFrameRate; }
            set
            {
                this._targetFrameRate = value;
                Application.targetFrameRate = this._targetFrameRate;
            }
        }

        private void AutoSetLevel()
        {

            int processorCount = SystemInfo.processorCount;
            string processorType = SystemInfo.processorType;

            int memorySize = SystemInfo.systemMemorySize;
            int gmemorySize = SystemInfo.graphicsMemorySize;

#if IPHONE_MY
            iPhoneGeneration iosG = iPhone.generation;
            if ((iosG == iPhoneGeneration.iPad4Gen) || (iosG == iPhoneGeneration.iPad5Gen) || (iosG == iPhoneGeneration.iPhone5) || (iosG == iPhoneGeneration.iPhone5C) || (iosG == iPhoneGeneration.iPhone5S) || (iosG == iPhoneGeneration.iPadMini2Gen) || (iosG == iPhoneGeneration.Unknown))
            {
                this._deviceLevel = (int)Level.High;
            }
            else if ((iosG == iPhoneGeneration.iPadMini1Gen) || (iosG == iPhoneGeneration.iPad2Gen) || (iosG == iPhoneGeneration.iPad3Gen) || (iosG == iPhoneGeneration.iPodTouch5Gen) || (iosG == iPhoneGeneration.iPhone4S))
            {
                this._deviceLevel = (int)Level.Medium;
            }
            else
            {
                this._deviceLevel = (int)Level.Low;
            }
#elif ANDROID_MY
            if (memorySize > 4000 && processorCount >= 4)
            {
                this._deviceLevel = (int)Level.High;
            }
            else if (memorySize > 2000 && processorCount >= 4)
            {
                this._deviceLevel = (int)Level.Medium;
            }
            else
            {
                this._deviceLevel = (int)Level.Low;
            }
#else
            this._deviceLevel = (int)Level.High;
#endif

            //string graphicsDeviceName = SystemInfo.graphicsDeviceName;
            //switch (graphicsDeviceName)
            //{
            //    case "Adreno 540":
            //    case "Adreno 530":
            //    case "Adreno 320":
            //        QualitySettings.SetQualityLevel(2, true);
            //        return;
            //    case "PowerVR SGX 543":
            //        QualitySettings.SetQualityLevel(2, true);
            //        return;
            //    case "PowerVR SGX 540":
            //    case "Mali-400 MP":
            //    case "Adreno (TM) 220":
            //    case "ULP GeForce":
            //    case "NVIDIA Tegra":
            //        QualitySettings.SetQualityLevel(1, true);
            //        return;
            //}
            //QualitySettings.SetQualityLevel(0, true);

        }

        private void ApplyLevel()
        {
            //if (this._deviceLevel == 0)
            //{
            //    this.targetFrameRate = 30;
            //}
            //else if (this._deviceLevel == 1)
            //{
            //    this.targetFrameRate = 45;
            //}
            //else
            //{
            //    this.targetFrameRate = 60;
            //}
        }

        #region UNITY

        private void Awake()
        {
            KQuality.Instance = this;

            this._deviceLevel = PlayerPrefs.GetInt("quality", -1);

            if (this._deviceLevel < 0)
            {
                this.AutoSetLevel();
            }
            this.ApplyLevel();
        }

        #endregion
    }
}
