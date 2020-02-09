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
    /// The unique identification of an account.
    /// </summary>
    public struct Account_Id : IId,
                               IEquatable<Account_Id>,
                               IComparable<Account_Id>
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
        /// The length of the account identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new account identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the account identification.</param>
        private Account_Id(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as an account identification.
        /// </summary>
        /// <param name="Text">A text representation of an account identification.</param>
        public static Account_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an account identification must not be null or empty!");

            #endregion

            return new Account_Id(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an account identification.
        /// </summary>
        /// <param name="Text">A text representation of an account identification.</param>
        public static Account_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Account_Id _AccountId))
                return _AccountId;

            return new Account_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out AccountId)

        /// <summary>
        /// Try to parse the given string as an account identification.
        /// </summary>
        /// <param name="Text">A text representation of an account identification.</param>
        /// <param name="AccountId">The parsed account identification.</param>
        public static Boolean TryParse(String Text, out Account_Id AccountId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an account identification must not be null or empty!");

            #endregion

            try
            {
                AccountId = new Account_Id(Text);
                return true;
            }
            catch (Exception)
            {
                AccountId = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an account identification.
        /// </summary>

        public Account_Id Clone
            => new Account_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Account_Id AccountId1, Account_Id AccountId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AccountId1, AccountId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AccountId1 == null) || ((Object) AccountId2 == null))
                return false;

            return AccountId1.Equals(AccountId2);

        }

        #endregion

        #region Operator != (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Account_Id AccountId1, Account_Id AccountId2)
            => !(AccountId1 == AccountId2);

        #endregion

        #region Operator <  (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Account_Id AccountId1, Account_Id AccountId2)
        {

            if ((Object) AccountId1 == null)
                throw new ArgumentNullException(nameof(AccountId1), "The given AccountId1 must not be null!");

            return AccountId1.CompareTo(AccountId2) < 0;

        }

        #endregion

        #region Operator <= (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Account_Id AccountId1, Account_Id AccountId2)
            => !(AccountId1 > AccountId2);

        #endregion

        #region Operator >  (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Account_Id AccountId1, Account_Id AccountId2)
        {

            if ((Object) AccountId1 == null)
                throw new ArgumentNullException(nameof(AccountId1), "The given AccountId1 must not be null!");

            return AccountId1.CompareTo(AccountId2) > 0;

        }

        #endregion

        #region Operator >= (AccountId1, AccountId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId1">A account identification.</param>
        /// <param name="AccountId2">Another account identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Account_Id AccountId1, Account_Id AccountId2)
            => !(AccountId1 < AccountId2);

        #endregion

        #endregion

        #region IComparable<AccountId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Account_Id))
                throw new ArgumentException("The given object is not an account identification!",
                                            nameof(Object));

            return CompareTo((Account_Id) Object);

        }

        #endregion

        #region CompareTo(AccountId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AccountId">An object to compare with.</param>
        public Int32 CompareTo(Account_Id AccountId)
        {

            if ((Object) AccountId == null)
                throw new ArgumentNullException(nameof(AccountId),  "The given account identification must not be null!");

            return String.Compare(InternalId, AccountId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<AccountId> Members

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

            if (!(Object is Account_Id))
                return false;

            return Equals((Account_Id) Object);

        }

        #endregion

        #region Equals(AccountId)

        /// <summary>
        /// Compares two account identifications for equality.
        /// </summary>
        /// <param name="AccountId">An account identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Account_Id AccountId)
        {

            if ((Object) AccountId == null)
                return false;

            return InternalId.Equals(AccountId.InternalId);

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
