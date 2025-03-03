﻿using System.Collections.Generic;
using System.IO;

namespace MelonLoader.Il2CppAssemblyGenerator.Packages
{
    internal class Cpp2IL : DumperBase
    {
        internal Cpp2IL()
        {
            Version = MelonLaunchOptions.Il2CppAssemblyGenerator.ForceVersion_Dumper;
#if !DEBUG
            if (string.IsNullOrEmpty(Version) || Version.Equals("0.0.0.0"))
                Version = RemoteAPI.Info.ForceDumperVersion;
#endif
            if (string.IsNullOrEmpty(Version) || Version.Equals("0.0.0.0"))
                Version = "2021.5.3";

            Destination = Path.Combine(Core.BasePath, "Cpp2IL");
            Output = Path.Combine(Destination, "cpp2il_out");
            URL = $"https://github.com/SamboyCoding/Cpp2IL/releases/download/{Version}/Cpp2IL-{Version}-Windows-Netframework472.zip";
            ExePath = Path.Combine(Destination, "Cpp2IL.exe"); 
        }

        private void Save()
        {
            Config.Values.DumperVersion = Version;
            Config.Save();
        }

        private bool ShouldDownload() =>
            string.IsNullOrEmpty(Config.Values.DumperVersion)
            || !Config.Values.DumperVersion.Equals(Version);

        internal override void Cleanup() { }

        internal override bool Download()
        {
            if (!ShouldDownload())
            {
                MelonLogger.Msg("Cpp2IL is up to date. No Download Needed.");
                return true;
            }
            MelonLogger.Msg("Downloading Cpp2IL...");
            if (base.Download())
            {
                Save();
                return true;
            }

            ThrowInternalFailure("Failed to Download Cpp2IL!");
            return false;
        }

        internal override bool Execute()
        {
            MelonLogger.Msg("Executing Cpp2IL...");
            if (Execute(new string[] {
                MelonDebug.IsEnabled() ? "--verbose" : string.Empty,
                "--game-path",
                "\"" + Path.GetDirectoryName(Core.GameAssemblyPath) + "\"",
                "--exe-name",
                "\"" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + "\"",
                "--skip-analysis",
                "--skip-metadata-txts",
                "--disable-registration-prompts"
            }, false, new Dictionary<string, string>() {
                {"NO_COLOR", "1"}
            }))
                return true;

            ThrowInternalFailure("Failed to Execute Cpp2IL!");
            return false;
        }
    }
}