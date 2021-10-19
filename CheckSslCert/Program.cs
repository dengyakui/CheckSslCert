/***
 * Small program to check ssl cert expiration
 */

using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

string hostName = "trojan001.southeastasia.cloudapp.azure.com";
int port = 443;

var cert = await GetCert(hostName, port);
if (cert == null)
{
    Console.WriteLine("Network error, can not check the cert currently!!");
    return 0;
}

Console.WriteLine("===============cert info======================");
Console.WriteLine($"Subject: {cert.Subject}");
Console.WriteLine($"Issuer: {cert.Issuer}");
Console.WriteLine($"Algorithm: {cert.SignatureAlgorithm.FriendlyName}");
Console.WriteLine($"Cert is valid from: {cert.NotBefore:yyyy/MM/dd} to: {cert.NotAfter:yyyy/MM/dd}");
Console.WriteLine("===============================================");

var result = (DateTime.Now - cert.NotAfter).TotalDays switch
{
    > 0 => $"Shit, your domain{hostName}'s ssl cert has been expired.",
    > -7 and < 0 =>
        $"Warning, Your domain({hostName})'s ssl cert will expire within 1 week", // TODO: send an email to notify me
    > -30 and < 0 => $"Careful,your domain({hostName})'s ssl cert will expire within 1 month",
    var days =>
        $"The domain({hostName})'s ssl cert still has {Math.Ceiling(Math.Abs(days))} days left before expire."
};

Console.WriteLine(result);
return 0;


static async Task<X509Certificate2?> GetCert(string hostName, int port = 443)
{
    using var tcpClient = new TcpClient(hostName, port);
    var sslStream = new SslStream(tcpClient.GetStream(), leaveInnerStreamOpen: true, (_, _, _, _) => true);

    // Initiate the connection, so it will download the server certificate
    await sslStream.AuthenticateAsClientAsync(hostName).ConfigureAwait(false);

    // before disposed, copy the data to a new cert obj
    return sslStream.RemoteCertificate != null ? new X509Certificate2(sslStream.RemoteCertificate) : null;
}