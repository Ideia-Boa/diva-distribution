/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.StructuredData;

namespace OpenSim.Region.Environment
{
    public class EventQueueHelper
    {
        private EventQueueHelper() {} // no construction possible, it's an utility class
        
        public static LLSD EnableSimulator(ulong Handle, IPEndPoint endPoint)
        {
            LLSDMap llsdSimInfo = new LLSDMap(3);
            byte[] regionhandle = new byte[8];
            int i = 0;
            
            // Reverse endianness of RegionHandle
            regionhandle[i++] = (byte)((Handle >> 56) % 256);
            regionhandle[i++] = (byte)((Handle >> 48) % 256);
            regionhandle[i++] = (byte)((Handle >> 40) % 256);
            regionhandle[i++] = (byte)((Handle >> 32) % 256);
            regionhandle[i++] = (byte)((Handle >> 24) % 256);
            regionhandle[i++] = (byte)((Handle >> 16) % 256);
            regionhandle[i++] = (byte)((Handle >> 8) % 256);
            regionhandle[i++] = (byte)(Handle % 256);

            llsdSimInfo.Add("Handle", new LLSDBinary(regionhandle));
            llsdSimInfo.Add("IP", new LLSDBinary(endPoint.Address.GetAddressBytes()));
            llsdSimInfo.Add("Port", new LLSDInteger(endPoint.Port));
            
            LLSDArray arr = new LLSDArray(1);
            arr.Add(llsdSimInfo);
            
            LLSDMap llsdBody = new LLSDMap(1);
            llsdBody.Add("SimulatorInfo", arr);

            LLSDMap llsdMessage = new LLSDMap(2);
            llsdMessage.Add("message", new LLSDString("EnableSimulator"));
            llsdMessage.Add("body", llsdBody);
            
            return llsdMessage;
        }
        public static LLSD KeepAliveEvent()
        {
            LLSDMap llsdSimInfo = new LLSDMap();
            LLSDMap llsdMessage = new LLSDMap(2);
            llsdMessage.Add("message", new LLSDString("FAKEEVENT"));
            llsdMessage.Add("body", llsdSimInfo);

            return llsdMessage;
        }
    }
}
