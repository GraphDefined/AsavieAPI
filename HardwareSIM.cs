﻿/*
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
    /// Information about the SIM hardware.
    /// </summary>
    public class HardwareSIM : IEquatable<HardwareSIM>,
                               IComparable<HardwareSIM>,
                               IComparable
    {

        #region Properties

        public CLI            CLI                   { get; } // "204046868148242"
        public SIM_Id         SIMNumber             { get; } // "89314404000452389019"
        public String         Description           { get; } // null
        public String         InventoryRef          { get; } // null
        public SimCardStates  State                 { get; } // 1
        public Byte?          Version               { get; } // 1
        public Byte?          ActorStatus           { get; } // 0
        public String         ForeignAttributes     { get; } // null
        public String         Tariff                { get; } // null
        public String         Bundle                { get; } // null
        public Operator_Id?   Operator              { get; } // "vodafone"
        public String         Hints                 { get; } // null
        public Byte?          NetworkStatus         { get; } // 0
        public DateTime?      Created               { get; } // "2018-07-31T14:42:01.8006321Z"
        public String         SOC                   { get; } // "SOC190"
        public String         InternalSOC           { get; } // null
        public String         PurchaseOrder         { get; } // "na"
        public Provider_Id?   Provider              { get; } // "westbase"
        public String         ProviderTariff        { get; } // "na"
        public String         ProviderPrice         { get; } // "na"
        public DateTime?      ProviderStartDate     { get; } // "2018-07-31T14:42:01.764135Z"

        #endregion

        #region Constructor(s)

        public HardwareSIM(CLI            CLI,
                           SIM_Id         SIMNumber,
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


        #region TryParseAsavie(JSON, out HardwareSIM)

        public static Boolean TryParseAsavie(JObject JSON, out HardwareSIM HardwareSIM)
        {

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


                HardwareSIM  = new HardwareSIM(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                               SIM_Id.Parse(JSON["SIMNumber"].Value<String>()),
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
                                               JSON["Created"]?.              Value<DateTime>(),
                                               JSON["SOC"]?.                  Value<String>(),
                                               JSON["InternalSOC"]?.          Value<String>(),
                                               PurchaseOrder,
                                               JSON["Provider"] != null ? Provider_Id.Parse(JSON["Provider"].Value<String>()) : default(Provider_Id?),
                                               ProviderTariff,
                                               ProviderPrice,
                                               JSON["ProviderStartDate"]?.    Value<DateTime>());

                return true;

            }
            catch (Exception)
            { }

            HardwareSIM = null;
            return false;

        }

        #endregion

        #region TryParse      (JSON, out HardwareSIM)

        public static Boolean TryParse(JObject JSON, out HardwareSIM HardwareSIM)
        {

            try
            {

                HardwareSIM  = new HardwareSIM(CLI.   Parse(JSON["CLI"      ].Value<String>()),
                                               SIM_Id.Parse(JSON["SIMNumber"].Value<String>()),
                                               JSON["description"]?.          Value<String>(),
                                               JSON["inventoryRef"]?.         Value<String>(),
                                               JSON["state"] != null ? (SimCardStates) Enum.Parse(typeof(SimCardStates), JSON["State"].Value<String>(), true) : SimCardStates.unknown,
                                               JSON["cersion"]?.              Value<Byte>(),
                                               JSON["actorStatus"]?.          Value<Byte>(),
                                               JSON["foreignAttributes"]?.    Value<String>(),
                                               JSON["tariff"]?.               Value<String>(),
                                               JSON["bundle"]?.               Value<String>(),
                                               JSON["operator"] != null ? Operator_Id.Parse(JSON["operator"].Value<String>()) : default(Operator_Id?),
                                               JSON["hints"]?.                Value<String>(),
                                               JSON["networkStatus"]?.        Value<Byte>(),
                                               JSON["created"]?.              Value<DateTime>(),
                                               JSON["SOC"]?.                  Value<String>(),
                                               JSON["internalSOC"]?.          Value<String>(),
                                               JSON["purchaseOrder"]?.        Value<String>(),
                                               JSON["provider"] != null ? Provider_Id.Parse(JSON["provider"].Value<String>()) : default(Provider_Id?),
                                               JSON["providerTariff"]?.       Value<String>(),
                                               JSON["providerPrice"]?.        Value<String>(),
                                               JSON["providerStartDate"]?.    Value<DateTime>());

                return true;

            }
            catch (Exception)
            { }

            HardwareSIM = null;
            return false;

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()

            => JSONObject.Create(new JProperty("CLI",                        CLI.      ToString()),
                                 new JProperty("SIMNumber",                  SIMNumber.ToString()),

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

        #region Operator == (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(HardwareSIM1, HardwareSIM2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) HardwareSIM1 == null) || ((Object) HardwareSIM2 == null))
                return false;

            return HardwareSIM1.Equals(HardwareSIM2);

        }

        #endregion

        #region Operator != (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
            => !(HardwareSIM1 == HardwareSIM2);

        #endregion

        #region Operator <  (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
        {

            if ((Object) HardwareSIM1 == null)
                throw new ArgumentNullException(nameof(HardwareSIM1), "The given HardwareSIM1 must not be null!");

            return HardwareSIM1.CompareTo(HardwareSIM2) < 0;

        }

        #endregion

        #region Operator <= (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
            => !(HardwareSIM1 > HardwareSIM2);

        #endregion

        #region Operator >  (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
        {

            if ((Object) HardwareSIM1 == null)
                throw new ArgumentNullException(nameof(HardwareSIM1), "The given HardwareSIM1 must not be null!");

            return HardwareSIM1.CompareTo(HardwareSIM2) > 0;

        }

        #endregion

        #region Operator >= (HardwareSIM1, HardwareSIM2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM1">A communicator identification.</param>
        /// <param name="HardwareSIM2">Another communicator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (HardwareSIM HardwareSIM1, HardwareSIM HardwareSIM2)
            => !(HardwareSIM1 < HardwareSIM2);

        #endregion

        #endregion

        #region IComparable<HardwareSIM> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is HardwareSIM HardwareSIM))
                throw new ArgumentException("The given object is not an communicator!");

            return CompareTo(HardwareSIM);

        }

        #endregion

        #region CompareTo(HardwareSIM)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="HardwareSIM">An communicator object to compare with.</param>
        public Int32 CompareTo(HardwareSIM HardwareSIM)
        {

            if ((Object) HardwareSIM == null)
                throw new ArgumentNullException(nameof(HardwareSIM), "The given communicator must not be null!");

            return CLI.CompareTo(HardwareSIM.CLI);

        }

        #endregion

        #endregion

        #region IEquatable<HardwareSIM> Members

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

            var HardwareSIM = Object as HardwareSIM;
            if (HardwareSIM is null)
                return false;

            return Equals(HardwareSIM);

        }

        #endregion

        #region Equals(HardwareSIM)

        /// <summary>
        /// Compares two communicators for equality.
        /// </summary>
        /// <param name="HardwareSIM">An communicator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(HardwareSIM HardwareSIM)
        {

            if ((Object) HardwareSIM == null)
                return false;

            return CLI.Equals(HardwareSIM.CLI);

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