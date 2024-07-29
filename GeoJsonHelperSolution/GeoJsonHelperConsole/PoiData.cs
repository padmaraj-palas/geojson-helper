using System.Collections.Generic;

namespace GeoJsonHelperConsole
{
    public sealed class PoiData
    {
        public int Id { get; set; }
        public ICollection<string> Categories { get; set; }
        public string NameInvariant { get; set; }
        public List<int> ControlledByPois { get; set; }
        public string Description { get; set; }

        // TODO: Images should be kept using custom class (ImageHandler)
        public string LogoImage { get; set; }
        public string MenuImage { get; set; }
        public string Name { get; set; }
        public string NavigationId { get; set; }
        public string NavigationType { get; set; }
        public string OpeningHours { get; set; }
        public int SlotId { get; set; }
        public string PoiType { get; set; }
        public string DisplayAttribute { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsPaymentEnabled { get; set; }
        public string Note { get; set; }
        public int? PoiGroupId { get; set; }
    }
}
