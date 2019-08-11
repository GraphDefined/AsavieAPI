/*
 * Copyright (c) 2017-2018, Achim 'ahzf' Friedland <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// The unique identification of an IoT device.
    /// </summary>
    public struct Device_Id : IId,
                              IEquatable<Device_Id>,
                              IComparable<Device_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the IoT device identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new IoT device identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the IoT device identification.</param>
        private Device_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as an IoT device identification.
        /// </summary>
        /// <param name="Text">A text representation of an IoT device identification.</param>
        public static Device_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an IoT device identification must not be null or empty!");

            #endregion

            return new Device_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an IoT device identification.
        /// </summary>
        /// <param name="Text">A text representation of an IoT device identification.</param>
        public static Device_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Device_Id _DeviceId))
                return _DeviceId;

            return new Device_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out DeviceId)

        /// <summary>
        /// Try to parse the given string as an IoT device identification.
        /// </summary>
        /// <param name="Text">A text representation of an IoT device identification.</param>
        /// <param name="DeviceId">The parsed IoT device identification.</param>
        public static Boolean TryParse(String Text, out Device_Id DeviceId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an IoT device identification must not be null or empty!");

            #endregion

            try
            {
                DeviceId = new Device_Id(Text);
                return true;
            }
            catch (Exception)
            {
                DeviceId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an IoT device identification.
        /// </summary>

        public Device_Id Clone
            => new Device_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Device_Id DeviceId1, Device_Id DeviceId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(DeviceId1, DeviceId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) DeviceId1 == null) || ((Object) DeviceId2 == null))
                return false;

            return DeviceId1.Equals(DeviceId2);

        }

        #endregion

        #region Operator != (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Device_Id DeviceId1, Device_Id DeviceId2)
            => !(DeviceId1 == DeviceId2);

        #endregion

        #region Operator <  (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Device_Id DeviceId1, Device_Id DeviceId2)
        {

            if ((Object) DeviceId1 == null)
                throw new ArgumentNullException(nameof(DeviceId1), "The given DeviceId1 must not be null!");

            return DeviceId1.CompareTo(DeviceId2) < 0;

        }

        #endregion

        #region Operator <= (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Device_Id DeviceId1, Device_Id DeviceId2)
            => !(DeviceId1 > DeviceId2);

        #endregion

        #region Operator >  (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Device_Id DeviceId1, Device_Id DeviceId2)
        {

            if ((Object) DeviceId1 == null)
                throw new ArgumentNullException(nameof(DeviceId1), "The given DeviceId1 must not be null!");

            return DeviceId1.CompareTo(DeviceId2) > 0;

        }

        #endregion

        #region Operator >= (DeviceId1, DeviceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId1">an IoT device identification.</param>
        /// <param name="DeviceId2">Another IoT device identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Device_Id DeviceId1, Device_Id DeviceId2)
            => !(DeviceId1 < DeviceId2);

        #endregion

        #endregion

        #region IComparable<DeviceId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Device_Id))
                throw new ArgumentException("The given object is not an IoT device identification!",
                                            nameof(Object));

            return CompareTo((Device_Id) Object);

        }

        #endregion

        #region CompareTo(DeviceId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="DeviceId">An object to compare with.</param>
        public Int32 CompareTo(Device_Id DeviceId)
        {

            if ((Object) DeviceId == null)
                throw new ArgumentNullException(nameof(DeviceId),  "The given IoT device identification must not be null!");

            return String.Compare(InternalId, DeviceId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<DeviceId> Members

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

            if (!(Object is Device_Id))
                return false;

            return Equals((Device_Id) Object);

        }

        #endregion

        #region Equals(DeviceId)

        /// <summary>
        /// Compares two IoT device identifications for equality.
        /// </summary>
        /// <param name="DeviceId">An IoT device identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Device_Id DeviceId)
        {

            if ((Object) DeviceId == null)
                return false;

            return InternalId.Equals(DeviceId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
