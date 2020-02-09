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
    /// The unique identification of an operator.
    /// </summary>
    public struct Operator_Id : IId,
                                IEquatable<Operator_Id>,
                                IComparable<Operator_Id>
    {

        #region Data

        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the Operator identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Operator identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the Operator identification.</param>
        private Operator_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an operator identification must not be null or empty!");

            #endregion

            return new Operator_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        public static Operator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Operator_Id _OperatorId))
                return _OperatorId;

            return new Operator_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given string as an operator identification.
        /// </summary>
        /// <param name="Text">A text representation of an operator identification.</param>
        /// <param name="OperatorId">The parsed Operator identification.</param>
        public static Boolean TryParse(String Text, out Operator_Id OperatorId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an operator identification must not be null or empty!");

            #endregion

            try
            {
                OperatorId = new Operator_Id(Text);
                return true;
            }
            catch (Exception)
            {
                OperatorId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an operator identification.
        /// </summary>

        public Operator_Id Clone
            => new Operator_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.Equals(OperatorId2);

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 == OperatorId2);

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) < 0;

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 > OperatorId2);

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => OperatorId1.CompareTo(OperatorId2) > 0;

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">A Operator identification.</param>
        /// <param name="OperatorId2">Another Operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 < OperatorId2);

        #endregion

        #endregion

        #region IComparable<OperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Operator_Id))
                throw new ArgumentException("The given object is not an operator identification!",
                                            nameof(Object));

            return CompareTo((Operator_Id) Object);

        }

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId">An object to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given Operator identification must not be null!");

            return String.Compare(InternalId, OperatorId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<OperatorId> Members

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

            if (!(Object is Operator_Id))
                return false;

            return Equals((Operator_Id) Object);

        }

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two Operator identifications for equality.
        /// </summary>
        /// <param name="OperatorId">An Operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                return false;

            return InternalId.Equals(OperatorId.InternalId);

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
