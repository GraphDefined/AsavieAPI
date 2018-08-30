﻿/*
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
    /// The unique identification of a SIM.
    /// </summary>
    public struct SIM_Id : IId,
                           IEquatable<SIM_Id>,
                           IComparable<SIM_Id>
    {

        #region Data

        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the SIM identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new SIM identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the SIM identification.</param>
        private SIM_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a SIM identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM identification.</param>
        public static SIM_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a SIM identification must not be null or empty!");

            #endregion

            return new SIM_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a SIM identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM identification.</param>
        public static SIM_Id? TryParse(String Text)
        {

            if (TryParse(Text, out SIM_Id _SIMId))
                return _SIMId;

            return new SIM_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out SIMId)

        /// <summary>
        /// Try to parse the given string as a SIM identification.
        /// </summary>
        /// <param name="Text">A text representation of a SIM identification.</param>
        /// <param name="SIMId">The parsed SIM identification.</param>
        public static Boolean TryParse(String Text, out SIM_Id SIMId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a SIM identification must not be null or empty!");

            #endregion

            try
            {
                SIMId = new SIM_Id(Text);
                return true;
            }
            catch (Exception)
            {
                SIMId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone a SIM identification.
        /// </summary>

        public SIM_Id Clone
            => new SIM_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SIM_Id SIMId1, SIM_Id SIMId2)
            => SIMId1.Equals(SIMId2);

        #endregion

        #region Operator != (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SIM_Id SIMId1, SIM_Id SIMId2)
            => !(SIMId1 == SIMId2);

        #endregion

        #region Operator <  (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SIM_Id SIMId1, SIM_Id SIMId2)
            => SIMId1.CompareTo(SIMId2) < 0;

        #endregion

        #region Operator <= (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SIM_Id SIMId1, SIM_Id SIMId2)
            => !(SIMId1 > SIMId2);

        #endregion

        #region Operator >  (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SIM_Id SIMId1, SIM_Id SIMId2)
            => SIMId1.CompareTo(SIMId2) > 0;

        #endregion

        #region Operator >= (SIMId1, SIMId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId1">A SIM identification.</param>
        /// <param name="SIMId2">Another SIM identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SIM_Id SIMId1, SIM_Id SIMId2)
            => !(SIMId1 < SIMId2);

        #endregion

        #endregion

        #region IComparable<SIMId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SIM_Id))
                throw new ArgumentException("The given object is not a SIM identification!",
                                            nameof(Object));

            return CompareTo((SIM_Id) Object);

        }

        #endregion

        #region CompareTo(SIMId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMId">An object to compare with.</param>
        public Int32 CompareTo(SIM_Id SIMId)
        {

            if ((Object) SIMId == null)
                throw new ArgumentNullException(nameof(SIMId),  "The given SIM identification must not be null!");

            return String.Compare(InternalId, SIMId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<SIMId> Members

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

            if (!(Object is SIM_Id))
                return false;

            return Equals((SIM_Id) Object);

        }

        #endregion

        #region Equals(SIMId)

        /// <summary>
        /// Compares two SIM identifications for equality.
        /// </summary>
        /// <param name="SIMId">An SIM identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SIM_Id SIMId)
        {

            if ((Object) SIMId == null)
                return false;

            return InternalId.Equals(SIMId.InternalId);

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