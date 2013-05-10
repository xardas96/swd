using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace SWD_projekt.GeoLocalization
{
    public abstract class GeoLocalizator
    {
        private static readonly string GOOGLE_MAPS_API = "http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false";

        public static double[] GetGeolocalization(string address)
        {
            double[] output = new double[2];
            string url = String.Format(GOOGLE_MAPS_API, address);
            var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            if (request != null)
            {
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727)";
                using (var webResponse = (request.GetResponse() as HttpWebResponse))
                    if (webResponse != null)
                    {
                        using (var reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            var doc = new XmlDocument();
                            doc.Load(reader);
                            var location = doc.GetElementsByTagName("location");
                            var loc = location[0] as XmlElement;
                            output[0] = Convert.ToDouble(loc.ChildNodes[0].InnerText.Replace(".", ","));
                            output[1] = Convert.ToDouble(loc.ChildNodes[1].InnerText.Replace(".", ","));
                        }
                    }
            }
            return output;
        }
    }
}