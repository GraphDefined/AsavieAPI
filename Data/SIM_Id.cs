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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// The unique identification of a SIM.
    /// </summary>
    public readonly struct SIM_Id : IId<SIM_Id>
    {

        #region Data

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

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
        /// The length of the SIM card identification.
        /// </summary>
        public UInt64 Length

            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SIM card identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the SIM card identification.</param>
        private SIM_Id(String String)
        {
            this.InternalId  = String;
        }

        #endregion


        #region (static) Random(Length)

        /// <summary>
        /// Create a new SIM identification.
        /// </summary>
        /// <param name="Length">The expected length of the SIM identification.</param>
        public static SIM_Id Random(Byte Length = 10)

            => new SIM_Id(_random.RandomString(Length).ToUpper());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a SIM card identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM card identification.</param>
        public static SIM_Id Parse(String Text)
        {

            if (TryParse(Text, out SIM_Id SIMId))
                return SIMId;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a SIM card identification must not be null or empty!");

            throw new ArgumentException("The given text representation of a SIM card identification is invalid!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a SIM card identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM card identification.</param>
        public static SIM_Id? TryParse(String Text)
        {

            if (TryParse(Text, out SIM_Id SIMId))
                return SIMId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out CommunicatorId)

        /// <summary>
        /// Try to parse the given string as a SIM card identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM card identification.</param>
        /// <param name="CommunicatorId">The parsed SIM card identification.</param>
        public static Boolean TryParse(String Text, out SIM_Id CommunicatorId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    CommunicatorId = new SIM_Id(Text.Trim());
                    return true;
                }
                catch (Exception)
                { }
            }

            CommunicatorId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this SIM card identification.
        /// </summary>
        public SIM_Id Clone

            => new SIM_Id(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SIM_Id CommunicatorId1,
                                           SIM_Id CommunicatorId2)

            => CommunicatorId1.Equals(CommunicatorId2);

        #endregion

        #region Operator != (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SIM_Id CommunicatorId1,
                                           SIM_Id CommunicatorId2)

            => !(CommunicatorId1 == CommunicatorId2);

        #endregion

        #region Operator <  (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SIM_Id CommunicatorId1,
                                          SIM_Id CommunicatorId2)

            => CommunicatorId1.CompareTo(CommunicatorId2) < 0;

        #endregion

        #region Operator <= (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SIM_Id CommunicatorId1,
                                           SIM_Id CommunicatorId2)

            => !(CommunicatorId1 > CommunicatorId2);

        #endregion

        #region Operator >  (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SIM_Id CommunicatorId1,
                                          SIM_Id CommunicatorId2)

            => CommunicatorId1.CompareTo(CommunicatorId2) > 0;

        #endregion

        #region Operator >= (CommunicatorId1, CommunicatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId1">A SIM card identification.</param>
        /// <param name="CommunicatorId2">Another SIM card identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SIM_Id CommunicatorId1,
                                           SIM_Id CommunicatorId2)

            => !(CommunicatorId1 < CommunicatorId2);

        #endregion

        #endregion

        #region IComparable<CommunicatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is SIM_Id SIMId)
                return CompareTo(SIMId);

            throw new ArgumentException("The given object is not a SIM card identification!",
                                        nameof(Object));

        }

        #endregion

        #region CompareTo(CommunicatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CommunicatorId">An object to compare with.</param>
        public Int32 CompareTo(SIM_Id CommunicatorId)

            => String.Compare(InternalId,
                              CommunicatorId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CommunicatorId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is SIM_Id SIMId)
                return Equals(SIMId);

            return false;

        }

        #endregion

        #region Equals(CommunicatorId)

        /// <summary>
        /// Compares two SIM card identifications for equality.
        /// </summary>
        /// <param name="CommunicatorId">An SIM card identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SIM_Id CommunicatorId)

            => String.Equals(InternalId,
                             CommunicatorId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
