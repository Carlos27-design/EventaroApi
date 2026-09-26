using System.ComponentModel.DataAnnotations;

namespace EventaroApi.DTOs.EventDTOs
{
    public class UpdateUbicationDTO
    {
        public string Address { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
