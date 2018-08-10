using System;

namespace Caching
{
    /// <summary>
    /// Represents a attribute for caching class.
    /// </summary>
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets an expiration time.
        /// </summary>
        public TimeSpan ExpirationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAttribute"/> class.
        /// </summary>
        public CachingAttribute()
        {
            ExpirationTime = new TimeSpan(0, 5, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAttribute"/> class.
        /// </summary>
        /// <param name="hour">How many hours.</param>
        /// <param name="min">How many minutes.</param>
        /// <param name="sec">How many seconds.</param>
        public CachingAttribute(int hour, int min, int sec)
        {
            ExpirationTime = new TimeSpan(hour, min, sec);
        }  
    }
}