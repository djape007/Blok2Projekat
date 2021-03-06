﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IReplicator
    {
        [OperationContract]
        void SendElements(string databaseName, Dictionary<int, Element> allElements);
        [OperationContract]
        void DeleteDataBase(string databaseName);
    }
}
