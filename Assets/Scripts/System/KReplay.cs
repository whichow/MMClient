// ***********************************************************************
// Company          : Kunpo
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
using UnityEngine;
using System.Collections;

namespace Game
{

    /// <summary>
    /// KReplay
    /// </summary>
    public class KReplay : Singleton<KReplay>
    {
        #region CONST

        #endregion

        #region ENUM

        #endregion

        #region MODEL

        #endregion

        #region FIELD

        #endregion

        #region PROPERTY

        public bool isSupported
        {
            get
            {
				#if IPHONE_MY
				return iOSRecordingIsSupported() == 1;
				#endif
                return false;
            }
        }

        public bool isRecording {
			get;
			set;
        }

        #endregion

        #region METHOD

        public void StartRecording()
        {
			isRecording = true;
			#if IPHONE_MY
			iOSStartRecording();
			#endif
        }

        public void StopRecording()
        {
			isRecording = false;
			#if IPHONE_MY
			iOSStopRecording();
			#endif
        }

		public void PreviewRecording()
        {
			#if IPHONE_MY
			iOSPreviewRecording();
			#endif
        }

		#if IPHONE_MY
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static int iOSRecordingIsSupported();
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void iOSStartRecording();
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void iOSStopRecording();
		[System.Runtime.InteropServices.DllImport("__Internal")]
		private extern static void iOSPreviewRecording();
		#endif

        #endregion

        #region UNITY 

//        void OnGUI()
//        {
//            if (!isSupported)
//            {
//                return;
//            }
//            var recording = isRecording;
//            string caption = recording ? "Stop Recording" : "Start Recording";
//            if (GUI.Button(new Rect(10, 10, 500, 200), caption))
//            {
//                recording = !recording;
//                if (recording)
//                {
//                    StartRecording();
//                }
//                else
//                {
//                    StopRecording();
//                }
//            }
//
//			if (true)
//            {
//                if (GUI.Button(new Rect(10, 350, 500, 200), "Preview"))
//                {
//					PreviewRecording();
//                }          
//            }
//        }

        #endregion

        #region STATIC


        #endregion
    }
}
