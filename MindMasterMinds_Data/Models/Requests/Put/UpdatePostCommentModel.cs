﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Put
{
    public class UpdatePostCommentModel
    {
        public Guid PostCommentId { get; set; }
        public string Comment { get; set; } = null!;
    }
}
