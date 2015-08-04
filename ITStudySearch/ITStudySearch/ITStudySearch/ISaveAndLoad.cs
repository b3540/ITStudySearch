﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStudySearch
{
    public interface ISaveAndLoad
    {
        void SaveData(string filename, string text);
        string LoadData(string filename);
        bool ClearData(string filename);
    }
}
