using Gemploy.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GesEmploy.models
{ 

    public class DayOff //weekDay
{
          [Key]
          public int IdDayOff { get; set; }
          public int WeeDay { get; set; }
          [JsonIgnore]
          public ICollection<DayOffEmployer>? DayOffEmployer { get; } = new List<DayOffEmployer>();
    }
}
