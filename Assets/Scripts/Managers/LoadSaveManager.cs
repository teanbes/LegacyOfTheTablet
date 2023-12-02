using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

public class LoadSaveManager : MonoBehaviour
{

    // Save game data

    public class GameStateData
    {
        public struct DataTransform
        {

            public float posX;
            public float posY;
            public float posZ;

            public float rotX;
            public float rotY;  
            public float rotZ;

            public float scaleX;
            public float scaleY;
            public float scaleZ;

        }

        // Data for enemy
        public class DataEnemy
        {
            //Enemy Transform Data
            public DataTransform posRotScale;

            //Enemy ID
            public int enemyID;

            //Health
            public int health;

        }

        // Data for player
        public class DataPlayer
        {
            public bool isSaved;

            //Transform Data
            public DataTransform posRotScale;

            //Collected combo power up?
            public bool collectedCombo;

            //Collected spell power up?
            public bool collectedSpell;


            //Has Collected sword ?
            public bool collectedSword;

            //Health
            public int health;
        }

        // Instance variables

        public List<DataEnemy> enemies = new List<DataEnemy>();
        public DataPlayer player = new DataPlayer();


    }

    // Game data to save/load
    public GameStateData gameState = new GameStateData();
    

    // Saves game data to XML file
    public void Save(string fileName = "GameData.xml")
    {


        EncryptedXmlSerializer.Save<GameStateData>(fileName, gameState);

        // Save game data

        /* XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
         FileStream stream = new FileStream(fileName, FileMode.Create);
         serializer.Serialize(stream, gameState);
         stream.Flush();
         stream.Dispose();
         stream.Close();*/


        Debug.Log("Game Saved");

    }

    // Load game data from XML file
    public void Load(string fileName = "GameData.xml")
    {

        EncryptedXmlSerializer.Load<GameStateData>(fileName);

            // Load game data
            /*XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
            FileStream stream = new FileStream(fileName, FileMode.Open);
            gameState = serializer.Deserialize(stream) as GameStateData;
            stream.Flush();
            stream.Dispose();
            stream.Close();*/

            Debug.Log("Game loaded");
       
    }


    public static class EncryptedXmlSerializer
    {
        private static readonly string PrivateKey = SystemInfo.deviceUniqueIdentifier.Replace("-", string.Empty);

        #region API

        /// <summary>
        /// Reads and decrypts file at specified path
        /// </summary>
        /// <param name="path">Patht to file</param>
        /// <typeparam name="T">Type of the serialized object</typeparam>
        /// <returns>Decrypted deserialized object or null if file does not exist</returns>
        public static T Load<T>(string path) where T : class
        {
            T result;

            if (!File.Exists(path))
            {
                Debug.LogWarning("File " + path + " does not exist!");
                return null;
            }

            string data;
            using (var reader = new StreamReader(path))
            {
                data = DecryptData(reader.ReadToEnd());
                Debug.Log(data);
            }

            var stream = new MemoryStream();
            using (var sw = new StreamWriter(stream) { AutoFlush = true })
            {
                sw.WriteLine(data);
                stream.Position = 0;
                result = new XmlSerializer(typeof(T)).Deserialize(stream) as T;
            }

            return result;
        }

        /// <summary>
        /// Encrypts and serializes file at the specified path.
        /// </summary>
        /// <param name="path">Path to save the file</param>
        /// <param name="value">Object to serialize</param>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        public static void Save<T>(string path, object value) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                stream.Flush();
                stream.Position = 0;
                string sr = new StreamReader(stream).ReadToEnd();
                var fileStream = new FileStream(path, FileMode.Create);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(EncryptData(sr));
                streamWriter.Close();
                fileStream.Close();
            }
        }

        #endregion

        #region encrypt_decrypt
        private static string EncryptData(string toEncrypt)
        {
#if UNITY_WP8
            return toEncrypt;
#else
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = CreateRijndaelManaged();
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
#endif
        }

        private static string DecryptData(string toDecrypt)
        {
#if UNITY_WP8
            return toDecrypt;
#else
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = CreateRijndaelManaged();
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
#endif
        }

#if !UNITY_WP8
        private static RijndaelManaged CreateRijndaelManaged()
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(PrivateKey);
            var result = new RijndaelManaged();

            var newKeysArray = new byte[16];
            Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }
#endif
        #endregion
    }

}