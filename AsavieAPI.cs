using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.GraphDefined.Asavie.API
{

    public enum SimCardStateValues
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


    public class AsavieAPI
    {

        public struct AuthToken
        {

            public String    Token     { get; }

            public DateTime  Expires   { get; }

            public TimeSpan  Timeout
                => Expires - DateTime.UtcNow;

            public AuthToken(String    Token,
                             DateTime  Expires)
            {
                this.Token    = Token;
                this.Expires  = Expires;
            }

            public override String ToString()

                => Token.Substring(0, 23) + "... expires in " +

                   (Timeout.TotalMinutes > 3
                        ? Timeout.TotalMinutes + " minutes!"
                        : Timeout.TotalSeconds + " seconds!");

        }

        public class APIResult<T>
        {

            public T        Data                 { get; }

            public Boolean  Success              { get; }
            public UInt16   Code                 { get; }
            public UInt16   ErrorCode            { get; }
            public UInt16   ErrorSubCode         { get; }
            public String   ErrorDescription     { get; }
            public String   Meta                 { get; }
            public String   StatusUrl            { get; }
            public String   ContinuationToken    { get; }

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


            public static Boolean TryParse(JObject           JSONObj,
                                           Lazy<T>           Data,
                                           out APIResult<T>  Result)
            {

                try
                {

                    if (!JSONObj.ParseMandatory("Success",
                                                "Success",
                                                out Boolean Success,
                                                out String ErrorResponse))
                    {
                        throw new Exception(ErrorResponse);
                    }

                    Result = new APIResult<T>(Data.Value,
                                              Success,
                                              0,
                                              0,
                                              0,
                                              "",
                                              "",
                                              "",
                                              "");

                    return true;

                }
                catch (Exception e)
                {

                    Result = new APIResult<T>(false,
                                              0,
                                              0,
                                              0,
                                              e.Message,
                                              "",
                                              "",
                                              "");

                    return false;

                }

            }


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

                    Result = new APIResult<T>(false,
                                              0,
                                              0,
                                              0,
                                              "",
                                              "",
                                              "",
                                              "");

                    return false;

                }
                catch (Exception e)
                {

                    Result = new APIResult<T>(false,
                                              0,
                                              0,
                                              0,
                                              e.Message,
                                              "",
                                              "",
                                              "");

                    return false;

                }

            }


            public override String ToString()
                => String.Concat(Success.ToString(), " => '", Data, "'");

        }


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

        #endregion

        #region Properties

        public String                               Login                         { get; }

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


        protected AuthToken CurrentAccessToken;
        public Version APIVersion { get; private set; }

        #region NewAuthToken   (...)

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

        #region GetAPIVersion  (...)

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

                    try
                    {

                        if (APIResult<Version>.TryParse(httpresponse,
                                                        //APIVersion,
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

                        return new APIResult<Version>(false,
                                                      0,
                                                      0,
                                                      0,
                                                      "Asavie NewAuthToken response JSON could not be parsed!",
                                                      "",
                                                      "",
                                                      "");

                    }
                    catch (Exception e)
                    {

                        return new APIResult<Version>(false,
                                                      0,
                                                      0,
                                                      0,
                                                      "Asavie NewAuthToken response JSON could not be parsed: " + e.Message,
                                                      "",
                                                      "",
                                                      "");

                    }

                }

                #endregion

                return new APIResult<Version>(false,
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

                return new APIResult<Version>(false,
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

        #region GetAccount     (AccountName, ...)

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
                                                        //APIJObject,
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

                        return new APIResult<JObject>(false,
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

                        return new APIResult<JObject>(false,
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

                return new APIResult<JObject>(false,
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

                return new APIResult<JObject>(false,
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


        #region GetHardwareSIMs(AccountName, ...)

        public async Task<APIResult<IEnumerable<JObject>>>

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

                        if (APIResult<IEnumerable<JObject>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<JObject>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "hardware SIMs information",
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
                                                                   "Asavie hardware SIMs information JSON could not be parsed!",
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
                                                                   "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
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

        #region GetNetworks    (AccountName, ...)

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

        #region GetDevicesAPNs (AccountName, NetworkName, ...)

        public async Task<APIResult<IEnumerable<JObject>>>

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

                        if (APIResult<IEnumerable<JObject>>.TryParse(httpresponse,
                                                                     out APIResult<IEnumerable<JObject>> Result,
                                                                     JSONObj => {

                                                                         if (!JSONObj.ParseMandatory("Data",
                                                                                                     "hardware SIMs information",
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
                                                                   "Asavie hardware SIMs information JSON could not be parsed!",
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
                                                                   "Asavie hardware SIMs information JSON could not be parsed: " + e.Message,
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


    }

}
