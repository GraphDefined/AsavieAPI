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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// Information about the IOT devices.
    /// </summary>
    public class IOTDevice : IEquatable<IOTDevice>,
                             IComparable<IOTDevice>,
                             IComparable
    {

        #region Properties

        public CLI              CLI                     { get; }
        public SIM_Id?          SIMNumber               { get; }
        public String           SOC                     { get; }
        public String           InternalSOC             { get; }
        public String           Description             { get; }
        public IIPAddress       IPAddress               { get; }
        public DateTime?        StartDate               { get; }
        public DateTime?        EndDate                 { get; }
        public String           FriendlyName            { get; }
        public String           Hints                   { get; }
        public String           ProvisioningMetaData    { get; }
        public Byte?            Status                  { get; }
        public Byte?            Version                 { get; }
        public Byte?            ActorStatus             { get; }
        public Byte?            NetworkStatus           { get; }
        public Boolean?         Enabled                 { get; }
        public Boolean?         LockIMEI                { get; }
        public String           IMEI                    { get; }
        public DateTime?        LastSync                { get; }
        public Last_Connected?  LastConnected           { get; }

        #endregion

        #region Constructor(s)

        public IOTDevice(CLI              CLI,
                         SIM_Id?          SIMNumber,
                         String           SOC,
                         String           InternalSOC,
                         String           Description,
                         IIPAddress       IPAddress,
                         DateTime?        StartDate,
                         DateTime?        EndDate,
                         String           FriendlyName,
                         String           Hints,
                         String           ProvisioningMetaData,
                         Byte?            Status,
                         Byte?            Version,
                         Byte?            ActorStatus,
                         Byte?            NetworkStatus,
                         Boolean?         Enabled,
                         Boolean?         LockIMEI,
                         String           IMEI,
                         DateTime?        LastSync,
                         Last_Connected?  LastConnected_Start)
        {

            this.CLI                   = CLI;
            this.SIMNumber             = SIMNumber;
            this.SOC                   = SOC;
            this.InternalSOC           = InternalSOC;
            this.Description           = Description;
            this.IPAddress             = IPAddress;
            this.StartDate             = StartDate;
            this.EndDate               = EndDate;
            this.FriendlyName          = FriendlyName;
            this.Hints                 = Hints;
            this.ProvisioningMetaData  = ProvisioningMetaData;
            this.Status                = Status;
            this.Version               = Version;
            this.ActorStatus           = ActorStatus;
            this.NetworkStatus         = NetworkStatus;
            this.Enabled               = Enabled;
            this.LockIMEI              = LockIMEI;
            this.IMEI                  = IMEI;
            this.LastSync              = LastSync;
            this.LastConnected         = LastConnected;

        }

        #endregion


        #region TryParseAsavie(JSON, out IOTDevice)

        public static Boolean TryParseAsavie(JObject JSON, out IOTDevice IOTDevice)
        {

            #region Documentation

            // GetIOTDevices:
            // {
            //    "CLI":                    "204043729927975",
            //    "SIMNumber":              "89314404000132906315",
            //    "SOC":                    null,
            //    "InternalSOC":            "iot",
            //    "Description":            null,
            //    "IPAddress":              "10.20.0.3",
            //    "StartDate":              "2018-06-20T11:13:47.4048154Z",
            //    "EndDate":                "0001-01-01T00:00:00",
            //    "FriendlyName":           "oem-204043729927975",
            //    "Hints":                  null,
            //    "ProvisioningMetaData":   null,
            //    "Status":                 1,
            //    "Version":                5,
            //    "ActorStatus":            0,
            //    "NetworkStatus":          0,
            //    "Enabled":                true,
            //    "LockIMEI":               false,
            //    "IMEI":                   null,
            //    "LastSync":               "2018-06-20T11:13:47.5767265Z",
            //    "LastConnected_Start":    null,
            //    "LastConnected_End":      null,
            //    "LastConnected_MCC":      "",
            //    "LastConnected_MNC":      ""
            // }

            #endregion

            try
            {

                var EndDate       = new DateTime?();
                var EndDateString = JSON["EndDate"]?.Value<String>();
                if (EndDateString.IsNotNullOrEmpty())
                {

                    if (EndDateString == "0001-01-01T00:00:00" ||
                        EndDateString == "01/01/0001 00:00:00")
                        EndDate = null;

                    else
                        EndDate = JSON["EndDate"]?.Value<DateTime>();

                }

                IOTDevice  = new IOTDevice(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                           JSON["SIMNumber"]?.Value<String>().IsNotNullOrEmpty() == true
                                               ? new SIM_Id?(SIM_Id.Parse(JSON["SIMNumber"].Value<String>()))
                                               : null,
                                           JSON["SOC"                 ]?.Value<String>(),
                                           JSON["InternalSOC"         ]?.Value<String>(),
                                           JSON["Description"         ]?.Value<String>(),
                                           JSON["IPAddress"]?.Value<String>().IsNotNullOrEmpty() == true
                                               ? org.GraphDefined.Vanaheimr.Hermod.IPAddress.Parse(JSON["IPAddress"]?.Value<String>())
                                               : null,
                                           JSON["StartDate"           ]?.Value<DateTime>(),
                                           EndDate,
                                           JSON["FriendlyName"        ]?.Value<String>(),
                                           JSON["Hints"               ]?.Value<String>(),
                                           JSON["ProvisioningMetaData"]?.Value<String>(),
                                           JSON["Status"              ]?.Value<Byte>(),
                                           JSON["Version"             ]?.Value<Byte>(),
                                           JSON["ActorStatus"         ]?.Value<Byte>(),
                                           JSON["NetworkStatus"       ]?.Value<Byte>(),
                                           JSON["Enabled"             ]?.Value<Boolean>(),
                                           JSON["LockIMEI"            ]?.Value<Boolean>(),
                                           JSON["IMEI"                ]?.Value<String>(),
                                           JSON["LastSync"            ]?.Value<DateTime>(),
                                           JSON["LastConnected_Start"] is JObject
                                               ? new Last_Connected?(new Last_Connected(JSON["LastConnected_Start"]. Value<DateTime>(),
                                                                                        EndDate,
                                                                                        JSON["LastConnected_MCC"  ]?.Value<String>(),
                                                                                        JSON["LastConnected_MNC"  ]?.Value<String>()))
                                               : null);

                return true;

            }
            catch (Exception)
            { }

            IOTDevice = null;
            return false;

        }

        #endregion

        #region TryParse      (JSON, out IOTDevice)

        public static Boolean TryParse(JObject JSON, out IOTDevice IOTDevice)
        {

            try
            {

                IOTDevice  = new IOTDevice(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                           JSON["SIMNumber"]?.Value<String>().IsNotNullOrEmpty() == true
                                               ? SIM_Id.Parse(JSON["SIMNumber"].Value<String>())
                                               : default,
                                           JSON["SOC"                 ]?.Value<String>(),
                                           JSON["internalSOC"         ]?.Value<String>(),
                                           JSON["description"         ]?.Value<String>(),
                                           JSON["IPAddress"]?.Value<String>().IsNotNullOrEmpty() == true
                                               ? org.GraphDefined.Vanaheimr.Hermod.IPAddress.Parse(JSON["IPAddress"]?.Value<String>())
                                               : default,
                                           JSON["startDate"           ]?.Value<DateTime>(),
                                           JSON["endDate"             ]?.Value<DateTime>(),
                                           JSON["friendlyName"        ]?.Value<String>(),
                                           JSON["hints"               ]?.Value<String>(),
                                           JSON["provisioningMetaData"]?.Value<String>(),
                                           JSON["status"              ]?.Value<Byte>(),
                                           JSON["version"             ]?.Value<Byte>(),
                                           JSON["actorStatus"         ]?.Value<Byte>(),
                                           JSON["networkStatus"       ]?.Value<Byte>(),
                                           JSON["enabled"             ]?.Value<Boolean>(),
                                           JSON["lockIMEI"            ]?.Value<Boolean>(),
                                           JSON["IMEI"                ]?.Value<String>(),
                                           JSON["lastSync"            ]?.Value<DateTime>(),
                                           JSON["lastConnected"] != null ? Last_Connected.Parse(JSON["lastConnected"] as JObject) : default(Last_Connected?));

                return true;

            }
            catch (Exception)
            { }

            IOTDevice = null;
            return false;

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()

            => JSONObject.Create(new JProperty("CLI",                           CLI.ToString()),

                                 SIMNumber.HasValue
                                     ? new JProperty("SIMNumber",               SIMNumber.Value.ToString())
                                     : null,

                                 SOC.IsNotNullOrEmpty()
                                     ? new JProperty("SOC",                     SOC)
                                     : null,

                                 InternalSOC.IsNotNullOrEmpty()
                                     ? new JProperty("internalSOC",             InternalSOC)
                                     : null,

                                 Description.IsNotNullOrEmpty()
                                     ? new JProperty("description",             Description)
                                     : null,

                                 IPAddress != null
                                     ? new JProperty("IPAddress",               IPAddress.ToString())
                                     : null,

                                 StartDate.HasValue
                                     ? new JProperty("startDate",               StartDate.Value.ToIso8601())
                                     : null,

                                 EndDate.HasValue
                                     ? new JProperty("endDate",                 EndDate.Value.ToIso8601())
                                     : null,

                                 FriendlyName.IsNotNullOrEmpty()
                                     ? new JProperty("friendlyName",            FriendlyName)
                                     : null,

                                 Hints.IsNotNullOrEmpty()
                                     ? new JProperty("hints",                   Hints)
                                     : null,

                                 ProvisioningMetaData.IsNotNullOrEmpty()
                                     ? new JProperty("provisioningMetaData",    ProvisioningMetaData)
                                     : null,

                                 Status.HasValue
                                     ? new JProperty("status",                  Status.Value)
                                     : null,

                                 Version.HasValue
                                     ? new JProperty("version",                 Version.Value)
                                     : null,

                                 ActorStatus.HasValue
                                     ? new JProperty("actorStatus",             ActorStatus.Value)
                                     : null,

                                 NetworkStatus.HasValue
                                     ? new JProperty("networkStatus",           NetworkStatus.Value)
                                     : null,

                                 Enabled.HasValue
                                     ? new JProperty("enabled",                 Enabled)
                                     : null,

                                 LockIMEI.HasValue
                                     ? new JProperty("lockIMEI",                LockIMEI)
                                     : null,

                                 IMEI.IsNotNullOrEmpty()
                                     ? new JProperty("IMEI",                    IMEI)
                                     : null,

                                 LastSync.HasValue
                                     ? new JProperty("lastSync",                LastSync.Value.ToIso8601())
                                     : null,

                                 LastConnected.HasValue
                                     ? new JProperty("lastConnected",           LastConnected.Value.ToJSON())
                                     : null);

        #endregion


        #region Operator overloading

        #region Operator == (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(IOTDevice1, IOTDevice2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) IOTDevice1 == null) || ((Object) IOTDevice2 == null))
                return false;

            return IOTDevice1.Equals(IOTDevice2);

        }

        #endregion

        #region Operator != (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
            => !(IOTDevice1 == IOTDevice2);

        #endregion

        #region Operator <  (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
        {

            if ((Object) IOTDevice1 == null)
                throw new ArgumentNullException(nameof(IOTDevice1), "The given IOTDevice1 must not be null!");

            return IOTDevice1.CompareTo(IOTDevice2) < 0;

        }

        #endregion

        #region Operator <= (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
            => !(IOTDevice1 > IOTDevice2);

        #endregion

        #region Operator >  (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
        {

            if ((Object) IOTDevice1 == null)
                throw new ArgumentNullException(nameof(IOTDevice1), "The given IOTDevice1 must not be null!");

            return IOTDevice1.CompareTo(IOTDevice2) > 0;

        }

        #endregion

        #region Operator >= (IOTDevice1, IOTDevice2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice1">A communicator identification.</param>
        /// <param name="IOTDevice2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (IOTDevice IOTDevice1, IOTDevice IOTDevice2)
            => !(IOTDevice1 < IOTDevice2);

        #endregion

        #endregion

        #region IComparable<IOTDevice> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is IOTDevice IOTDevice))
                throw new ArgumentException("The given object is not an communicator!");

            return CompareTo(IOTDevice);

        }

        #endregion

        #region CompareTo(IOTDevice)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IOTDevice">An communicator object to compare with.</param>
        public Int32 CompareTo(IOTDevice IOTDevice)
        {

            if ((Object) IOTDevice == null)
                throw new ArgumentNullException(nameof(IOTDevice), "The given communicator must not be null!");

            return CLI.CompareTo(IOTDevice.CLI);

        }

        #endregion

        #endregion

        #region IEquatable<IOTDevice> Members

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

            var IOTDevice = Object as IOTDevice;
            if (IOTDevice is null)
                return false;

            return Equals(IOTDevice);

        }

        #endregion

        #region Equals(IOTDevice)

        /// <summary>
        /// Compares two communicators for equality.
        /// </summary>
        /// <param name="IOTDevice">An communicator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IOTDevice IOTDevice)
        {

            if ((Object) IOTDevice == null)
                return false;

            return CLI.Equals(IOTDevice.CLI);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => CLI.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(CLI, " / ", SIMNumber, " => ", Status);

        #endregion

    }

}
