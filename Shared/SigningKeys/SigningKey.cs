using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SigningKeys
{
    public static class SigningKeys
    {
        public static char[] GetPublicKey()
        {
            RSA rsa = RSA.Create();
            var publicKey = File.ReadAllText("../Shared/SigningKeys/public_key.pem");

            rsa.Dispose();
            return publicKey.ToCharArray();
        }

        public static char[] GetPrivateKey()
        {
            RSA rsa = RSA.Create();
            var privateKey = File.ReadAllText("../Shared/SigningKeys/private_key.pem");

            rsa.Dispose();
            return privateKey.ToCharArray();
        }
    }
}
