using System;
using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Configuration.File.InvokePolicy
{
    /// <summary>
    /// Sealed class containing the file age policy logic
    /// </summary>
    public sealed class FileAgePolicy : InvokePolicy
    {
        #region Variables

        private int _maxHours;
        private int _maxMinutes;
        private int _maxSeconds;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the maximum age in days of a file
        /// </summary>
        [XmlElement("MaxDays")]
        public int MaxDays { get; set; }

        /// <summary>
        /// Gets or sets the maximum age in hours of a file
        /// </summary>
        /// <exception cref="ArgumentException">When the value exceed 23</exception>
        [XmlElement("MaxHours")]
        public int MaxHours
        {
            get => _maxHours;
            set
            {
                if (value > 23) throw new ArgumentException(nameof(value));
                _maxHours = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum age in minutes of a file
        /// </summary>
        /// <exception cref="ArgumentException">When the value exceeds 59</exception>
        [XmlElement("MaxMinutes")]
        public int MaxMinutes
        {
            get => _maxMinutes;
            set
            {
                if (value > 59) throw new ArgumentException(nameof(value));
                _maxMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum age in seconds of a file
        /// </summary>
        /// <exception cref="ArgumentException">When the value exceeds 59</exception>
        [XmlElement("MaxSeconds")]
        public int MaxSeconds
        {
            get => _maxSeconds;
            set
            {
                if (value > 59) throw new ArgumentException(nameof(value));
                _maxSeconds = value;
            }
        }
        #endregion

        /// <summary>
        /// Initialize a new FileAgePolicy
        /// </summary>
        public FileAgePolicy()
        {
            MaxDays = -1;
            MaxHours = -1;
            MaxMinutes = -1;
            MaxSeconds = -1;
        }

        /// <summary>
        /// Method that is called when the file age policy should be checked
        /// </summary>
        /// <param name="filePath">The path of the file that should be checked</param>
        /// <returns>True if the FileConfiguration should be invoked, otherwise false</returns>
        public override bool ShouldInvoke(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return false;
            if (MaxDays <= 0 && MaxHours <= 0 && MaxMinutes <= 0 && MaxSeconds <= 0) return false;

            DateTime creation = System.IO.File.GetCreationTime(filePath);
            TimeSpan ts = DateTime.Now - creation;

            if (MaxDays <= 0 && MaxHours <= 0 && MaxMinutes <= 0 && MaxSeconds > 0)
            {
                return ts.Seconds >= MaxSeconds;
            }

            if (MaxDays <= 0 && MaxHours <= 0 && MaxMinutes > 0 && MaxSeconds <= 0)
            {
                return ts.Hours >= MaxHours;
            }

            if (MaxDays <= 0 && MaxHours <= 0 && MaxMinutes > 0 && MaxSeconds > 0)
            {
                return ts.Minutes > MaxMinutes && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays <= 0 && MaxHours > 0 && MaxMinutes <= 0 && MaxSeconds <= 0)
            {
                return ts.Hours >= MaxHours;
            }

            if (MaxDays <= 0 && MaxHours > 0 && MaxMinutes <= 0 && MaxSeconds > 0)
            {
                return ts.Hours >= MaxHours && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays <= 0 && MaxHours > 0 && MaxMinutes > 0 && MaxSeconds <= 0)
            {
                return ts.Hours >= MaxHours && ts.Minutes >= MaxMinutes;
            }

            if (MaxDays <= 0 && MaxHours > 0 && MaxMinutes > 0 && MaxSeconds > 0)
            {
                return ts.Hours >= MaxHours && ts.Minutes >= MaxMinutes && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays > 0 && MaxHours <= 0 && MaxMinutes <= 0 && MaxSeconds <= 0)
            {
                return ts.Days >= MaxDays;
            }

            if (MaxDays > 0 && MaxHours <= 0 && MaxMinutes <= 0 && MaxSeconds > 0)
            {
                return ts.Days >= MaxDays && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays > 0 && MaxHours <= 0 && MaxMinutes > 0 && MaxSeconds <= 0)
            {
                return ts.Days >= MaxDays && ts.Minutes >= MaxMinutes;
            }

            if (MaxDays > 0 && MaxHours <= 0 && MaxMinutes > 0 && MaxSeconds > 0)
            {
                return ts.Days >= MaxDays && ts.Minutes >= MaxMinutes && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays > 0 && MaxHours > 0 && MaxMinutes <= 0 && MaxSeconds <= 0)
            {
                return ts.Days >= MaxDays && ts.Hours >= MaxHours;
            }

            if (MaxDays > 0 && MaxHours > 0 && MaxMinutes <= 0 && MaxSeconds > 0)
            {
                return ts.Days >= MaxDays && ts.Hours >= MaxHours && ts.Seconds >= MaxSeconds;
            }

            if (MaxDays > 0 && MaxHours > 0 && MaxMinutes > 0 && MaxSeconds <= 0)
            {
                return ts.Days >= MaxDays && ts.Hours >= MaxHours && ts.Minutes >= MaxMinutes;
            }

            if (MaxDays > 0 && MaxHours > 0 && MaxMinutes > 0 && MaxSeconds > 0)
            {
                return ts.Days >= MaxDays && ts.Hours >= MaxHours && ts.Minutes >= MaxMinutes &&
                       ts.Seconds >= MaxSeconds;
            }
            return false;
        }
    }
}