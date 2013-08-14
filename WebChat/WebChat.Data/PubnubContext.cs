using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Data
{
    public static class PubnubContext
    {
        private static PubnubApi pubnub = new PubnubApi(
                                "pub-c-90df6afa-5804-4bbc-a401-7b8ae210ad93",               // PUBLISH_KEY
                                "sub-c-d8e4713e-0512-11e3-8dc9-02ee2ddab7fe",               // SUBSCRIBE_KEY
                                "sec-c-M2EwNmMyNjMtNmUzZi00NzM0LTk2YjAtMDgwOGVjODAzY2Yz",   // SECRET_KEY
                                true                                                        // SSL_ON?
                                );


        public static void Publish(string channel, string message)
        {
            pubnub.Publish(channel, message);
        }
    }
}
