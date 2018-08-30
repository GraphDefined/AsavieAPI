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

#endregion

namespace com.GraphDefined.Asavie.API
{

    public struct AuthToken
    {

        #region Properties

        public String    Token     { get; }

        public DateTime  Expires   { get; }

        public TimeSpan  Timeout
            => Expires - DateTime.UtcNow;

        #endregion

        #region Constructor(s)

        public AuthToken(String    Token,
                         DateTime  Expires)
        {
            this.Token    = Token;
            this.Expires  = Expires;
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Token.Substring(0, 23) + "... expires in " +

               (Timeout.TotalMinutes > 3
                    ? Timeout.TotalMinutes + " minutes!"
                    : Timeout.TotalSeconds + " seconds!");

        #endregion

    }

}
