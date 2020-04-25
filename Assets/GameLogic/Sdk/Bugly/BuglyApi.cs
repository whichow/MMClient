public class BuglyApi
{
    private const string BuglyAppIDForiOS = "xxxx";
    private const string BuglyAppIDForAndroid = "51eea13792";

    public static void InitBuglySDK ()
    {
        Debuger.Log("Bugly InitBuglySDK -----");

        // TODO NOT Required. Set the crash reporter type and log to report
        // BuglyAgent.ConfigCrashReporter (1, 2);

        // TODO NOT Required. Enable debug log print, please set false for release version
#if DEBUG_MY
        BuglyAgent.ConfigDebugMode (true);
#endif
        // TODO NOT Required. Register log callback with 'BuglyAgent.LogCallbackDelegate' to replace the 'Application.RegisterLogCallback(Application.LogCallback)'
        //BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);

        // BuglyAgent.ConfigDefault ("Bugly", null, "ronnie", 0);

        #if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId (BuglyAppIDForiOS);
        #elif UNITY_ANDROID
        BuglyAgent.InitWithAppId (BuglyAppIDForAndroid);
        #endif

        // TODO Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
        // please call this method to enable c# exception handler only.
        BuglyAgent.EnableExceptionHandler ();

        // TODO NOT Required. If you need to report extra data with exception, you can set the extra handler
        // BuglyAgent.SetLogCallbackExtrasHandler (MyLogCallbackExtrasHandler);

        BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
    }

//    // Extra data handler to packet data and report them with exception.
//    // Please do not do hard work in this handler 
//    Dictionary<string, string> MyLogCallbackExtrasHandler ()
//    {
//        // TODO Test log, please do not copy it
//        BuglyAgent.PrintLog (LogSeverity.Log, "extra handler");
//
//        // TODO Sample code, please do not copy it
//        Dictionary<string, string> extras = new Dictionary<string, string> ();
//        extras.Add ("ScreenSolution", string.Format ("{0}x{1}", Screen.width, Screen.height));
//        extras.Add ("deviceModel", SystemInfo.deviceModel);
//        extras.Add ("deviceName", SystemInfo.deviceName);
//        extras.Add ("deviceType", SystemInfo.deviceType.ToString ());
//
//        extras.Add ("deviceUId", SystemInfo.deviceUniqueIdentifier);
//        extras.Add ("gDId", string.Format ("{0}", SystemInfo.graphicsDeviceID));
//        extras.Add ("gDName", SystemInfo.graphicsDeviceName);
//        extras.Add ("gDVdr", SystemInfo.graphicsDeviceVendor);
//        extras.Add ("gDVer", SystemInfo.graphicsDeviceVersion);
//        extras.Add ("gDVdrID", string.Format ("{0}", SystemInfo.graphicsDeviceVendorID));
//
//        extras.Add ("graphicsMemorySize", string.Format ("{0}", SystemInfo.graphicsMemorySize));
//        extras.Add ("systemMemorySize", string.Format ("{0}", SystemInfo.systemMemorySize));
//        extras.Add ("UnityVersion", Application.unityVersion);
// 
//        BuglyAgent.PrintLog (LogSeverity.LogInfo, "Package extra data");
//        return extras;
//    }

}
