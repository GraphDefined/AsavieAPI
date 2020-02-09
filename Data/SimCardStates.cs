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

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// All states of SIM cards.
    /// </summary>
    public enum SimCardStates
    {
        notactivated  = 0,
        ready         = 1,
        test          = 2,
        live          = 3,
        suspend       = 4,
        bar           = 5,
        unknown       = 6,
        terminated    = 100
    }

}
