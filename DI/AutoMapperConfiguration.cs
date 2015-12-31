using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using AutoMapper;

namespace Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {


            #region "Added for ClassifiedFeed"

            Mapper.CreateMap<AutoClassifiedFeed, Repository.Models.Admin.Auto.Auto>()

              //.ForMember(dest => dest, opt => opt.Ignore())
              .ForMember(dest => dest.AdId, opt => opt.MapFrom(m => m.AdId))
              .ForMember(dest => dest.Bodystyle,
              opt => opt.MapFrom(m => m.Bodystyle))
              .ForMember(dest => dest.Description,
                  opt => opt.MapFrom(m => m.Description))
              .ForMember(dest => dest.Enginetext,
                  opt => opt.MapFrom(m => m.Enginetext))
              .ForMember(dest => dest.ExteriorColor,
                  opt => opt.MapFrom(m => m.Exteriorcolor))
              .ForMember(dest => dest.Fueltype,
                  opt => opt.MapFrom(m => m.Fueltype))
              .ForMember(dest => dest.DealerCity,
                  opt => opt.MapFrom(m => m.City))
              .ForMember(dest => dest.DealerState,
                    opt => opt.MapFrom(m => m.State))
              .ForMember(dest => dest.DealerZip,
                  opt => opt.MapFrom(m => m.Zip))
              .ForMember(dest => dest.InteriorColor,
                  opt => opt.MapFrom(m => m.Interiorcolor))
              .ForMember(dest => dest.Year,
              opt => opt.MapFrom(m => String.IsNullOrEmpty(m.Year)? 0 :Convert.ToInt32(m.Year)))
              .ForMember(dest => dest.Make,
                  opt => opt.MapFrom(m => m.Make))
              .ForMember(dest => dest.Model,
                  opt => opt.MapFrom(m => m.Model)) 
             .ForMember(dest => dest.Mileage,
                  opt => opt.MapFrom(m => string.IsNullOrEmpty(m.Mileage) ? 0 : Convert.ToDouble(m.Mileage)))
              .ForMember(dest => dest.Postdate,
                  opt => opt.MapFrom(m => m.Postdate))
              .ForMember(dest => dest.Expiredate,
                  opt => opt.MapFrom(m => m.Expiredate))
              .ForMember(dest => dest.Price,
                  opt => opt.MapFrom(m => m.Price))
            .ForMember(dest => dest.Price,
            opt => opt.MapFrom(m => string.IsNullOrEmpty(m.Price) ? 0 : Convert.ToDouble(m.Price.Contains(',') ? m.Price.Replace(",", "") : m.Price)))
              .ForMember(dest => dest.Transmission,
                  opt => opt.MapFrom(m => m.Transmissiontype))
              .ForMember(dest => dest.Drivetype,
                  opt => opt.MapFrom(m => m.Drivetype))
              .ForMember(dest => dest.Style,
                  opt => opt.MapFrom(m => m.Style))
             .ForMember(dest => dest.Vin,
                  opt => opt.MapFrom(m => m.Vin))
            .ForMember(dest => dest.PhotosUrl,
                  opt => opt.MapFrom(m => m.Images.Image))
            .ForMember(dest => dest.DealerName,
                  opt => opt.MapFrom(m => m.Name))
            .ForMember(dest => dest.DealerPhone,
                  opt => opt.MapFrom(m => m.Phone))
            .ForMember(dest => dest.DealerPhone,
                  opt => opt.MapFrom(m => m.Alternatephone))
            .ForMember(dest => dest.DealerEmail,
                  opt => opt.MapFrom(m => m.Email))
            .ForMember(dest => dest.IsClassified,
                  opt => opt.MapFrom(m => true))
            .ForMember(dest => dest.UpsellFeaturedAd,
            opt => opt.MapFrom(m => m.Upsell.UpsellFeaturedAd == "0" ? "true" : "false"))
            .ForMember(dest => dest.UpsellSpotlightAd,
                  opt => opt.MapFrom(m => m.Upsell.UpsellSpotlightAd == "0" ? "true" : "false"));
            #endregion

        }
    }
}
