using System;
using System.IO;
using UnityEngine;

namespace solacerxt.Saving
{
    public static class Saves 
    {
        public static string GetGameDirectory(int index) => 
            Application.persistentDataPath + "/saves/" + index;

        /// <summary> 
        /// Loads data with given id. Returns false if there is no such data saved in this game 
        /// </summary>
        public static bool TryLoad<S>(string id, ISavedGame savedGame, ref S obj, Func<string, string>? decoder = null) where S : struct, IStorable => 
            _Load(GetGameDirectory(savedGame.Index) + "/", id, ref obj, decoder);

        /// <summary> 
        /// Loads data from box with given id. Returns false if there is no such data saved in this game 
        /// </summary>
        public static bool TryLoadFromBox<T>(string id, ISavedGame savedGame, ref T obj, Func<string, string>? decoder = null) =>
            _LoadFromBox(GetGameDirectory(savedGame.Index) + "/", id, ref obj, decoder);

        /// <summary> 
        /// Loads data from box with given id. Returns defaultValue if there is no such data saved in this game
        /// </summary>
        public static T LoadFromBoxOrDefault<T>(string id, ISavedGame savedGame, T defaultValue, Func<string, string>? decoder = null) 
        {
            _LoadFromBox(GetGameDirectory(savedGame.Index) + "/", id, ref defaultValue, decoder);
            return defaultValue;
        }

        ///<summary> 
        ///Loads data with given id. Returns null if there is no such data saved in this game 
        ///</summary>
        public static S? Load<S>(string id, ISavedGame savedGame, Func<string, string>? decoder = null) where S : struct, IStorable => 
            _Load<S>(GetGameDirectory(savedGame.Index) + "/", id, decoder);

        /// <summary> 
        /// Loads data with given id. Returns false if there is no such data saved in app 
        /// </summary>
        public static bool TryLoad<S>(string id, ref S obj, Func<string, string>? decoder = null) where S : struct, IStorable => 
            _Load<S>("", id, ref obj, decoder);

        /// <summary> 
        /// Loads data from box with given id. Returns false if there is no such data saved in app
        /// </summary>
        public static bool TryLoadFromBox<T>(string id, ref T obj, Func<string, string>? decoder = null) =>
            _LoadFromBox("", id, ref obj, decoder);

        /// <summary> 
        /// Loads data from box with given id. Returns defaultValue if there is no such data saved in app
        /// </summary>
        public static T LoadFromBoxOrDefault<T>(string id, T defaultValue, Func<string, string>? decoder = null) 
        {
            _LoadFromBox("", id, ref defaultValue, decoder);
            return defaultValue;
        }

        ///<summary> 
        ///Loads data with given id. Returns null if there is no such data saved in app 
        ///</summary>
        public static S? Load<S>(string id, Func<string, string>? decoder = null) where S : struct, IStorable => 
            _Load<S>("", id, decoder);

        /// <summary>
        /// Saves data in scope of given game 
        /// </summary>
        public static void Save<S>(string id, in S data, ISavedGame savedGame, Func<string, string>? encoder = null) where S : struct, IStorable => 
            _Save(data, GetGameDirectory(savedGame.Index) + id, encoder);

        /// <summary>
        /// Saves data boxed in scope of given game 
        /// </summary>
        public static void SaveBoxed<T>(string id, in T data, ISavedGame savedGame, Func<string, string>? encoder = null) => 
            _SaveBoxed(data, GetGameDirectory(savedGame.Index) + id, encoder);
        
        /// <summary>
        /// Saves data boxed in scope of given game 
        /// </summary>
        public static void SaveBoxed<T>(string id, T data, ISavedGame savedGame, Func<string, string>? encoder = null) =>
            _SaveBoxed(data, GetGameDirectory(savedGame.Index) + id, encoder);

        /// <summary> 
        /// Saves data in app scope 
        /// </summary>
        public static void Save<S>(string id, in S data, Func<string, string>? encoder = null) where S : struct, IStorable => 
            _Save(data, id, encoder);

        /// <summary> 
        /// Saves data boxed in app scope 
        /// </summary>
        public static void SaveBoxed<T>(string id, in T data, Func<string, string>? encoder = null) => 
            _SaveBoxed(data, id, encoder);
        
        /// <summary> 
        /// Saves data boxed in app scope 
        /// </summary>
        public static void SaveBoxed<T>(string id, T data, Func<string, string>? encoder = null) =>
            _SaveBoxed(data, id, encoder);

        private static void _SaveBoxed<T>(in T data, string path, Func<string, string>? encoder)
        {
            var box = new SBox<T>(data);
            _Save(box, path, encoder);
        }

        private static void _Save<S>(in S data, string path, Func<string, string>? encoder) where S : struct, IStorable
        {
            var serialized = JsonUtility.ToJson(data);
            if (encoder != null) serialized = encoder.Invoke(serialized);
            File.WriteAllText(Application.persistentDataPath + "/" + path, serialized);
        }

        private static bool _LoadFromBox<T>(string localPath, string id, ref T obj, Func<string, string>? decoder)
        {
            var box = new SBox<T>();

            if (_Load(localPath, id, ref box, decoder))
            {
                obj = box.Value;
                return true;
            }

            return false;
        }
        
        private static S? _Load<S>(string localPath, string id, Func<string, string>? decoder) where S : struct, IStorable
        {
            S data = new S();
            if (!_Load(localPath, id, ref data, decoder)) return null;

            return data;
        }

        private static bool _Load<S>(string localPath, string id, ref S obj, Func<string, string>? decoder) where S : struct, IStorable
        {
            var path = Application.persistentDataPath + "/" + localPath + id;

            if (!File.Exists(path)) return false;
            var str = File.ReadAllText(path);
            if (decoder != null) str = decoder.Invoke(str);
            JsonUtility.FromJsonOverwrite(str, obj);

            return true;
        }
    }
}
