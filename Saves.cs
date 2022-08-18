using System.IO;
using UnityEngine;

namespace solacerxt.Saving
{
    public static class Saves 
    {
        public static string GetGameDirectory(int index) => 
            Application.persistentDataPath + "/games/" + index;

        /// <summary> 
        /// Loads data with given id. Returns false if there is no such data saved in this game 
        /// </summary>
        public static bool TryLoad<S>(string id, Game game, ref S obj) where S : struct, IStorable => 
            _Load(GetGameDirectory(game.Index) + "/", id, ref obj);

        /// <summary> 
        /// Loads data from box with given id. Returns false if there is no such data saved in this game 
        /// </summary>
        public static bool TryLoadFromBox<T>(string id, Game game, ref T obj) =>
            _LoadFromBox(GetGameDirectory(game.Index) + "/", id, ref obj);

        /// <summary> 
        /// Loads data from box with given id. Returns defaultValue if there is no such data saved in this game
        /// </summary>
        public static T LoadFromBoxOrDefault<T>(string id, Game game, T defaultValue) 
        {
            _LoadFromBox(GetGameDirectory(game.Index) + "/", id, ref defaultValue);
            return defaultValue;
        }

        /// <summary> 
        /// Loads data with given id. Returns null if there is no such data saved in this game 
        /// </summary>
        public static S? Load<S>(string id, Game game) where S : struct, IStorable => 
            _Load<S>(GetGameDirectory(game.Index) + "/", id);

        /// <summary> 
        /// Loads data with given id. Returns false if there is no such data saved in app 
        /// </summary>
        public static bool TryLoad<S>(string id, ref S obj) where S : struct, IStorable => 
            _Load<S>("", id, ref obj);

        /// <summary> 
        /// Loads data from box with given id. Returns false if there is no such data saved in app
        /// </summary>
        public static bool TryLoadFromBox<T>(string id, ref T obj) =>
            _LoadFromBox("", id, ref obj);

        /// <summary> 
        /// Loads data from box with given id. Returns defaultValue if there is no such data saved in app
        /// </summary>
        public static T LoadFromBoxOrDefault<T>(string id, T defaultValue) 
        {
            _LoadFromBox("", id, ref defaultValue);
            return defaultValue;
        }

        /// <summary> 
        /// Loads data with given id. Returns null if there is no such data saved in app 
        /// </summary>
        public static S? Load<S>(string id) where S : struct, IStorable => 
            _Load<S>("", id);

        /// <summary>
        /// Saves data in scope of given game 
        /// </summary>
        public static void Save<S>(ref S data, string id, Game game) where S : struct, IStorable => 
            _Save(ref data, GetGameDirectory(game.Index) + id);

        /// <summary>
        /// Saves data boxed in scope of given game 
        /// </summary>
        public static void SaveBoxed<T>(ref T data, string id, Game game) => 
            _SaveBoxed(ref data, GetGameDirectory(game.Index) + id);

        /// <summary> 
        /// Saves data in app scope 
        /// </summary>
        public static void Save<S>(ref S data, string id) where S : struct, IStorable => 
            _Save(ref data, id);

        /// <summary> 
        /// Saves data boxed in app scope 
        /// </summary>
        public static void SaveBoxed<T>(ref T data, string id) => 
            _SaveBoxed(ref data, id);

        private static void _SaveBoxed<T>(ref T data, string path)
        {
            var box = new SBox<T>(data);
            _Save(ref box, path);
        }

        private static void _Save<S>(ref S data, string path) where S : struct, IStorable
        {
            var serialized = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/" + path, serialized);
        }

        private static bool _LoadFromBox<T>(string localPath, string id, ref T obj)
        {
            var box = new SBox<T>();

            if (_Load(localPath, id, ref box))
            {
                obj = box.Value;
                return true;
            }

            return false;
        }

        private static bool _Load<S>(string localPath, string id, ref S obj) where S : struct, IStorable
        {
            var path = Application.persistentDataPath + "/" + localPath + id;

            if (!File.Exists(path)) return false;
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), obj);

            return true;
        }

        private static S? _Load<S>(string localPath, string id) where S : struct, IStorable
        {
            S data = new S();
            if (!_Load(localPath, id, ref data)) return null;

            return data;
        }
    }
}
