/*
 * Copyright (c) 2017-2020, Achim 'ahzf' Friedland <achim.friedland@graphdefined.com>
 * This file is part of GraphDefined AsavieAPI <https://github.com/GraphDefined/AsavieAPI>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// Information about the last connection of a device.
    /// </summary>
    public struct Last_Connected : IEquatable<Last_Connected>,
                                   IComparable<Last_Connected>,
                                   IComparable
    {

        #region Properties

        public DateTime   Start   { get; }
        public DateTime?  End     { get; }
        public String     MCC     { get; }
        public String     MNC     { get; }

        #endregion

        #region Constructor(s)

        public Last_Connected(DateTime   Start,
                              DateTime?  End,
                              String     MCC,
                              String     MNC)
        {

            this.Start  = Start;
            this.End    = End;
            this.MCC    = MCC;
            this.MNC    = MNC;

        }

        #endregion


        #region Parse   (JSON)

        public static Last_Connected Parse(JObject JSON)
        {

            if (TryParse(JSON, out Last_Connected lastConnected))
                return lastConnected;

            throw new ArgumentException("The given JSON could not be parsed!");

        }

        #endregion

        #region TryParse(JSON)

        public static Last_Connected? TryParse(JObject JSON)
        {

            if (TryParse(JSON, out Last_Connected lastConnected))
                return lastConnected;

            return new Last_Connected?();

        }

        #endregion

        #region TryParse(JSON, out LastConnected)

        public static Boolean TryParse(JObject JSON, out Last_Connected LastConnected)
        {

            try
            {

                LastConnected  = new Last_Connected(JSON["start"]. Value<DateTime>(),
                                                   JSON["end"  ]?.Value<DateTime>(),
                                                   JSON["MCC"  ]?.Value<String>(),
                                                   JSON["MNC"  ]?.Value<String>());

                return true;

            }
            catch (Exception)
            { }

            LastConnected = default;
            return true;

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()

            => JSONObject.Create(new JProperty("start",        Start.ToIso8601()),

                                 End.HasValue
                                     ? new JProperty("end",    End.Value.ToIso8601())
                                     : null,

                                 MCC.IsNotNullOrEmpty()
                                     ? new JProperty("MCC",    MCC)
                                     : null,

                                 MNC.IsNotNullOrEmpty()
                                     ? new JProperty("MNC",    MNC)
                                     : null);

        #endregion


        #region Operator overloading

        #region Operator == (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Last_Connected LastConnected1, Last_Connected LastConnected2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(LastConnected1, LastConnected2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) LastConnected1 == null) || ((Object) LastConnected2 == null))
                return false;

            return LastConnected1.Equals(LastConnected2);

        }

        #endregion

        #region Operator != (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Last_Connected LastConnected1, Last_Connected LastConnected2)
            => !(LastConnected1 == LastConnected2);

        #endregion

        #region Operator <  (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Last_Connected LastConnected1, Last_Connected LastConnected2)
            => LastConnected1.CompareTo(LastConnected2) < 0;

        #endregion

        #region Operator <= (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Last_Connected LastConnected1, Last_Connected LastConnected2)
            => !(LastConnected1 > LastConnected2);

        #endregion

        #region Operator >  (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Last_Connected LastConnected1, Last_Connected LastConnected2)
            => LastConnected1.CompareTo(LastConnected2) > 0;

        #endregion

        #region Operator >= (LastConnected1, LastConnected2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected1">A LastConnected object identification.</param>
        /// <param name="LastConnected2">Another LastConnected object identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Last_Connected LastConnected1, Last_Connected LastConnected2)
            => !(LastConnected1 < LastConnected2);

        #endregion

        #endregion

        #region IComparable<LastConnected> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Last_Connected LastConnected))
                throw new ArgumentException("The given object is not an LastConnected object!");

            return CompareTo(LastConnected);

        }

        #endregion

        #region CompareTo(LastConnected)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LastConnected">An LastConnected object object to compare with.</param>
        public Int32 CompareTo(Last_Connected LastConnected)
        {

            var result = Start.CompareTo(LastConnected.Start);
            if (result != 0)
                return result;

            if (End.HasValue && LastConnected.End.HasValue)
            {
                result = DateTime.Compare(End.Value, LastConnected.End.Value);
                if (result != 0)
                    return result;
            }

            if (MCC.IsNotNullOrEmpty() && LastConnected.MCC.IsNotNullOrEmpty())
            {
                result = String.Compare(MCC, LastConnected.MCC, StringComparison.OrdinalIgnoreCase);
                if (result != 0)
                    return result;
            }

            if (MNC.IsNotNullOrEmpty() && LastConnected.MNC.IsNotNullOrEmpty())
            {
                result = String.Compare(MNC, LastConnected.MNC, StringComparison.OrdinalIgnoreCase);
                if (result != 0)
                    return result;
            }

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<LastConnected> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is Last_Connected))
                return false;

            return Equals((Last_Connected) Object);

        }

        #endregion

        #region Equals(LastConnected)

        /// <summary>
        /// Compares two LastConnected objects for equality.
        /// </summary>
        /// <param name="LastConnected">An LastConnected object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Last_Connected LastConnected)
        {

            if ((Object) LastConnected == null)
                return false;

            return Start.       Equals(LastConnected.Start)        &&

                   ((!End.HasValue && !LastConnected.End.HasValue) ||
                     (End.HasValue &&  LastConnected.End.HasValue && End.Value.Equals(LastConnected.End.Value))) &&

                   ((!MCC.IsNotNullOrEmpty() && !LastConnected.MCC.IsNotNullOrEmpty()) ||
                     (MCC.IsNotNullOrEmpty() &&  LastConnected.MCC.IsNotNullOrEmpty() && MCC.Equals(LastConnected.MCC))) &&

                   ((!MNC.IsNotNullOrEmpty() && !LastConnected.MNC.IsNotNullOrEmpty()) ||
                     (MNC.IsNotNullOrEmpty() &&  LastConnected.MNC.IsNotNullOrEmpty() && MNC.Equals(LastConnected.MNC)));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Start.     GetHashCode() * 11 ^

                       (End.HasValue
                            ? End.GetHashCode()
                            : 0) * 7 ^

                       (MCC.IsNotNullOrEmpty()
                            ? MCC.GetHashCode()
                            : 0) * 5 ^

                       (MNC.IsNotNullOrEmpty()
                            ? MNC.GetHashCode()
                            : 0) * 3;


            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Start.ToIso8601() + (End.HasValue ? " => " + End.Value.ToIso8601() : "");

        #endregion

    }

}
