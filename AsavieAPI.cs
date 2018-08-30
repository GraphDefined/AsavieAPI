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

        #region Data

        /// <summary>
        /// The default remote hostname.
        /// </summary>
        public const           String             DefaultHostname            = "api.provisioning.asavie.com";

        /// <summary>
        /// The default HTTP port.
        /// </summary>
        public static readonly IPPort             DefaultHTTPPort            = IPPort.HTTPS;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly TimeSpan           DefaultRequestTimeout      = TimeSpan.FromSeconds(180);

        /// <summary>
        /// The current auth token.
        /// </summary>
        protected AuthToken CurrentAccessToken;

        #endregion

        #region Properties

        /// <summary>
        /// The remote login.
        /// </summary>
        public String                               Login                         { get; }

        /// <summary>
        /// The remote password.
        /// </summary>
        public String                               Password                      { get; }

        /// <summary>
        /// The remote hostname.
        /// </summary>
        public String                               Hostname                      { get; }

        /// <summary>
        /// The remote virtual hostname.
        /// </summary>
        public String                               VirtualHostname               { get; }

        /// <summary>
        /// The remote HTTPS port.
        /// </summary>
        public IPPort                               HTTPPort                      { get; }

        /// <summary>
        /// The remote SSL/TLS certificate validator.
        /// </summary>
        public RemoteCertificateValidationCallback  RemoteCertificateValidator    { get; }

        /// <summary>
        /// The request timeout.
        /// </summary>
        public TimeSpan?                            RequestTimeout                { get; }

        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                            DNSClient                     { get; }


        /// <summary>
        /// The remote version of the API.
        /// </summary>
        public Version                              APIVersion                    { get; private set; }

        #endregion

        #region Constructor(s)

        public AsavieAPI(String                               Login,
                         String                               Password,
                         String                               Hostname                     = null,
                         String                               VirtualHostname              = null,
                         IPPort?                              HTTPPort                     = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                         TimeSpan?                            RequestTimeout               = null,
                         DNSClient                            DNSClient                    = null)

        {

            this.Login                       = Login?.   Trim();
            this.Password                    = Password?.Trim();

            if (Login.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Login),     "The given login must not be null or empty!");

            if (Password.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Password),  "The given password must not be null or empty!");

            this.Hostname                    = Hostname?.       Trim();
            this.VirtualHostname             = VirtualHostname?.Trim();
            this.HTTPPort                    = HTTPPort                   ?? DefaultHTTPPort;
            this.RemoteCertificateValidator  = RemoteCertificateValidator ?? ((sender, certificate, chain, policyErrors) => true);
            this.RequestTimeout              = RequestTimeout             ?? DefaultRequestTimeout;
            this.DNSClient                   = DNSClient;

            if (Hostname.IsNullOrEmpty())
                this.Hostname         = DefaultHostname;

            if (VirtualHostname.IsNullOrEmpty())
                this.VirtualHostname  = this.Hostname;

            if (DNSClient == null)
                throw new ArgumentNullException(nameof(Hostname),  "The given DNS client must not be null or empty!");

        }

        #endregion


        #region NewAuthToken       (...)

        public async Task<AuthToken>

            NewAuthToken(DateTime?                Timestamp           = null,
                         CancellationToken?       CancellationToken   = null,
                         EventTracking_Id         EventTrackingId     = null,
                         TimeSpan?                RequestTimeout      = null)

        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                       RemoteCertificateValidator,
                                                       RemotePort:  HTTPPort,
                                                       DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.POST(HTTPURI.Parse("/v1/authtoken"),

                                                                         requestbuilder => {
                                                                             requestbuilder.Host         = VirtualHostname;
                                                                             requestbuilder.ContentType  = HTTPContentType.XWWWFormUrlEncoded;
                                                                             requestbuilder.Content      = ("grant_type=client_credentials&client_id=" + Login + "&client_secret=" + Password).
                                                                                                               ToUTF8Bytes();
                                                                             requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                         }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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
                    //   "access_token": "xkitMAgNLytyGOaqNDY4IZplPA0aTKSF3ak2aSMLUxSqjdBYOpIs8jrBYfjr9EhsJdjM5dSgsPkga0Z97tz_NR7P9P7710Dmva24__o6Jr4p09iAClQGKdnZAdCc_Rn8hdHE6N0Ce-DKakIrpESxMXsSJ6-3ZluArVYVYZOvlECIJ1Plr7tumBae2j87O4kX3W9MbCvt4tuibNnu4zRcyd3jD2wtKk323w00V4RqOzgmcDZ3Mz__52mJqtVlUWvvrPLA6FbdhhD0zcjvSIFk9yk3of9CGUQlEILkaqAKqYrjVO-slyvd8-biE0qAaZkWJhoZrE7i2oRCi8tk-7Y3h0UuAWWa0Hs8TgUC7_BoOBp4VOJpwwjIFJo92u3MbTAL0iSltfxyXfUrNfiKxu_IZR3oXQc9pUuCI9moTCbFaGfjwOK1RxQEWdGKbDwEP2aOYhpr5i_-9xHRGxqZRnHMiaoseNIcxE4Leswlpo6qM7M",
                    //   "token_type":   "bearer",
                    //   "expires_in":   3599,
                    //   "userName":     "cardilink",
                    //   ".issued":      "Mon, 16 Jul 2018 21:10:29 GMT",
                    //   ".expires":     "Mon, 16 Jul 2018 22:10:29 GMT"
                    // }

                    try
                    {

                        if (httpresponse.TryParseJObjectResponseBody(out JObject JSONObj))
                        {

                            if (!JSONObj.ParseMandatory("access_token",
                                                        "access token",
                                                        out String Token,
                                                        out String ErrorResponse))
                            {
                                throw new Exception(ErrorResponse);
                            }

                            if (!JSONObj.ParseMandatory(".expires",
                                                        "expires timestamp",
                                                        out DateTime Expires,
                                                        out          ErrorResponse))
                            {
                                throw new Exception(ErrorResponse);
                            }

                            return CurrentAccessToken = new AuthToken(Token,
                                                                      Expires);

                        }

                        else
                        {
                            DebugX.LogT("Asavie NewAuthToken response JSON could not be parsed! " + Environment.NewLine + httpresponse.EntirePDU);
                            throw new Exception("Asavie NewAuthToken response JSON could not be parsed!");
                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.LogT("Asavie NewAuthToken response JSON could not be parsed! " + Environment.NewLine + httpresponse.EntirePDU + Environment.NewLine + e.Message);
                        throw new Exception("Asavie NewAuthToken response JSON could not be parsed: " + e.Message);
                    }

                }

                #endregion

                return new AuthToken(httpresponse.HTTPStatusCode.ToString(),
                                     DateTime.UtcNow);

            }

            catch (Exception e)
            {
                return new AuthToken(e.Message, DateTime.UtcNow);
            }

        }

        #endregion

        #region GetAPIVersion      (...)

        public async Task<APIResult<Version>>

            GetAPIVersion(DateTime?                Timestamp           = null,
                          CancellationToken?       CancellationToken   = null,
                          EventTracking_Id         EventTrackingId     = null,
                          TimeSpan?                RequestTimeout      = null)

        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/version"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
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
                    //   "Data":               "2.109.6764.20476",
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

                    try
                    {

                        if (APIResult<Version>.TryParse(httpresponse,
                                                        out APIResult<Version> Result,
                                                        JSONObj => {

                                                            if (!JSONObj.ParseMandatory("Data",
                                                                                        "API version",
                                                                                        Version.TryParse,
                                                                                        out Version APIVersion,
                                                                                        out String  ErrorResponse))
                                                            {
                                                                throw new Exception(ErrorResponse);
                                                            }

                                                            return APIVersion;

                                                        }))
                        {
                            this.APIVersion = Result.Data;
                            return Result;
                        }

                        return new APIResult<Version>("HTTP JSON response could not be parsed!");

                    }
                    catch (Exception e)
                    {
                        return new APIResult<Version>("HTTP JSON response could not be parsed!", e);
                    }

                }

                #endregion

                return new APIResult<Version>(httpresponse.HTTPStatusCode.ToString());

            }

            catch (Exception e)
            {
                return new APIResult<Version>("HTTP JSON response could not be parsed!", e);
            }

        }

        #endregion

        #region GetAccount         (AccountName, ...)

        public async Task<APIResult<JObject>>

            GetAccount(String                   AccountName,

                       DateTime?                Timestamp           = null,
                       CancellationToken?       CancellationToken   = null,
                       EventTracking_Id         EventTrackingId     = null,
                       TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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

                    try
                    {

                        if (APIResult<JObject>.TryParse(httpresponse,
                                                        out APIResult<JObject> Result,
                                                        JSONObj => {

                                                            if (!JSONObj.ParseMandatory("Data",
                                                                                        "account information",
                                                                                        out JObject APIJObject,
                                                                                        out String  ErrorResponse))
                                                            {
                                                                throw new Exception(ErrorResponse);
                                                            }

                                                            return APIJObject;

                                                        }))
                        {
                            return Result;
                        }

                        return new APIResult<JObject>("HTTP JSON response could not be parsed!");

                    }
                    catch (Exception e)
                    {
                        return new APIResult<JObject>("HTTP JSON response could not be parsed!", e);
                    }

                }

                #endregion

                return new APIResult<JObject>(httpresponse.HTTPStatusCode.ToString());

            }

            catch (Exception e)
            {
                return new APIResult<JObject>("HTTP JSON response could not be parsed!", e);
            }

        }

        #endregion


        #region GetHardwareSIMs    (AccountName, ...)

        public async Task<APIResult<IEnumerable<HardwareSIM>>>

            GetHardwareSIMs(String                   AccountName,

                            DateTime?                Timestamp           = null,
                            CancellationToken?       CancellationToken   = null,
                            EventTracking_Id         EventTrackingId     = null,
                            TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/hardware/sims"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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
                    //   "Data":               [{ ... }, { ... }],
                    //   "Success":            true,
                    //   "Code":               200,
                    //   "ErrorCode":          0,
                    //   "ErrorSubCode":       0,
                    //   "ErrorDescription":   "",
                    //   "Meta":               "",
                    //   "StatusUrl":          null,
                    //   "ContinuationToken":  ""
                    // }

                    try
                    {

                        if (APIResult<IEnumerable<HardwareSIM>>.TryParse(httpresponse,
                                                                         out APIResult<IEnumerable<HardwareSIM>> Result,
                                                                         JSONObj => {

                                                                             if (!JSONObj.ParseMandatory("Data",
                                                                                                         "hardware SIMs information",
                                                                                                         out JArray  ArrayOfSIMs,
                                                                                                         out String  ErrorResponse))
                                                                             {
                                                                                 throw new Exception(ErrorResponse);
                                                                             }

                                                                             var ListOfSIMs = new List<HardwareSIM>();

                                                                             foreach (var jToken in ArrayOfSIMs)
                                                                             {

                                                                                 if (jToken is JObject jObject &&
                                                                                     HardwareSIM.TryParseAsavie(jObject, out HardwareSIM hardwareSIM))
                                                                                 {
                                                                                     ListOfSIMs.Add(hardwareSIM);
                                                                                 }

                                                                                 else
                                                                                     throw new Exception("Could not parse '" + jToken + "' as HardwareSIM information!");

                                                                             }

                                                                             return ListOfSIMs;

                                                                         }))
                        {
                            return Result;
                        }

                        return new APIResult<IEnumerable<HardwareSIM>>(false,
                                                                       0,
                                                                       0,
                                                                       0,
                                                                       "Asavie hardware SIMs information JSON could not be parsed!",
                                                                       "",
                                                                       "",
                                                                       "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<IEnumerable<HardwareSIM>>(false,
                                                                       0,
                                                                       0,
                                                                       0,
                                                                       "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
                                                                       "",
                                                                       "",
                                                                       "");

                    }

                }

                #endregion

                return new APIResult<IEnumerable<HardwareSIM>>(false,
                                                               0,
                                                               0,
                                                               0,
                                                               httpresponse.HTTPStatusCode.ToString(),
                                                               "",
                                                               "",
                                                               "");

            }

            catch (Exception e)
            {

                return new APIResult<IEnumerable<HardwareSIM>>(false,
                                                               0,
                                                               0,
                                                               0,
                                                               e.Message,
                                                               "",
                                                               "",
                                                               "");

            }

        }

        #endregion

        #region GetHardwareSIM     (AccountName, SIMId, ...)

        public async Task<APIResult<HardwareSIM>>

            GetHardwareSIM(String                   AccountName,
                           String                   SIMId,

                           DateTime?                Timestamp           = null,
                           CancellationToken?       CancellationToken   = null,
                           EventTracking_Id         EventTrackingId     = null,
                           TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/hardware/sims/" + SIMId),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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
                    //   "Data":               { ... },
                    //   "Success":            true,
                    //   "Code":               200,
                    //   "ErrorCode":          0,
                    //   "ErrorSubCode":       0,
                    //   "ErrorDescription":   "",
                    //   "Meta":               "",
                    //   "StatusUrl":          null,
                    //   "ContinuationToken":  ""
                    // }

                    try
                    {

                        if (APIResult<HardwareSIM>.TryParse(httpresponse,
                                                            out APIResult<HardwareSIM> Result,
                                                            JSONObj => {

                                                                if (!JSONObj.ParseMandatory("Data",
                                                                                            "hardware SIMs information",
                                                                                            out JObject SIM,
                                                                                            out String  ErrorResponse))
                                                                {
                                                                    throw new Exception(ErrorResponse);
                                                                }

                                                                if (HardwareSIM.TryParseAsavie(SIM, out HardwareSIM hardwareSIM))
                                                                    return hardwareSIM;

                                                                return null;

                                                            }))
                        {
                            return Result;
                        }

                        return new APIResult<HardwareSIM>(false,
                                                          0,
                                                          0,
                                                          0,
                                                          "Asavie hardware SIMs information JSON could not be parsed!",
                                                          "",
                                                          "",
                                                          "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<HardwareSIM>(false,
                                                          0,
                                                          0,
                                                          0,
                                                          "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
                                                          "",
                                                          "",
                                                          "");

                    }

                }

                #endregion

                return new APIResult<HardwareSIM>(false,
                                                  0,
                                                  0,
                                                  0,
                                                  httpresponse.HTTPStatusCode.ToString(),
                                                  "",
                                                  "",
                                                  "");

            }

            catch (Exception e)
            {

                return new APIResult<HardwareSIM>(false,
                                                  0,
                                                  0,
                                                  0,
                                                  e.Message,
                                                  "",
                                                  "",
                                                  "");

            }

        }

        #endregion

        #region SetHardwareSIMState(AccountName, SIMId, NewSIMState, ...)

        public async Task<APIResult<HardwareSIM>>

            SetHardwareSIMState(String              AccountName,
                                String              SIMId,
                                SimCardStates       NewSIMState,

                                DateTime?           Timestamp           = null,
                                CancellationToken?  CancellationToken   = null,
                                EventTracking_Id    EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.PATCH(HTTPURI.Parse("/v1/accounts/" + AccountName + "/hardware/sims/" + SIMId),

                                                                          requestbuilder => {
                                                                              requestbuilder.Host           = VirtualHostname;
                                                                              requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                              requestbuilder.ContentType    = HTTPContentType.JSON_UTF8;
                                                                              requestbuilder.Content        = JSONObject.Create(
                                                                                                                  new JProperty("State", (Int32) NewSIMState)
                                                                                                              ).ToUTF8Bytes();
                                                                              requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                          }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

                    // HTTP/1.1 200 OK
                    // Cache-Control: no-cache
                    // Pragma: no-cache
                    // Content-Length: 811
                    // Content-Type: application/json; charset=utf-8
                    // Expires: -1
                    // Request-Context: appId=cid-v1:975a755f-8eba-463d-9561-4f15833798f3
                    // Date: Tue, 17 Jul 2018 13:31:03 GMT
                    // 
                    // {
                    //   "Data": {
                    //     "CLI":       "204046868148000",
                    //     "SIMNumber": "89314404000452386593",
                    //     "Description":null,
                    //     "InventoryRef":null,
                    //     "State":3,
                    //     "Version":1,
                    //     "ActorStatus":0,
                    //     "ForeignAttributes":null,
                    //     "Tariff":null,
                    //     "Bundle":null,
                    //     "Operator":"vodafone",
                    //     "Hints":null,
                    //     "NetworkStatus":0,
                    //     "Created":"2018-07-17T11:22:56.7941452Z",
                    //     "SOC":"SOC190",
                    //     "InternalSOC":null,
                    //     "PurchaseOrder":"na",
                    //     "Provider":"westbase",
                    //     "ProviderTariff":"na",
                    //     "ProviderPrice":"na",
                    //     "ProviderStartDate":"2018-07-17T11:22:56.4326309Z"
                    //   },
                    //   "Success":true,
                    //   "Code":202,
                    //   "ErrorCode":0,
                    //   "ErrorSubCode":0,
                    //   "ErrorDescription":"",
                    //   "Meta":"",
                    //   "StatusUrl":"/v1/accounts/cardilink/hardware/sims/204046868148000/status?cmdid=Z2EvOWB9EDgRdW1qYitvNyYrODVuHGE3cj9gSBQpHnQfIig1KXIOXxAEBR9ddyJcT21ec05%2FLEZuOyEsYG89aTRBo%2FQzOTA0MnFqMVEWen95DVYlYj42MTBpYSw1OigpJDAzbG0%2FIkFvTXo9MzkE",
                    //   "ContinuationToken":""
                    // }

                    try
                    {

                        if (APIResult<HardwareSIM>.TryParse(httpresponse,
                                                            out APIResult<HardwareSIM> Result,
                                                            JSONObj => {

                                                                if (!JSONObj.ParseMandatory("Data",
                                                                                            "hardware SIM information",
                                                                                            out JObject SIM,
                                                                                            out String  ErrorResponse))
                                                                {
                                                                    throw new Exception(ErrorResponse);
                                                                }

                                                                if (HardwareSIM.TryParseAsavie(SIM, out HardwareSIM hardwareSIM))
                                                                    return hardwareSIM;

                                                                return null;

                                                            }))
                        {
                            return Result;
                        }

                        return new APIResult<HardwareSIM>(false,
                                                          0,
                                                          0,
                                                          0,
                                                          "Asavie hardware SIMs information JSON could not be parsed!",
                                                          "",
                                                          "",
                                                          "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<HardwareSIM>(false,
                                                          0,
                                                          0,
                                                          0,
                                                          "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
                                                          "",
                                                          "",
                                                          "");

                    }

                }

                #endregion

                return new APIResult<HardwareSIM>(false,
                                                  0,
                                                  0,
                                                  0,
                                                  httpresponse.HTTPStatusCode.ToString(),
                                                  "",
                                                  "",
                                                  "");

            }

            catch (Exception e)
            {

                return new APIResult<HardwareSIM>(false,
                                                  0,
                                                  0,
                                                  0,
                                                  e.Message,
                                                  "",
                                                  "",
                                                  "");

            }

        }

        #endregion


        #region GetNetworks        (AccountName, ...)

        public async Task<APIResult<IEnumerable<JObject>>>

            GetNetworks(String                   AccountName,

                        DateTime?                Timestamp           = null,
                        CancellationToken?       CancellationToken   = null,
                        EventTracking_Id         EventTrackingId     = null,
                        TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/networks"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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

                    try
                    {

                        if (APIResult<IEnumerable<JObject>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<JObject>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "account information",
                                                                                                     out JArray  ArrayOfSIMs,
                                                                                                     out String  ErrorResponse))
                                                                         {
                                                                             throw new Exception(ErrorResponse);
                                                                         }

                                                                         var ListOfSIMs = new List<JObject>();
                                                                         JObject simInfo = null;

                                                                         foreach (var json in ArrayOfSIMs)
                                                                         {

                                                                             simInfo = json as JObject;

                                                                             if (simInfo != null)
                                                                                 ListOfSIMs.Add(simInfo);

                                                                         }

                                                                         return ListOfSIMs;

                                                                     }))
                        {
                            return Result;
                        }

                        return new APIResult<IEnumerable<JObject>>(false,
                                                                   0,
                                                                   0,
                                                                   0,
                                                                   "Asavie account information JSON could not be parsed!",
                                                                   "",
                                                                   "",
                                                                   "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<IEnumerable<JObject>>(false,
                                                                   0,
                                                                   0,
                                                                   0,
                                                                   "Asavie account information JSON could not be parsed: " + e.Message,
                                                                   "",
                                                                   "",
                                                                   "");

                    }

                }

                #endregion

                return new APIResult<IEnumerable<JObject>>(false,
                                                           0,
                                                           0,
                                                           0,
                                                           httpresponse.HTTPStatusCode.ToString(),
                                                           "",
                                                           "",
                                                           "");

            }

            catch (Exception e)
            {

                return new APIResult<IEnumerable<JObject>>(false,
                                                           0,
                                                           0,
                                                           0,
                                                           e.Message,
                                                           "",
                                                           "",
                                                           "");

            }

        }

        #endregion

        #region GetDevicesAPNs     (AccountName, NetworkName, ...)

        public async Task<APIResult<IEnumerable<DevicesAPN>>>

            GetDevicesAPNs(String                   AccountName,
                           String                   NetworkName,

                           DateTime?                Timestamp           = null,
                           CancellationToken?       CancellationToken   = null,
                           EventTracking_Id         EventTrackingId     = null,
                           TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/networks/" + NetworkName + "/devices/apns"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

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
                    //   "Data":               [{ ... }, { ... }],
                    //   "Success":            true,
                    //   "Code":               200,
                    //   "ErrorCode":          0,
                    //   "ErrorSubCode":       0,
                    //   "ErrorDescription":   "",
                    //   "Meta":               "",
                    //   "StatusUrl":          null,
                    //   "ContinuationToken":  ""
                    // }

                    try
                    {

                        if (APIResult<IEnumerable<DevicesAPN>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<DevicesAPN>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "devices APN information",
                                                                                                     out JArray  ArrayOfDevicesAPNs,
                                                                                                     out String  ErrorResponse))
                                                                         {
                                                                             throw new Exception(ErrorResponse);
                                                                         }

                                                                         var resultList = new List<DevicesAPN>();

                                                                         foreach (var jToken in ArrayOfDevicesAPNs)
                                                                         {

                                                                             if (jToken is JObject jObject &&
                                                                                 DevicesAPN.TryParseAsavie(jObject, out DevicesAPN devicesAPN))
                                                                             {
                                                                                 resultList.Add(devicesAPN);
                                                                             }

                                                                             else
                                                                                 throw new Exception("Could not parse '" + jToken + "' as DevicesAPN information!");

                                                                         }

                                                                         return resultList;

                                                                     }))
                        {
                            return Result;
                        }

                        return new APIResult<IEnumerable<DevicesAPN>>(false,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      "Asavie hardware SIMs information JSON could not be parsed!",
                                                                      "",
                                                                      "",
                                                                      "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<IEnumerable<DevicesAPN>>(false,
                                                                      0,
                                                                      0,
                                                                      0,
                                                                      "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
                                                                      "",
                                                                      "",
                                                                      "");

                    }

                }

                #endregion

                return new APIResult<IEnumerable<DevicesAPN>>(false,
                                                              0,
                                                              0,
                                                              0,
                                                              httpresponse.HTTPStatusCode.ToString(),
                                                              "",
                                                              "",
                                                              "");

            }

            catch (Exception e)
            {

                return new APIResult<IEnumerable<DevicesAPN>>(false,
                                                              0,
                                                              0,
                                                              0,
                                                              e.Message,
                                                              "",
                                                              "",
                                                              "");

            }

        }

        #endregion


        #region GetClientSessions  (AccountName, NetworkName, From = null, To = null, ...)

        public async Task<APIResult<IEnumerable<JObject>>>

            GetClientSessions(String              AccountName,
                              String              NetworkName,
                              DateTime?           From                = null,
                              DateTime?           To                  = null,

                              DateTime?           Timestamp           = null,
                              CancellationToken?  CancellationToken   = null,
                              EventTracking_Id    EventTrackingId     = null,
                              TimeSpan?           RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            var Filter = "";

            if (From.HasValue)
                Filter += "?from=" + From.Value.ToUniversalTime().ToString("dMMMyyyy HH:mm");

            if (To.HasValue)
                Filter += (Filter.Length > 0 ? "&" : "?") + "to=" + To.Value.ToUniversalTime().ToString("dMMMyyyy HH:mm");

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/networks/" + NetworkName + "/analytics/clientsessions" + Filter.Replace(" ", "%20")), //?from=01jul2016%2000:00&to=11jul2018%2000:00"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
                {

                    #region Documentation

                    // HTTP/1.1 200 OK
                    // Cache-Control: no-cache
                    // Pragma: no-cache
                    // Transfer-Encoding: chunked
                    // Content-Type: application/octet-stream
                    // Expires: -1
                    // Request-Context: appId=cid-v1:975a755f-8eba-463d-9561-4f15833798f3
                    // Date: Tue, 17 Jul 2018 15:59:01 GMT
                    // 
                    //  {
                    //    "Data": [
                    //      {
                    //        "DeviceName":        "467190019369536",
                    //        "CLI":               "(+46)07190019369536",
                    //        "StartTime":         "2018-07-17 15:46:29Z",
                    //        "EndTime":           "2018-07-17 15:46:30Z",
                    //        "SessionEndCause":   "AccountingStop",
                    //        "DataTx":            0,
                    //        "DataRx":            0,
                    //        "MCC":               "204",
                    //        "MNC":               "16",
                    //        "RestrictionCause":  "None",
                    //        "RestrictionTime":   "0001-01-01 00:00:00Z",
                    //        "DataAccessGroup":   "",
                    //        "Zone":              ""
                    //      },
                    //      ...
                    //    ],
                    //    "Success":            true,
                    //    "Code":               200,
                    //    "ErrorCode":          0,
                    //    "ErrorSubCode":       0,
                    //    "ErrorDescription":   "",
                    //    "Meta":               "",
                    //    "StatusUrl":          null,
                    //    "ContinuationToken":  null
                    //  }

                    #endregion

                    try
                    {

                        if (APIResult<IEnumerable<JObject>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<JObject>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "account information",
                                                                                                     out JArray  ArrayOfSIMs,
                                                                                                     out String  ErrorResponse))
                                                                         {
                                                                             throw new Exception(ErrorResponse);
                                                                         }

                                                                         var ListOfSIMs = new List<JObject>();
                                                                         JObject simInfo = null;

                                                                         foreach (var json in ArrayOfSIMs)
                                                                         {

                                                                             simInfo = json as JObject;

                                                                             if (simInfo != null)
                                                                                 ListOfSIMs.Add(simInfo);

                                                                         }

                                                                         return ListOfSIMs;

                                                                     }))
                        {
                            return Result;
                        }

                        return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!");

                    }
                    catch (Exception e)
                    {
                        return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!", e);
                    }

                }

                #endregion

                return new APIResult<IEnumerable<JObject>>(httpresponse.HTTPStatusCode.ToString());

            }

            catch (Exception e)
            {
                return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!", e);
            }

        }

        #endregion

        #region GetLogs            (AccountName, ...)

        public async Task<APIResult<IEnumerable<JObject>>>

            GetLogs(String                   AccountName,

                    DateTime?                Timestamp           = null,
                    CancellationToken?       CancellationToken   = null,
                    EventTracking_Id         EventTrackingId     = null,
                    TimeSpan?                RequestTimeout      = null)

        {

            AccountName = AccountName?.Trim();

            if (AccountName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(AccountName), "The given account name must not be null or empty!");

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            try
            {

                #region Upstream HTTP request...

                var httpresponse = await new HTTPSClient(Hostname,
                                                         RemoteCertificateValidator,
                                                         RemotePort:  HTTPPort,
                                                         DNSClient:   DNSClient ?? this.DNSClient).

                                           Execute(client => client.GET(HTTPURI.Parse("/v1/accounts/" + AccountName + "/logs"),

                                                                        requestbuilder => {
                                                                            requestbuilder.Host           = VirtualHostname;
                                                                            requestbuilder.Authorization  = new HTTPBearerAuthentication(CurrentAccessToken.Token);
                                                                            requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                        }),

                                                   //RequestLogDelegate:   OnRemoteStartRequest,
                                                   //ResponseLogDelegate:  OnRemoteStartResponse,
                                                   CancellationToken:    CancellationToken,
                                                   EventTrackingId:      EventTrackingId,
                                                   RequestTimeout:       RequestTimeout ?? TimeSpan.FromSeconds(60)).

                                           ConfigureAwait(false);

                #endregion

                #region HTTPStatusCode.OK

                if (httpresponse.HTTPStatusCode == HTTPStatusCode.OK)
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

                    try
                    {

                        if (APIResult<IEnumerable<JObject>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<JObject>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "account information",
                                                                                                     out JArray  ArrayOfSIMs,
                                                                                                     out String  ErrorResponse))
                                                                         {
                                                                             throw new Exception(ErrorResponse);
                                                                         }

                                                                         var ListOfSIMs = new List<JObject>();
                                                                         JObject simInfo = null;

                                                                         foreach (var json in ArrayOfSIMs)
                                                                         {

                                                                             simInfo = json as JObject;

                                                                             if (simInfo != null)
                                                                                 ListOfSIMs.Add(simInfo);

                                                                         }

                                                                         return ListOfSIMs;

                                                                     }))
                        {
                            return Result;
                        }

                        return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!");

                    }
                    catch (Exception e)
                    {
                        return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!", e);
                    }

                }

                #endregion

                return new APIResult<IEnumerable<JObject>>(httpresponse.HTTPStatusCode.ToString());

            }

            catch (Exception e)
            {
                return new APIResult<IEnumerable<JObject>>("HTTP JSON response could not be parsed!", e);
            }

        }

        #endregion

    }

}
