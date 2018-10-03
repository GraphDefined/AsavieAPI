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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace com.GraphDefined.Asavie.API
{

    /// <summary>
    /// Information about a SIM card.
    /// </summary>
    public class SIMHardware : IEquatable<SIMHardware>,
                               IComparable<SIMHardware>,
                               IComparable
    {

        #region Properties

        public CLI            CLI                   { get; }
        public SIM_Id?        SIMNumber             { get; }
        public String         Description           { get; }
        public String         InventoryRef          { get; }
        public SimCardStates  State                 { get; }
        public Byte?          Version               { get; }
        public Byte?          ActorStatus           { get; }
        public String         ForeignAttributes     { get; }
        public String         Tariff                { get; }
        public String         Bundle                { get; }
        public Operator_Id?   Operator              { get; }
        public String         Hints                 { get; }
        public Byte?          NetworkStatus         { get; }
        public DateTime?      Created               { get; }
        public String         SOC                   { get; }
        public String         InternalSOC           { get; }
        public String         PurchaseOrder         { get; }
        public Provider_Id?   Provider              { get; }
        public String         ProviderTariff        { get; }
        public String         ProviderPrice         { get; }
        public DateTime?      ProviderStartDate     { get; }

        #endregion

        #region Constructor(s)

        public SIMHardware(CLI            CLI,
                           SIM_Id?        SIMNumber,
                           String         Description,
                           String         InventoryRef,
                           SimCardStates  State,
                           Byte?          Version,
                           Byte?          ActorStatus,
                           String         ForeignAttributes,
                           String         Tariff,
                           String         Bundle,
                           Operator_Id?   Operator,
                           String         Hints,
                           Byte?          NetworkStatus,
                           DateTime?      Created,
                           String         SOC,
                           String         InternalSOC,
                           String         PurchaseOrder,
                           Provider_Id?   Provider,
                           String         ProviderTariff,
                           String         ProviderPrice,
                           DateTime?      ProviderStartDate)
        {

            this.CLI                 = CLI;
            this.SIMNumber           = SIMNumber;
            this.Description         = Description;
            this.InventoryRef        = InventoryRef;
            this.State               = State;
            this.Version             = Version;
            this.ActorStatus         = ActorStatus;
            this.ForeignAttributes   = ForeignAttributes;
            this.Tariff              = Tariff;
            this.Bundle              = Bundle;
            this.Operator            = Operator;
            this.Hints               = Hints;
            this.NetworkStatus       = NetworkStatus;
            this.Created             = Created;
            this.SOC                 = SOC;
            this.InternalSOC         = InternalSOC;
            this.PurchaseOrder       = PurchaseOrder;
            this.Provider            = Provider;
            this.ProviderTariff      = ProviderTariff;
            this.ProviderPrice       = ProviderPrice;
            this.ProviderStartDate   = ProviderStartDate;

        }

        #endregion


        #region TryParseAsavie(JSON, out SIMHardware)

        public static Boolean TryParseAsavie(JObject JSON, out SIMHardware SIMHardware)
        {

            #region Documentation

            // GetSIMHardwares:
            // {
            //    "CLI":                    "204043729xxxxxx",
            //    "SIMNumber":              "89314404000132xxxxxx",
            //    "Description":            null,
            //    "InventoryRef":           null,
            //    "State":                  3,
            //    "Version":                5,
            //    "ActorStatus":            0,
            //    "ForeignAttributes":      null,
            //    "Tariff":                 null,
            //    "Bundle":                 null,
            //    "Operator":               "vodafone",
            //    "Hints":                  null,
            //    "NetworkStatus":          0,
            //    "Created":                "2018-04-31T10:44:41.7019783Z",
            //    "SOC":                    null,
            //    "InternalSOC":            null,
            //    "PurchaseOrder":          "na",
            //    "Provider":               "westbase",
            //    "ProviderTariff":         "SP01_500MAX#10mb",
            //    "ProviderPrice":          "2.00gbp",
            //    "ProviderStartDate":      "2018-04-31T11:00:00Z"
            // }

            #endregion

            try
            {

                var PurchaseOrder = JSON["PurchaseOrder"  ]?.Value<String>();
                if (PurchaseOrder == "na")
                    PurchaseOrder = null;

                var ProviderTariff = JSON["ProviderTariff"]?.Value<String>();
                if (ProviderTariff == "na")
                    ProviderTariff = null;

                var ProviderPrice = JSON["ProviderPrice"  ]?.Value<String>();
                if (ProviderPrice == "na")
                    ProviderPrice = null;


                SIMHardware  = new SIMHardware(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                               JSON["SIMNumber"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? SIM_Id.Parse(JSON["SIMNumber"].Value<String>())
                                                   : default,
                                               JSON["Description"]?.          Value<String>(),
                                               JSON["InventoryRef"]?.         Value<String>(),
                                               JSON["State"]    != null ? (SimCardStates) Enum.Parse(typeof(SimCardStates), JSON["State"].Value<String>(), true) : SimCardStates.unknown,
                                               JSON["Version"]?.              Value<Byte>(),
                                               JSON["ActorStatus"]?.          Value<Byte>(),
                                               JSON["ForeignAttributes"]?.    Value<String>(),
                                               JSON["Tariff"]?.               Value<String>(),
                                               JSON["Bundle"]?.               Value<String>(),
                                               JSON["Operator"] != null ? Operator_Id.Parse(JSON["Operator"].Value<String>()) : default(Operator_Id?),
                                               JSON["Hints"]?.                Value<String>(),
                                               JSON["NetworkStatus"]?.        Value<Byte>(),
                                               JSON["Created"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? JSON["Created"].Value<DateTime>()
                                                   : default,
                                               JSON["SOC"]?.                  Value<String>(),
                                               JSON["InternalSOC"]?.          Value<String>(),
                                               PurchaseOrder,
                                               JSON["Provider"] != null ? Provider_Id.Parse(JSON["Provider"].Value<String>()) : default(Provider_Id?),
                                               ProviderTariff,
                                               ProviderPrice,
                                               JSON["ProviderStartDate"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? JSON["ProviderStartDate"].Value<DateTime>()
                                                   : default);

                return true;

            }
            catch (Exception)
            { }

            SIMHardware = null;
            return false;

        }

        #endregion

        #region TryParse      (JSON, out SIMHardware)

        public static Boolean TryParse(JObject JSON, out SIMHardware SIMHardware)
        {

            try
            {

                SIMHardware  = new SIMHardware(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                               JSON["SIMNumber"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? new SIM_Id?(SIM_Id.Parse(JSON["SIMNumber"].Value<String>()))
                                                   : null,
                                               JSON["description"]?.          Value<String>(),
                                               JSON["inventoryRef"]?.         Value<String>(),
                                               JSON["state"] != null ? (SimCardStates) Enum.Parse(typeof(SimCardStates), JSON["State"].Value<String>(), true) : SimCardStates.unknown,
                                               JSON["cersion"]?.              Value<Byte>(),
                                               JSON["actorStatus"]?.          Value<Byte>(),
                                               JSON["foreignAttributes"]?.    Value<String>(),
                                               JSON["tariff"]?.               Value<String>(),
                                               JSON["bundle"]?.               Value<String>(),
                                               JSON["operator"] != null ? new Operator_Id?(Operator_Id.Parse(JSON["operator"].Value<String>())) : null,
                                               JSON["hints"]?.                Value<String>(),
                                               JSON["networkStatus"]?.        Value<Byte>(),
                                               JSON["created"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? JSON["created"]?.Value<DateTime>()
                                                   : null,
                                               JSON["SOC"]?.                  Value<String>(),
                                               JSON["internalSOC"]?.          Value<String>(),
                                               JSON["purchaseOrder"]?.        Value<String>(),
                                               JSON["provider"] != null ? new Provider_Id?(Provider_Id.Parse(JSON["provider"].Value<String>())) : null,
                                               JSON["providerTariff"]?.       Value<String>(),
                                               JSON["providerPrice"]?.        Value<String>(),
                                               JSON["providerStartDate"]?.Value<String>().IsNotNullOrEmpty() == true
                                                   ? JSON["providerStartDate"]?.Value<DateTime>()
                                                   : null);

                return true;

            }
            catch (Exception)
            { }

            SIMHardware = null;
            return false;

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()

            => JSONObject.Create(new JProperty("CLI",                        CLI.      ToString()),

                                 SIMNumber.HasValue
                                     ? new JProperty("SIMNumber",            SIMNumber.Value.ToString())
                                     : null,

                                 Description.IsNotNullOrEmpty()
                                     ? new JProperty("description",          Description)
                                     : null,

                                 InventoryRef.IsNotNullOrEmpty()
                                     ? new JProperty("inventoryRef",         InventoryRef)
                                     : null,

                                 State != SimCardStates.unknown
                                     ? new JProperty("state",                State.ToString())
                                     : null,

                                 Version.HasValue
                                     ? new JProperty("version",              Version)
                                     : null,

                                 ActorStatus.HasValue
                                     ? new JProperty("actorStatus",          ActorStatus)
                                     : null,

                                 ForeignAttributes.IsNotNullOrEmpty()
                                     ? new JProperty("foreignAttributes",    ForeignAttributes)
                                     : null,

                                 Tariff.IsNotNullOrEmpty()
                                     ? new JProperty("tariff",               Tariff)
                                     : null,

                                 Bundle.IsNotNullOrEmpty()
                                     ? new JProperty("bundle",               Bundle)
                                     : null,

                                 Operator.HasValue
                                     ? new JProperty("operator",             Operator.ToString())
                                     : null,

                                 Hints.IsNotNullOrEmpty()
                                     ? new JProperty("hints",                Hints)
                                     : null,

                                 NetworkStatus.HasValue
                                     ? new JProperty("networkStatus",        NetworkStatus)
                                     : null,

                                 Created.HasValue
                                     ? new JProperty("created",              Created.Value.ToIso8601())
                                     : null,

                                 SOC.IsNotNullOrEmpty()
                                     ? new JProperty("SOC",                  SOC)
                                     : null,

                                 InternalSOC.IsNotNullOrEmpty()
                                     ? new JProperty("internalSOC",          InternalSOC)
                                     : null,

                                 PurchaseOrder.IsNotNullOrEmpty()
                                     ? new JProperty("purchaseOrder",        PurchaseOrder)
                                     : null,

                                 Provider.HasValue
                                     ? new JProperty("provider",             Provider.ToString())
                                     : null,

                                 ProviderTariff.IsNotNullOrEmpty()
                                     ? new JProperty("providerTariff",       ProviderTariff)
                                     : null,

                                 ProviderPrice.IsNotNullOrEmpty()
                                     ? new JProperty("providerPrice",        ProviderPrice)
                                     : null,

                                 ProviderStartDate.HasValue
                                     ? new JProperty("providerStartDate",    ProviderStartDate.Value.ToIso8601())
                                     : null);

        #endregion


        #region Operator overloading

        #region Operator == (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SIMHardware1, SIMHardware2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SIMHardware1 == null) || ((Object) SIMHardware2 == null))
                return false;

            return SIMHardware1.Equals(SIMHardware2);

        }

        #endregion

        #region Operator != (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
            => !(SIMHardware1 == SIMHardware2);

        #endregion

        #region Operator <  (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
        {

            if ((Object) SIMHardware1 == null)
                throw new ArgumentNullException(nameof(SIMHardware1), "The given SIMHardware1 must not be null!");

            return SIMHardware1.CompareTo(SIMHardware2) < 0;

        }

        #endregion

        #region Operator <= (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
            => !(SIMHardware1 > SIMHardware2);

        #endregion

        #region Operator >  (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
        {

            if ((Object) SIMHardware1 == null)
                throw new ArgumentNullException(nameof(SIMHardware1), "The given SIMHardware1 must not be null!");

            return SIMHardware1.CompareTo(SIMHardware2) > 0;

        }

        #endregion

        #region Operator >= (SIMHardware1, SIMHardware2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware1">A communicator identification.</param>
        /// <param name="SIMHardware2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SIMHardware SIMHardware1, SIMHardware SIMHardware2)
            => !(SIMHardware1 < SIMHardware2);

        #endregion

        #endregion

        #region IComparable<SIMHardware> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is SIMHardware SIMHardware))
                throw new ArgumentException("The given object is not an communicator!");

            return CompareTo(SIMHardware);

        }

        #endregion

        #region CompareTo(SIMHardware)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SIMHardware">An communicator object to compare with.</param>
        public Int32 CompareTo(SIMHardware SIMHardware)
        {

            if ((Object) SIMHardware == null)
                throw new ArgumentNullException(nameof(SIMHardware), "The given communicator must not be null!");

            return CLI.CompareTo(SIMHardware.CLI);

        }

        #endregion

        #endregion

        #region IEquatable<SIMHardware> Members

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

            var SIMHardware = Object as SIMHardware;
            if (SIMHardware is null)
                return false;

            return Equals(SIMHardware);

        }

        #endregion

        #region Equals(SIMHardware)

        /// <summary>
        /// Compares two communicators for equality.
        /// </summary>
        /// <param name="SIMHardware">An communicator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SIMHardware SIMHardware)
        {

            if ((Object) SIMHardware == null)
                return false;

            return CLI.Equals(SIMHardware.CLI);

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
            => String.Concat(CLI, " / ", SIMNumber, " => ", State);

        #endregion

    }

}
