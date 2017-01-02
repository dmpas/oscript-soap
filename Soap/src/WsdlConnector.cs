using System;
using System.IO;
using System.Net;

namespace OneScript.Soap
{
	internal class WsdlConnector
	{

		private static string GenerateBasicAuthHeader(string username, string password)
		{
			var encoding = new System.Text.UTF8Encoding(false);
			byte[] data = encoding.GetBytes(username + ":" + password);
			return "Basic " + Convert.ToBase64String(data);
		}

		private static Stream ReadHttp(string url, string userName, string password)
		{
			var webRequest = WebRequest.Create(url);

			if (userName != null)
			{
				var basicAuth = GenerateBasicAuthHeader(userName, password);
				webRequest.Headers.Add(@"Authorization", basicAuth);
			}
			webRequest.ContentType = @"text/xml;charset=""utf-8""";
			webRequest.Method = @"GET";

			var response = webRequest.GetResponse();

			return response.GetResponseStream();
		}

		// TODO: Timeout, Proxy, Secured
		public static Stream ReachWsdl(string wsdl, string userName = null, string password = null)
		{

			UriBuilder uri = new UriBuilder(wsdl);
			if (uri.Scheme == "file")
			{
				return new FileStream(uri.Path, FileMode.Open);
			}

			if (uri.Scheme == "http")
			{
				// TODO: Timeout, Proxy, Secured
				return ReadHttp(wsdl, userName, password);
			}

			if (uri.Scheme == "https")
			{
				// TODO: SecuredConnection
			}
				
			throw new NotImplementedException(string.Format("Cannot load WSDL from {0}", wsdl));
		}

	}
}

