using UnityEngine;
using System;
using System.Collections;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// Object that contains information about loaded saved game snapshot
    /// </summary>
    [Serializable]
    public class UM_SavedGameData
    {
        /*
        public long PlayedTimeMillis = 0;
        public long ProgressValue = 0;

        public String Describtion;
        public String CoverImageBase64;
        */

        [SerializeField] byte[] m_data = null;


        public UM_SavedGameData(byte[] data) {
            m_data = data;
        }


        /// <summary>
        /// Loaded game data
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data {
            get {
                return m_data;
            }
        }


        /*

        /// <summary>
        /// Cover image to set for the snapshot.
        /// </summary>
        /// <param name="coverImage"></param>
        public void SetCoverImage(Texture2D coverImage) {
           CoverImageBase64 = coverImage.ToBase64String();
        }

        /// <summary>
        /// Description to set for the snapshot.
        /// </summary>
        /// <param name="description">description</param>
        public void SetDescription(string description) {
            Describtion = description;
        }


        /// <summary>
        /// The new played time to set for the snapshot.
        /// </summary>
        /// <param name="playedTimeMillis">player played time in milliseconds</param>
        public void SetPlayedTimeMillis(long playedTimeMillis) {
            PlayedTimeMillis = playedTimeMillis;
        }


        /// <summary>
        /// The new progress value to set for the snapshot.
        /// </summary>
        /// <param name="progressValue">player progress value</param>
        public void SetProgressValue(long progressValue) {
            ProgressValue = progressValue;
        }


        /// <summary>
        /// Sets the game save data, represented as bytes array.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SetData(byte[] data) {
            Data = data;
        }


        /// <summary>
        /// Sets the game save data, represented as object.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SetData(object data) {
           string json =  JsonUtility.ToJson(data);
            Data = json.ToBytes();
        }
        */
    }
}