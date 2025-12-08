using AutoMapper;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Dtos.Admin.AdminProductBrandDto;
using StoreApp.Application.Dtos.Admin.AdminProductCategoryDto;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Application.Dtos.Admin.AdminProductTypeDtp;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.CreateProduct;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            CreateMap<EditUserProfileCommand, Address>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Address, AddressDto>();

            CreateMap<User, UserDto>()
            .ForMember(d => d.Token, opt => opt.Ignore())  
            .ForMember(d => d.Role, opt => opt.Ignore());

            CreateMap<User, AdminUserDto>()
            .ForMember(d => d.Role, opt => opt.Ignore());

            CreateMap<Permission, PermissionDto>();
            CreateMap<CreatePermissionDto, Permission>();
            CreateMap<UpdatePermissionDto, Permission>();

            CreateMap<Product, AdminProductListDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand != null ? s.ProductBrand.Title : ""))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType != null ? s.ProductType.Title : ""))
                .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.Category != null ? s.Category.Name : ""))
                .ForMember(d => d.Colors, o => o.MapFrom(s => s.Colors != null ? string.Join(",", s.Colors.Select(c => c.ColorCode)) : ""))
                .ForMember(d => d.Sizes, o => o.MapFrom(s => s.Sizes != null ? string.Join(",", s.Sizes.Select(sz => sz.Size)) : ""))
                .ForMember(d => d.MainImage, o => o.MapFrom(s => s.PictureUrl));

            CreateMap<Product, AdminCreateProductDto>()
                .ForMember(d => d.ProductBrandId, o => o.MapFrom(s => s.ProductBrandId))
                 .ForMember(d => d.ProductTypeId, o => o.MapFrom(s => s.ProductTypeId))
                 .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                 .ForMember(d => d.Summary, o => o.MapFrom(s => s.Summary))
                 .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                 .ForMember(d => d.OldPrice, o => o.MapFrom(s => s.OldPrice))
                 .ForMember(d => d.Colors, o => o.MapFrom(s => s.Colors != null ? s.Colors.Select(c => c.ColorCode).ToList() : new List<string>()))
                 .ForMember(d => d.Sizes, o => o.MapFrom(s => s.Sizes != null ? s.Sizes.Select(sz => sz.Size).ToList() : new List<string>()))
                 .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand))
                 .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType))
                 .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.Category))
                 .ForMember(d => d.MainImage, o => o.Ignore())
                 .ForMember(d => d.Gallery, o => o.Ignore());

            CreateMap<AdminCreateProductCommand, Product>()
                .ForMember(d => d.ProductImages, o => o.Ignore())
                .ForMember(d => d.Colors, o => o.Ignore())
                .ForMember(d => d.Sizes, o => o.Ignore())
                .ForMember(d => d.PictureUrl, o => o.Ignore());

            CreateMap<User, AdminUserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.MainRole));

            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<ProductType, ProductTypeDto>().ReverseMap();
            CreateMap<ProductBrand, ProductBrandDto>().ReverseMap();

            CreateMap<Product, AdminUpdateProductDto>()
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.PictureUrl))
                .ForMember(dest => dest.Gallery, opt => opt.MapFrom(src => src.ProductImages.Select(p => p.ImageUrl)))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.Colors.Select(c => c.ColorCode)))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(s => s.Size)))
                .ForMember(dest => dest.ProductBrandId, opt => opt.MapFrom(src => src.ProductBrandId))
                .ForMember(dest => dest.ProductTypeId, opt => opt.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Colors, opt => opt.Ignore())
                .ForMember(dest => dest.Sizes, opt => opt.Ignore());

            CreateMap<Product, AdminProductDetailsDto>()
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.PictureUrl))

                .ForMember(dest => dest.Gallery, opt => opt.MapFrom(src =>
                        src.ProductImages != null
                            ? src.ProductImages.Select(x => x.ImageUrl).ToList()
                            : new List<string>()))

                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src =>
                        src.Colors != null
                            ? src.Colors.Select(x => x.ColorCode).ToList()
                            : new List<string>()))

                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src =>
                        src.Sizes != null
                            ? src.Sizes.Select(x => x.Size).ToList()
                            : new List<string>()))

                .ForMember(d => d.ProductCategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.ProductBrandName, o => o.MapFrom(s => s.ProductBrand.Title))
                .ForMember(d => d.ProductTypeName, o => o.MapFrom(s => s.ProductType.Title));

        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            var mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count <= 0) continue;
                    foreach (var interfaceMethodInfo in interfaces.Select(@interface =>
                                 @interface.GetMethod(mappingMethodName, argumentTypes)))
                    {
                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
