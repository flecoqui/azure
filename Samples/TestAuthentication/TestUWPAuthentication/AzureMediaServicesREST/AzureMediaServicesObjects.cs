using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace TestUWPAuthentication.AzureMediaServicesREST
{
    [DataContract]
    class RootObject
    {
        [DataMember]
        string Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Type: " + this.GetType().Name + " Name: " + Name + " Id: " + Id);
        }
    }

    [DataContract]
    class MediaProcessor : RootObject
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Sku { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public string Version { get; set; }
    }

    [DataContract]
    class Locator : RootObject
    {
        [DataMember]
        public string ExpirationDateTime { get; set; }
        public int Type { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string BaseUri { get; set; }
        [DataMember]
        public string ContentAccessComponent { get; set; }
        [DataMember]
        public string AccessPolicyId { get; set; }
        [DataMember]
        public string AssetId { get; set; }
        [DataMember]
        public string StartTime { get; set; }

    }

    [DataContract]
    class StreamingEndpoint : RootObject
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Created { get; set; }
        [DataMember]
        public string LastModified { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string HostName { get; set; }
        [DataMember]
        public int ScaleUnits { get; set; }
        [DataMember]
        public List<object> CustomHostNames { get; set; }
        [DataMember]
        public object AccessControl { get; set; }
        [DataMember]
        public object CacheControl { get; set; }
        [DataMember]
        public object CrossSiteAccessPolicies { get; set; }
        [DataMember]
        public bool CdnEnabled { get; set; }
    }
    [DataContract]
    class AccessPolicie : RootObject
    {
        [DataMember]
        public string Created { get; set; }
        [DataMember]
        public string LastModified { get; set; }
        [DataMember]
        public double DurationInMinutes { get; set; }
        [DataMember]
        public int Permissions { get; set; }

    }

    [DataContract]
    class Program : RootObject
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Created { get; set; }
        [DataMember]
        public string LastModified { get; set; }
        [DataMember]
        public string ChannelId { get; set; }
        [DataMember]
        public string AssetId { get; set; }
        [DataMember]
        public string ArchiveWindowLength { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ManifestName { get; set; }
    }

    [DataContract]
    public class Allow
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int SubnetPrefixLength { get; set; }
    }
    [DataContract]
    public class IP
    {
        [DataMember]
        public List<Allow> Allow { get; set; }
    }
    [DataContract]
    public class AccessControl
    {
        [DataMember]
        public IP IP { get; set; }
    }
    [DataContract]
    public class Endpoint
    {
        [DataMember]
        public string Protocol { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
    [DataContract]
    public class Input
    {
        [DataMember]
        public object KeyFrameInterval { get; set; }
        [DataMember]
        public string StreamingProtocol { get; set; }
        [DataMember]
        public AccessControl AccessControl { get; set; }
        [DataMember]
        public List<Endpoint> Endpoints { get; set; }
    }
    [DataContract]
    public class Allow2
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int SubnetPrefixLength { get; set; }
    }
    [DataContract]
    public class IP2
    {
        [DataMember]
        public List<Allow2> Allow { get; set; }
    }
    [DataContract]
    public class AccessControl2
    {
        [DataMember]
        public IP2 IP { get; set; }
    }
    [DataContract]
    public class Endpoint2
    {
        [DataMember]
        public string Protocol { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
    [DataContract]
    public class Preview
    {
        [DataMember]
        public AccessControl2 AccessControl { get; set; }
        [DataMember]
        public List<Endpoint2> Endpoints { get; set; }
    }
    [DataContract]
    public class CrossSiteAccessPolicies
    {
        [DataMember]
        public object ClientAccessPolicy { get; set; }
        [DataMember]
        public object CrossDomainPolicy { get; set; }
    }
    [DataContract]
    class Channel : RootObject
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Created { get; set; }
        [DataMember]
        public string LastModified { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public Input Input { get; set; }
        [DataMember]
        public Preview Preview { get; set; }
        [DataMember]
        public object Output { get; set; }
        [DataMember]
        public CrossSiteAccessPolicies CrossSiteAccessPolicies { get; set; }
        [DataMember]
        public string EncodingType { get; set; }
        [DataMember]
        public object Encoding { get; set; }
        [DataMember]
        public object Slate { get; set; }
    }


    [DataContract]
    class Asset : RootObject
    {
        [DataMember]
        Int32 State { get; set; }
        [DataMember]
        string Created { get; set; }
        [DataMember]
        string LastModified { get; set; }
        [DataMember]
        public object AlternateId { get; set; }

        [DataMember]
        public int Options { get; set; }
        [DataMember]
        public string Uri { get; set; }
        [DataMember]
        public string StorageAccountName { get; set; }
    }
    [DataContract]
    class ResultObject<T>
    {
        [DataMember(Name = "odata.metadata")]
        public string odata { get; set; }
        [DataMember(Name = "value")]
        public List<T> value { get; set; }
    }

}
