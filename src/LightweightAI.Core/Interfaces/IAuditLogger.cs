// Project Name: ${File.ProjectName}
// File Name: ${File.FileName}
// Author: ${User.FullName}
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightweightAI.Core.Loaders.Events;



namespace LightweightAI.Core.Interfaces;

//TODO: NEED TO VERIFY IF THESE INTERFACES ARE STILL NEEDED THEN IMPLEMENT THEM AS REQUIRED


public interface IIntakeDriver
    {
    }



    public interface IAuditLogger
    {
        void Error(string P0, Exception P1);


        void Trace(string S);


        void Info(string S);


        void Warn(string S);
    }



    public interface IAppLogParser
    {
        TelemetryEvent Parse(string Line);
    }
