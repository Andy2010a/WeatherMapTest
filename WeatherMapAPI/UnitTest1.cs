using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherMapAPI
{
	[TestClass]
	public class UnitTest1
	{

		public class Client
		{
			public string URL { get; set; }
			//public string Parameters  { get; set; }
			public string ContentType { get; set; }
			public string Method { get; set; }

		

			public string Request(string Parameters)
			{
				var request = (HttpWebRequest)WebRequest.Create(URL + Parameters);
				request.Method = Method.ToString();
				request.ContentType = ContentType;
				using (var response = (HttpWebResponse)request.GetResponse())
				{
					string responsevalue = string.Empty;
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{

						//var result = JsonConvert.DeserializeObject(response..Content.ReadAsStringAsync().Result.ToString());
						using (var responseStream = response.GetResponseStream())
						{

							if (responseStream != null)
								using (var reader = new StreamReader(responseStream))
								{
									responsevalue = reader.ReadToEnd();
								}
						}

					}
					return responsevalue;
				}

			
			}
			//The API doesn't return the day of the week so a method to get Thurusday
			public int GetNextThursday()
			{

				DateTime today = DateTime.Today;
				int daysUntilThurusday = ((int)DayOfWeek.Thursday - (int)today.DayOfWeek + 7) % 7;
				return daysUntilThurusday;

			}
		}

		[TestMethod]
		public void TestMethod()
		{

		

			var client = new Client
			{
				URL = "https://openweathermap.org/data/2.5/forecast/daily",
				ContentType = "application/JSON",
				Method = "GET"
			};

			//query parameters
			int cityId = 2147714; //sydney city id
			string key = "b1b15e88fa797225412429c1c50c122a1";// key to access API
			int count = 16;//forecast days
			string unit = "metric"; // unit as metric so as to see temp in  celsius 

			JObject json  = JObject.Parse (client.Request(string.Format("?id={0}&appid={1}&cnt={2}&units={3}", cityId, key, count, unit)));


			Assert.AreEqual("Sydney", json["city"]["name"].ToString());

	
			int daysuntilThurusday = client.GetNextThursday();
			JToken weatherThu = json["list"][daysuntilThurusday];


			Assert.IsTrue(decimal.Parse(weatherThu["temp"]["min"].ToString()) > 10, "Temp is less than 10 ");
			//Assert.IsTrue(decimal.Parse(weatherThu["temp"]["min"].ToString()) > 20, "Temp is less than 10 ");













		}
	}
}

