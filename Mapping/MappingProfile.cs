using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Controllers.Resources;
using VEGA.Models;
using VEGA.Models_API; 

namespace VEGA.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain to API models (resources)

            CreateMap<Brand, BrandResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Vehicle, VehicleSaveResource>() 
                .ForMember(dest => dest.FeatureIds, operations => operations.MapFrom(src => src.Features.Select(feature => feature.FeatureId)))  
                // Contact
                .AfterMap((src,  dest) =>
                {
                    dest.Contact.Name = src.ContactName;
                    dest.Contact.Phone = src.ContactPhone;
                    dest.Contact.Email = src.ContactEmail;
                })
                ;
            CreateMap<Vehicle, VehicleGetResource>()
                .ForMember(dest => dest.Features, operations => operations.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (dest == null)
                        dest = new VehicleGetResource();
                    dest.Brand.Id = src.Model.Brand.Id;
                    dest.Brand.Name = src.Model.Brand.Name;
                    dest.Contact.Name = src.ContactName;
                    dest.Contact.Phone = src.ContactPhone;
                    dest.Contact.Email = src.ContactEmail;
                    dest.UserID = src.UserID;
                    dest.Features = src.Features.Select(vf => new IdNameResourceType { Id = vf.Feature.Id, Name = vf.Feature.Name }); 
                })
                ;
            CreateMap<Brand, IdNameResourceType>();
            CreateMap<VehicleFilter, VehicleFilterResource>();
            CreateMap<VehiclesQueryResult, VehiclesQueryResultResource>();
            CreateMap<Photo, PhotoResource>();
            CreateMap<BrandVehicleCount, BrandVehicleCountResource>();

            // API to Domain models

            CreateMap<VehicleSaveResource, Vehicle>()
                .ForMember(dest => dest.Features, operations => operations.Ignore())
                // Contact
                .AfterMap((src, dest) =>
                {
                    dest.ContactName = src.Contact.Name;
                    dest.ContactPhone = src.Contact.Phone;
                    dest.ContactEmail = src.Contact.Email; 
                })
                // Features (detail 1:n)
                .AfterMap((src, dest) =>
                {
                    // Remove unselected features
                    var removedFeatures = new List<VehicleFeature>();
                    foreach (var destFeature in dest.Features)
                    {
                        if (! src.FeatureIds.Contains(destFeature.FeatureId)) 
                            removedFeatures.Add(destFeature); 
                    }
                    foreach (var removedFeature in removedFeatures)
                    {
                        dest.Features.Remove(removedFeature);
                    }
                    // Add new features
                    foreach (var srcFeatureId in src.FeatureIds)
                    {
                        if (!dest.Features.Any(destFeature => destFeature.FeatureId == srcFeatureId))
                            dest.Features.Add(new VehicleFeature { FeatureId = srcFeatureId });
                    }
                })
                ;
            CreateMap<IdNameResourceType, Brand>();
            CreateMap<VehicleFilterResource, VehicleFilter>();
            CreateMap<VehiclesQueryResultResource, VehiclesQueryResult>();
            CreateMap<PhotoResource, Photo>();
            CreateMap<BrandVehicleCountResource, BrandVehicleCount>();

        }
    }
}
