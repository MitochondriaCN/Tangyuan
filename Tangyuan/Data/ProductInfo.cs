using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tangyuan.Data
{
    internal class ProductInfo
    {
        internal uint ID { get; private set; }
        internal uint PublisherID { get; private set; }
        internal uint CategoryID { get; private set; }
        internal uint Views { get; private set; }
        internal bool TakeOffAfterSelling { get; private set; }
        internal XDocument Content { get; private set; }
        internal ProductInfo(uint id, uint publisherID, uint categoryID, uint views, bool takeOffAfterSelling, XDocument content)
        {
            ID = id;
            PublisherID = publisherID;
            CategoryID = categoryID;
            Views = views;
            TakeOffAfterSelling = takeOffAfterSelling;
            Content = content;
        }
    }
}
