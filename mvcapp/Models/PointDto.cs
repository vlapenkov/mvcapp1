using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StimulsoftForGd.Models
{
    public class PointDto
    {
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }

        public DateTime SDate { get; set; }
    }

    public class ContractType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }


    public class PointExtended:PointDto
    {
        public int RegionId { get; set; }
        ////  public ContractType ContractType { get; set; }

        
    }

    public class PointsWithRegions {
        public IEnumerable<PointExtended> Points { get; set; }

        public IEnumerable<Region> Regions { get; set; }
    }

    public class PointAndRegion : PointDto
    {
        public Region Region { get; set; }
        

    }
}
