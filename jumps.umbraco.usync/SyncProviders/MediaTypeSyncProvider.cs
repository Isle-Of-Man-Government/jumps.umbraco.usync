using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq; 

using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;


namespace jumps.umbraco.usync.SyncProviders
{
    /// <summary>
    ///  provides the core syncronization functions for
    ///  mediaTypes (against the 6.1.x API)
    /// </summary>
    public static class MediaTypeSyncProvider
    {
        // static PackagingService _packService;
        static IContentTypeService _contentTypeService;
        static IDataTypeService _dataTypeService; 

        static MediaTypeSyncProvider()
        {
            // _packService = ApplicationContext.Current.Services.PackagingService;
            _contentTypeService = ApplicationContext.Current.Services.ContentTypeService;
            _dataTypeService = ApplicationContext.Current.Services.DataTypeService;
        }

        /// <summary>
        ///  exports the media type to XML - this is a copy of the internal class
        ///  Export(IMediaType mediaType) from the packaging Service (which is internal)
        ///  
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static XElement SyncExport(this Umbraco.Core.Models.IMediaType mediaType)
        {
            var info = new XElement("Info",
                                   new XElement("Name", mediaType.Name),
                                   new XElement("Alias", mediaType.Alias),
                                   new XElement("Icon", mediaType.Icon),
                                   new XElement("Thumbnail", mediaType.Thumbnail),
                                   new XElement("Description", mediaType.Description),
                                   new XElement("AllowAtRoot", mediaType.AllowedAsRoot.ToString()));

            var masterContentType = mediaType.CompositionAliases().FirstOrDefault();
            if (masterContentType != null)
                info.Add(new XElement("Master", masterContentType));

            var structure = new XElement("Structure");
            foreach (var allowedType in mediaType.AllowedContentTypes)
            {
                structure.Add(new XElement("MediaType", allowedType.Alias));
            }

            var genericProperties = new XElement("GenericProperties");
            foreach (var propertyType in mediaType.PropertyTypes)
            {
                var definition = _dataTypeService.GetDataTypeDefinitionById(propertyType.DataTypeDefinitionId);
                var propertyGroup = mediaType.PropertyGroups.FirstOrDefault(x => x.Id == propertyType.PropertyGroupId.Value);
                var genericProperty = new XElement("GenericProperty",
                                                   new XElement("Name", propertyType.Name),
                                                   new XElement("Alias", propertyType.Alias),
                                                   new XElement("Type", propertyType.DataTypeId.ToString()),
                                                   new XElement("Definition", definition.Key),
                                                   new XElement("Tab", propertyGroup == null ? "" : propertyGroup.Name),
                                                   new XElement("Mandatory", propertyType.Mandatory.ToString()),
                                                   new XElement("Validation", propertyType.ValidationRegExp),
                                                   new XElement("Description", new XCData(propertyType.Description)));
                genericProperties.Add(genericProperty);
            }

            var tabs = new XElement("Tabs");
            foreach (var propertyGroup in mediaType.PropertyGroups)
            {
                var tab = new XElement("Tab",
                                       new XElement("Id", propertyGroup.Id.ToString(CultureInfo.InvariantCulture)),
                                       new XElement("Caption", propertyGroup.Name));
                tabs.Add(tab);
            }

            var xml = new XElement("MediaType",
                                   info,
                                   structure,
                                   genericProperties,
                                   tabs);
            return xml;
        }

        public static void ImportMediaType(this XElement node)
        {

        }

        public static void SyncImportStructure(this IMediaType item, XElement node)
        {
        }

        public static void SyncRemoveMissingProperties(this IMediaType item, XElement node)
        {
        }


        public static string GetSyncPath(this IMediaType item)
        {
            string path = "";

            if (item != null)
            {
                if (item.ParentId != 0)
                {
                    path = _contentTypeService.GetMediaType(item.ParentId).GetSyncPath();
                }

                path = string.Format("{0}\\{1}", path, helpers.XmlDoc.ScrubFile(item.Alias));
            }
            return path;
        }

    }
}
