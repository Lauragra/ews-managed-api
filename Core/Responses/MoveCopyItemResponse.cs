// ---------------------------------------------------------------------------
// <copyright file="MoveCopyItemResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------

//-----------------------------------------------------------------------
// <summary>Defines the MoveCopyItemResponse class.</summary>
//-----------------------------------------------------------------------

namespace Microsoft.Exchange.WebServices.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// Represents a response to a Move or Copy operation.
    /// </summary>
    public sealed class MoveCopyItemResponse : ServiceResponse
    {
        private Item item;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCopyItemResponse"/> class.
        /// </summary>
        internal MoveCopyItemResponse()
            : base()
        {
        }

        /// <summary>
        /// Gets Item instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="xmlElementName">Name of the XML element.</param>
        /// <returns>Item.</returns>
        private Item GetObjectInstance(ExchangeService service, string xmlElementName)
        {
            return EwsUtilities.CreateEwsObjectFromXmlElementName<Item>(service, xmlElementName);
        }

        /// <summary>
        /// Reads response elements from XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        internal override void ReadElementsFromXml(EwsServiceXmlReader reader)
        {
            base.ReadElementsFromXml(reader);

            List<Item> items = reader.ReadServiceObjectsCollectionFromXml<Item>(
                XmlElementNames.Items,
                this.GetObjectInstance,
                false,  /* clearPropertyBag */
                null,   /* requestedPropertySet */
                false); /* summaryPropertiesOnly */

            // We only receive the copied or moved items if the copy or move operation was within
            // a single mailbox. No item is returned if the operation is cross-mailbox, from a
            // mailbox to a public folder or from a public folder to a mailbox.
            if (items.Count > 0)
            {
                this.item = items[0];
            }
        }

        /// <summary>
        /// Reads response elements from Json.
        /// </summary>
        /// <param name="responseObject">The response object.</param>
        /// <param name="service">The service.</param>
        internal override void ReadElementsFromJson(JsonObject responseObject, ExchangeService service)
        {
            EwsServiceJsonReader jsonReader = new EwsServiceJsonReader(service);

            List<Item> items = jsonReader.ReadServiceObjectsCollectionFromJson<Item>(
                responseObject,
                XmlElementNames.Folders,
                this.GetObjectInstance,
                false,  /* clearPropertyBag */
                null,   /* requestedPropertySet */
                false); /* summaryPropertiesOnly */

            // We only receive the copied or moved items if the copy or move operation was within
            // a single mailbox. No item is returned if the operation is cross-mailbox, from a
            // mailbox to a public folder or from a public folder to a mailbox.
            if (items.Count > 0)
            {
                this.item = items[0];
            }
        }

        /// <summary>
        /// Gets the copied or moved item. Item is null if the copy or move operation was between
        /// two mailboxes or between a mailbox and a public folder.
        /// </summary>
        public Item Item
        {
            get { return this.item; }
        }
    }
}
