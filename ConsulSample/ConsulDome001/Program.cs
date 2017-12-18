﻿using Consul;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsulDome001
{
    class Program
    {
        static void Main(string[] args)
        {
             var client = new ConsulClient();

            QueryServer();

            //var getPair =  client.KV.Get("d").GetAwaiter().GetResult();
            //var value= Encoding.UTF8.GetString(getPair.Response.Value, 0,
            //    getPair.Response.Value.Length);

            //Console.WriteLine(value);
        }

        static void ACL()
        {
            var client = new ConsulClient();

            var aclList = client.ACL.List().GetAwaiter().GetResult();

            foreach (var acl in aclList.Response)
            {
                Console.WriteLine(acl.Name);

            }
        }

        static void QueryServer()
        {
           // var client = new ConsulClient(new ConsulClientConfiguration() {  }, );


            var client = new ConsulClient(opt=> { opt.Datacenter = "dc1";  });

            Console.WriteLine("client.Catalog.Nodes");
            foreach (var dic in client.Catalog.Nodes().GetAwaiter().GetResult().Response)
            {        
              
                Console.WriteLine($"name:{dic.Name}  url:{dic.Address }");
            }
            Console.WriteLine("client.Catalog.Services");
            foreach (var dic in client.Catalog.Services().GetAwaiter().GetResult().Response)
            {
                Console.WriteLine($"name:{dic.Key} ");

                foreach(var s in dic.Value)
                {
                    Console.WriteLine("value:"+s);
                }
            }
         

            Console.WriteLine("Agent.Members");
            foreach (var dic in client.Agent.Members(true).GetAwaiter().GetResult().Response)
            {
              
                
                Console.WriteLine($"name:{dic.Name}  url:{dic.Addr }:{dic.Port} 状态：{dic.Status}");
            }
            var client1 = new ConsulClient(opt => { opt.Datacenter = "dc1"; });

            Console.WriteLine("Agent.Services");
            foreach (var dic in client1.Agent.Services().GetAwaiter().GetResult().Response)
            {
                Console.WriteLine(dic.Key + "  " + dic.Value.Address+"  "+dic.Value.Port);
            }
            Console.WriteLine("Agent.Checks");
            foreach (var dic in client.Agent.Checks().GetAwaiter().GetResult().Response)
            {
                Console.WriteLine($"{dic.Key} {dic.Value.CheckID}  {dic.Value.Name} {dic.Value.ServiceName} {dic.Value.Node}");
            }

            Console.ReadLine();
        }
        static void KV()
        {
            var client = new ConsulClient();
            var kv = client.KV;
            var key = "abcd123";
            var value = Encoding.UTF8.GetBytes("test");
            var getRequest = kv.Get(key).Result;
            // Assert.IsNull(getRequest.Response);
            var pair = new KVPair(key)
            {
                Flags = 42,
                Value = value
            };
            var putRequest = kv.Put(pair).Result;
            //  Assert.IsTrue(putRequest.Response);
            Console.WriteLine(putRequest.Response);
            getRequest = kv.Get(key).Result;
            var res = getRequest.Response;
            Console.WriteLine(res.Value);
            //Assert.IsNotNull(res);
            //Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(value, res.Value));
            // Assert.AreEqual(pair.Flags, res.Flags);
            // Assert.IsTrue(getRequest.LastIndex > 0);
            var del = kv.Delete(key).Result;
            Console.WriteLine(del.Response);
            // Assert.IsTrue(del.Response);
            getRequest = kv.Get(key).Result;
            Console.WriteLine(getRequest.Response);
            // Assert.IsNull(getRequest.Response);

            var result = HelloConsul().GetAwaiter().GetResult();
            Console.WriteLine(result);
        }

        public static async Task<string> HelloConsul()
        {
            using (var client = new ConsulClient())
            {
                var putPair = new KVPair("hello")
                {
                    Value = Encoding.UTF8.GetBytes("Hello Consul")
                };

                var putAttempt = await client.KV.Put(putPair);

                if (putAttempt.Response)
                {
                    var getPair = await client.KV.Get("hello");
                    return Encoding.UTF8.GetString(getPair.Response.Value, 0,
                        getPair.Response.Value.Length);
                }
                return "";
            }
        }
    }
}
