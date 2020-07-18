using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CQ.AntiCheat
{
    namespace Android
    {
        public class AndroidRootDetector : AndroidBase
        {
            /// <summary>
            /// 루팅시 생성되는 경로 리스트
            /// </summary>
            static readonly string[] paths = new[]
            {
                "/system/bin/su",
                "/system/xbin/su",
                "/system/app/SuperUser.apk",
                "/data/data/com.noshufou.android.su",
                "/sbin/su",
            };
            
            public static string GetData()
            {
                StringBuilder sb = new StringBuilder();
#if UNITY_ANDROID
                if (Application.platform == RuntimePlatform.Android)
                {
                    // Store (PlayStore, App Store)
                    sb.Append(Application.installerName); 
                    sb.Append("/");

                    // deployed mode Debug, Release
                    sb.Append(Application.installMode);
                    sb.Append("/");

                    // unique guid for each build
                    sb.Append(Application.buildGUID);
                    sb.Append("/");

                    // TF (extracted or not)
                }
#endif
                return sb.ToString();
            }

            public static bool IsRooted()
            {
                return paths.Any(IsRooted_Internal);
            }

            static bool IsRooted_Internal(string path)
            {
                bool boolTemp = File.Exists(path);

                return boolTemp;
            }

            public static bool IsEmulator()
            {
                if (Application.platform != RuntimePlatform.Android) 
                    return true;
                
                AndroidJavaClass osBuild = new AndroidJavaClass("android.os.Build");

                string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
                string model = osBuild.GetStatic<string>("MODEL");
                string menufacturer = osBuild.GetStatic<string>("MANUFACTURER");
                string brand = osBuild.GetStatic<string>("BRAND");
                string device = osBuild.GetStatic<string>("DEVICE");
                string product = osBuild.GetStatic<string>("PRODUCT");

                return fingerPrint.Contains("generic")
                       || fingerPrint.Contains("unknown")
                       || model.Contains("google_sdk")
                       || model.Contains("Emulator")
                       || model.Contains("Android SDK built for x86")
                       || menufacturer.Contains("Genymotion")
                       || (brand.Contains("generic") && device.Contains("generic"))
                       || product.Equals("google_sdk")
                       || product.Equals("unknown");
            }
        }
    }
}