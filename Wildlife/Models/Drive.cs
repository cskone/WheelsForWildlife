using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wildlife.Models
{
    [Table("dbo.Drives")]
    public class Drive
    {
        [Key]
        [Required]
        [Display(Name = "Drive Id")]
        public int DriveId { get; set; }
        [Required]
        [Display(Name = "Drive Name")]
        public string DriveName { get; set; }
        //[Required] - this restructures the entire database and breaks everything whoever did this in microsoft is mean[Display(Name = "Drive Starting Location")]
        public CivicAddress StartLocation { get; set; }
        //[Required] - this restructures the entire database and breaks everything whoever did this in microsoft is mean
        [Display(Name = "Drive Ending Location")]
        public CivicAddress EndLocation { get; set; }
        [Display(Name = "Drive Details")]
        public string ExtraDetails { get; set; }
        [Display(Name = "Driver Email")]
        public string DriverId { get; set; }
        [DisplayFormat(DataFormatString = "{0:2}", ApplyFormatInEditMode = true)]
        public decimal DriveDistance { get; set; }
        [DisplayFormat(DataFormatString = "{0:2}", ApplyFormatInEditMode = true)]
        public decimal DriveDuration { get; set; }


        public Drive(string driveName, CivicAddress startLocation, CivicAddress endLocation, string driverId)
        {
            DriveName = driveName;
            StartLocation = startLocation;
            EndLocation = endLocation;  
            DriverId = driverId;
            Tuple<int, int> driveDetails = CalcDriveDetails();
            DriveDistance = driveDetails.Item2;
            DriveDuration = driveDetails.Item1;
        }

        public Drive(string driveName, string extraDetails, CivicAddress startLocation, CivicAddress endLocation, string driverId)
        {
            DriveName = driveName;
            ExtraDetails = extraDetails;
            StartLocation = startLocation;
            EndLocation = endLocation;
            DriverId = driverId;
            Tuple<int, int> driveDetails = CalcDriveDetails();
            DriveDuration = driveDetails.Item1;
            DriveDistance = driveDetails.Item2;
        }

        public Drive()
        {

        }

        public Tuple<int, int> CalcDriveDetails()
        {
            try
            {
                int alongroaddis = Convert.ToInt32(ConfigurationManager.AppSettings["alongroad"].ToString());
                string keyString = ConfigurationManager.AppSettings["keyString"].ToString(); // passing API key
                string clientID = ConfigurationManager.AppSettings["clientID"].ToString();
                string startLocation = StartLocation.AddressLine1 + ","
                    + StartLocation.City + ","
                    + StartLocation.StateProvince + ","
                    + StartLocation.PostalCode;

                string endLocation = EndLocation.AddressLine1 + ","
                + EndLocation.City + ","
                + EndLocation.StateProvince + ","
                + EndLocation.PostalCode;

                string ApiURL = "https://maps.googleapis.com/maps/api/distancematrix/json?";
                string p = "units=imperial&origins=" + startLocation + "&destinations=" + endLocation + "&mode=Driving";
                string urlRequest = ApiURL + p;
                urlRequest += "&key=" + keyString;
                //if (keyString.ToString() != "")
                //{
                //    urlRequest += "&client=" + clientID;
                //    urlRequest = Sign(urlRequest, keyString); // request with api key and client id
                //}
                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                //string resp = "{\n   \"destination_addresses\" : [ \"300 Rodgers Blvd, Honolulu, HI 96819, USA\" ],\n   \"origin_addresses\" : [ \"1 Aloha Tower Dr, Honolulu, HI 96813, USA\" ],\n   \"rows\" : [\n      {\n         \"elements\" : [\n            {\n               \"distance\" : {\n                  \"text\" : \"5.5 mi\",\n                  \"value\" : 8850\n               },\n               \"duration\" : {\n                  \"text\" : \"13 mins\",\n                  \"value\" : 808\n               },\n               \"status\" : \"OK\"\n            }\n         ]\n      }\n   ],\n   \"status\" : \"OK\"\n}\n";


                JObject values = JObject.Parse(resp);
                var duration = (string)values["rows"][0]["elements"][0]["duration"]["value"];
                var distance = (string)values["rows"][0]["elements"][0]["distance"]["value"];
                return Tuple.Create(Int32.Parse(duration), Int32.Parse(distance));
                //return Tuple.Create(values[], values[1]);
            }

            catch (Exception ex)
            {
                throw ex;
            }       
        }

        public string Sign(string url, string keyString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
            string usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
            byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);
            Uri uri = new Uri(url);
            byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);
            // compute the hash
            HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
            byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);
            // convert the bytes to string and make url-safe by replacing '+' and '/' characters
            string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
            // Add the signature to the existing URI.
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
        }
    }

}