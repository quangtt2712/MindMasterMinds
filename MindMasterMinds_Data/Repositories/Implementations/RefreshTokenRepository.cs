﻿using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Repositories.Implementations
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MindMasterMinds_DBContext context) : base(context)
        {
        }
    }
}
