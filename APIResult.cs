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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// The Asavie API.
    /// </summary>
    public partial class AsavieAPI
    {

        /// <summary>
        /// A generic Asavie API.
        /// </summary>
        public class APIResult<T>
        {

            #region Properties

            public T        Data                 { get; }

            public Boolean  Success              { get; }
            public UInt16   Code                 { get; }
            public UInt16   ErrorCode            { get; }
            public UInt16   ErrorSubCode         { get; }
            public String   ErrorDescription     { get; }
            public String   Meta                 { get; }
            public String   StatusUrl            { get; }
            public String   ContinuationToken    { get; }

            #endregion

            #region APIResult

            #region APIResult(Data, Success, ...)

            public APIResult(T        Data,

                             Boolean  Success,
                             UInt16   Code,
                             UInt16   ErrorCode,
                             UInt16   ErrorSubCode,
                             String   ErrorDescription,
                             String   Meta,
                             String   StatusUrl,
                             String   ContinuationToken)

                : this(Success,
                       Code,
                       ErrorCode,
                       ErrorSubCode,
                       ErrorDescription,
                       Meta,
                       StatusUrl,
                       ContinuationToken)

            {

                this.Data               = Data;

            }

            #endregion

            #region APIResult(Success, ...)

            public APIResult(Boolean  Success,
                             UInt16   Code,
                             UInt16   ErrorCode,
                             UInt16   ErrorSubCode,
                             String   ErrorDescription,
                             String   Meta,
                             String   StatusUrl,
                             String   ContinuationToken)
            {

                this.Success            = Success;
                this.Code               = Code;
                this.ErrorCode          = ErrorCode;
                this.ErrorSubCode       = ErrorSubCode;
                this.ErrorDescription   = ErrorDescription;
                this.Meta               = Meta;
                this.StatusUrl          = StatusUrl;
                this.ContinuationToken  = ContinuationToken;

            }

            #endregion

            #region APIResult(ErrorDescription, Exception = null)

            public APIResult(String     ErrorDescription,
                             Exception  Exception = null)

                : this(false,
                       0,
                       0,
                       0,
                       ErrorDescription,
                       Exception?.Message ?? "",
                       "",
                       "")

            { }

            #endregion

            #endregion


            #region (static) TryParse(JSONObj, Data, Result)

            public static Boolean TryParse(JObject           JSONObj,
                                           Lazy<T>           Data,
                                           out APIResult<T>  Result)
            {

                try
                {

                    #region Documentation

                    // HTTP/1.1 200 OK
                    // Cache-Control:    no-cache
                    // Pragma:           no-cache
                    // Content-Length:   658
                    // Content-Type:     application/json;charset=UTF-8
                    // Expires:          -1
                    // Request-Context:  appId=cid-v1:975a755f-8eba-463d-9561-4f15833798f3
                    // Date:             Mon, 16 Jul 2018 21:10:29 GMT
                    // 
                    // {
                    //   "Data":               "...",
                    //   "Success":            true,
                    //   "Code":               200,
                    //   "ErrorCode":          0,
                    //   "ErrorSubCode":       0,
                    //   "ErrorDescription":   "",
                    //   "Meta":               "",
                    //   "StatusUrl":          null,
                    //   "ContinuationToken":  ""
                    // }

                    #endregion

                    #region Parse JSON

                    if (!JSONObj.ParseMandatory("Success",
                                                "Success",
                                                out Boolean Success,
                                                out String  ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("Code",
                                                "Code",
                                                UInt16.TryParse,
                                                out UInt16 Code,
                                                out        ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("ErrorCode",
                                                "error code",
                                                UInt16.TryParse,
                                                out UInt16 ErrorCode,
                                                out ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("ErrorSubCode",
                                                "error subcode",
                                                UInt16.TryParse,
                                                out UInt16 ErrorSubCode,
                                                out ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("ErrorDescription",
                                                "error description",
                                                out String ErrorDescription,
                                                out ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("Meta",
                                                "meta",
                                                out String Meta,
                                                out ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("StatusUrl",
                                                "status url",
                                                out String StatusURL,
                                                out        ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    if (!JSONObj.ParseMandatory("ContinuationToken",
                                                "continuation token",
                                                out String ContinuationToken,
                                                out ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    #endregion

                    Result = new APIResult<T>(Data.Value,
                                              Success,
                                              Code,
                                              ErrorCode,
                                              ErrorSubCode,
                                              ErrorDescription,
                                              Meta,
                                              StatusURL,
                                              ContinuationToken);

                    return true;

                }
                catch (Exception e)
                {
                    Result = new APIResult<T>("Could not parse HTTP response!", e);
                    return false;
                }

            }

            #endregion

            #region (static) TryParse(HttpResponse, Result, Parser)

            public static Boolean TryParse(HTTPResponse      HttpResponse,
                                           out APIResult<T>  Result,
                                           Func<JObject, T>  Parser)
            {

                try
                {

                    if (HttpResponse.TryParseJObjectResponseBody(out JObject JSONObj) &&
                        TryParse(JSONObj, new Lazy<T>(() => Parser(JSONObj)), out Result))
                    {
                        return true;
                    }

                    Result = new APIResult<T>("Could not parse HTTP response!");

                    return false;

                }
                catch (Exception e)
                {
                    Result = new APIResult<T>("Could not parse HTTP response!", e);
                    return false;
                }

            }

            #endregion


            #region (override) ToString()

            /// <summary>
            /// Return a text representation of this object.
            /// </summary>
            public override String ToString()
                => String.Concat(Success.ToString(), " => '", Data, "'");

            #endregion

        }

    }

}
