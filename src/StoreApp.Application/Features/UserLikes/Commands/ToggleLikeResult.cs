using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserLikes.Commands
{
    public class ToggleLikeResult
    {
        public bool Liked { get; set; }

        public int LikeCount { get; set; }
    }
}
