using StoreApp.Domain.Entities.Base;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class Product : BaseAuditableEntity, ICommands
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductBrandId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public string Summary { get; set; }

        public ProductBrand ProductBrand { get; set; }

        public ProductType ProductType { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastModifiedBy { get; set; }

        public User.User? CreatedByUser { get; set; }

        public User.User? LastModifiedByUser { get; set; }

        [JsonIgnore]
        public ICollection<UserLike>? UserLikes { get; set; } = new List<UserLike>();

        public int ViewCount { get; set; } = 0;

        public int LikeCount { get; set; } = 0;

        [JsonIgnore]
        public ICollection<ProductReview>? Reviews { get; set; }

        [JsonIgnore]
        public List<ProductImage>? ProductImages { get; set; } = new();

        [JsonIgnore]
        public List<ProductColor>? Colors { get; set; } = new();

        [JsonIgnore]
        public List<ProductSize>? Sizes { get; set; } = new();

        public decimal? OldPrice { get; set; }

        public double AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public int? CategoryId { get; set; }

        [JsonIgnore]
        public ProductCategory Category { get; set; }

        [JsonIgnore]
        public ICollection<UserWishlist> UserWishlists { get; set; } = new List<UserWishlist>();
    }
}
