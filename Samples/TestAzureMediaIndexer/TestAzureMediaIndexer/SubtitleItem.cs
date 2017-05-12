//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAzureMediaIndexer
{
    public class Item
    {
        public string Name;
        public string Value;
        public Item(string name, string value)
        {
            Name = name; Value = value;
        }
        public override string ToString()
        {
            // Generates the text shown in the combo box
            return Name;
        }
    }
    public class SubtitileItem
    {
        public string startTime;
        public string endTime;
        public string subtitle;
        public SubtitileItem(string start, string end, string sub)
        {
            startTime = start;
            endTime = end;
            subtitle = sub;
        }
        public override string ToString()
        {
            // Generates the text shown in the combo box
            return startTime + " --> " + endTime + "\r\n" + subtitle + "\r\n\r\n";
        }
    }
}
