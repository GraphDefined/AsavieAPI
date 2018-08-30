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
    /// A unique Asavie identification.
    /// </summary>
    public struct CLI : IId,
                        IEquatable<CLI>,
                        IComparable<CLI>
    {

        #region Data

        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the Asavie identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Asavie identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the Asavie identification.</param>
        private CLI(String String)
        {
            InternalId = String;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a Asavie identification.
        /// </summary>
        /// <param name="Text">A text representation of a Asavie identification.</param>
        public static CLI Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a Asavie identification must not be null or empty!");

            #endregion

            return new CLI(Text);

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a Asavie identification.
        /// </summary>
        /// <param name="Text">A text representation of a Asavie identification.</param>
        public static CLI? TryParse(String Text)
        {

            if (TryParse(Text, out CLI _CLI))
                return _CLI;

            return new CLI?();

        }

        #endregion

        #region (static) TryParse(Text, out CLI)

        /// <summary>
        /// Try to parse the given string as a Asavie identification.
        /// </summary>
        /// <param name="Text">A text representation of a Asavie identification.</param>
        /// <param name="CLI">The parsed Asavie identification.</param>
        public static Boolean TryParse(String Text, out CLI CLI)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a Asavie identification must not be null or empty!");

            #endregion

            try
            {
                CLI = new CLI(Text);
                return true;
            }
            catch (Exception)
            {
                CLI = default;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone a Asavie identification.
        /// </summary>

        public CLI Clone
            => new CLI(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CLI CLI1, CLI CLI2)
            => CLI1.Equals(CLI2);

        #endregion

        #region Operator != (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CLI CLI1, CLI CLI2)
            => !(CLI1 == CLI2);

        #endregion

        #region Operator <  (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CLI CLI1, CLI CLI2)
            => CLI1.CompareTo(CLI2) < 0;

        #endregion

        #region Operator <= (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CLI CLI1, CLI CLI2)
            => !(CLI1 > CLI2);

        #endregion

        #region Operator >  (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CLI CLI1, CLI CLI2)
            => CLI1.CompareTo(CLI2) > 0;

        #endregion

        #region Operator >= (CLI1, CLI2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI1">A Asavie identification.</param>
        /// <param name="CLI2">Another Asavie identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CLI CLI1, CLI CLI2)
            => !(CLI1 < CLI2);

        #endregion

        #endregion

        #region IComparable<CLI> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is CLI))
                throw new ArgumentException("The given object is not a Asavie identification!",
                                            nameof(Object));

            return CompareTo((CLI) Object);

        }

        #endregion

        #region CompareTo(CLI)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CLI">An object to compare with.</param>
        public Int32 CompareTo(CLI CLI)
        {

            if ((Object) CLI == null)
                throw new ArgumentNullException(nameof(CLI),  "The given Asavie identification must not be null!");

            return String.Compare(InternalId, CLI.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<CLI> Members

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

            if (!(Object is CLI))
                return false;

            return Equals((CLI) Object);

        }

        #endregion

        #region Equals(CLI)

        /// <summary>
        /// Compares two Asavie identifications for equality.
        /// </summary>
        /// <param name="CLI">An Asavie identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CLI CLI)
        {

            if ((Object) CLI == null)
                return false;

            return InternalId.Equals(CLI.InternalId);

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
