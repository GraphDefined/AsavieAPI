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
    /// The unique identification of a network.
    /// </summary>
    public struct Network_Id : IId,
                               IEquatable<Network_Id>,
                               IComparable<Network_Id>
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
        /// The length of the network identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new network identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the network identification.</param>
        private Network_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as a network identification.
        /// </summary>
        /// <param name="Text">A text representation of a network identification.</param>
        public static Network_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a network identification must not be null or empty!");

            #endregion

            return new Network_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a network identification.
        /// </summary>
        /// <param name="Text">A text representation of a network identification.</param>
        public static Network_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Network_Id _NetworkId))
                return _NetworkId;

            return new Network_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out NetworkId)

        /// <summary>
        /// Try to parse the given string as a network identification.
        /// </summary>
        /// <param name="Text">A text representation of a network identification.</param>
        /// <param name="NetworkId">The parsed network identification.</param>
        public static Boolean TryParse(String Text, out Network_Id NetworkId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a network identification must not be null or empty!");

            #endregion

            try
            {
                NetworkId = new Network_Id(Text);
                return true;
            }
            catch (Exception)
            {
                NetworkId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone a network identification.
        /// </summary>

        public Network_Id Clone
            => new Network_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Network_Id NetworkId1, Network_Id NetworkId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(NetworkId1, NetworkId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) NetworkId1 == null) || ((Object) NetworkId2 == null))
                return false;

            return NetworkId1.Equals(NetworkId2);

        }

        #endregion

        #region Operator != (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Network_Id NetworkId1, Network_Id NetworkId2)
            => !(NetworkId1 == NetworkId2);

        #endregion

        #region Operator <  (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Network_Id NetworkId1, Network_Id NetworkId2)
        {

            if ((Object) NetworkId1 == null)
                throw new ArgumentNullException(nameof(NetworkId1), "The given NetworkId1 must not be null!");

            return NetworkId1.CompareTo(NetworkId2) < 0;

        }

        #endregion

        #region Operator <= (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Network_Id NetworkId1, Network_Id NetworkId2)
            => !(NetworkId1 > NetworkId2);

        #endregion

        #region Operator >  (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Network_Id NetworkId1, Network_Id NetworkId2)
        {

            if ((Object) NetworkId1 == null)
                throw new ArgumentNullException(nameof(NetworkId1), "The given NetworkId1 must not be null!");

            return NetworkId1.CompareTo(NetworkId2) > 0;

        }

        #endregion

        #region Operator >= (NetworkId1, NetworkId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId1">A network identification.</param>
        /// <param name="NetworkId2">Another network identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Network_Id NetworkId1, Network_Id NetworkId2)
            => !(NetworkId1 < NetworkId2);

        #endregion

        #endregion

        #region IComparable<NetworkId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Network_Id))
                throw new ArgumentException("The given object is not a network identification!",
                                            nameof(Object));

            return CompareTo((Network_Id) Object);

        }

        #endregion

        #region CompareTo(NetworkId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NetworkId">An object to compare with.</param>
        public Int32 CompareTo(Network_Id NetworkId)
        {

            if ((Object) NetworkId == null)
                throw new ArgumentNullException(nameof(NetworkId),  "The given network identification must not be null!");

            return String.Compare(InternalId, NetworkId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<NetworkId> Members

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

            if (!(Object is Network_Id))
                return false;

            return Equals((Network_Id) Object);

        }

        #endregion

        #region Equals(NetworkId)

        /// <summary>
        /// Compares two network identifications for equality.
        /// </summary>
        /// <param name="NetworkId">An network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Network_Id NetworkId)
        {

            if ((Object) NetworkId == null)
                return false;

            return InternalId.Equals(NetworkId.InternalId);

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
