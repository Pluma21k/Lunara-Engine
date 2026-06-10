using Lunara2D.Common;
using Lunara2D.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace Lunara2D.DataManagement
{
    public enum LAssetType
    {
        Texture,
        Audio,
        Mesh,
        Scene
    }

    public struct LAsset
    {
        public string Name;
        public string Path;
        public LAssetType Type;
        public object RuntimeRef;
    }

    public static class LAssetManager
    {
        public static string EditorFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "editor");
        public static string AssetsFolderPath = Path.Combine(EditorFolderPath, "assets");
        public static string CacheFolderPath = Path.Combine(EditorFolderPath, "cache");
        public static string TexturesFolderPath = Path.Combine(AssetsFolderPath, "textures");

        private static Dictionary<string, LAsset> _registry = new();

        internal static void Init()
        {
            Directory.CreateDirectory(EditorFolderPath);
            Directory.CreateDirectory(AssetsFolderPath);
            Directory.CreateDirectory(CacheFolderPath);
            Directory.CreateDirectory(TexturesFolderPath);

            ScanAssets();

            LDebug.LogInfo($"Scanned in found all assets ({_registry.Count}) from assets folder path: " + AssetsFolderPath);
        }

        internal static void Dispose()
        {
            _registry.Clear();
        }

        private static void ScanAssets()
        {
            if (!Directory.Exists(TexturesFolderPath)) return;

            var files = Directory.GetFiles(TexturesFolderPath);
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);

                if (_registry.ContainsKey(name)) continue;

                var texture = new LTexture(name);

                _registry[name] = new LAsset
                {
                    Name = name,
                    Path = file,
                    Type = LAssetType.Texture,
                    RuntimeRef = texture
                };
            }
        }

        internal static LAsset ImportTexture(string name)
        {
            if (_registry.TryGetValue(name, out var existingAsset))
            {
                return existingAsset;
            }

            string path = Path.Combine(TexturesFolderPath, name + ".png");

            LTexture texture = new LTexture(name);

            LAsset asset = new LAsset
            {
                Name = name,
                Path = path,
                Type = LAssetType.Texture,
                RuntimeRef = texture
            };

            _registry[name] = asset;
            return asset;
        }

        public static LAsset GetAsset(string name)
        {
            if (_registry.TryGetValue(name, out var asset))
            {
                return asset;
            }
            return default;
        }

        public static LTexture GetTexture(string name)
        {
            if (_registry.TryGetValue(name, out var asset) && asset.Type == LAssetType.Texture)
            {
                return (LTexture)asset.RuntimeRef;
            }
            return null;
        }

        public static Dictionary<string, LAsset> GetAssets()
        {
            return _registry;
        }
    }
}