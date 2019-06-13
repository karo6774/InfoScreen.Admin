using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using InfoScreen.Admin.Logic.Entity;
using InfoScreen.Admin.Logic.Internal;

namespace InfoScreen.Admin.Logic
{
    public interface ILunchplanRepository
    {
        Task<Lunchplan> GetLunchplan(int week);
    }
}