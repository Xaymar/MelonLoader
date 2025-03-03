﻿using System.IO;
using UnityEngine;
using Tomlet;
using Tomlet.Attributes;
using Tomlet.Models;
using MelonLoader;

namespace MelonLoader.MelonStartScreen
{
    internal static class UICustomization
    {
        private static string FilePath;

        internal static cGeneral General;
        internal static cBackground Background;
        internal static cLogoImage LogoImage;
        internal static cLoadingImage LoadingImage;
        internal static cVersionText VersionText;
        internal static cProgressText ProgressText;
        internal static cProgressBar ProgressBar;

        internal static void Load()
        {
            FilePath = Path.Combine(Core.FolderPath, "Config.cfg");
            TomletMain.RegisterMapper(WriteColor, ReadColor);

            General = CreateCat<cGeneral>(nameof(General));
            Background = CreateCat<cBackground>(nameof(Background));
            LogoImage = CreateCat<cLogoImage>(nameof(LogoImage));
            LoadingImage = CreateCat<cLoadingImage>(nameof(LoadingImage));
            VersionText = CreateCat<cVersionText>(nameof(VersionText));
            ProgressText = CreateCat<cProgressText>(nameof(ProgressText));
            ProgressBar = CreateCat<cProgressBar>(nameof(ProgressBar));
        }

        private static T CreateCat<T>(string name) where T : new()
        {
            Preferences.MelonPreferences_ReflectiveCategory cat = MelonPreferences.CreateCategory<T>(name, name);
            cat.SetFilePath(FilePath, printmsg: false);
            cat.SaveToFile(false);
            return cat.GetValue<T>();
        }
        
        internal class cGeneral
        {
            [TomlPrecedingComment("Toggles the Entire Start Screen")]
            internal bool UseStartScreen = true;
        }

        internal class cBackground
        {
            [TomlPrecedingComment("Solid RGBA Color for Background")]
            internal Color SolidColor = new Color(0.08f, 0.09f, 0.10f);
            [TomlPrecedingComment("If it should Scan for and Load Custom Background Image (.gif | .png | .jpg | .jpeg)")]
            internal bool ScanForCustomImage = true;
            [TomlPrecedingComment("UnityEngine.FilterMode for Custom Background Image (Point | Bilinear | Trilinear)")]
            internal FilterMode Filter = FilterMode.Bilinear;
        }

        internal class cLogoImage
        {
            [TomlPrecedingComment("Toggles the Logo Image")]
            internal bool Enabled = true;
            [TomlPrecedingComment("If it should Scan for and Load Custom Logo Image (.gif | .png | .jpg | .jpeg)")]
            internal bool ScanForCustomImage = true;
            [TomlPrecedingComment("UnityEngine.FilterMode for Custom Logo Image (Point | Bilinear | Trilinear)")]
            internal FilterMode Filter = FilterMode.Bilinear;
        }

        internal class cLoadingImage
        {
            [TomlPrecedingComment("Toggles the Loading Image")]
            internal bool Enabled = true;
            [TomlPrecedingComment("If it should Scan for and Load Custom Loading Image (.gif | .png | .jpg | .jpeg)")]
            internal bool ScanForCustomImage = true;
            [TomlPrecedingComment("UnityEngine.FilterMode for Custom Loading Image (Point | Bilinear | Trilinear)")]
            internal FilterMode Filter = FilterMode.Bilinear;
        }

        internal class cVersionText
        {
            [TomlPrecedingComment("Toggles the Version Text")]
            internal bool Enabled = true;
            [TomlPrecedingComment("RGBA Color of Version Text")]
            internal Color TextColor = new Color(1, 1, 1);
        }

        internal class cProgressText
        {
            [TomlPrecedingComment("Toggles the Progress Text")]
            internal bool Enabled = true;
            [TomlPrecedingComment("RGBA Color of Progress Text")]
            internal Color TextColor = new Color(1, 1, 1);
        }

        internal class cProgressBar
        {
            [TomlPrecedingComment("Toggles the Progress Bar")]
            internal bool Enabled = true;
            [TomlPrecedingComment("Inner RGBA Color of Progress Bar")]
            internal Color InnerColor = new Color(1.00f, 0.23f, 0.42f);
            [TomlPrecedingComment("Outer RGBA Color of Progress Bar")]
            internal Color OuterColor = new Color(0.47f, 0.97f, 0.39f);
        }

        private static Color ReadColor(TomlValue value)
        {
            float[] floats = MelonPreferences.Mapper.ReadArray<float>(value);
            if (floats == null || floats.Length != 4)
                return default;
            return new Color(floats[0] / 255f, floats[1] / 255f, floats[2] / 255f, floats[3] / 255f);
        }

        private static TomlValue WriteColor(Color value)
        {
            float[] floats = new[] { value.r * 255, value.g * 255, value.b * 255, value.a * 255 };
            return MelonPreferences.Mapper.WriteArray(floats);
        }
    }
}
