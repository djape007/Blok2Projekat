using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace Common
{
    public class HelperFunctions
    {
        public static NetTcpBinding SetBindingSecurity(NetTcpBinding binding) {
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            return binding; 
        }

        public static Tuple<NetTcpBinding, EndpointAddress> PrepBindingAndAddressForClient(string ServiceCertCN) {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, ServiceCertCN);

            EndpointAddress address = new EndpointAddress(new Uri(Config.ReaderWriterServiceAddress), new X509CertificateEndpointIdentity(srvCert));

            return new Tuple<NetTcpBinding, EndpointAddress>(binding, address);
        }
    }
}