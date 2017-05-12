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

    public class Media
    {
        public string keyId { set; get; }
        public string mediaId { set; get; }
        public string mediaName { set; get; }
        public string mediaUrl { set; get; }
        public bool isAudio { set; get; }
        public string subtitleLanguage { set; get; }
        public string subtitleUrl { set; get; }
        public string subtitleStartTime { set; get; }
        public string subtitleEndTime { set; get; }
        public string subtitleContent { set; get; }
        public override string ToString()
        {
            return String.Format(
                "ID: {0}\tName: {1}\tUrl: {2}\tLanguage: {3}\tUrl: {4}\tStartTime: {5}\tEndTime: {6}\tSubtitle: {7}\tIsAudio: {8}",
                mediaId,
                mediaName,
                mediaUrl,
                subtitleLanguage,
                subtitleUrl,
                subtitleStartTime,
                subtitleEndTime,
                subtitleContent,
                isAudio);
        }
    }
}
