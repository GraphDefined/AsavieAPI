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
    /// The unique identification of a provider.
    /// </summary>
    public struct Provider_Id : IId,
                                IEquatable<Provider_Id>,
                                IComparable<Provider_Id>
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
        /// The length of the Provider identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Provider identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the Provider identification.</param>
        private Provider_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a provider identification.</param>
        public static Provider_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a provider identification must not be null or empty!");

            #endregion

            return new Provider_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a provider identification.</param>
        public static Provider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Provider_Id _ProviderId))
                return _ProviderId;

            return new Provider_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out ProviderId)

        /// <summary>
        /// Try to parse the given string as a provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a provider identification.</param>
        /// <param name="ProviderId">The parsed Provider identification.</param>
        public static Boolean TryParse(String Text, out Provider_Id ProviderId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a provider identification must not be null or empty!");

            #endregion

            try
            {
                ProviderId = new Provider_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ProviderId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone a provider identification.
        /// </summary>

        public Provider_Id Clone
            => new Provider_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => ProviderId1.Equals(ProviderId2);

        #endregion

        #region Operator != (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 == ProviderId2);

        #endregion

        #region Operator <  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => ProviderId1.CompareTo(ProviderId2) < 0;

        #endregion

        #region Operator <= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 > ProviderId2);

        #endregion

        #region Operator >  (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => ProviderId1.CompareTo(ProviderId2) > 0;

        #endregion

        #region Operator >= (ProviderId1, ProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId1">A Provider identification.</param>
        /// <param name="ProviderId2">Another Provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Provider_Id ProviderId1, Provider_Id ProviderId2)
            => !(ProviderId1 < ProviderId2);

        #endregion

        #endregion

        #region IComparable<ProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Provider_Id))
                throw new ArgumentException("The given object is not a provider identification!",
                                            nameof(Object));

            return CompareTo((Provider_Id) Object);

        }

        #endregion

        #region CompareTo(ProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ProviderId">An object to compare with.</param>
        public Int32 CompareTo(Provider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The given Provider identification must not be null!");

            return String.Compare(InternalId, ProviderId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<ProviderId> Members

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

            if (!(Object is Provider_Id))
                return false;

            return Equals((Provider_Id) Object);

        }

        #endregion

        #region Equals(ProviderId)

        /// <summary>
        /// Compares two Provider identifications for equality.
        /// </summary>
        /// <param name="ProviderId">An Provider identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Provider_Id ProviderId)
        {

            if ((Object) ProviderId == null)
                return false;

            return InternalId.Equals(ProviderId.InternalId);

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
